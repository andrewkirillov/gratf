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
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

using AForge.Math;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Vision.GlyphRecognition;
using AForge.Imaging;
using AForge.Imaging.Filters;

using Xna3DViewer;

namespace GlyphRecognitionStudio
{
    public partial class MainForm : Form
    {
        // collection of glyph databases
        private GlyphDatabases glyphDatabases = new GlyphDatabases( );
        // active glyph database
        private string activeGlyphDatabaseName = null;
        private GlyphDatabase activeGlyphDatabase = null;
        // stop watch used for measuring FPS
        private Stopwatch stopWatch = null;

        private ImageList glyphsImageList = new ImageList( );

        private AugmentedRealityForm arForm = null;
        private GlyphImageProcessor imageProcessor = new GlyphImageProcessor( );
        private bool autoDetectFocalLength = true;

        // object used for synchronization
        private object sync = new object( );

        private const string ErrorBoxTitle = "Error";

        #region Configuration Option Names
        private const string activeDatabaseOption = "ActiveDatabase";
        private const string mainFormXOption = "MainFormX";
        private const string mainFormYOption = "MainFormY";
        private const string mainFormWidthOption = "MainFormWidth";
        private const string mainFormHeightOption = "MainFormHeight";
        private const string mainFormStateOption = "MainFormState";
        private const string mainSplitterOption = "MainSplitter";
        private const string glyphSizeOption = "GlyphSize";
        private const string focalLengthOption = "FocalLength";
        private const string detectFocalLengthOption = "DetectFocalLength";
        private const string autoDetectFocalLengthOption = "AutoDetectFocalLength";
        #endregion

        // Class constructor
        public MainForm( )
        {
            InitializeComponent( );

            glyphsImageList.ImageSize = new Size( 32, 32 );
            glyphList.LargeImageList = glyphsImageList;

            bordersToolStripMenuItem.Tag = VisualizationType.BorderOnly;
            namesToolStripMenuItem.Tag   = VisualizationType.Name;
            imagesToolStripMenuItem.Tag  = VisualizationType.Image;
            modelToolStripMenuItem.Tag   = VisualizationType.Model;
        }

        // On File->Exit menu item click
        private void exitToolStripMenuItem_Click( object sender, EventArgs e )
        {
            Application.Exit( );
        }

        // Show standard error bot
        private void ShowErrorBox( string message )
        {
            MessageBox.Show( message, ErrorBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error );
        }

        // On loading of the main form
        private void MainForm_Load( object sender, EventArgs e )
        {
            // load configuratio
            Configuration config = Configuration.Instance;

            if ( config.Load( glyphDatabases ) )
            {
                RefreshListOfGlyphDatabases( );
                ActivateGlyphDatabase( config.GetConfigurationOption( activeDatabaseOption ) );

                try
                {
                    Location = new Point(
                        int.Parse( config.GetConfigurationOption( mainFormXOption ) ),
                        int.Parse( config.GetConfigurationOption( mainFormYOption ) ) );

                    Size = new Size(
                        int.Parse( config.GetConfigurationOption( mainFormWidthOption ) ),
                        int.Parse( config.GetConfigurationOption( mainFormHeightOption ) ) );

                    WindowState = (FormWindowState) Enum.Parse( typeof( FormWindowState ),
                        config.GetConfigurationOption( mainFormStateOption ) );

                    splitContainer.SplitterDistance = int.Parse( config.GetConfigurationOption( mainSplitterOption ) );

                    autoDetectFocalLength = bool.Parse( config.GetConfigurationOption( autoDetectFocalLengthOption ) );
                    imageProcessor.GlyphSize = float.Parse( config.GetConfigurationOption( glyphSizeOption ) );
                    if ( !autoDetectFocalLength )
                    {
                        imageProcessor.CameraFocalLength = float.Parse( config.GetConfigurationOption( focalLengthOption ) );
                    }
                }
                catch
                {
                }
            }
        }

        // On closing of the main form
        private void MainForm_FormClosing( object sender, FormClosingEventArgs e )
        {
            Configuration config = Configuration.Instance;

            if ( WindowState != FormWindowState.Minimized )
            {
                if ( WindowState != FormWindowState.Maximized )
                {
                    config.SetConfigurationOption( mainFormXOption, Location.X.ToString( ) );
                    config.SetConfigurationOption( mainFormYOption, Location.Y.ToString( ) );
                    config.SetConfigurationOption( mainFormWidthOption, Width.ToString( ) );
                    config.SetConfigurationOption( mainFormHeightOption, Height.ToString( ) );
                }
                config.SetConfigurationOption( mainFormStateOption, WindowState.ToString( ) );
                config.SetConfigurationOption( mainSplitterOption, splitContainer.SplitterDistance.ToString( ) );
            }

            config.SetConfigurationOption( activeDatabaseOption, activeGlyphDatabaseName );

            config.SetConfigurationOption( autoDetectFocalLengthOption, autoDetectFocalLength.ToString( ) );
            config.SetConfigurationOption( focalLengthOption, imageProcessor.CameraFocalLength.ToString( ) );
            config.SetConfigurationOption( glyphSizeOption, imageProcessor.GlyphSize.ToString( ) );

            try
            {
                config.Save( glyphDatabases );
            }
            catch ( IOException ex )
            {
                ShowErrorBox( "Failed saving confguration file.\r\n\r\n" + ex.Message );
            }

            if ( videoSourcePlayer.VideoSource != null )
            {
                videoSourcePlayer.SignalToStop( );
                videoSourcePlayer.WaitForStop( );
            }
        }

        // Show about form
        private void aboutToolStripMenuItem_Click( object sender, EventArgs e )
        {
            AboutForm form = new AboutForm( );

            form.ShowDialog( );
        }

        // Open local video capture device
        private void localVideoCaptureDeviceToolStripMenuItem_Click( object sender, EventArgs e )
        {
            VideoCaptureDeviceForm form = new VideoCaptureDeviceForm( );

            if ( form.ShowDialog( this ) == DialogResult.OK )
            {
                // open it
                OpenVideoSource( form.VideoDevice );
            }
        }

        // Open video file using DirectShow
        private void openVideofileToolStripMenuItem_Click( object sender, EventArgs e )
        {
            if ( openFileDialog.ShowDialog( ) == DialogResult.OK )
            {
                // create video source
                FileVideoSource fileSource = new FileVideoSource( openFileDialog.FileName );

                // open it
                OpenVideoSource( fileSource );
            }
        }

        // Open video source
        private void OpenVideoSource( IVideoSource source )
        {
            // set busy cursor
            this.Cursor = Cursors.WaitCursor;

            // reset glyph processor
            imageProcessor.Reset( );            

            // stop current video source
            videoSourcePlayer.SignalToStop( );
            videoSourcePlayer.WaitForStop( );

            // start new video source
            videoSourcePlayer.VideoSource = new AsyncVideoSource( source );
            videoSourcePlayer.Start( );

            // reset stop watch
            stopWatch = null;

            // start timer
            timer.Start( );

            this.Cursor = Cursors.Default;
        }

        // On timer event - gather statistics
        private void timer_Tick( object sender, EventArgs e )
        {
            IVideoSource videoSource = videoSourcePlayer.VideoSource;

            if ( videoSource != null )
            {
                // get number of frames since the last timer tick
                int framesReceived = videoSource.FramesReceived;

                if ( stopWatch == null )
                {
                    stopWatch = new Stopwatch( );
                    stopWatch.Start( );
                }
                else
                {
                    stopWatch.Stop( );

                    float fps = 1000.0f * framesReceived / stopWatch.ElapsedMilliseconds;
                    fpsLabel.Text = fps.ToString( "F2" ) + " fps";

                    stopWatch.Reset( );
                    stopWatch.Start( );
                }
            }
        }

        // Add new glyph collection
        private void newToolStripMenuItem_Click( object sender, EventArgs e )
        {
            NewGlyphCollectionForm newCollectionForm = new NewGlyphCollectionForm( glyphDatabases.GetDatabaseNames( ) );

            if ( newCollectionForm.ShowDialog( ) == DialogResult.OK )
            {
                string name = newCollectionForm.CollectionName;
                int size = newCollectionForm.GlyphSize;

                GlyphDatabase db = new GlyphDatabase( size );

                try
                {
                    glyphDatabases.AddGlyphDatabase( name, db );

                    // add new item to list view
                    ListViewItem lvi = glyphCollectionsList.Items.Add( name );
                    lvi.SubItems.Add( string.Format( "{0}x{1}", size, size ) );
                    lvi.Name = name;
                }
                catch
                {
                    ShowErrorBox( string.Format( "A glyph database with the name '{0}' already exists.", name ) );
                }
            }
        }

        // Opening context menu for glyph collections
        private void glyphCollectionsContextMenu_Opening( object sender, CancelEventArgs e )
        {
            activateToolStripMenuItem.Enabled =
            renameToolStripMenuItem.Enabled =
            deleteToolStripMenuItem.Enabled = ( glyphCollectionsList.SelectedIndices.Count != 0 );
        }

        // Opening context menu for a glyph collection
        private void glyphCollectionContextMenu_Opening( object sender, CancelEventArgs e )
        {
            newGlyphToolStripMenuItem.Enabled = ( activeGlyphDatabase != null );

            editGlyphToolStripMenuItem.Enabled =
            deleteGlyphToolStripMenuItem.Enabled = 
            printPreviewToolStripMenuItem.Enabled =
            printToolStripMenuItem.Enabled = ( activeGlyphDatabase != null ) && ( glyphList.SelectedIndices.Count != 0 );
        }

        // Add new glyph to the active collection
        private void newGlyphToolStripMenuItem_Click( object sender, EventArgs e )
        {
            if ( activeGlyphDatabase != null )
            {
                // create new glyph ...
                Glyph glyph = new Glyph( string.Empty, activeGlyphDatabase.Size );
                glyphNameInEditor = string.Empty;
                // ... and pass it the glyph editting form
                EditGlyphForm glyphForm = new EditGlyphForm( glyph, activeGlyphDatabase.GetGlyphNames( ) );
                glyphForm.Text = "New Glyph";

                // set glyph data checking handler
                glyphForm.SetGlyphDataCheckingHandeler( new GlyphDataCheckingHandeler( CheckGlyphData ) );

                if ( glyphForm.ShowDialog( ) == DialogResult.OK )
                {
                    try
                    {
                        lock ( sync )
                        {
                            // add glyph to active database
                            activeGlyphDatabase.Add( glyph );
                        }

                        // create an icon for it
                        glyphsImageList.Images.Add( glyph.Name, CreateGlyphIcon( glyph ) );

                        // add it to list view
                        ListViewItem lvi = glyphList.Items.Add( glyph.Name );
                        lvi.ImageKey = glyph.Name;
                    }
                    catch
                    {
                        ShowErrorBox( string.Format( "A glyph with the name '{0}' already exists in the database.", glyph.Name ) );
                    }
                }
            }
        }

        // Double click in glyphs' list
        private void glyphList_MouseDoubleClick( object sender, MouseEventArgs e )
        {
            EditSelectedGlyph( );
        }

        // "Edit" glyph from context menu
        private void editGlyphToolStripMenuItem_Click( object sender, EventArgs e )
        {
            EditSelectedGlyph( );
        }

        private void EditSelectedGlyph( )
        {
            if ( ( activeGlyphDatabase != null ) && ( glyphList.SelectedIndices.Count != 0 ) )
            {
                // get selected item and it glyph ...
                ListViewItem lvi = glyphList.SelectedItems[0];
                Glyph glyph = (Glyph) activeGlyphDatabase[lvi.Text].Clone( );
                glyphNameInEditor = glyph.Name;
                // ... and pass it to the glyph editting form
                EditGlyphForm glyphForm = new EditGlyphForm( glyph, activeGlyphDatabase.GetGlyphNames( ) );
                glyphForm.Text = "Edit Glyph";

                // set glyph data checking handler
                glyphForm.SetGlyphDataCheckingHandeler( new GlyphDataCheckingHandeler( CheckGlyphData ) );

                if ( glyphForm.ShowDialog( ) == DialogResult.OK )
                {
                    try
                    {
                        // replace glyph in the database
                        lock ( sync )
                        {
                            activeGlyphDatabase.Replace( glyphNameInEditor, glyph );
                        }

                        lvi.Text = glyph.Name;

                        // temporary remove icon from the list item
                        lvi.ImageKey = null;

                        // remove old icon and add new one
                        glyphsImageList.Images.RemoveByKey( glyphNameInEditor );
                        glyphsImageList.Images.Add( glyph.Name, CreateGlyphIcon( glyph ) );

                        // restore item's icon
                        lvi.ImageKey = glyph.Name;
                    }
                    catch
                    {
                        ShowErrorBox( string.Format( "A glyph with the name '{0}' already exists in the database.", glyph.Name ) );
                    }
                }
            }
        }

        string glyphNameInEditor = string.Empty;

        // Handler for checking glyph data - need to make sure there is not such glyph in database already
        private bool CheckGlyphData( byte[,] glyphData )
        {
            if ( activeGlyphDatabase != null )
            {
                int rotation;
                Glyph recognizedGlyph = activeGlyphDatabase.RecognizeGlyph( glyphData, out rotation );

                if ( ( recognizedGlyph != null ) && ( recognizedGlyph.Name != glyphNameInEditor ) )
                {
                    ShowErrorBox( "The database already contains a glyph which looks the same as it is or after rotation." );
                    return false;
                }
            }

            return true;
        }

        // Delete selected glyph
        private void deleteGlyphToolStripMenuItem_Click( object sender, EventArgs e )
        {
            if ( ( activeGlyphDatabase != null ) && ( glyphList.SelectedIndices.Count != 0 ) )
            {
                // get selected item
                ListViewItem lvi = glyphList.SelectedItems[0];

                // remove glyph from database, from list view and image list
                lock ( sync )
                {
                    activeGlyphDatabase.Remove( lvi.Text );
                }
                glyphList.Items.Remove( lvi );
                glyphsImageList.Images.RemoveByKey( lvi.Text );
            }
        }

        // Activate currently selected database
        private void activateToolStripMenuItem_Click( object sender, EventArgs e )
        {
            if ( glyphCollectionsList.SelectedIndices.Count == 1 )
            {
                ActivateGlyphDatabase( glyphCollectionsList.SelectedItems[0].Text );
            }
        }

        // Delete currently selected glyph database
        private void deleteToolStripMenuItem_Click( object sender, EventArgs e )
        {
            if ( glyphCollectionsList.SelectedIndices.Count == 1 )
            {
                string selecteItem = glyphCollectionsList.SelectedItems[0].Text;

                if ( selecteItem == activeGlyphDatabaseName )
                {
                    ActivateGlyphDatabase( null );
                }

                glyphDatabases.RemoveGlyphDatabase( selecteItem );
                glyphCollectionsList.Items.Remove( glyphCollectionsList.SelectedItems[0] );
            }
        }

        // Rename glyph collection
        private void renameToolStripMenuItem_Click( object sender, EventArgs e )
        {
            if ( glyphCollectionsList.SelectedIndices.Count == 1 )
            {
                glyphCollectionsList.Items[glyphCollectionsList.SelectedIndices[0]].BeginEdit( );
            }
        }

        // On after editing of collect name's label - check if the name is correct
        private void glyphCollectionsList_AfterLabelEdit( object sender, LabelEditEventArgs e )
        {
            if ( e.Label != null )
            {
                string newName = e.Label.Trim( );

                if ( newName == string.Empty )
                {
                    ShowErrorBox( "Collection name cannot be emtpy." );
                    e.CancelEdit = true;
                    return;
                }
                else
                {
                    string oldName = glyphCollectionsList.Items[e.Item].Text;

                    if ( oldName != newName )
                    {
                        if ( glyphDatabases.GetDatabaseNames( ).Contains( newName ) )
                        {
                            ShowErrorBox( "A collection with such name already exists." );
                            e.CancelEdit = true;
                            return;
                        }

                        glyphDatabases.RenameGlyphDatabase( oldName, newName );

                        // update name of active database if it was renamed
                        if ( activeGlyphDatabaseName == oldName )
                            activeGlyphDatabaseName = newName;

                        if ( newName != e.Label )
                        {
                            glyphCollectionsList.Items[e.Item].Text = newName;
                            e.CancelEdit = true;
                        }
                    }
                    else
                    {
                        e.CancelEdit = true;
                    }
                }
            }
        }

        // Set new visualization method
        private void visualizationTypeMenuItem_Click( object sender, EventArgs e )
        {
            ToolStripMenuItem item = (ToolStripMenuItem) sender;

            if ( item.Tag is VisualizationType )
            {
                imageProcessor.VisualizationType = (VisualizationType) item.Tag;

                lock ( this )
                {
                    if ( imageProcessor.VisualizationType == VisualizationType.Model )
                    {
                        if ( arForm == null )
                        {
                            arForm = new AugmentedRealityForm( );

                            arForm.FormClosing += new FormClosingEventHandler( arForm_FormClosing );
                            arForm.Show( );
                        }
                    }
                    else
                    {
                        if ( arForm != null )
                        {
                            arForm.Close( );
                        }
                    }
                }
            }
        }

        // Update status of glyph visualization menu items, so user can see what is selected
        private void visualizationTypeToolStripMenuItem_DropDownOpening( object sender, EventArgs e )
        {
            bordersToolStripMenuItem.Checked = ( imageProcessor.VisualizationType == VisualizationType.BorderOnly );
            namesToolStripMenuItem.Checked   = ( imageProcessor.VisualizationType == VisualizationType.Name );
            imagesToolStripMenuItem.Checked  = ( imageProcessor.VisualizationType == VisualizationType.Image );
            modelToolStripMenuItem.Checked   = ( imageProcessor.VisualizationType == VisualizationType.Model );
        }

        // Activate glyph database with the specified name
        private void ActivateGlyphDatabase( string name )
        {
            ListViewItem lvi;

            // deactivate previous database
            if ( activeGlyphDatabase != null )
            {
                lvi = GetListViewItemByName( glyphCollectionsList, activeGlyphDatabaseName );

                if ( lvi != null )
                {
                    Font font = new Font( lvi.Font, FontStyle.Regular );
                    lvi.Font = font;
                }
            }

            // activate new database
            activeGlyphDatabaseName = name;

            if ( name != null )
            {
                try
                {
                    activeGlyphDatabase = glyphDatabases[name];

                    lvi = GetListViewItemByName( glyphCollectionsList, name );

                    if ( lvi != null )
                    {
                        Font font = new Font( lvi.Font, FontStyle.Bold );
                        lvi.Font = font;
                    }
                }
                catch
                {
                }
            }
            else
            {
                activeGlyphDatabase = null;
            }

            // set the database to image processor ...
            imageProcessor.GlyphDatabase = activeGlyphDatabase;
            // ... and show it to user
            RefreshListOfGlyps( );
        }

        // Get item from a list view by its name
        private ListViewItem GetListViewItemByName( ListView lv, string name )
        {
            try
            {
                return lv.Items[name];
            }
            catch
            {
                return null;
            }
        }

        // Refresh the list displaying available databases of glyphss
        private void RefreshListOfGlyphDatabases( )
        {
            glyphCollectionsList.Items.Clear( );

            List<string> dbNames = glyphDatabases.GetDatabaseNames( );

            foreach ( string name in dbNames )
            {
                GlyphDatabase db = glyphDatabases[name];
                ListViewItem lvi = glyphCollectionsList.Items.Add( name );
                lvi.Name = name;

                lvi.SubItems.Add( string.Format( "{0}x{1}", db.Size, db.Size ) );
            }
        }

        // Refresh the list of glyph contained in currently active database
        private void RefreshListOfGlyps( )
        {
            // clear list view and its image list
            glyphList.Items.Clear( );
            glyphsImageList.Images.Clear( );

            if ( activeGlyphDatabase != null )
            {
                // update image list first
                foreach ( Glyph glyph in activeGlyphDatabase )
                {
                    // create icon for the glyph first
                    glyphsImageList.Images.Add( glyph.Name, CreateGlyphIcon( glyph ) );

                    // create glyph's list view item
                    ListViewItem lvi = glyphList.Items.Add( glyph.Name );
                    lvi.ImageKey = glyph.Name;
                }
            }
        }

        // Create icon for a glyph
        private Bitmap CreateGlyphIcon( Glyph glyph )
        {
            return CreateGlyphImage( glyph, 32 );
        }

        private Bitmap CreateGlyphImage( Glyph glyph, int width )
        {
            Bitmap bitmap = new Bitmap( width, width, System.Drawing.Imaging.PixelFormat.Format32bppArgb );

            int cellSize = width / glyph.Size;
            int glyphSize = glyph.Size;

            for ( int i = 0; i < width; i++ )
            {
                int yCell = i / cellSize;

                for ( int j = 0; j < width; j++ )
                {
                    int xCell = j / cellSize;

                    if ( ( yCell >= glyphSize ) || ( xCell >= glyphSize ) )
                    {
                        // set pixel to transparent if it outside of the glyph
                        bitmap.SetPixel( j, i, Color.Transparent );
                    }
                    else
                    {
                        // set pixel to black or white depending on glyph value
                        bitmap.SetPixel( j, i,
                            ( glyph.Data[yCell, xCell] == 0 ) ? Color.Black : Color.White );
                    }
                }
            }

            return bitmap;
        }

        // On new video frame
        private void videoSourcePlayer_NewFrame( object sender, ref Bitmap image )
        {
            if ( activeGlyphDatabase != null )
            {
                if ( image.PixelFormat == PixelFormat.Format8bppIndexed )
                {
                    // convert image to RGB if it is grayscale
                    GrayscaleToRGB filter = new GrayscaleToRGB( );

                    Bitmap temp = filter.Apply( image );
                    image.Dispose( );
                    image = temp;
                }

                lock ( sync )
                {
                    List<ExtractedGlyphData> glyphs = imageProcessor.ProcessImage( image );

                    if ( arForm != null )
                    {
                        List<VirtualModel> modelsToDisplay = new List<VirtualModel>( );

                        foreach ( ExtractedGlyphData glyph in glyphs )
                        {
                            if ( ( glyph.RecognizedGlyph != null ) &&
                                 ( glyph.RecognizedGlyph.UserData != null ) &&
                                 ( glyph.RecognizedGlyph.UserData is GlyphVisualizationData ) &&
                                 ( glyph.IsTransformationDetected ) )
                            {
                                modelsToDisplay.Add( new VirtualModel(
                                    ( (GlyphVisualizationData) glyph.RecognizedGlyph.UserData ).ModelName,
                                    glyph.TransformationMatrix,
                                    imageProcessor.GlyphSize ) );
                            }
                        }

                        arForm.UpdateScene( image, modelsToDisplay );
                    }
                }
            }
        }

        // On finishing video playing
        private void videoSourcePlayer_PlayingFinished( object sender, ReasonToFinishPlaying reason )
        {
            if ( arForm != null )
            {
                arForm.UpdateScene( null, new List<VirtualModel>( ) );
            }
        }

        // On closing the Augmented Reality form
        private void arForm_FormClosing( object sender, FormClosingEventArgs e )
        {
            arForm.FormClosing -= new FormClosingEventHandler( arForm_FormClosing );
            arForm = null;

            // reset visualization type if form was closed by user
            if ( imageProcessor.VisualizationType == VisualizationType.Model )
            {
                imageProcessor.VisualizationType = VisualizationType.Name;
            }
        }

        // Show configuration options
        private void optionsToolStripMenuItem_Click( object sender, EventArgs e )
        {
            OptionsForm optionsForm = new OptionsForm( );

            optionsForm.AutoDetectFocalLength = autoDetectFocalLength;
            optionsForm.CameraFocalLength = imageProcessor.CameraFocalLength;
            optionsForm.GlyphSize = imageProcessor.GlyphSize;

            if ( optionsForm.ShowDialog( ) == DialogResult.OK )
            {
                imageProcessor.GlyphSize = optionsForm.GlyphSize;
                autoDetectFocalLength = optionsForm.AutoDetectFocalLength;

                if ( !autoDetectFocalLength )
                {
                    imageProcessor.CameraFocalLength = optionsForm.CameraFocalLength;
                }
            }
        }

        // Print selected glyph
        private void printToolStripMenuItem_Click( object sender, EventArgs e )
        {
            try
            {
                printDialog.Document = printDocument;
                if ( printDialog.ShowDialog( ) == DialogResult.OK )
                {
                    printDocument.Print( );
                }
            }
            catch ( InvalidPrinterException ex )
            {
                ShowErrorBox( "Failed accessing printer device.\r\n\r\n" + ex.Message );
            }
        }

        // Preview printing of the selected glyph
        private void printPreviewToolStripMenuItem_Click( object sender, EventArgs e )
        {
            try
            {
                printPreviewDialog.Document = printDocument;
                printPreviewDialog.ShowDialog( );
            }
            catch ( InvalidPrinterException ex )
            {
                ShowErrorBox( "Failed accessing printer device.\r\n\r\n" + ex.Message );
            }
        }

        // Glyph's printing routine
        private void printDocument_PrintPage( object sender, System.Drawing.Printing.PrintPageEventArgs e )
        {
            if ( ( activeGlyphDatabase != null ) && ( glyphList.SelectedIndices.Count != 0 ) )
            {
                // get selected item and its glyph ...
                ListViewItem lvi = glyphList.SelectedItems[0];
                Glyph glyph = activeGlyphDatabase[lvi.Text];

                // convert glyph size from mm to inches
                float glyphSizeInches = imageProcessor.GlyphSize / 25.4f;
                // calculate image width at 96 DPI resolution
                int imageWidth = (int) ( glyphSizeInches * 96 );

                // get glyph's image
                Bitmap glyphImage = CreateGlyphImage( glyph, imageWidth );
                glyphImage.SetResolution( 96, 96 );

                float xInches = ( (float) e.PageBounds.Width / 100 - glyphSizeInches ) / 2;
                float yInches = ( (float) e.PageBounds.Height / 100 - glyphSizeInches ) / 2;

                int xPixels = (int) ( xInches * e.Graphics.VisibleClipBounds.Width / ( e.PageBounds.Width / 100 ) );
                int yPixels = (int) ( yInches * e.Graphics.VisibleClipBounds.Height / ( e.PageBounds.Height / 100 ) );

                e.Graphics.DrawImage( glyphImage, xPixels, yPixels );
            }
        }
    }
}