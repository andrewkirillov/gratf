// Glyph Recognition Studio
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010
// andrew.kirillov@aforgenet.com
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

namespace GlyphRecognitionStudio
{
    // Class to access images embedded into the application
    class EmbeddedImageCollection
    {
        private static EmbeddedImageCollection singleton = null;
        private const string imagesNameSpace = "GlyphRecognitionStudio.Images";

        private List<string> imageNames = new List<string>( );
        private Dictionary<string, Bitmap> images = new Dictionary<string, Bitmap>( );

        // Disable creating instances
        private EmbeddedImageCollection( )
        {
            Assembly assembly = this.GetType( ).Assembly;
            string[] resources = assembly.GetManifestResourceNames( );

            // collect list of embedded PNG files
            foreach ( string resourceName in resources )
            {
                if ( resourceName.IndexOf( imagesNameSpace ) == 0 )
                {
                    string embeddedFileName = resourceName.Substring( imagesNameSpace.Length + 1 );

                    if ( embeddedFileName.EndsWith( ".png" ) )
                    {
                        imageNames.Add( embeddedFileName.Replace( ".png", "" ) );
                    }
                }
            }
        }

        // Get instance of the singleton
        public static EmbeddedImageCollection Instance
        {
            get
            {
                if ( singleton == null )
                {
                    singleton = new EmbeddedImageCollection( );
                }
                return singleton;
            }
        }

        // Get collection of available image names
        public ReadOnlyCollection<string> GetImageNames( )
        {
            return imageNames.AsReadOnly( );
        }

        // Get image with the specified name
        public Bitmap GetImage( string name )
        {
            if ( imageNames.Contains( name ) )
            {
                if ( images.ContainsKey( name ) )
                {
                    return images[name];
                }
                else
                {
                    try
                    {
                        Assembly assembly = this.GetType( ).Assembly;
                        // load image and make sure it is 24 bpp RGB image by cloning to the format
                        Bitmap image = AForge.Imaging.Image.Clone(
                            new Bitmap( assembly.GetManifestResourceStream(
                                string.Format( "{0}.{1}.png", imagesNameSpace, name ) ) ),
                            PixelFormat.Format24bppRgb );

                        images.Add( name, image );

                        return image;
                    }
                    catch
                    {
                    }
                }
            }

            return null;
        }
    }
}
