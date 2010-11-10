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
        // glyph recognizer to use for glyph recognition in video
        private GlyphRecognizer recognizer = new GlyphRecognizer( 5 );

        // quadrilateral transformation used to put image in place of glyph
        private BackwardQuadrilateralTransformation quadrilateralTransformation = new BackwardQuadrilateralTransformation( );

        // default font to highlight glyphs
        private Font defaultFont = new Font( FontFamily.GenericSerif, 15, FontStyle.Bold );

        // Database of glyphs to recognize
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

        // Glyphs' visualization type
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
        private VisualizationType visualizationType = VisualizationType.Name;

        // Process image searching for glyphs and highlighting them
        public void ProcessImage( Bitmap bitmap )
        {
            lock ( this )
            {
                // get list of recognized glyphs
                List<ExtractedGlyphData> glyphs = recognizer.FindGlyphs( bitmap );

                if ( glyphs.Count > 0 )
                {
                    if ( ( visualizationType == VisualizationType.BorderOnly ) ||
                         ( visualizationType == VisualizationType.Name ) )
                    {
                        Graphics g = Graphics.FromImage( bitmap );

                        // highlight each found glyph
                        foreach ( ExtractedGlyphData glyphData in glyphs )
                        {
                            Pen pen = new Pen( ( ( glyphData.RecognizedGlyph == null ) || ( glyphData.RecognizedGlyph.UserData == null ) ) ?
                                Color.Red : ( (GlyphVisualizationData) glyphData.RecognizedGlyph.UserData).Color, 3 );

                            // highlight border
                            g.DrawPolygon( pen, ToPointsArray( glyphData.Quadrilateral ) );

                            // show glyph's name
                            if ( ( visualizationType == VisualizationType.Name ) && (  glyphData.RecognizedGlyph != null ) )
                            {
                                // get glyph's center point
                                IntPoint minXY, maxXY;
                                PointsCloud.GetBoundingRectangle( glyphData.Quadrilateral, out minXY, out maxXY );
                                IntPoint center = ( minXY + maxXY ) / 2;

                                // glyph's name size
                                SizeF nameSize = g.MeasureString( glyphData.RecognizedGlyph.Name, defaultFont );

                                // paint the name
                                Brush brush = new SolidBrush( pen.Color);

                                g.DrawString( glyphData.RecognizedGlyph.Name, defaultFont, brush,
                                    new Point( center.X - (int) nameSize.Width / 2, center.Y - (int) nameSize.Height / 2 ) );

                                brush.Dispose( );
                            }

                            pen.Dispose( );
                        }
                    }
                    else
                    {
                        // lock image for further processing
                        BitmapData bitmapData = bitmap.LockBits( new Rectangle( 0, 0, bitmap.Width, bitmap.Height ),
                            ImageLockMode.ReadWrite, bitmap.PixelFormat );
                        UnmanagedImage unmanagedImage = new UnmanagedImage( bitmapData );

                        // highlight each found glyph
                        foreach ( ExtractedGlyphData glyphData in glyphs )
                        {
                            if ( ( glyphData.RecognizedGlyph != null ) && ( glyphData.RecognizedGlyph.UserData != null ) )
                            {
                                GlyphVisualizationData visualization =
                                    (GlyphVisualizationData) glyphData.RecognizedGlyph.UserData;

                                if ( visualization.ImageName != null )
                                {
                                    // get image associated with the glyph
                                    Bitmap glyphImage = EmbeddedImageCollection.Instance.GetImage( visualization.ImageName );

                                    if ( glyphImage != null )
                                    {
                                        // put glyph's image onto the glyph using quadrilateral transformation
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

        #region Helper methods
        // Convert list of AForge.NET framework's points to array of .NET's points
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
        #endregion
    }
}
