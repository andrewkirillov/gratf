// Glyph Recognition Studio
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010-2011
// andrew.kirillov@aforgenet.com
//

using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Xna3DViewer
{
    // Augmented reality scene displaying video and models
    class SceneViewerControl : GraphicsDeviceControl
    {
        private Texture2D texture = null;
        private SpriteBatch mainSpriteBatch;
        private bool isInitialized = false;

        public SceneViewerControl( )
        {
        }

        protected override void Initialize( )
        {
            mainSpriteBatch = new SpriteBatch( GraphicsDevice );
            isInitialized = true;
        }

        // Update scene with new video frame
        public void UpdateScene( System.Drawing.Bitmap bitmap )
        {
            lock ( this )
            {
                if ( isInitialized )
                {
                    if ( texture != null )
                    {
                        texture.Dispose( );
                        texture = null;
                    }

                    texture = Tools.XNATextureFromBitmap( bitmap, GraphicsDevice );
                }
            }

            Invalidate( );
        }

        // Draws the control
        protected override void Draw( )
        {
            GraphicsDevice.Clear( Color.Black );

            lock ( this )
            {
                if ( texture != null )
                {
                    // draw texture containing video frame
                    mainSpriteBatch.Begin( SpriteBlendMode.None );
                    mainSpriteBatch.Draw( texture, new Vector2( 0, 0 ), Color.White );
                    mainSpriteBatch.End( );

                    // restore state of some graphics device's properties after 2D graphics
                    GraphicsDevice.RenderState.DepthBufferEnable = true;
                    GraphicsDevice.RenderState.AlphaBlendEnable = false;
                    GraphicsDevice.RenderState.AlphaTestEnable = false;

                    GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
                    GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
                }
            }
        }
    }
}