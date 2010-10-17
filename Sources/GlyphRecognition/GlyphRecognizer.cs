using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;

namespace AForge.Vision.GlyphRecognition
{
    public class GlyphRecognizer
    {
        private int glyphSize;
        private GlyphDatabase glyphDatabase;

        private DifferenceEdgeDetector edgeDetector = new DifferenceEdgeDetector( );
        private Threshold thresholdFilter = new Threshold( 40 );
        private BlobCounter blobCounter = new BlobCounter( );
        private SimpleShapeChecker shapeChecker = new SimpleShapeChecker( );
        private QuadrilateralTransformation quadrilateralTransformation = new QuadrilateralTransformation( );
        private OtsuThreshold otsuThresholdFilter = new OtsuThreshold( );
        private SquareBinaryGlyphRecognizer binaryGlyphRecognizer = new SquareBinaryGlyphRecognizer( );

        private float minConfidenceLevel = 0.60f;
        private int maxNumberOfGlyphsToSearch = 3;

        public int GlyphSize
        {
            get { return glyphSize; }
            set
            {
                if ( glyphDatabase != null )
                {
                    throw new ApplicationException( "Glyph size cannot be set if glyph database is set" );
                }

                if ( ( value < 3 ) || ( value > 23 ) )
                {
                    throw new ArgumentException( "Invalid glyph size was specified" );
                }

                lock ( this )
                {
                    glyphSize = value;
                    quadrilateralTransformation.NewWidth = quadrilateralTransformation.NewHeight = glyphSize * 20;
                    binaryGlyphRecognizer.GlyphSize = glyphSize;
                }
            }
        }

        public GlyphDatabase GlyphDatabase
        {
            get { return glyphDatabase; }
            set
            {
                lock ( this )
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

        private GlyphRecognizer( )
        {
            blobCounter.MinHeight    = 32;
            blobCounter.MinWidth     = 32;
            blobCounter.FilterBlobs  = true;
            blobCounter.ObjectsOrder = ObjectsOrder.Size;

            quadrilateralTransformation.AutomaticSizeCalculaton = false;
        }

        public GlyphRecognizer( int glyphSize ) : this( )
        {
            GlyphSize = glyphSize;
        }

        public GlyphRecognizer( GlyphDatabase glyphDatabase ) : this( )
        {
            GlyphDatabase = glyphDatabase;
        }

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

        public List<ExtractedGlyphData> FindGlyphs( UnmanagedImage image )
        {
            List<ExtractedGlyphData> extractedGlyphs = new List<ExtractedGlyphData>( );

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
                if ( ( CheckIfGlyphHasBorder( glyphValues ) ) &&
                     ( CheckIfEveryRowColumnHasValue( glyphValues ) ) ) 
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

                                /*foundGlyph.RecognizedQuadrilateral.Insert( 0, foundGlyph.RecognizedQuadrilateral[3] );
                                foundGlyph.RecognizedQuadrilateral.RemoveAt( 4 );*/


                                rotation -= 90;
                            }
                        }
                    }

                    return foundGlyph;
                }
            }

            return null;
        }

        private bool CheckIfGlyphHasBorder( byte[,] rawGlyphData )
        {
            int sizeM1 = rawGlyphData.GetLength( 0 ) - 1;

            for ( int i = 0; i <= sizeM1; i++ )
            {
                if ( rawGlyphData[0, i] == 1 )
                    return false;
                if ( rawGlyphData[sizeM1, i] == 1 )
                    return false;

                if ( rawGlyphData[i, 0] == 1)
                    return false;
                if ( rawGlyphData[i, sizeM1] == 1 )
                    return false;
            }

            return true;
        }

        private bool CheckIfEveryRowColumnHasValue( byte[,] rawGlyphData )
        {
            int sizeM1 = rawGlyphData.GetLength( 0 ) - 1;
            byte[] rows = new byte[sizeM1];
            byte[] cols = new byte[sizeM1];

            for ( int i = 1; i < sizeM1; i++ )
            {
                for ( int j = 1; j < sizeM1; j++ )
                {
                    byte value = rawGlyphData[i, j];

                    rows[i] |= value;
                    cols[j] |= value;
                }
            }

            for ( int i = 1; i < sizeM1; i++ )
            {
                if ( ( rows[i] == 0 ) || ( cols[i] == 0 ) )
                    return false;
            }

            return true;
        }

        



        private const double angleError1 = 45;
        private const double angleError2 = 75;
        private const double lengthError = 0.75;

        // Check if quadrilateral's shape is acceptable for further analysis
        private bool CheckIfShapeIsAcceptable( List<IntPoint> corners )
        {
            // get angles between 2 pairs of opposite sides
            double angleBetween1stPair = GeometryTools.GetAngleBetweenLines( corners[0], corners[1], corners[2], corners[3] );
            double angleBetween2ndPair = GeometryTools.GetAngleBetweenLines( corners[1], corners[2], corners[3], corners[0] );

            // check that angle between opposite side is not too big
            if ( ( angleBetween1stPair <= angleError1 ) && ( angleBetween2ndPair <= angleError1 ) )
            {
                double angle1 = GeometryTools.GetAngleBetweenVectors( corners[1], corners[0], corners[2] );
                double angle2 = GeometryTools.GetAngleBetweenVectors( corners[2], corners[1], corners[3] );
                double angle3 = GeometryTools.GetAngleBetweenVectors( corners[3], corners[2], corners[0] );
                double angle4 = GeometryTools.GetAngleBetweenVectors( corners[0], corners[3], corners[1] );

                // check that angle between adjacent sides is not very small or too flat
                if ( ( System.Math.Abs( angle1 - 90 ) <= angleError2 ) &&
                     ( System.Math.Abs( angle2 - 90 ) <= angleError2 ) &&
                     ( System.Math.Abs( angle3 - 90 ) <= angleError2 ) &&
                     ( System.Math.Abs( angle4 - 90 ) <= angleError2 ) )
                {
                    // get length of all sides
                    float side1Length = (float) corners[0].DistanceTo( corners[1] );
                    float side2Length = (float) corners[1].DistanceTo( corners[2] );
                    float side3Length = (float) corners[2].DistanceTo( corners[3] );
                    float side4Length = (float) corners[3].DistanceTo( corners[0] );

                    float max = System.Math.Max( System.Math.Max( side1Length, side2Length ), System.Math.Max( side3Length, side4Length ) );
                    float min = System.Math.Min( System.Math.Min( side1Length, side2Length ), System.Math.Min( side3Length, side4Length ) );

                    // check that shortest side is not too small compared to the longest side
                    if ( min >= max * ( 1 - lengthError ) )
                        return true;
                }
            }

            return false;
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
    }
}
