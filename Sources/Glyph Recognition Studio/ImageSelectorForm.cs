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

namespace GlyphRecognitionStudio
{
    // Form, which allows to select on of the images embedded in the assembly
    public partial class ImageSelectorForm : Form
    {
        private ImageList imageList = new ImageList( );
        private string selectedImageName;

        // Name of selected image
        public string ImageName
        {
            get { return selectedImageName; }
            set { selectedImageName = value; }
        }

        public ImageSelectorForm( )
        {
            InitializeComponent( );

            imageList.ImageSize = new Size( 32, 32 );
        }

        // On form loading - show available images
        private void ImageSelectorForm_Load( object sender, EventArgs e )
        {
            EmbeddedImageCollection imageCollection = EmbeddedImageCollection.Instance;
            ReadOnlyCollection<string> imageNames = imageCollection.GetImageNames( );
            ListViewItem selectedItem = null;

            listView.LargeImageList = imageList;

            foreach ( string imageName in imageNames )
            {
                Bitmap image = imageCollection.GetImage( imageName );

                if ( image != null )
                {
                    imageList.Images.Add( imageName, image );

                    ListViewItem lvi = listView.Items.Add( imageName );
                    lvi.ImageKey = imageName;

                    if ( imageName == selectedImageName )
                    {
                        lvi.Selected = true;
                        selectedItem = lvi;
                    }
                }
            }

            if ( selectedItem != null )
                selectedItem.EnsureVisible( );
        }

        // On button "OK" click - get name of selected image
        private void okButton_Click( object sender, EventArgs e )
        {
            if ( listView.SelectedIndices.Count == 0 )
            {
                selectedImageName = null;
            }
            else
            {
                selectedImageName = listView.SelectedItems[0].Text;
            }
        }
    }
}
