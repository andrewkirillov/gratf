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
using System.Text;
using System.Windows.Forms;

using Microsoft.Xna.Framework.Graphics;

namespace Xna3DViewer
{
    public partial class ModelSelectorForm : Form
    {
        private ModelsCollection models = ModelsCollection.Instance;
        private string selectedModelName;

        // Name of selected image
        public string ModelName
        {
            get { return selectedModelName; }
            set { selectedModelName = value; }
        }

        public ModelSelectorForm( )
        {
            InitializeComponent( );
        }

        // On form loading
        private void ModelSelectorForm_Load( object sender, EventArgs e )
        {
            // get all models' names
            ReadOnlyCollection<string> modelNames = models.GetModelNames( );

            foreach ( string modelName in modelNames )
            {
                modelsCombo.Items.Add( modelName );
            }

            if ( modelNames.Count == 0 )
            {
                okButton.Enabled = false;
            }
            else
            {
                int selectedIndex = modelNames.IndexOf( selectedModelName );

                modelsCombo.SelectedIndex = ( selectedIndex != -1 ) ? selectedIndex : 0;
            }
        }

        // Selection changes in models' combo box
        private void modelsCombo_SelectedIndexChanged( object sender, EventArgs e )
        {
            selectedModelName = modelsCombo.SelectedItem.ToString( );

            Model model = models.GetModel( modelViewerControl, selectedModelName );

            if ( model != null )
            {
                modelViewerControl.SetModelToView( model );
            }
        }
    }
}
