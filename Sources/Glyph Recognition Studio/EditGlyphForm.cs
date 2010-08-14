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

        public EditGlyphForm( Glyph glyph )
        {
            InitializeComponent( );

            originalGlyph = glyph;
            glyphToEdit   = (Glyph) glyph.Clone( );

            glyphEditor.SetGlyph( glyphToEdit );
        }

        private void nameBox_TextChanged( object sender, EventArgs e )
        {
            okButton.Enabled = ( nameBox.Text.Trim( ).Length != 0 );
        }

        private void okButton_Click( object sender, EventArgs e )
        {
            originalGlyph.Name = nameBox.Text.Trim( );
            originalGlyph.Data = glyphToEdit.Data;
        }
    }
}
