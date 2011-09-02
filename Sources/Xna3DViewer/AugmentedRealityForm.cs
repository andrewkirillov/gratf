// Glyph Recognition Studio
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010-2011
// andrew.kirillov@aforgenet.com
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using AForge.Math;

namespace Xna3DViewer
{
    public partial class AugmentedRealityForm : Form
    {
        private int viewerWidth  = 320;
        private int viewerHeight = 240;

        public AugmentedRealityForm( )
        {
            InitializeComponent( );
        }

        // Update AR form displaying video and models
        public void UpdateScene( Bitmap newFrame, List<VirtualModel> modelsToDisplay )
        {
            if ( ( newFrame != null ) &&
               ( ( newFrame.Width != viewerWidth ) || ( newFrame.Height != viewerHeight ) ) )
            {
                viewerWidth  = newFrame.Width;
                viewerHeight = newFrame.Height;
                ResizeViewer( );
            }

            sceneViewerControl.UpdateScene( newFrame, modelsToDisplay );
        }

        delegate void ResizeViewerHandler( );

        // Resize viewer control so it fits video and is centered in the form
        private void ResizeViewer( )
        {
            if ( InvokeRequired )
            {
                Invoke( new ResizeViewerHandler( ResizeViewer) );
            }
            else
            {
                sceneViewerControl.SuspendLayout( );
                sceneViewerControl.Size = new Size( viewerWidth, viewerHeight );
                sceneViewerControl.Location = new Point(
                    ( ClientRectangle.Width - viewerWidth ) / 2,
                    ( ClientRectangle.Height - viewerHeight ) / 2 );
                sceneViewerControl.ResumeLayout( );
            }
        }

        private void AugmentedRealityForm_Resize( object sender, EventArgs e )
        {
            ResizeViewer( );
        }
    }
}
