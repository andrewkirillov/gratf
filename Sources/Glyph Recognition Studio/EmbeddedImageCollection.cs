using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

namespace GlyphRecognitionStudio
{
    class EmbeddedImageCollection
    {
        private static EmbeddedImageCollection singleton = null;

        private static string[] embeddedImages = new string[]
        {
            "Elephant", "Fish", "Fly"
        };

        private List<string> imageNames = new List<string>( );
        private Dictionary<string, Bitmap> images = new Dictionary<string, Bitmap>( );

        // Disable creating instances
        private EmbeddedImageCollection( )
        {
            imageNames.AddRange( embeddedImages );
        }

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

        public ReadOnlyCollection<string> GetImageNames( )
        {
            return imageNames.AsReadOnly( );
        }

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
                        Bitmap image = AForge.Imaging.Image.Clone(
                            new Bitmap( assembly.GetManifestResourceStream(
                                string.Format( "GlyphRecognitionStudio.Images.{0}.png", name ) ) ),
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
