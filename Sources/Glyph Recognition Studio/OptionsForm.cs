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
using System.Windows.Forms;

namespace GlyphRecognitionStudio
{
    public partial class OptionsForm : Form
    {
        private bool autoDetectFocalLength = true;
        private float focalLength = 640;
        private float glyphSize = 100;

        public bool AutoDetectFocalLength
        {
            get { return autoDetectFocalLength; }
            set { autoDetectFocalLength = value; }
        }

        public float CameraFocalLength
        {
            get { return focalLength; }
            set { focalLength = value; }
        }

        public float GlyphSize
        {
            get { return glyphSize; }
            set { glyphSize = value; }
        }

        public OptionsForm( )
        {
            InitializeComponent( );
        }

        // On form loading - set all controls
        private void OptionsForm_Load( object sender, EventArgs e )
        {
            autoDetectFocalLengthCheck.Checked = autoDetectFocalLength;

            if ( !autoDetectFocalLength )
            {
                focalLengthBox.Text = focalLength.ToString( );
            }
            else
            {
                focalLengthBox.Enabled = false;
            }

            glyphSizeBox.Text = glyphSize.ToString( );
        }

        // On OK button - read all options
        private void okButton_Click( object sender, EventArgs e )
        {
            autoDetectFocalLength = autoDetectFocalLengthCheck.Checked;

            if  ( ( ( !autoDetectFocalLength ) &&
                  ( !GetFloatValue( focalLengthBox, errorProvider, "Focal Length", out focalLength ) ) ) ||
                  ( !GetFloatValue( glyphSizeBox, errorProvider, "Glyph Size", out glyphSize ) ) )
            {
                return;
            }

            if ( ( glyphSize < 0 ) || ( glyphSize > 200 ) )
            {
                errorProvider.SetError( glyphSizeBox, "Glyph size must be in [0, 200]mm range." );
                return;
            }

            DialogResult = DialogResult.OK;
            Close( );
        }

        private static bool GetFloatValue( TextBox textBox, ErrorProvider errorProvider,
            string valueName, out float value )
        {
            value = 0;

            if ( !float.TryParse( textBox.Text, out value ) )
            {
                errorProvider.SetError( textBox, string.Format( "Failed parsing the '{0}' value as float",
                    valueName ) );
                return false;
            }

            return true;
        }

        // Focal length auto detetion turned On/Off
        private void autoDetectFocalLengthCheck_CheckedChanged( object sender, EventArgs e )
        {
            focalLengthBox.Enabled = !autoDetectFocalLengthCheck.Checked;
        }
    }
}
