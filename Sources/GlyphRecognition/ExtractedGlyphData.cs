using System;
using System.Collections.Generic;
using System.Text;

using AForge;

namespace AForge.Vision.GlyphRecognition
{
    public class ExtractedGlyphData
    {
        public readonly List<IntPoint> Quadrilateral;
        public readonly byte[,] RawData;
        public readonly float Confidence;

        public Glyph RecognizedGlyph;

        public ExtractedGlyphData( List<IntPoint> quadrilateral, byte[,] rawData, float confidence )
        {
            Quadrilateral = quadrilateral;
            RawData = rawData;
            Confidence = confidence;
        }

    }
}
