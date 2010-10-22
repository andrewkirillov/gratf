// Gliph Recognition Library
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010
// andrew.kirillov@aforgenet.com
//

namespace AForge.Vision.GlyphRecognition
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using AForge;

    /// <summary>
    /// Information about the glyph extracted from an image using <see cref="GlyphRecognizer"/>.
    /// </summary>
    public class ExtractedGlyphData
    {
        /// <summary>
        /// Quadrilateral of the raw glyph detected (see <see cref="RawData"/>). First point
        /// of this quadrilateral corresponds to upper-left point of the raw glyph data.
        /// </summary>
        public readonly List<IntPoint> Quadrilateral;

        /// <summary>
        /// Raw glyph data extacted from processed image.
        /// </summary>
        public readonly byte[,] RawData;

        /// <summary>
        /// Confidence level of <see cref="RawData"/> recognition, [0.5, 0.1].
        /// </summary>
        ///
        /// <remarks><para>The confidence level is a reflection of how <see cref="RawData"/> property
        /// is reliable. If it equals to 1.0, then <see cref="GlyphRecognizer"/>
        /// (and <see cref="SquareBinaryGlyphRecognizer"/>) is 100% sure about the glyph data found.
        /// But if it getting closer to 0.5, then recognizer is uncertain about one or more values of the
        /// raw glyph's data, which affect uncertainty level of the entire glyph.</para></remarks>
        ///
        public readonly float Confidence;

        /// <summary>
        /// Recognized glyph from a <see cref="GlyphDatabase"/>.
        /// </summary>
        ///
        /// <remarks><para>This property is set by <see cref="GlyphRecognizer"/> in the case if
        /// <see cref="RawData"/> matches (see <see cref="Glyph.CheckForMatching"/>) to any of the glyphs
        /// in the specified glyphs' database (see <see cref="GlyphRecognizer.GlyphDatabase"/>. If a match is found
        /// then this property is set to the matching glyph. Otherwise it is set to <see langword="null"/>.
        /// </para></remarks>
        ///
        public Glyph RecognizedGlyph
        {
            get { return recognizedGlyph; }
            internal set { recognizedGlyph = value; }
        }
        private Glyph recognizedGlyph;

        /// <summary>
        /// Quadrilateral area corresponding to the <see cref="RecognizedGlyph"/>.
        /// </summary>
        /// 
        /// <remarks><para>First point of this quadrilateral corresponds to upper-left point of the
        /// recognized glyph, not the raw extracted glyph. This property may not be equal to <see cref="Quadrilateral"/>
        /// since the raw glyph data may represent rotation of the glyph registered in glyphs' database.</para>
        /// 
        /// <para>This property is really important for applications like augmented reality, where it is required to know
        /// coordinates of points corresponding to each corner of the recognized glyph.</para>
        /// 
        /// <para>This property is always set together with <see cref="RecognizedGlyph"/> on successful glyph matching. Otherwise
        /// it is set to <see langword="null"/>.</para>
        /// </remarks>
        /// 
        public List<IntPoint> RecognizedQuadrilateral
        {
            get { return recognizedQuadrilateral; }
            internal set { recognizedQuadrilateral = value; }
        }
        private List<IntPoint> recognizedQuadrilateral;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtractedGlyphData"/> class.
        /// </summary>
        /// 
        /// <param name="quadrilateral">Quadrilateral of the raw glyph detected.</param>
        /// <param name="rawData">Raw glyph data extacted from processed image.</param>
        /// <param name="confidence">Confidence level of <paramref name="rawData"/> recognition.</param>
        /// 
        public ExtractedGlyphData( List<IntPoint> quadrilateral, byte[,] rawData, float confidence )
        {
            Quadrilateral = quadrilateral;
            RawData = rawData;
            Confidence = confidence;
        }
    }
}
