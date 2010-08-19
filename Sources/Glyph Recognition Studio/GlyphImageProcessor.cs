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
using AForge.Math.Geometry;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Vision.GlyphRecognition;

namespace GlyphRecognitionStudio
{
    class GlyphImageProcessor
    {
        private GlyphRecognizer recognizer = new GlyphRecognizer( 5 );
        private VisualizationType visualizationType = VisualizationType.Name;

        private Pen defaultPen = new Pen( Color.Red, 3 );
        private Font defaultFont = new Font( FontFamily.GenericSerif, 15, FontStyle.Bold );

        private BackwardQuadrilateralTransformation quadrilateralTransformation = new BackwardQuadrilateralTransformation( );

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

        public VisualizationType VisualizationType
        {
            get { return visualizationType; }
            set
            {
                lock ( this )
                {
                    visualizationType = value;
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
                    if ( ( visualizationType == VisualizationType.BorderOnly ) ||
                         ( visualizationType == VisualizationType.Name ) )
                    {
                        Graphics g = Graphics.FromImage( bitmap );

                        // highlight found each glyph
                        foreach ( ExtractedGlyphData glyphData in glyphs )
                        {
                            if ( ( glyphData.RecognizedGlyph == null ) || ( glyphData.RecognizedGlyph.UserData == null ) )
                            {
                                // highlight with default pen
                                g.DrawPolygon( defaultPen, ToPointsArray( glyphData.Quadrilateral ) );
                            }
                            else
                            {
                                GlyphVisualizationData visualization =
                                    (GlyphVisualizationData) glyphData.RecognizedGlyph.UserData;

                                Pen pen = new Pen( visualization.Color, 3 );

                                g.DrawPolygon( pen, ToPointsArray( glyphData.Quadrilateral ) );

                                if ( visualizationType == VisualizationType.Name )
                                {
                                    IntPoint minXY, maxXY;
                                    PointsCloud.GetBoundingRectangle( glyphData.Quadrilateral, out minXY, out maxXY );

                                    IntPoint center = ( minXY + maxXY ) / 2;

                                    SizeF nameSize = g.MeasureString( glyphData.RecognizedGlyph.Name, defaultFont );

                                    Brush brush = new SolidBrush( visualization.Color );


                                    g.DrawString( glyphData.RecognizedGlyph.Name, defaultFont, brush,
                                        new Point( center.X - (int) nameSize.Width / 2, center.Y - (int) nameSize.Height / 2 ) );

                                    brush.Dispose( );
                                }

                                pen.Dispose( );
                            }
                        }
                    }
                    else
                    {
                        BitmapData bitmapData = bitmap.LockBits( new Rectangle( 0, 0, bitmap.Width, bitmap.Height ),
                            ImageLockMode.ReadWrite, bitmap.PixelFormat );
                        UnmanagedImage unmanagedImage = new UnmanagedImage( bitmapData );

                        // highlight found each glyph
                        foreach ( ExtractedGlyphData glyphData in glyphs )
                        {
                            if ( ( glyphData.RecognizedGlyph != null ) && ( glyphData.RecognizedGlyph.UserData != null ) )
                            {
                                GlyphVisualizationData visualization =
                                    (GlyphVisualizationData) glyphData.RecognizedGlyph.UserData;

                                if ( visualization.ImageName != null )
                                {
                                    Bitmap glyphImage = EmbeddedImageCollection.Instance.GetImage( visualization.ImageName );

                                    if ( glyphImage != null )
                                    {
                                        quadrilateralTransformation.SourceImage = glyphImage;
                                        quadrilateralTransformation.DestinationQuadrilateral = glyphData.RecognizedQuadrilateral;

                                        quadrilateralTransformation.ApplyInPlace( unmanagedImage );
                                    }
                                }
                            }
                        }

                        bitmap.UnlockBits( bitmapData );
                    }
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
