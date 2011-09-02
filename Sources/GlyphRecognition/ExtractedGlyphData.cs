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
    using AForge.Math;

    /// <summary>
    /// Information about the glyph extracted from an image using <see cref="GlyphRecognizer"/>.
    /// </summary>
    public class ExtractedGlyphData : ICloneable
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
        /// <see cref="RawData"/> matches (see <see cref="Glyph.CheckForMatching(byte[,])"/>) to any of the glyphs
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
        /// Glyphs transformation matrix.
        /// </summary>
        /// 
        /// <remarks><para>The property provides real world glyph's transformation, which is
        /// estimated by <see cref="GlyphTracker.TrackGlyphs">glyph tracking routine</see>.</para>
        /// </remarks>
        /// 
        public Matrix4x4 TransformationMatrix
        {
            get { return transformationMatrix; }
            internal set { transformationMatrix = value; }
        }
        private Matrix4x4 transformationMatrix;

        /// <summary>
        /// Check if glyph pose was estimated or not.
        /// </summary>
        /// 
        /// <remarks><para>The property tells if <see cref="TransformationMatrix"/> property
        /// was calculated for this glyph or not.</para></remarks>
        ///
        public bool IsTransformationDetected
        {
            get { return isTransformationDetected; }
            internal set { isTransformationDetected = value; }
        }
        private bool isTransformationDetected = false;

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

        /// <summary>
        /// Clone the object by making its exact copy.
        /// </summary>
        /// 
        /// <returns>Returns clone of the object.</returns>
        /// 
        public object Clone( )
        {
            ExtractedGlyphData clone = new ExtractedGlyphData(
                new List<IntPoint>( Quadrilateral ), (byte[,]) RawData.Clone( ), Confidence );

            if ( recognizedGlyph != null )
            {
                clone.RecognizedGlyph = (Glyph) recognizedGlyph.Clone( );
            }
            if ( recognizedQuadrilateral != null )
            {
                clone.RecognizedQuadrilateral = new List<IntPoint>( recognizedQuadrilateral );
            }

            return clone;
        }
    }
}
