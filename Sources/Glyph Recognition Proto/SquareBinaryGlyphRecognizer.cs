// Glyph Recognition Prototyping
//
// Copyright © Andrew Kirillov, 2009-2010
// andrew.kirillov@aforgenet.com
//

using System;
using System.Drawing;
using System.Drawing.Imaging;

using AForge;
using AForge.Imaging;

namespace GlyphRecognitionProto
{
    class SquareBinaryGlyphRecognizer
    {
        private int glyphSize;
        public double confidence;

        // Glyph recognition confidence
        public double Confidence
        {
            get { return confidence; }
        }

        public SquareBinaryGlyphRecognizer( int glyphSize )
        {
            this.glyphSize = glyphSize;
        }

        // Recognize glyph in managed image
        public bool[,] Recognize( Bitmap image, Rectangle rect )
        {
            bool[,] glyph;

            BitmapData data = image.LockBits( new Rectangle( 0, 0, image.Width, image.Height ),
                ImageLockMode.ReadWrite, image.PixelFormat );

            try
            {
                glyph = Recognize( new UnmanagedImage( data ), rect );
            }
            finally
            {
                image.UnlockBits( data );
            }

            return glyph;
        }

        // Recognize glyph in locked bitmap data
        public bool[,] Recognize( BitmapData imageData, Rectangle rect )
        {
            return Recognize( new UnmanagedImage( imageData ), rect );
        }

        // Recognize glyph in unmanaged image
        public bool[,] Recognize( UnmanagedImage image, Rectangle rect )
        {
            int glyphStartX = rect.Left;
            int glyphStartY = rect.Top;

            int glyphWidth  = rect.Width;
            int glyphHeight = rect.Height;

            // glyph's cell size
            int cellWidth  = glyphWidth  / glyphSize;
            int cellHeight = glyphHeight / glyphSize;

            // allow some gap for each cell, which is not scanned
            int cellOffsetX = (int) ( cellWidth  * 0.2 );
            int cellOffsetY = (int) ( cellHeight * 0.2 );

            // cell's scan size
            int cellScanX = (int) ( cellWidth  * 0.6 );
            int cellScanY = (int) ( cellHeight * 0.6 );
            int cellScanArea = cellScanX * cellScanY;

            // summary intensity for each glyph's cell
            int[,] cellIntensity = new int[glyphSize, glyphSize];

            unsafe
            {
                int stride = image.Stride;

                byte* srcBase = (byte*) image.ImageData.ToPointer( ) +
                    ( glyphStartY + cellOffsetY ) * stride + glyphStartX + cellOffsetX;
                byte* srcLine;
                byte* src;

                // for all glyph's rows
                for ( int gi = 0; gi < glyphSize; gi++ )
                {
                    srcLine = srcBase + cellHeight * gi * stride;

                    // for all lines in the row
                    for ( int y = 0; y < cellScanY; y++ )
                    {

                        // for all glyph columns
                        for ( int gj = 0; gj < glyphSize; gj++ )
                        {
                            src = srcLine + cellWidth * gj;

                            // for all pixels in the column
                            for ( int x = 0; x < cellScanX; x++, src++ )
                            {
                                cellIntensity[gi, gj] += *src;
                            }
                        }

                        srcLine += stride;
                    }
                }
            }

            // calculate value of each glyph's cell and set confidence to minim value
            bool[,] glyphValues = new bool[glyphSize, glyphSize];
            confidence = 1.0;

            for ( int gi = 0; gi < glyphSize; gi++ )
            {
                for ( int gj = 0; gj < glyphSize; gj++ )
                {
                    double fullness = (double) ( cellIntensity[gi, gj] / 255 ) / cellScanArea;
                    double conf = Math.Abs( fullness - 0.5 ) + 0.5;

                    glyphValues[gi, gj] = ( fullness > 0.5 );

                    if ( conf < confidence )
                        confidence = conf;
                }
            }

            // (for debugging) draw lines showing cells' gaps and scan area
            for ( int gi = 0; gi < glyphSize; gi++ )
            {
                Drawing.Line( image,
                    new IntPoint( glyphStartX, glyphStartY + cellHeight * gi ),
                    new IntPoint( glyphStartX + glyphWidth, glyphStartY + cellHeight * gi ),
                    Color.FromArgb( 128, 128, 128 ) );

                Drawing.Line( image,
                    new IntPoint( glyphStartX, glyphStartY + cellHeight * gi + cellOffsetY ),
                    new IntPoint( glyphStartX + glyphWidth, glyphStartY + cellHeight * gi + cellOffsetY ),
                    Color.FromArgb( 192, 192, 192) );

                Drawing.Line( image,
                    new IntPoint( glyphStartX, glyphStartY + cellHeight * gi + cellOffsetY + cellScanY ),
                    new IntPoint( glyphStartX + glyphWidth, glyphStartY + cellHeight * gi + cellOffsetY + cellScanY ),
                    Color.FromArgb( 192, 192, 192 ) );

            }

            for ( int gj = 0; gj < glyphSize; gj++ )
            {
                Drawing.Line( image,
                    new IntPoint( glyphStartX + cellWidth * gj, glyphStartY ),
                    new IntPoint( glyphStartX + cellWidth * gj, glyphStartY + glyphHeight ),
                    Color.FromArgb( 128, 128, 128 ) );

                Drawing.Line( image,
                    new IntPoint( glyphStartX + cellWidth * gj + cellOffsetX, glyphStartY ),
                    new IntPoint( glyphStartX + cellWidth * gj + cellOffsetX, glyphStartY + glyphHeight ),
                    Color.FromArgb( 192, 192, 192 ) );

                Drawing.Line( image,
                    new IntPoint( glyphStartX + cellWidth * gj + cellOffsetX + cellScanX, glyphStartY ),
                    new IntPoint( glyphStartX + cellWidth * gj + cellOffsetX + cellScanX, glyphStartY + glyphHeight ),
                    Color.FromArgb( 192, 192, 192 ) );
            }

            return glyphValues;
        }
    }
}
