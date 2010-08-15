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
        private Glyph originalGlyph;
        private Glyph glyphToEdit;
        private List<string> forbiddenNames;

        public EditGlyphForm( Glyph glyph, List<string> existingNames )
        {
            InitializeComponent( );

            originalGlyph = glyph;
            glyphToEdit   = (Glyph) glyph.Clone( );

            forbiddenNames = existingNames;

            glyphEditor.SetGlyph( glyphToEdit );
            nameBox.Text = glyphToEdit.Name;
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
            else if ( ( name != originalGlyph.Name ) && ( forbiddenNames.IndexOf( name ) != -1 ) )
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
            originalGlyph.Name = nameBox.Text.Trim( );
            originalGlyph.Data = glyphToEdit.Data;
        }
    }
}
