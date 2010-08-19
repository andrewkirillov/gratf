using System;
using System.Drawing;

namespace GlyphRecognitionStudio
{
    struct GlyphVisualizationData
    {
        public Color Color;
        public string ImageName;

        public GlyphVisualizationData( Color color )
        {
            Color = color;
            ImageName = null;
        }
    }
}
