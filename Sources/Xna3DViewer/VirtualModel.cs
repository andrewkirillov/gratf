// Glyph Recognition Studio
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010-2011
// andrew.kirillov@aforgenet.com
//

using System;
using System.Collections.Generic;
using AForge.Math;

namespace Xna3DViewer
{
    // Describes a virtual 3D model to display instead of glyph
    public class VirtualModel
    {
        // Model's name to display
        public readonly string Name;

        // Model's transformation matrix in the real world (right-handed coordinate system)
        public readonly Matrix4x4 Transformation;

        // Model's size in real world (same units are used as for translation in transformation matrix)
        public readonly float Size;

        public VirtualModel( string name, Matrix4x4 transformation, float size )
        {
            Name = name;
            Transformation = transformation;
            Size = size;
        }
    }
}
