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

namespace Xna3DViewer
{
    class ModelViewerControl : GraphicsDeviceControl
    {
        private Stopwatch timer;
        private Model model = null;

        // object used for synchronization
        private object sync = new object( );

        public ModelViewerControl( )
        {
        }
       
        // Initializes the control
        protected override void Initialize( )
        {
            this.Click += new System.EventHandler( this.ModelViewerControl_Click );

            // start the animation timer
            timer = Stopwatch.StartNew( );

            // hook the idle event to constantly redraw our animation
            Application.Idle += delegate { Invalidate( ); };
        }

        // Set model to view int the control
        public void SetModelToView( Model model )
        {
            lock ( sync )
            {
                this.model = model;
                Invalidate( );
            }
        }

        // Draws the control
        protected override void Draw( )
        {
            GraphicsDevice.Clear( Color.Black );

            lock ( sync )
            {
                // draw simple models for now with single mesh
                if ( ( model != null ) && ( model.Meshes.Count == 1 ) )
                {
                    ModelMesh mesh = model.Meshes[0];

                    // spin the object according to how much time has passed
                    float time = (float) timer.Elapsed.TotalSeconds;

                    // object's rotation and transformation matrices
                    Matrix rotation = Matrix.CreateFromYawPitchRoll(
                        time * 0.5f, time * 0.6f, time * 0.7f );
                    Matrix translation = Matrix.CreateTranslation( 0, 0, 0 );

                    // create transform matrices
                    Matrix viewMatrix = Matrix.CreateLookAt(
                        new Vector3( 0, 0, 3 ), Vector3.Zero, Vector3.Up );
                    Matrix projectionMatrix = Matrix.CreatePerspective(
                        1, 1 / GraphicsDevice.Viewport.AspectRatio, 1f, 10000 );
                    Matrix world = Matrix.CreateScale( 1 / mesh.BoundingSphere.Radius ) *
                        rotation * translation;

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
        }

        // On mouse click in the control - pause/run animation
        private void ModelViewerControl_Click( object sender, System.EventArgs e )
        {
            if ( timer.IsRunning )
            {
                timer.Stop( );
            }
            else
            {
                timer.Start( );
            }
        }
    }
}
