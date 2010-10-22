// Gliph Recognition Library
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010
// andrew.kirillov@aforgenet.com
//

namespace AForge.Vision.GlyphRecognition
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    using AForge;
    using AForge.Imaging;

    /// <summary>
    /// Recognition of square glyphs in binary images.
    /// </summary>
    /// 
    /// <remarks><para>The class performs processing of binary images trying to recognize square glyph
    /// of the specified <see cref="GlyphSize">size</see>.</para></remarks>
    /// 
    public class SquareBinaryGlyphRecognizer
    {
        private int glyphSize = 5;

        /// <summary>
        /// Glyph size to recognize, [5, 23].
        /// </summary>
        ///
        /// <remarks><para>The property sets glyphs' size to recognize in given images.</para>
        /// 
        /// <para>Default value is set to <b>5</b>.</para>
        /// </remarks>
        ///
        public int GlyphSize
        {
            get { return glyphSize; }
            set { glyphSize = System.Math.Max( 5, System.Math.Min( 23, value ) ); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SquareBinaryGlyphRecognizer"/> class.
        /// </summary>
        /// 
        public SquareBinaryGlyphRecognizer( )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SquareBinaryGlyphRecognizer"/> class.
        /// </summary>
        /// 
        /// <param name="glyphSize">Glyph <see cref="GlyphSize">size</see> to recognize.</param>
        /// 
        public SquareBinaryGlyphRecognizer( int glyphSize )
        {
            GlyphSize = glyphSize;
        }

        /// <summary>
        /// Recognize glyph in the specified image.
        /// </summary>
        /// 
        /// <param name="image">Image to recognize glyph in.</param>
        /// <param name="rect">Rectangle of the glyph to recognize.</param>
        /// <param name="confidence">Gets recognition confidence on return.</param>
        /// 
        /// <returns>Returns recognized glyph's data - 2D array of <see cref="GlyphSize"/>x<see cref="GlyphSize"/>.</returns>
        /// 
        /// <remarks><para>Performs processing of the specified image recognizing a glyph in it and providing confidence
        /// level of recognition. If the confidence level equals to 1.0, then this routine is 100% confident about the recognized glyph.
        /// If the confidence level is getting closer to 0.5, then it means that some glyph's values are not reliable
        /// enough – kind of 50/50: certain glyph’s rectangle may not be white enough or may not be black enough.</para>
        /// </remarks>
        /// 
        /// <exception cref="UnsupportedImageFormatException">Pixel format of the specified image is not supported. Must be 8 bpp indexed image.</exception>
        /// 
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

        /// <summary>
        /// Recognize glyph in the specified image.
        /// </summary>
        /// 
        /// <param name="image">Image to recognize glyph in.</param>
        /// <param name="rect">Rectangle of the glyph to recognize.</param>
        /// <param name="confidence">Gets recognition confidence on return.</param>
        /// 
        /// <returns>Returns recognized glyph's data - 2D array of <see cref="GlyphSize"/>x<see cref="GlyphSize"/>.</returns>
        /// 
        /// <remarks><para>Performs processing of the specified image recognizing a glyph in it and providing confidence
        /// level of recognition. If the confidence level equals to 1.0, then this routine is 100% confident about the recognized glyph.
        /// If the confidence level is getting closer to 0.5, then it means that some glyph's values are not reliable
        /// enough – kind of 50/50: certain glyph’s rectangle may not be white enough or may not be black enough.</para>
        /// </remarks>
        /// 
        /// <exception cref="UnsupportedImageFormatException">Pixel format of the specified image is not supported. Must be 8 bpp indexed image.</exception>
        /// 
        public byte[,] Recognize( UnmanagedImage image, Rectangle rect, out float confidence )
        {
            if ( image.PixelFormat != PixelFormat.Format8bppIndexed )
            {
                throw new UnsupportedImageFormatException( "Pixel format of the specified image is not supported." );
            }

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
