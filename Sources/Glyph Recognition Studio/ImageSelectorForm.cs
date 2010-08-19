using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GlyphRecognitionStudio
{
    public partial class ImageSelectorForm : Form
    {
        private ImageList imageList = new ImageList( );

        private string selectedImageName;

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

        private void ImageSelectorForm_Load( object sender, EventArgs e )
        {
            EmbeddedImageCollection imageCollection = EmbeddedImageCollection.Instance;
            ReadOnlyCollection<string> imageNames = imageCollection.GetImageNames( );

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
                    }
                }
            }
        }

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
