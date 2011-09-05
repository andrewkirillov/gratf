// Glyph Recognition Studio
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010
// andrew.kirillov@aforgenet.com
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using System.Xml;

namespace GlyphRecognitionStudio
{
    class Configuration
    {
        private static Configuration singleton = null;

        // list of configuration options
        private Dictionary<string, string> options = new Dictionary<string, string>( );

        private const string configFolderName = "AForge";
        private const string baseConfigFileName = "glyph recognition studio.cfg";
        private string configFileName = null;
        bool isSuccessfullyLoaded = false;

        // object used for synchronization
        private object sync = new object( );

        #region XML Tag Names
        private const string mainTag = "GlyphRecognitionStudio";
        private const string glyphDatabasesTag = "GlyphDatabases";
        private const string optionsTag = "Options";
        #endregion

        // Configuration load status
        public bool IsSuccessfullyLoaded
        {
            get { return isSuccessfullyLoaded; }
        }

        // Disable making class instances
        private Configuration( )
        {
            configFileName = Path.Combine(
                Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ),
                configFolderName );
            configFileName = Path.Combine( configFileName, baseConfigFileName );
        }

        // Get instance of the configuration storage
        public static Configuration Instance
        {
            get
            {
                if ( singleton == null )
                {
                    singleton = new Configuration( );
                }
                return singleton;
            }
        }

        // Set configuration option's value to store
        public void SetConfigurationOption( string option, string value )
        {
            if ( options.ContainsKey( option ) )
            {
                options[option] = value;
            }
            else
            {
                options.Add( option, value );
            }
        }

        // Get value of configuration option
        public string GetConfigurationOption( string option )
        {
            return ( options.ContainsKey( option ) ) ? options[option] : null;
        }

        // Save application's configuration
        public void Save( GlyphDatabases dbs )
        {
            lock ( sync )
            {
                // make sure directory exists
                Directory.CreateDirectory( Path.GetDirectoryName( configFileName ) );

                // open file
                FileStream fs = new FileStream( configFileName, FileMode.Create );
                // create XML writer
                XmlTextWriter xmlOut = new XmlTextWriter( fs, Encoding.UTF8 );

                // use indenting for readability
                xmlOut.Formatting = Formatting.Indented;

                // start document
                xmlOut.WriteStartDocument( );
                xmlOut.WriteComment( "Glyph Recognition Studio configuration file" );

                // main node
                xmlOut.WriteStartElement( mainTag );

                // save configuration options
                xmlOut.WriteStartElement( optionsTag );
                SaveOptions( xmlOut );
                xmlOut.WriteEndElement( );

                // save glyph databases
                xmlOut.WriteStartElement( glyphDatabasesTag );
                dbs.Save( xmlOut );
                xmlOut.WriteEndElement( );

                xmlOut.WriteEndElement( ); // end of main node
                // close file
                xmlOut.Close( );
            }
        }

        // Load application's configration
        public bool Load( GlyphDatabases dbs )
        {
            isSuccessfullyLoaded = false;

            lock ( sync )
            {
                // check file existance
                if ( File.Exists( configFileName ) )
                {
                    FileStream fs = null;
                    XmlTextReader xmlIn = null;

                    try
                    {
                        // open file
                        fs = new FileStream( configFileName, FileMode.Open );
                        // create XML reader
                        xmlIn = new XmlTextReader( fs );

                        xmlIn.WhitespaceHandling = WhitespaceHandling.None;
                        xmlIn.MoveToContent( );

                        // check main node
                        if ( xmlIn.Name != mainTag )
                            throw new ApplicationException( );

                        // move to next node
                        xmlIn.Read( );

                        while ( true )
                        {
                            // ignore anything if it is not under main tag
                            while ( ( xmlIn.Depth > 1 ) || (
                                    ( xmlIn.Depth == 1 ) && ( xmlIn.NodeType != XmlNodeType.Element ) ) )
                            {
                                xmlIn.Read( );
                            }

                            // break if end element is reached
                            if ( xmlIn.Depth == 0 )
                                break;

                            int tagStartLineNummber = xmlIn.LineNumber;

                            switch ( xmlIn.Name )
                            {
                                case glyphDatabasesTag:
                                    dbs.Load( xmlIn );
                                    break;

                                case optionsTag:
                                    LoadOptions( xmlIn );
                                    break;
                            }

                            // read to the next node, if loader did not move any further
                            if ( xmlIn.LineNumber == tagStartLineNummber )
                            {
                                xmlIn.Read( );
                            }
                        }

                        isSuccessfullyLoaded = true;
                        // ignore the rest
                    }
                    catch
                    {
                    }
                    finally
                    {
                        if ( xmlIn != null )
                            xmlIn.Close( );
                    }
                }
            }

            return isSuccessfullyLoaded;
        }

        // Save configuration options
        private void SaveOptions( XmlTextWriter xmlOut )
        {
            foreach ( KeyValuePair<string, string> kvp in options )
            {
                xmlOut.WriteStartElement( kvp.Key );
                xmlOut.WriteString( kvp.Value );
                xmlOut.WriteEndElement( );
            }
        }

        // Load configuration options
        private void LoadOptions( XmlTextReader xmlIn )
        {
            options.Clear( );
            // read to the first node
            xmlIn.Read( );

            int startingDept = xmlIn.Depth;

            while ( ( xmlIn.NodeType == XmlNodeType.Element ) && ( xmlIn.Depth == startingDept ) )
            {
                string option = xmlIn.Name;
                string value = null;

                if ( !xmlIn.IsEmptyElement )
                {
                    // read to the content
                    xmlIn.Read( );
                    // read content as string
                    value = xmlIn.ReadString( );

                    // add the value to options list
                    options.Add( option, value );
                }

                // read to the next option
                xmlIn.Read( );
            }
        }
    }
}
