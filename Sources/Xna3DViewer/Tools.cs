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

        // Get Bitmap screenshot of specified XNA device
        public static Bitmap BitmapFromDevice( GraphicsDevice device )
        {
            // get texture out of XNA device first
            ResolveTexture2D deviceTexture = new ResolveTexture2D( device,
                device.PresentationParameters.BackBufferWidth,
                device.PresentationParameters.BackBufferHeight,
                1, device.PresentationParameters.BackBufferFormat );
            device.ResolveBackBuffer( deviceTexture );

            // convert texture to bitmap
            Bitmap bitmap = BitmapFromTexture( deviceTexture );

            deviceTexture.Dispose( );

            return bitmap;
        }

        // Convert XNA texture to GDI+ Bitmap
        public static Bitmap BitmapFromTexture( Texture2D texture )
        {
            int width  = texture.Width;
            int height = texture.Height;

            // create bitmap and lock it
            Bitmap bitmap = new Bitmap( width, height, PixelFormat.Format32bppArgb );
            BitmapData data = bitmap.LockBits( new Rectangle( 0, 0, width, height ),
                ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb );

            // get texture data
            int bufferSize = data.Height * data.Stride;
            byte[] bytes = new byte[bufferSize];
            texture.GetData<byte>( bytes );

            // copy data into Bitmap
            Marshal.Copy( bytes, 0, data.Scan0,  bufferSize );

            bitmap.UnlockBits( data );

            return bitmap;
        }
    }
}
