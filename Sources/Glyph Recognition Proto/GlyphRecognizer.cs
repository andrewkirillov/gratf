// Glyph Recognition Prototyping
//
// Copyright © Andrew Kirillov, 2009-2010
// andrew.kirillov@aforgenet.com
//

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

using AForge;
using AForge.Math;
using AForge.Math.Geometry;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Imaging.IPPrototyper;

namespace GlyphRecognitionProto
{
    public class GlyphRecognizer : IImageProcessingRoutine
    {
        // Image processing routine's name
        public string Name
        {
            get { return "Glyph Recognizer"; }
        }

        // Process specified image trying to recognize counter's image
        public void Process( Bitmap image, IImageProcessingLog log )
        {
            log.AddMessage( "Image size: " + image.Width + " x " + image.Height );

            // 1 - Grayscale
            Bitmap grayImage = Grayscale.CommonAlgorithms.BT709.Apply( image );
            log.AddImage( "Grayscale", grayImage );

            // 2 - Edge detection
            DifferenceEdgeDetector edgeDetector = new DifferenceEdgeDetector( ); 
            Bitmap edges = edgeDetector.Apply( grayImage );
            log.AddImage( "Edges", edges );

            // 3 - Threshold edges
            Threshold thresholdFilter = new Threshold( 40 ); 
            thresholdFilter.ApplyInPlace( edges );
            log.AddImage( "Thresholded Edges", edges );

            // 4 - Blob Counter
            BlobCounter blobCounter = new BlobCounter( );
            blobCounter.MinHeight = 32;
            blobCounter.MinWidth  = 32;
            blobCounter.FilterBlobs  = true;
            blobCounter.ObjectsOrder = ObjectsOrder.Size;

            blobCounter.ProcessImage( edges );
            Blob[] blobs = blobCounter.GetObjectsInformation( );

            // create copy of source image, so we could draw on it
            Bitmap imageCopy = AForge.Imaging.Image.Clone( image );

            BitmapData imageData = imageCopy.LockBits( new Rectangle( 0, 0, image.Width, image.Height ),
                ImageLockMode.ReadWrite, imageCopy.PixelFormat );

            // lock grayscale image, so we could access it's pixel values
            BitmapData grayData = grayImage.LockBits( new Rectangle( 0, 0, image.Width, image.Height ),
                ImageLockMode.ReadOnly, grayImage.PixelFormat );
            UnmanagedImage grayUI = new UnmanagedImage( grayData );

            // list of found dark/black quadrilaterals surrounded by white area
            List<List<IntPoint>> foundObjects = new List<List<IntPoint>>( );
            // shape checker for checking quadrilaterals
            SimpleShapeChecker shapeChecker = new SimpleShapeChecker( );

            // 5 - check each blob
            for ( int i = 0, n = blobs.Length; i < n; i++ )
            {
                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints( blobs[i] );
                List<IntPoint> corners = null;

                // does it look like a quadrilateral ?
                if ( shapeChecker.IsQuadrilateral( edgePoints, out corners ) )
                {
                    // do some more checks to filter so unacceptable shapes
                    // if ( CheckIfShapeIsAcceptable( corners ) )
                    {
                        log.AddMessage( "Blob size: " + blobs[i].Rectangle.Width + " x " + blobs[i].Rectangle.Height );

                        // get edge points on the left and on the right side
                        List<IntPoint> leftEdgePoints, rightEdgePoints;
                        blobCounter.GetBlobsLeftAndRightEdges( blobs[i], out leftEdgePoints, out rightEdgePoints );

                        // calculate average difference between pixel values from outside of the shape and from inside
                        float diff = CalculateAverageEdgesBrightnessDifference(
                            leftEdgePoints, rightEdgePoints, grayUI );

                        log.AddMessage( "Avg Diff: " + diff );

                        // check average difference, which tells how much outside is lighter than inside on the average
                        if ( diff > 20 )
                        {
                            Drawing.Polygon( imageData, corners, Color.Red );
                            // add the object to the list of interesting objects for further processing
                            foundObjects.Add( corners );
                        }
                    }
                }
            }

            imageCopy.UnlockBits( imageData );
            grayImage.UnlockBits( grayData );

            log.AddImage( "Potential glyps", imageCopy );

            int counter = 1;

            // further processing of each potential glyph
            foreach ( List<IntPoint> corners in foundObjects )
            {
                log.AddMessage( "Glyph #" + counter );
                
                log.AddMessage( string.Format( "Corners: ({0}), ({1}), ({2}), ({3})",
                    corners[0], corners[1], corners[2], corners[3] ) );

                // 6 - do quadrilateral transformation
                QuadrilateralTransformation quadrilateralTransformation =
                    new QuadrilateralTransformation( corners, 250, 250 );

                Bitmap transformed = quadrilateralTransformation.Apply( grayImage );

                log.AddImage( "Transformed #" + counter, transformed );

                // 7 - otsu thresholding
                OtsuThreshold otsuThresholdFilter = new OtsuThreshold( ); 
                Bitmap transformedOtsu = otsuThresholdFilter.Apply( transformed );
                log.AddImage( "Transformed Otsu #" + counter, transformedOtsu );

                int glyphSize = 5;
                SquareBinaryGlyphRecognizer gr = new SquareBinaryGlyphRecognizer( glyphSize );

                bool[,] glyphValues = gr.Recognize( transformedOtsu,
                    new Rectangle( 0, 0, 250, 250 ) );

                log.AddImage( "Glyph lines #" + counter, transformedOtsu );

                // output recognize glyph to log
                log.AddMessage( string.Format( "glyph: {0:F2}%", gr.confidence * 100 ) );
                for ( int i = 0; i < glyphSize; i++ )
                {
                    StringBuilder sb = new StringBuilder( "   " );

                    for ( int j = 0; j < glyphSize; j++ )
                    {
                        sb.Append( ( glyphValues[i, j] ) ? "1 " : "0 " );
                    }

                    log.AddMessage( sb.ToString( ) );
                }

                counter++;
            }
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
                if ( ( Math.Abs( angle1 - 90 ) <= angleError2 ) &&
                     ( Math.Abs( angle2 - 90 ) <= angleError2 ) &&
                     ( Math.Abs( angle3 - 90 ) <= angleError2 ) &&
                     ( Math.Abs( angle4 - 90 ) <= angleError2 ) )
                {
                    // get length of all sides
                    float side1Length = (float) corners[0].DistanceTo( corners[1] );
                    float side2Length = (float) corners[1].DistanceTo( corners[2] );
                    float side3Length = (float) corners[2].DistanceTo( corners[3] );
                    float side4Length = (float) corners[3].DistanceTo( corners[0] );

                    float max = Math.Max( Math.Max( side1Length, side2Length ), Math.Max( side3Length, side4Length ) );
                    float min = Math.Min( Math.Min( side1Length, side2Length ), Math.Min( side3Length, side4Length ) );

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
