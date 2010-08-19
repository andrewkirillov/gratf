using System;
using System.Drawing;
using System.Drawing.Imaging;

using AForge;
using AForge.Imaging;

namespace AForge.Vision.GlyphRecognition
{
    class SquareBinaryGlyphRecognizer
    {
        private int glyphSize = 5;

        public int GlyphSize
        {
            get { return glyphSize; }
            set { glyphSize = System.Math.Max( 5, System.Math.Min( 23, value ) ); }
        }

        public SquareBinaryGlyphRecognizer( )
        {
        }

        public SquareBinaryGlyphRecognizer( int glyphSize )
        {
            this.glyphSize = glyphSize;
        }

        // Recognize glyph in managed image
        public byte[,] Recognize( Bitmap image, Rectangle rect, out float confidence )
        {
            BitmapData data = image.LockBits( new Rectangle( 0, 0, image.Width, image.Height ),
                ImageLockMode.ReadWrite, image.PixelFormat );

            try
            {
                return Recognize( new UnmanagedImage( data ), rect, out confidence );
            }
            finally
            {
                image.UnlockBits( data );
            }
        }

        // Recognize glyph in locked bitmap data
        public byte[,] Recognize( BitmapData imageData, Rectangle rect, out float confidence )
        {
            return Recognize( new UnmanagedImage( imageData ), rect, out confidence );
        }

        // Recognize glyph in unmanaged image
        public byte[,] Recognize( UnmanagedImage image, Rectangle rect, out float confidence )
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
            byte[,] glyphValues = new byte[glyphSize, glyphSize];
            confidence = 1f;

            for ( int gi = 0; gi < glyphSize; gi++ )
            {
                for ( int gj = 0; gj < glyphSize; gj++ )
                {
                    float fullness = (float) ( cellIntensity[gi, gj] / 255 ) / cellScanArea;
                    float conf = (float) System.Math.Abs( fullness - 0.5 ) + 0.5f;

                    glyphValues[gi, gj] = (byte) ( ( fullness > 0.5f ) ? 1 : 0 );

                    if ( conf < confidence )
                        confidence = conf;
                }
            }

            return glyphValues;
        }
    }
}
