namespace Xna3DViewer
{
    partial class AugmentedRealityForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose( );
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent( )
        {
            this.sceneViewerControl = new Xna3DViewer.SceneViewerControl( );
            this.SuspendLayout( );
            // 
            // sceneViewerControl
            // 
            this.sceneViewerControl.Location = new System.Drawing.Point( 162, 117 );
            this.sceneViewerControl.Name = "sceneViewerControl";
            this.sceneViewerControl.Size = new System.Drawing.Size( 320, 240 );
            this.sceneViewerControl.TabIndex = 0;
            // 
            // AugmentedRealityForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 644, 484 );
            this.Controls.Add( this.sceneViewerControl );
            this.Name = "AugmentedRealityForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Augmented Reality";
            this.Resize += new System.EventHandler( this.AugmentedRealityForm_Resize );
            this.ResumeLayout( false );

        }

        #endregion

        private SceneViewerControl sceneViewerControl;
    }
}