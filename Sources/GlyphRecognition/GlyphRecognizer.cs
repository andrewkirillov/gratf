// Gliph Recognition Library
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010
// andrew.kirillov@aforgenet.com
//

namespace AForge.Vision.GlyphRecognition
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;

    using AForge;
    using AForge.Imaging;
    using AForge.Imaging.Filters;
    using AForge.Math.Geometry;

    /// <summary>
    /// The class locates and recognizes glyphs in a specified image.
    /// </summary>
    /// 
    public class GlyphRecognizer
    {
        // ---> set of image processing routines in use
        private DifferenceEdgeDetector edgeDetector = new DifferenceEdgeDetector( );
        private Threshold thresholdFilter = new Threshold( 40 );
        private BlobCounter blobCounter = new BlobCounter( );
        private SimpleShapeChecker shapeChecker = new SimpleShapeChecker( );
        private QuadrilateralTransformation quadrilateralTransformation = new QuadrilateralTransformation( );
        private OtsuThreshold otsuThresholdFilter = new OtsuThreshold( );
        private SquareBinaryGlyphRecognizer binaryGlyphRecognizer = new SquareBinaryGlyphRecognizer( );
        // <---

        // size of glyphs to search/recognize
        private int glyphSize;
        // database of glyphs to recognize
        private GlyphDatabase glyphDatabase;
        // maximum number of glyph to search in single image
        private int maxNumberOfGlyphsToSearch = 3;
        // mimimum confidance level for extracted raw glyph data
        private float minConfidenceLevel = 0.60f;

        // object used for synchronization
        private object sync = new object( );

        /// <summary>
        /// Size of glyph to search and recognize, [5, 23].
        /// </summary>
        /// 
        /// <remarks><para>The property specifies the size of glyphs the instance of the class will be searching for.</para>
        /// 
        /// <para><note>Setting of this property is allowed only in the case if <see cref="GlyphDatabase"/> is set
        /// to <see langword="null"/>. If glyph database is set, then this property is initialized with glyphs' size of the
        /// database.</note></para>
        /// </remarks>
        /// 
        /// <exception cref="ApplicationException">Glyph size cannot be set if glyph database is set.</exception>
        /// <exception cref="ArgumentException">Invalid glyph size was specified.</exception>
        /// 
        public int GlyphSize
        {
            get { return glyphSize; }
            set
            {
                if ( glyphDatabase != null )
                {
                    throw new ApplicationException( "Glyph size cannot be set if glyph database is set." );
                }

                if ( ( value < 5 ) || ( value > 23 ) )
                {
                    throw new ArgumentException( "Invalid glyph size was specified." );
                }

                lock ( sync )
                {
                    glyphSize = value;
                    quadrilateralTransformation.NewWidth = quadrilateralTransformation.NewHeight = glyphSize * 20;
                    binaryGlyphRecognizer.GlyphSize = glyphSize;
                }
            }
        }

        /// <summary>
        /// Database of glyphs to recognize.
        /// </summary>
        /// 
        /// <remarks><para>The property sets database of glyphs, which could be recognized by an instance of the class.
        /// In the case if glyph recognizer finds some glyphs which are not listed in the database, it will still provide
        /// information about them, but <see cref="ExtractedGlyphData.RecognizedGlyph">RecognizedGlyph</see> and
        /// <see cref="ExtractedGlyphData.RecognizedQuadrilateral">RecognizedQuadrilateral</see>
        /// properties of <see cref="ExtractedGlyphData"/> will not be set.</para>
        /// 
        /// <para>Setting this property will also set <see cref="GlyphSize"/> automatically to the glyphs' size specified
        /// in the database.</para>
        /// 
        /// <para><note>If the property is set to <see langword="null"/>, the class will only do searching for glyphs - objects
        /// which look like a glyph and satisfy certain constaints applicable to a valid glyph. In this case it will
        /// provide information about all found glyphs - their position and raw data.</note></para>
        /// </remarks>
        /// 
        public GlyphDatabase GlyphDatabase
        {
            get { return glyphDatabase; }
            set
            {
                lock ( sync )
                {
                    glyphDatabase = null;

                    if ( value != null )
                    {
                        GlyphSize = value.Size;
                    }

                    glyphDatabase = value;
                }
            }
        }

        /// <summary>
        /// Maximum number of glyph to search for in a single image, [1, 10].
        /// </summary>
        /// 
        /// <remarks><para>The property sets maximum number of glyphs to search in a given image. If
        /// image processing routines finds the specified number of glyphs it will stop further image
        /// processing even if there are some more data to process is available.</para>
        /// 
        /// <para><note>The image processing routine analyzes found objects in size descending order -
        /// larger objects are analyzed first.</note></para>
        /// 
        /// <para>Default value is set to <b>3</b>.</para>
        /// </remarks>
        /// 
        public int MaxNumberOfGlyphsToSearch
        {
            get { return maxNumberOfGlyphsToSearch; }
            set { maxNumberOfGlyphsToSearch = Math.Max( 1, Math.Min( 10, value ) ); }
        }

        // Private constructor
        private GlyphRecognizer( )
        {
            blobCounter.MinHeight    = 32;
            blobCounter.MinWidth     = 32;
            blobCounter.FilterBlobs  = true;
            blobCounter.ObjectsOrder = ObjectsOrder.Size;

            quadrilateralTransformation.AutomaticSizeCalculaton = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlyphRecognizer"/> class.
        /// </summary>
        /// 
        /// <param name="glyphSize"><see cref="GlyphSize">Size</see> of glyphs to search for and recognize.</param>
        /// 
        public GlyphRecognizer( int glyphSize ) : this( )
        {
            GlyphSize = glyphSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlyphRecognizer"/> class.
        /// </summary>
        /// 
        /// <param name="glyphDatabase"><see cref="GlyphRecognizer">Database of glyphs</see> to recognize.</param>
        /// 
        public GlyphRecognizer( GlyphDatabase glyphDatabase ) : this( )
        {
            GlyphDatabase = glyphDatabase;
        }

        /// <summary>
        /// Search for glyphs in the specified image and recognize them.
        /// </summary>
        /// 
        /// <param name="image">Image to search glyphs in.</param>
        /// 
        /// <returns>Return a list of found glyphs.</returns>
        /// 
        /// <remarks><para>The method does processing of the specified image and searches for glyphs in it of
        /// the specified <see cref="GlyphSize">size</see>. In the case if <see cref="GlyphDatabase">glyphs' database</see>
        /// is set, it tries to find a matching glyph in it for each found glyph in the image. If matching is found,
        /// then <see cref="ExtractedGlyphData.RecognizedGlyph">RecognizedGlyph</see> and
        /// <see cref="ExtractedGlyphData.RecognizedQuadrilateral">RecognizedQuadrilateral</see>
        /// properties of <see cref="ExtractedGlyphData"/> are set correspondingly.</para></remarks>
        /// 
        /// <exception cref="UnsupportedImageFormatException">Pixel format of the specified image is not supported.
        /// It must be 8 bpp indexed or 24/32 bpp color image.</exception>
        ///
        public List<ExtractedGlyphData> FindGlyphs( Bitmap image )
        {
            BitmapData bitmapData = image.LockBits( new Rectangle( 0, 0, image.Width, image.Height ),
                ImageLockMode.ReadOnly, image.PixelFormat );

            try
            {
                return FindGlyphs( new UnmanagedImage( bitmapData ) );
            }
            finally
            {
                image.UnlockBits( bitmapData );
            }
        }

        /// <summary>
        /// Search for glyphs in the specified image and recognize them.
        /// </summary>
        /// 
        /// <param name="image">Image to search glyphs in.</param>
        /// 
        /// <returns>Return a list of found glyphs.</returns>
        /// 
        /// <remarks><para>The method does processing of the specified image and searches for glyphs in it of
        /// the specified <see cref="GlyphSize">size</see>. In the case if <see cref="GlyphDatabase">glyphs' database</see>
        /// is set, it tries to find a matching glyph in it for each found glyph in the image. If matching is found,
        /// then <see cref="ExtractedGlyphData.RecognizedGlyph">RecognizedGlyph</see> and
        /// <see cref="ExtractedGlyphData.RecognizedQuadrilateral">RecognizedQuadrilateral</see>
        /// properties of <see cref="ExtractedGlyphData"/> are set correspondingly.</para></remarks>
        /// 
        /// <exception cref="UnsupportedImageFormatException">Pixel format of the specified image is not supported.
        /// It must be 8 bpp indexed or 24/32 bpp color image.</exception>
        /// 
        public List<ExtractedGlyphData> FindGlyphs( UnmanagedImage image )
        {
            List<ExtractedGlyphData> extractedGlyphs = new List<ExtractedGlyphData>( );

            if ( ( image.PixelFormat != PixelFormat.Format8bppIndexed ) &&
                 ( !Grayscale.CommonAlgorithms.BT709.FormatTranslations.ContainsKey( image.PixelFormat ) ) )
            {
                throw new UnsupportedImageFormatException( "Pixel format of the specified image is not supported." );
            }

            // 1 - grayscaling
            UnmanagedImage grayImage = null;

            if ( image.PixelFormat == PixelFormat.Format8bppIndexed )
            {
                grayImage = image;
            }
            else
            {
                grayImage = UnmanagedImage.Create( image.Width, image.Height, PixelFormat.Format8bppIndexed );
                Grayscale.CommonAlgorithms.BT709.Apply( image, grayImage );
            }

            // 2 - Edge detection
            UnmanagedImage edgesImage = edgeDetector.Apply( grayImage );

            // 3 - Threshold edges
            thresholdFilter.ApplyInPlace( edgesImage );

            // 4 - Blob Counter
            blobCounter.ProcessImage( edgesImage );
            Blob[] blobs = blobCounter.GetObjectsInformation( );

            // 5 - check each blob
            for ( int i = 0, n = blobs.Length; i < n; i++ )
            {
                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints( blobs[i] );
                List<IntPoint> corners = null;

                // does it look like a quadrilateral ?
                if ( shapeChecker.IsQuadrilateral( edgePoints, out corners ) )
                {
                    // get edge points on the left and on the right side
                    List<IntPoint> leftEdgePoints, rightEdgePoints;
                    blobCounter.GetBlobsLeftAndRightEdges( blobs[i], out leftEdgePoints, out rightEdgePoints );

                    // calculate average difference between pixel values from outside of the shape and from inside
                    float diff = CalculateAverageEdgesBrightnessDifference(
                        leftEdgePoints, rightEdgePoints, grayImage );

                    // check average difference, which tells how much outside is lighter than inside on the average
                    if ( diff > 20 )
                    {
                        // perform glyph recognition
                        ExtractedGlyphData glyphData = RecognizeGlyph( grayImage, corners );

                        if ( glyphData != null )
                        {
                            extractedGlyphs.Add( glyphData );

                            if ( extractedGlyphs.Count >= maxNumberOfGlyphsToSearch )
                                break;
                        }
                    }
                }
            }

            // dispose resources
            if ( image.PixelFormat != PixelFormat.Format8bppIndexed )
            {
                grayImage.Dispose( );
            }
            edgesImage.Dispose( );


            return extractedGlyphs;
        }

        #region Helper methods
        // Try recognizing the glyph in the specified image defined by the specified quadrilateral
        private ExtractedGlyphData RecognizeGlyph( UnmanagedImage image, List<IntPoint> quadrilateral )
        {
            // extract glyph image
            quadrilateralTransformation.SourceQuadrilateral = quadrilateral;
            UnmanagedImage glyphImage = quadrilateralTransformation.Apply( image );

            // otsu thresholding
            otsuThresholdFilter.ApplyInPlace( glyphImage );

            // recognize raw glyph
            float confidence;

            byte[,] glyphValues = binaryGlyphRecognizer.Recognize( glyphImage,
                new Rectangle( 0, 0, glyphImage.Width, glyphImage.Height ), out confidence );

            if ( confidence >= minConfidenceLevel )
            {
                if ( ( Glyph.CheckIfGlyphHasBorder( glyphValues ) ) &&
                     ( Glyph.CheckIfEveryRowColumnHasValue( glyphValues ) ) ) 
                {
                    ExtractedGlyphData foundGlyph = new ExtractedGlyphData( quadrilateral, glyphValues, confidence );

                    if ( glyphDatabase != null )
                    {
                        int rotation;

                        foundGlyph.RecognizedGlyph = glyphDatabase.RecognizeGlyph( glyphValues, out rotation );

                        if ( rotation != -1 )
                        {
                            foundGlyph.RecognizedQuadrilateral = foundGlyph.Quadrilateral;

                            while ( rotation > 0 )
                            {
                                foundGlyph.RecognizedQuadrilateral.Add( foundGlyph.RecognizedQuadrilateral[0] );
                                foundGlyph.RecognizedQuadrilateral.RemoveAt( 0 );

                                rotation -= 90;
                            }
                        }
                    }

                    return foundGlyph;
                }
            }

            return null;
        }

        private const int stepSize = 3;

        // Calculate average brightness difference between pixels outside and inside of the object
        // bounded by specified left and right edge
        private float CalculateAverageEdgesBrightnessDifference( List<IntPoint> leftEdgePoints,
            List<IntPoint> rightEdgePoints, UnmanagedImage image )
        {
            // create list of points, which are a bit on the left/right from edges
            List<IntPoint> leftEdgePoints1  = new List<IntPoint>( );
            List<IntPoint> leftEdgePoints2  = new List<IntPoint>( );
            List<IntPoint> rightEdgePoints1 = new List<IntPoint>( );
            List<IntPoint> rightEdgePoints2 = new List<IntPoint>( );

            int tx1, tx2, ty;
            int widthM1 = image.Width - 1;

            for ( int k = 0; k < leftEdgePoints.Count; k++ )
            {
                tx1 = leftEdgePoints[k].X - stepSize;
                tx2 = leftEdgePoints[k].X + stepSize;
                ty = leftEdgePoints[k].Y;

                leftEdgePoints1.Add( new IntPoint( ( tx1 < 0 ) ? 0 : tx1, ty ) );
                leftEdgePoints2.Add( new IntPoint( ( tx2 > widthM1 ) ? widthM1 : tx2, ty ) );

                tx1 = rightEdgePoints[k].X - stepSize;
                tx2 = rightEdgePoints[k].X + stepSize;
                ty = rightEdgePoints[k].Y;

                rightEdgePoints1.Add( new IntPoint( ( tx1 < 0 ) ? 0 : tx1, ty ) );
                rightEdgePoints2.Add( new IntPoint( ( tx2 > widthM1 ) ? widthM1 : tx2, ty ) );
            }

            // collect pixel values from specified points
            byte[] leftValues1  = image.Collect8bppPixelValues( leftEdgePoints1 );
            byte[] leftValues2  = image.Collect8bppPixelValues( leftEdgePoints2 );
            byte[] rightValues1 = image.Collect8bppPixelValues( rightEdgePoints1 );
            byte[] rightValues2 = image.Collect8bppPixelValues( rightEdgePoints2 );

            // calculate average difference between pixel values from outside of the shape and from inside
            float diff = 0;
            int pixelCount = 0;
            
            for ( int k = 0; k < leftEdgePoints.Count; k++ )
            {
                if ( rightEdgePoints[k].X - leftEdgePoints[k].X > stepSize * 2 )
                {
                    diff += ( leftValues1[k]  - leftValues2[k] );
                    diff += ( rightValues2[k] - rightValues1[k] );
                    pixelCount += 2;
                }
            }

            return diff / pixelCount;
        }
        #endregion
    }
}
