// Glyph Recognition Studio
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010-2011
// andrew.kirillov@aforgenet.com
//

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Drawing;

using AForge.Vision.GlyphRecognition;

namespace GlyphRecognitionStudio
{
    class GlyphDatabases
    {
        private Dictionary<string, GlyphDatabase> dbs = new Dictionary<string, GlyphDatabase>( );

        public GlyphDatabase this[string name]
        {
            get { return dbs[name]; }
        }

        #region XML Tag Names
        private const string databaseTag = "Database";
        private const string glyphTag = "Glyph";
        private const string nameAttr = "name";
        private const string sizeAttr = "size";
        private const string dataAttr = "data";
        private const string countAttr = "count";
        private const string colorAttr = "color";
        private const string iconAttr = "icon";
        private const string modelAttr = "model";
        #endregion

        // Add glyph database to collection
        public void AddGlyphDatabase( string name, GlyphDatabase db )
        {
            if ( !dbs.ContainsKey( name ) )
            {
                dbs.Add( name, db );
            }
            else
            {
                throw new ApplicationException( "A glyph database with such name already exists : " + name );
            }
        }

        // Remove glyph database from collection
        public void RemoveGlyphDatabase( string name )
        {
            if ( dbs.ContainsKey( name ) )
            {
                dbs.Remove( name );
            }
        }

        // Rename glyph database
        public void RenameGlyphDatabase( string oldName, string newName )
        {
            if ( oldName != newName )
            {
                if ( dbs.ContainsKey( newName ) )
                {
                    throw new ApplicationException( "A glyph database with such name already exists : " + newName );
                }

                if ( !dbs.ContainsKey( oldName ) )
                {
                    throw new ApplicationException( "A glyph database with such name does not exist : " + oldName );
                }

                // insert it with new key
                dbs.Add( newName, dbs[oldName] );
                // remove it from dictonary with the old key
                dbs.Remove( oldName );
            }
        }

        // Get list of available databases' names
        public List<string> GetDatabaseNames( )
        {
            return new List<string>( dbs.Keys );
        }

        // Save infromation about all databases and glyphs into XML writer
        public void Save( XmlTextWriter xmlOut )
        {
            foreach ( KeyValuePair<string, GlyphDatabase> kvp in dbs )
            {
                xmlOut.WriteStartElement( databaseTag );
                xmlOut.WriteAttributeString( nameAttr, kvp.Key );
                xmlOut.WriteAttributeString( sizeAttr, kvp.Value.Size.ToString( ) );
                xmlOut.WriteAttributeString( countAttr, kvp.Value.Count.ToString( ) );

                // save glyps
                foreach ( Glyph glyph in kvp.Value )
                {
                    xmlOut.WriteStartElement( glyphTag );
                    xmlOut.WriteAttributeString( nameAttr, glyph.Name );
                    xmlOut.WriteAttributeString( dataAttr, GlyphDataToString( glyph.Data ) );

                    if ( glyph.UserData != null )
                    {
                        GlyphVisualizationData visualization = (GlyphVisualizationData) glyph.UserData;

                        // highlight color
                        xmlOut.WriteAttributeString( colorAttr, string.Format( "{0},{1},{2}",
                            visualization.Color.R, visualization.Color.G, visualization.Color.B ) );
                        // glyph's image
                        xmlOut.WriteAttributeString( iconAttr, visualization.ImageName );
                        // glyph's 3D model
                        xmlOut.WriteAttributeString( modelAttr, visualization.ModelName );
                    }

                    xmlOut.WriteEndElement( );
                }

                xmlOut.WriteEndElement( );
            }
        }

        // Load information about databases and glyphs from XML reader
        public void Load( XmlTextReader xmlIn )
        {
            // read to the first node
            xmlIn.Read( );

            int startingDept = xmlIn.Depth;

            while ( ( xmlIn.Name == databaseTag ) && ( xmlIn.NodeType == XmlNodeType.Element ) && ( xmlIn.Depth >= startingDept ) )
            {
                string name = xmlIn.GetAttribute( nameAttr );
                int size = int.Parse( xmlIn.GetAttribute( sizeAttr ) );
                int count = int.Parse( xmlIn.GetAttribute( countAttr ) );

                // create new database and add it to collection
                GlyphDatabase db = new GlyphDatabase( size );
                AddGlyphDatabase( name, db );

                if ( count > 0 )
                {
                    // read all glyphs
                    for ( int i = 0; i < count; i++ )
                    {
                        // read to the next glyph node
                        xmlIn.Read( );

                        string glyphName = xmlIn.GetAttribute( nameAttr );
                        string glyphStrData = xmlIn.GetAttribute( dataAttr );

                        // create new glyph and add it database
                        Glyph glyph = new Glyph( glyphName, GlyphDataFromString( glyphStrData, size ) );
                        db.Add( glyph );

                        // read visualization params
                        GlyphVisualizationData visualization = new GlyphVisualizationData( Color.Red );

                        visualization.ImageName = xmlIn.GetAttribute( iconAttr );
                        visualization.ModelName = xmlIn.GetAttribute( modelAttr );

                        string colorStr = xmlIn.GetAttribute( colorAttr );

                        if ( colorStr != null )
                        {
                            string[] rgbStr = colorStr.Split( ',' );

                            visualization.Color = Color.FromArgb(
                                int.Parse( rgbStr[0] ), int.Parse( rgbStr[1] ), int.Parse( rgbStr[2] ) );
                        }

                        glyph.UserData = visualization;
                    }

                    // read to the end tag
                    xmlIn.Read( );
                }

                // read to the next node
                xmlIn.Read( );
            }
        }

        #region Tool Methods

        private static string GlyphDataToString( byte[,] glyphData )
        {
            StringBuilder sb = new StringBuilder( );
            int glyphSize = glyphData.GetLength( 0 );

            for ( int i = 0; i < glyphSize; i++ )
            {
                for ( int j = 0; j < glyphSize; j++ )
                {
                    sb.Append( glyphData[i, j] );
                }
            }

            return sb.ToString( );
        }

        private static byte[,] GlyphDataFromString( string glyphStrData, int glyphSize )
        {
            byte[,] glyphData = new byte[glyphSize, glyphSize];

            for ( int i = 0, k = 0; i < glyphSize; i++ )
            {
                for ( int j = 0; j < glyphSize; j++, k++ )
                {
                    glyphData[i, j] = (byte) ( ( glyphStrData[k] == '0' ) ? 0 : 1 );
                }
            }

            return glyphData;
        }

        #endregion
    }
}
