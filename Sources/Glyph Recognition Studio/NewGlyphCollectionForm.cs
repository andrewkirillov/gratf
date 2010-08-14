using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GlyphRecognitionStudio
{
    public partial class NewGlyphCollectionForm : Form
    {
        public string CollectionName
        {
            get { return nameBox.Text.Trim( ); }
        }

        public int GlyphSize
        {
            get { return sizeCombo.SelectedIndex + 5; }
        }

        public NewGlyphCollectionForm( )
        {
            InitializeComponent( );
            sizeCombo.SelectedIndex = 0;
        }

        // Name was changed
        private void nameBox_TextChanged( object sender, EventArgs e )
        {
            okButton.Enabled = ( CollectionName.Length != 0 );
        }
    }
}
