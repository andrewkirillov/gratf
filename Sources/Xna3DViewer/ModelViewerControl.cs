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
                    // spin the object according to how much time has passed
                    float time = (float) timer.Elapsed.TotalSeconds;

                    float yaw   = time * 0.5f;
                    float pitch = time * 0.6f;
                    float roll  = time * 0.7f;

                    // create transform matrices
                    Matrix rotation = Matrix.CreateFromYawPitchRoll( yaw, pitch, roll );
                    Matrix viewMatrix = Matrix.CreateLookAt( new Vector3( 0, 0, 3 ), Vector3.Zero, Vector3.Up );
                    Matrix projectionMatrix = Matrix.CreatePerspectiveFieldOfView( MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1f, 10 );

                    Vector3 pos = new Vector3(0, 0, 0 );

                    foreach ( ModelMesh mesh in model.Meshes )
                    {
                        Matrix world = rotation *
                            Matrix.CreateScale( 1 / mesh.BoundingSphere.Radius ) *
                            Matrix.CreateTranslation( pos );

                        foreach ( Effect effect in mesh.Effects )
                        {
                            if ( effect is BasicEffect )
                            {
                                ( (BasicEffect) effect ).EnableDefaultLighting( );
                            }

                            effect.Parameters["World"].SetValue( world );
                            effect.Parameters["View"].SetValue( viewMatrix );
                            effect.Parameters["Projection"].SetValue( projectionMatrix );
                        }

                        mesh.Draw( );
                    }
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
