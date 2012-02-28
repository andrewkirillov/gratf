// Glyph Recognition Studio
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010-2012
// andrew.kirillov@aforgenet.com
//

using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using AForge.Math;

namespace Xna3DViewer
{
    // Augmented reality scene displaying video and models
    class SceneViewerControl : GraphicsDeviceControl
    {
        private ModelsCollection modelsCollection = ModelsCollection.Instance;

        private Texture2D texture = null;
        private SpriteBatch mainSpriteBatch;
        private bool isInitialized = false;
        private List<VirtualModel> modelsToDisplay = new List<VirtualModel>( );

        // object used for synchronization
        private object sync = new object( );

        public SceneViewerControl( )
        {
        }

        protected override void Initialize( )
        {
            mainSpriteBatch = new SpriteBatch( GraphicsDevice );
            isInitialized = true;
        }

        // Update scene with new video frame
        public void UpdateScene( System.Drawing.Bitmap bitmap, List<VirtualModel> modelsToDisplay )
        {
            lock ( sync )
            {
                if ( isInitialized )
                {
                    if ( texture != null )
                    {
                        texture.Dispose( );
                        texture = null;
                    }

                    if ( bitmap != null )
                    {
                        texture = Tools.XNATextureFromBitmap( bitmap, GraphicsDevice );
                    }
                }

                this.modelsToDisplay.Clear( );
                this.modelsToDisplay.AddRange( modelsToDisplay );
            }

            Invalidate( );
        }

        // Draws the control
        protected override void Draw( )
        {
            GraphicsDevice.Clear( Color.Black );

            lock ( sync )
            {
                if ( texture != null )
                {
                    // draw texture containing video frame
                    mainSpriteBatch.Begin( 0, BlendState.Opaque );
                    mainSpriteBatch.Draw( texture, new Vector2( 0, 0 ), Color.White );
                    mainSpriteBatch.End( );

                    // restore state of some graphics device's properties after 2D graphics,
                    // so 3D rendering will work fine
                    GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                    GraphicsDevice.SamplerStates[0] = new SamplerState
                    {
                        AddressU = TextureAddressMode.Wrap,
                        AddressV = TextureAddressMode.Wrap
                    };
                }

                if ( modelsToDisplay.Count != 0 )
                {
                    // create camera view matrix
                    Matrix viewMatrix = Matrix.CreateLookAt(
                        new Microsoft.Xna.Framework.Vector3( 0, 0, 3 ),
                            Microsoft.Xna.Framework.Vector3.Zero,
                            Microsoft.Xna.Framework.Vector3.Up );
                    // create projection matrix
                    Matrix projectionMatrix = Matrix.CreatePerspective(
                        1, 1 / GraphicsDevice.Viewport.AspectRatio, 1f, 10000 );

                    // display all models
                    foreach ( VirtualModel virtualModel in modelsToDisplay )
                    {
                        if ( virtualModel.Size <= 0 )
                            continue;

                        try
                        {
                            Model model = modelsCollection.GetModel( this, virtualModel.Name );
                            Matrix4x4 modelTransformaton = virtualModel.Transformation;

                            // since model's transformation is provided in right-handed coordinate
                            // system, but XNA uses left-handed system, we need to invert rotations
                            // around X (pitch) and Y (yaw) axes and also negate Z coordinate
                            // of translation

                            // extract rotation angle from the original tranformation
                            float yaw, pitch, roll;
                            modelTransformaton.ExtractYawPitchRoll( out yaw, out pitch, out roll );
                            // create XNA's rotation matrix
                            Matrix rotation = Matrix.CreateFromYawPitchRoll( -yaw, -pitch, roll );
                            // create XNA's translation matrix
                            Matrix translation = Matrix.CreateTranslation(
                                modelTransformaton.V03, modelTransformaton.V13, -modelTransformaton.V23 );

                            // retrieve transformation matrices for all meshes of the model
                            Matrix[] transforms = new Matrix[model.Bones.Count];
                            model.CopyAbsoluteBoneTransformsTo( transforms );

                            // create scaling matrix, so model fits its glyph
                            Matrix scaling = Matrix.CreateScale( virtualModel.Size );

                            // display all meshes of the model
                            // (note: the code will fine only for model with single mesh so far)
                            // (to make it work with complex models, it is required to get routine
                            // (for calculation of model's radius, not just mesh radius)
                            foreach ( ModelMesh mesh in model.Meshes )
                            {
                                Matrix world = 
                                    transforms[mesh.ParentBone.Index] *
                                    Matrix.CreateScale( 1 / mesh.BoundingSphere.Radius ) *
                                    scaling * rotation * translation;

                                // set matrices for all effects
                                foreach ( Effect effect in mesh.Effects )
                                {
                                    if ( effect is BasicEffect )
                                    {
                                        BasicEffect basicEffect = (BasicEffect) effect;

                                        basicEffect.EnableDefaultLighting( );

                                        basicEffect.World = world;
                                        basicEffect.View = viewMatrix;
                                        basicEffect.Projection = projectionMatrix;
                                    }
                                    else
                                    {
                                        EffectParameter param = effect.Parameters["World"];
                                        if ( param != null )
                                        {
                                            param.SetValue( world );
                                        }

                                        param = effect.Parameters["View"];
                                        if ( param != null )
                                        {
                                            param.SetValue( viewMatrix );
                                        }

                                        param = effect.Parameters["Projection"];
                                        if ( param != null )
                                        {
                                            param.SetValue( projectionMatrix );
                                        }
                                    }
                                }

                                mesh.Draw( );
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }
    }
}