// Glyph Recognition Studio
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010-2011
// andrew.kirillov@aforgenet.com
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Xna3DViewer
{
    public class ModelsCollection
    {
        private static ModelsCollection singleton = null;
        private const string imagesNameSpace = "Xna3DViewer.ModelImages";

        private ContentManager content = null;

        private List<string> modelNames = new List<string>( );
        private Dictionary<string, Bitmap> images = new Dictionary<string, Bitmap>( );
        private Dictionary<string, Model> models = new Dictionary<string, Model>( );

        // Disable creating instances
        private ModelsCollection( )
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
                        modelNames.Add( embeddedFileName.Replace( ".png", "" ) );
                    }
                }
            }
        }

        // Get instance of the singleton
        public static ModelsCollection Instance
        {
            get
            {
                if ( singleton == null )
                {
                    singleton = new ModelsCollection( );
                }
                return singleton;
            }
        }

        // Get collection of available image names
        public ReadOnlyCollection<string> GetModelNames( )
        {
            return modelNames.AsReadOnly( );
        }

        // Get image representing the specified model
        public Bitmap GetModelImage( string name )
        {
            if ( modelNames.Contains( name ) )
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
                        // load image from resources
                        Bitmap image = new Bitmap( assembly.GetManifestResourceStream(
                                string.Format( "{0}.{1}.png", imagesNameSpace, name ) ) );

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

        // Load 3D model
        public Model GetModel( GraphicsDeviceControl graphicsDevice, string modelName )
        {
            if ( modelNames.Contains( modelName ) )
            {
                if ( models.ContainsKey( modelName ) )
                {
                    return models[modelName];
                }

                if ( content == null )
                {
                    graphicsDevice.Disposed += new EventHandler(graphicsDevice_Disposed);
                    content = new ContentManager( graphicsDevice.Services, "Content" );
                }

                try
                {
                    Model model = content.Load<Model>( modelName );
                    models.Add( modelName, model );
                    return model;
                }
                catch
                {
                }
            }

            return null;
        }

        // Reset models' collection if graphics device gets disposed
        private void graphicsDevice_Disposed( object sender, EventArgs e )
        {
            content = null;
            models.Clear( );
        }
    }
}
