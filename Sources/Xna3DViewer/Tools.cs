// Glyph Recognition Studio
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010-2011
// andrew.kirillov@aforgenet.com
//

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Graphics;

namespace Xna3DViewer
{
    internal static class Tools
    {
        // Convert GDI+ bitmap to XNA texture
        public static Texture2D XNATextureFromBitmap( Bitmap bitmap, GraphicsDevice device )
        {
            int width  = bitmap.Width;
            int height = bitmap.Height;

            Texture2D texture = new Texture2D( device, width, height,
                1, TextureUsage.None, SurfaceFormat.Color );

            BitmapData data = bitmap.LockBits( new Rectangle( 0, 0, width, height ),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb );

            int bufferSize = data.Height * data.Stride;

            // copy bitmap data into texture
            byte[] bytes = new byte[bufferSize];
            Marshal.Copy( data.Scan0, bytes, 0, bytes.Length );
            texture.SetData( bytes );

            bitmap.UnlockBits( data );

            return texture;
        }
    }
}
