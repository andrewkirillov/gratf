using System;
using System.Collections.Generic;
using System.Text;

using AForge;

namespace AForge.Vision.GlyphRecognition
{
    public class ExtractedGlyphData
    {
        public readonly List<IntPoint> Quadrilateral;
        public readonly bool[,] RawData;
        public readonly float Confidence;

        public ExtractedGlyphData( List<IntPoint> quadrilateral, bool[,] rawData, float confidence )
        {
            Quadrilateral = quadrilateral;
            RawData = rawData;
            Confidence = confidence;
        }

    }
}
