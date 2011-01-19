// Glyph Recognition Studio
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010-2011
// andrew.kirillov@aforgenet.com
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using AForge.Vision.GlyphRecognition;
using Xna3DViewer;

namespace GlyphRecognitionStudio
{
    public partial class EditGlyphForm : Form
    {
        private Glyph glyph;
        private GlyphVisualizationData visualizationData;
        private ReadOnlyCollection<string> forbiddenNames;
        private GlyphDataCheckingHandeler glyphDataChecker = null;

        public EditGlyphForm( Glyph glyph, ReadOnlyCollection<string> existingNames )
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
            UpdateModelImage( );
        }

        public void SetGlyphDataCheckingHandeler( GlyphDataCheckingHandeler handler )
        {
            glyphDataChecker = handler;
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
            if ( !Glyph.CheckIfEveryRowColumnHasValue( glyphEditor.GlyphData ) )
            {
                MessageBox.Show( "A glyph must have at least one white cell in every row and column.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            if ( Glyph.CheckIfRotationInvariant( glyphEditor.GlyphData ) )
            {
                MessageBox.Show( "The glyph is rotation invariant (it looks the same if rotated), so its rotaton will not be recognized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            if ( ( glyphDataChecker != null ) && ( !glyphDataChecker( glyphEditor.GlyphData ) ) )
            {
                // return since external glyph data checker does not like the glyph
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

        // Picture box clicked - select image for the glyph
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

        // Model picture box clicked - select 3D model for the glyph
        private void modelBox_Click( object sender, EventArgs e )
        {
            ModelSelectorForm form = new ModelSelectorForm( );

            form.ModelName = visualizationData.ModelName;

            if ( form.ShowDialog( ) == DialogResult.OK )
            {
                visualizationData.ModelName = form.ModelName;
                UpdateModelImage( );
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

        // Show image corresponding to glyph's 3D models
        private void UpdateModelImage( )
        {
            if ( visualizationData.ModelName == null )
            {
                modelBox.Image = null;
            }
            else
            {
                modelBox.Image = ModelsCollection.Instance.GetModelImage( visualizationData.ModelName );
            }
        }
    }


    public delegate bool GlyphDataCheckingHandeler( byte[,] glyphData );
}
