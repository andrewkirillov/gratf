// Glyph Recognition Studio
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010
// andrew.kirillov@aforgenet.com
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using AForge.Vision.GlyphRecognition;

namespace GlyphRecognitionStudio
{
    public partial class EditGlyphForm : Form
    {
        private Glyph glyph;
        private GlyphVisualizationData visualizationData;
        private List<string> forbiddenNames;

        public EditGlyphForm( Glyph glyph, List<string> existingNames )
        {
            InitializeComponent( );

            this.glyph = glyph;
            if ( glyph.UserData == null )
                glyph.UserData = new GlyphVisualizationData( Color.Red );
            visualizationData = (GlyphVisualizationData) glyph.UserData;

            forbiddenNames = existingNames;

            // show information about the glyph
            glyphEditor.GlyphData = (byte[,]) glyph.Data.Clone( );
            nameBox.Text = glyph.Name;
            colorButton.BackColor = visualizationData.Color;
            UpdateGlyphIcon( );
        }

        // On glyph's name editing
        private void nameBox_TextChanged( object sender, EventArgs e )
        {
            string name = nameBox.Text.Trim( );

            okButton.Enabled = false;

            if ( name.Length == 0 )
            {
                errorProvider.SetError( nameBox, "Glyph name can not be empty" );
                return;
            }
            else if ( ( name != glyph.Name ) && ( forbiddenNames.IndexOf( name ) != -1 ) )
            {
                errorProvider.SetError( nameBox, "A glyph with such name already exists" );
                return;
            }

            errorProvider.Clear( );
            okButton.Enabled = true;
        }

        // On button "OK" click
        private void okButton_Click( object sender, EventArgs e )
        {
            if ( Glyph.CheckIfRotationInvariant( glyphEditor.GlyphData ) )
            {
                MessageBox.Show( "The glyph is rotation invariant (it looks the same if rotated), so its rotaton will not be recognized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            glyph.Name = nameBox.Text.Trim( );
            glyph.Data = glyphEditor.GlyphData;
            glyph.UserData = visualizationData;

            DialogResult = DialogResult.OK;
            Close( );
        }

        // Select color for glyph highlight
        private void colorButton_Click( object sender, EventArgs e )
        {
            colorDialog.Color = visualizationData.Color;

            if ( colorDialog.ShowDialog( ) == DialogResult.OK )
            {
                visualizationData.Color = colorDialog.Color;
                colorButton.BackColor = visualizationData.Color;
            }
        }

        // Picture box clicked - select image for a glyph
        private void pictureBox_Click( object sender, EventArgs e )
        {
            ImageSelectorForm form = new ImageSelectorForm( );

            form.ImageName = visualizationData.ImageName;

            if ( form.ShowDialog( ) == DialogResult.OK )
            {
                visualizationData.ImageName = form.ImageName;
                UpdateGlyphIcon( );
            }
        }

        // Show image corresponding to the glyph
        private void UpdateGlyphIcon( )
        {
            if ( visualizationData.ImageName == null )
            {
                pictureBox.Image = null;
            }
            else
            {
                pictureBox.Image = EmbeddedImageCollection.Instance.GetImage( visualizationData.ImageName );
            }
        }
    }
}
