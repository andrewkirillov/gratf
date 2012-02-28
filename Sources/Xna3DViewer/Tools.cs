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

            Texture2D texture = new Texture2D( device, width, height, false, SurfaceFormat.Color );

            BitmapData data = bitmap.LockBits( new Rectangle( 0, 0, width, height ),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb );

            int bufferSize = width * height * 4;
            byte[] bytes = new byte[bufferSize];
            
            // copy bitmap data into texture
            unsafe
            {
                byte* ptr = (byte*) data.Scan0.ToPointer( );
                int offset = data.Stride - 4 * width;

                for ( int y = 0, i = 0; y < height; y++ )
                {
                    for ( int x = 0; x < width; x++, ptr += 4 )
                    {
                        bytes[i++] = ptr[2];
                        bytes[i++] = ptr[1];
                        bytes[i++] = ptr[0];
                        bytes[i++] = ptr[3];
                    }
                    ptr += offset;
                }
            }

            bitmap.UnlockBits( data );

            texture.SetData( bytes );

            return texture;
        }

        // Get Bitmap screenshot of specified XNA device
        public static Bitmap BitmapFromDevice( GraphicsDevice device )
        {
            PresentationParameters pp = device.PresentationParameters;

            // get texture out of XNA device first
            RenderTarget2D  deviceTexture = new RenderTarget2D( device,
                device.PresentationParameters.BackBufferWidth,
                device.PresentationParameters.BackBufferHeight,
                false, device.PresentationParameters.BackBufferFormat,
                pp.DepthStencilFormat );
            device.SetRenderTarget( deviceTexture );

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
