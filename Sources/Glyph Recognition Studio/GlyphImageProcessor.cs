// Glyph Recognition Studio
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010
// andrew.kirillov@aforgenet.com
//

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

using AForge;
using AForge.Vision.GlyphRecognition;

namespace GlyphRecognitionStudio
{
    class GlyphImageProcessor
    {
        private GlyphRecognizer recognizer = new GlyphRecognizer( 5 );

        public GlyphDatabase GlyphDatabase
        {
            get { return recognizer.GlyphDatabase; }
            set
            {
                lock ( this )
                {
                    recognizer.GlyphDatabase = value;
                }
            }
        }

        public void ProcessImage( Bitmap bitmap )
        {
            lock ( this )
            {
                List<ExtractedGlyphData> glyphs = recognizer.FindGlyphs( bitmap );

                if ( glyphs.Count > 0 )
                {
                    Graphics g = Graphics.FromImage( bitmap );

                    using ( Pen pen = new Pen( Color.Red, 3 ) )
                    {
                        using ( Font font = new Font( FontFamily.GenericSerif, 20 ) )
                        {
                            using ( Brush brush = new SolidBrush( Color.Red ) )
                            {
                                foreach ( ExtractedGlyphData glyphData in glyphs )
                                {
                                    g.DrawPolygon( pen, ToPointsArray( glyphData.Quadrilateral ) );

                                    if ( glyphData.RecognizedGlyph != null )
                                    {
                                        g.DrawString( glyphData.RecognizedGlyph.Name, font, brush,
                                            new Point( glyphData.Quadrilateral[0].X, glyphData.Quadrilateral[0].Y ) );
                                    }
                                }
                            }
                        }
                    }
                    g.Dispose( );
                }
            }
        }

        private Point[] ToPointsArray( List<IntPoint> points )
        {
            int count = points.Count;
            Point[] pointsArray = new Point[count];

            for ( int i = 0; i < count; i++ )
            {
                pointsArray[i] = new Point( points[i].X, points[i].Y );
            }

            return pointsArray;
        }
    }
}
