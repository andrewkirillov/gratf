namespace GlyphRecognitionStudio
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose( );
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent( )
        {
            this.components = new System.ComponentModel.Container( );
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( MainForm ) );
            this.mainMenu = new System.Windows.Forms.MenuStrip( );
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
            this.localVideoCaptureDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
            this.openVideofileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator( );
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
            this.settingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
            this.visualizationTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
            this.bordersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
            this.namesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
            this.imagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
            this.modelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator( );
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
            this.statusStrip = new System.Windows.Forms.StatusStrip( );
            this.fpsLabel = new System.Windows.Forms.ToolStripStatusLabel( );
            this.restStatusLabel = new System.Windows.Forms.ToolStripStatusLabel( );
            this.mainPanel = new System.Windows.Forms.Panel( );
            this.splitContainer = new System.Windows.Forms.SplitContainer( );
            this.videoSourcePlayer = new AForge.Controls.VideoSourcePlayer( );
            this.groupBox2 = new System.Windows.Forms.GroupBox( );
            this.glyphList = new System.Windows.Forms.ListView( );
            this.glyphCollectionContextMenu = new System.Windows.Forms.ContextMenuStrip( this.components );
            this.newGlyphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
            this.editGlyphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
            this.deleteGlyphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
            this.groupBox1 = new System.Windows.Forms.GroupBox( );
            this.glyphCollectionsList = new System.Windows.Forms.ListView( );
            this.nameHeader = new System.Windows.Forms.ColumnHeader( );
            this.sizeHeader = new System.Windows.Forms.ColumnHeader( );
            this.glyphCollectionsContextMenu = new System.Windows.Forms.ContextMenuStrip( this.components );
            this.activateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator( );
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
            this.videoSourcePlayer1 = new AForge.Controls.VideoSourcePlayer( );
            this.videoSourcePlayer2 = new AForge.Controls.VideoSourcePlayer( );
            this.timer = new System.Windows.Forms.Timer( this.components );
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog( );
            this.toolTip = new System.Windows.Forms.ToolTip( this.components );
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator( );
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
            this.printPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
            this.printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog( );
            this.printDialog = new System.Windows.Forms.PrintDialog( );
            this.printDocument = new System.Drawing.Printing.PrintDocument( );
            this.mainMenu.SuspendLayout( );
            this.statusStrip.SuspendLayout( );
            this.mainPanel.SuspendLayout( );
            this.splitContainer.Panel1.SuspendLayout( );
            this.splitContainer.Panel2.SuspendLayout( );
            this.splitContainer.SuspendLayout( );
            this.groupBox2.SuspendLayout( );
            this.glyphCollectionContextMenu.SuspendLayout( );
            this.groupBox1.SuspendLayout( );
            this.glyphCollectionsContextMenu.SuspendLayout( );
            this.SuspendLayout( );
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingToolStripMenuItem,
            this.helpToolStripMenuItem} );
            this.mainMenu.Location = new System.Drawing.Point( 0, 0 );
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size( 694, 24 );
            this.mainMenu.TabIndex = 0;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.localVideoCaptureDeviceToolStripMenuItem,
            this.openVideofileToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem} );
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size( 37, 20 );
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // localVideoCaptureDeviceToolStripMenuItem
            // 
            this.localVideoCaptureDeviceToolStripMenuItem.Name = "localVideoCaptureDeviceToolStripMenuItem";
            this.localVideoCaptureDeviceToolStripMenuItem.Size = new System.Drawing.Size( 218, 22 );
            this.localVideoCaptureDeviceToolStripMenuItem.Text = "Local &Video Capture Device";
            this.localVideoCaptureDeviceToolStripMenuItem.Click += new System.EventHandler( this.localVideoCaptureDeviceToolStripMenuItem_Click );
            // 
            // openVideofileToolStripMenuItem
            // 
            this.openVideofileToolStripMenuItem.Name = "openVideofileToolStripMenuItem";
            this.openVideofileToolStripMenuItem.Size = new System.Drawing.Size( 218, 22 );
            this.openVideofileToolStripMenuItem.Text = "Open Video &File";
            this.openVideofileToolStripMenuItem.Click += new System.EventHandler( this.openVideofileToolStripMenuItem_Click );
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size( 215, 6 );
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size( 218, 22 );
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler( this.exitToolStripMenuItem_Click );
            // 
            // settingToolStripMenuItem
            // 
            this.settingToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.visualizationTypeToolStripMenuItem,
            this.toolStripMenuItem3,
            this.optionsToolStripMenuItem} );
            this.settingToolStripMenuItem.Name = "settingToolStripMenuItem";
            this.settingToolStripMenuItem.Size = new System.Drawing.Size( 61, 20 );
            this.settingToolStripMenuItem.Text = "&Settings";
            // 
            // visualizationTypeToolStripMenuItem
            // 
            this.visualizationTypeToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.bordersToolStripMenuItem,
            this.namesToolStripMenuItem,
            this.imagesToolStripMenuItem,
            this.modelToolStripMenuItem} );
            this.visualizationTypeToolStripMenuItem.Name = "visualizationTypeToolStripMenuItem";
            this.visualizationTypeToolStripMenuItem.Size = new System.Drawing.Size( 169, 22 );
            this.visualizationTypeToolStripMenuItem.Text = "&Visualization Type";
            this.visualizationTypeToolStripMenuItem.DropDownOpening += new System.EventHandler( this.visualizationTypeToolStripMenuItem_DropDownOpening );
            // 
            // bordersToolStripMenuItem
            // 
            this.bordersToolStripMenuItem.Name = "bordersToolStripMenuItem";
            this.bordersToolStripMenuItem.Size = new System.Drawing.Size( 125, 22 );
            this.bordersToolStripMenuItem.Text = "&Borders";
            this.bordersToolStripMenuItem.Click += new System.EventHandler( this.visualizationTypeMenuItem_Click );
            // 
            // namesToolStripMenuItem
            // 
            this.namesToolStripMenuItem.Name = "namesToolStripMenuItem";
            this.namesToolStripMenuItem.Size = new System.Drawing.Size( 125, 22 );
            this.namesToolStripMenuItem.Text = "&Names";
            this.namesToolStripMenuItem.Click += new System.EventHandler( this.visualizationTypeMenuItem_Click );
            // 
            // imagesToolStripMenuItem
            // 
            this.imagesToolStripMenuItem.Name = "imagesToolStripMenuItem";
            this.imagesToolStripMenuItem.Size = new System.Drawing.Size( 125, 22 );
            this.imagesToolStripMenuItem.Text = "&Images";
            this.imagesToolStripMenuItem.Click += new System.EventHandler( this.visualizationTypeMenuItem_Click );
            // 
            // modelToolStripMenuItem
            // 
            this.modelToolStripMenuItem.Name = "modelToolStripMenuItem";
            this.modelToolStripMenuItem.Size = new System.Drawing.Size( 125, 22 );
            this.modelToolStripMenuItem.Text = "3D &Model";
            this.modelToolStripMenuItem.Click += new System.EventHandler( this.visualizationTypeMenuItem_Click );
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size( 166, 6 );
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size( 169, 22 );
            this.optionsToolStripMenuItem.Text = "&Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler( this.optionsToolStripMenuItem_Click );
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem} );
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size( 44, 20 );
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size( 107, 22 );
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler( this.aboutToolStripMenuItem_Click );
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.fpsLabel,
            this.restStatusLabel} );
            this.statusStrip.Location = new System.Drawing.Point( 0, 449 );
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size( 694, 22 );
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // fpsLabel
            // 
            this.fpsLabel.AutoSize = false;
            this.fpsLabel.BorderSides = ( (System.Windows.Forms.ToolStripStatusLabelBorderSides) ( ( ( ( System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top )
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right )
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom ) ) );
            this.fpsLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.fpsLabel.Name = "fpsLabel";
            this.fpsLabel.Size = new System.Drawing.Size( 100, 17 );
            this.fpsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // restStatusLabel
            // 
            this.restStatusLabel.BorderSides = ( (System.Windows.Forms.ToolStripStatusLabelBorderSides) ( ( ( ( System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top )
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right )
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom ) ) );
            this.restStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.restStatusLabel.Name = "restStatusLabel";
            this.restStatusLabel.Size = new System.Drawing.Size( 579, 17 );
            this.restStatusLabel.Spring = true;
            this.restStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add( this.splitContainer );
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point( 0, 24 );
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size( 694, 425 );
            this.mainPanel.TabIndex = 2;
            // 
            // splitContainer
            // 
            this.splitContainer.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add( this.videoSourcePlayer );
            this.splitContainer.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add( this.groupBox2 );
            this.splitContainer.Panel2.Controls.Add( this.groupBox1 );
            this.splitContainer.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer.Size = new System.Drawing.Size( 694, 425 );
            this.splitContainer.SplitterDistance = 461;
            this.splitContainer.TabIndex = 0;
            // 
            // videoSourcePlayer
            // 
            this.videoSourcePlayer.AutoSizeControl = true;
            this.videoSourcePlayer.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.videoSourcePlayer.ForeColor = System.Drawing.Color.White;
            this.videoSourcePlayer.Location = new System.Drawing.Point( 67, 89 );
            this.videoSourcePlayer.Name = "videoSourcePlayer";
            this.videoSourcePlayer.Size = new System.Drawing.Size( 322, 242 );
            this.videoSourcePlayer.TabIndex = 0;
            this.videoSourcePlayer.VideoSource = null;
            this.videoSourcePlayer.PlayingFinished += new AForge.Video.PlayingFinishedEventHandler( this.videoSourcePlayer_PlayingFinished );
            this.videoSourcePlayer.NewFrame += new AForge.Controls.VideoSourcePlayer.NewFrameHandler( this.videoSourcePlayer_NewFrame );
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.groupBox2.Controls.Add( this.glyphList );
            this.groupBox2.Location = new System.Drawing.Point( 5, 130 );
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size( 214, 288 );
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Glyphs";
            // 
            // glyphList
            // 
            this.glyphList.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.glyphList.ContextMenuStrip = this.glyphCollectionContextMenu;
            this.glyphList.Location = new System.Drawing.Point( 5, 20 );
            this.glyphList.MultiSelect = false;
            this.glyphList.Name = "glyphList";
            this.glyphList.Size = new System.Drawing.Size( 203, 262 );
            this.glyphList.TabIndex = 0;
            this.toolTip.SetToolTip( this.glyphList, "Right click for options" );
            this.glyphList.UseCompatibleStateImageBehavior = false;
            this.glyphList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler( this.glyphList_MouseDoubleClick );
            // 
            // glyphCollectionContextMenu
            // 
            this.glyphCollectionContextMenu.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.newGlyphToolStripMenuItem,
            this.editGlyphToolStripMenuItem,
            this.deleteGlyphToolStripMenuItem,
            this.toolStripMenuItem4,
            this.printToolStripMenuItem,
            this.printPreviewToolStripMenuItem} );
            this.glyphCollectionContextMenu.Name = "glyphCollectionContextMenu";
            this.glyphCollectionContextMenu.Size = new System.Drawing.Size( 144, 120 );
            this.glyphCollectionContextMenu.Opening += new System.ComponentModel.CancelEventHandler( this.glyphCollectionContextMenu_Opening );
            // 
            // newGlyphToolStripMenuItem
            // 
            this.newGlyphToolStripMenuItem.Name = "newGlyphToolStripMenuItem";
            this.newGlyphToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
            this.newGlyphToolStripMenuItem.Text = "&New";
            this.newGlyphToolStripMenuItem.Click += new System.EventHandler( this.newGlyphToolStripMenuItem_Click );
            // 
            // editGlyphToolStripMenuItem
            // 
            this.editGlyphToolStripMenuItem.Name = "editGlyphToolStripMenuItem";
            this.editGlyphToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
            this.editGlyphToolStripMenuItem.Text = "&Edit";
            this.editGlyphToolStripMenuItem.Click += new System.EventHandler( this.editGlyphToolStripMenuItem_Click );
            // 
            // deleteGlyphToolStripMenuItem
            // 
            this.deleteGlyphToolStripMenuItem.Name = "deleteGlyphToolStripMenuItem";
            this.deleteGlyphToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
            this.deleteGlyphToolStripMenuItem.Text = "&Delete";
            this.deleteGlyphToolStripMenuItem.Click += new System.EventHandler( this.deleteGlyphToolStripMenuItem_Click );
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.groupBox1.Controls.Add( this.glyphCollectionsList );
            this.groupBox1.Location = new System.Drawing.Point( 5, 5 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 214, 120 );
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Glyph Collections";
            // 
            // glyphCollectionsList
            // 
            this.glyphCollectionsList.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.glyphCollectionsList.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.nameHeader,
            this.sizeHeader} );
            this.glyphCollectionsList.ContextMenuStrip = this.glyphCollectionsContextMenu;
            this.glyphCollectionsList.FullRowSelect = true;
            this.glyphCollectionsList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.glyphCollectionsList.LabelEdit = true;
            this.glyphCollectionsList.Location = new System.Drawing.Point( 5, 20 );
            this.glyphCollectionsList.MultiSelect = false;
            this.glyphCollectionsList.Name = "glyphCollectionsList";
            this.glyphCollectionsList.Size = new System.Drawing.Size( 203, 94 );
            this.glyphCollectionsList.TabIndex = 0;
            this.toolTip.SetToolTip( this.glyphCollectionsList, "Right click for options" );
            this.glyphCollectionsList.UseCompatibleStateImageBehavior = false;
            this.glyphCollectionsList.View = System.Windows.Forms.View.Details;
            this.glyphCollectionsList.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler( this.glyphCollectionsList_AfterLabelEdit );
            // 
            // nameHeader
            // 
            this.nameHeader.Text = "Name";
            this.nameHeader.Width = 120;
            // 
            // sizeHeader
            // 
            this.sizeHeader.Text = "Size";
            // 
            // glyphCollectionsContextMenu
            // 
            this.glyphCollectionsContextMenu.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.activateToolStripMenuItem,
            this.toolStripMenuItem2,
            this.newToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.deleteToolStripMenuItem} );
            this.glyphCollectionsContextMenu.Name = "glyphCollectionsContextMenu";
            this.glyphCollectionsContextMenu.Size = new System.Drawing.Size( 118, 98 );
            this.glyphCollectionsContextMenu.Opening += new System.ComponentModel.CancelEventHandler( this.glyphCollectionsContextMenu_Opening );
            // 
            // activateToolStripMenuItem
            // 
            this.activateToolStripMenuItem.Name = "activateToolStripMenuItem";
            this.activateToolStripMenuItem.Size = new System.Drawing.Size( 117, 22 );
            this.activateToolStripMenuItem.Text = "&Activate";
            this.activateToolStripMenuItem.Click += new System.EventHandler( this.activateToolStripMenuItem_Click );
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size( 114, 6 );
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size( 117, 22 );
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler( this.newToolStripMenuItem_Click );
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size( 117, 22 );
            this.renameToolStripMenuItem.Text = "&Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler( this.renameToolStripMenuItem_Click );
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size( 117, 22 );
            this.deleteToolStripMenuItem.Text = "&Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler( this.deleteToolStripMenuItem_Click );
            // 
            // videoSourcePlayer1
            // 
            this.videoSourcePlayer1.Location = new System.Drawing.Point( 0, 0 );
            this.videoSourcePlayer1.Name = "videoSourcePlayer1";
            this.videoSourcePlayer1.Size = new System.Drawing.Size( 0, 0 );
            this.videoSourcePlayer1.TabIndex = 0;
            this.videoSourcePlayer1.VideoSource = null;
            // 
            // videoSourcePlayer2
            // 
            this.videoSourcePlayer2.Location = new System.Drawing.Point( 0, 0 );
            this.videoSourcePlayer2.Name = "videoSourcePlayer2";
            this.videoSourcePlayer2.Size = new System.Drawing.Size( 0, 0 );
            this.videoSourcePlayer2.TabIndex = 0;
            this.videoSourcePlayer2.VideoSource = null;
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler( this.timer_Tick );
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "AVI files (*.avi)|*.avi|All files (*.*)|*.*";
            this.openFileDialog.Title = "Opem movie";
            // 
            // toolTip
            // 
            this.toolTip.BackColor = System.Drawing.Color.FromArgb( ( (int) ( ( (byte) ( 192 ) ) ) ), ( (int) ( ( (byte) ( 255 ) ) ) ), ( (int) ( ( (byte) ( 192 ) ) ) ) );
            this.toolTip.ToolTipTitle = "Hint:";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size( 149, 6 );
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
            this.printToolStripMenuItem.Text = "&Print";
            this.printToolStripMenuItem.Click += new System.EventHandler( this.printToolStripMenuItem_Click );
            // 
            // printPreviewToolStripMenuItem
            // 
            this.printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
            this.printPreviewToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
            this.printPreviewToolStripMenuItem.Text = "Print Preview";
            this.printPreviewToolStripMenuItem.Click += new System.EventHandler( this.printPreviewToolStripMenuItem_Click );
            // 
            // printPreviewDialog
            // 
            this.printPreviewDialog.AutoScrollMargin = new System.Drawing.Size( 0, 0 );
            this.printPreviewDialog.AutoScrollMinSize = new System.Drawing.Size( 0, 0 );
            this.printPreviewDialog.ClientSize = new System.Drawing.Size( 400, 300 );
            this.printPreviewDialog.Enabled = true;
            this.printPreviewDialog.Icon = ( (System.Drawing.Icon) ( resources.GetObject( "printPreviewDialog.Icon" ) ) );
            this.printPreviewDialog.Name = "printPreviewDialog";
            this.printPreviewDialog.Visible = false;
            // 
            // printDialog
            // 
            this.printDialog.UseEXDialog = true;
            // 
            // printDocument
            // 
            this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler( this.printDocument_PrintPage );
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 694, 471 );
            this.Controls.Add( this.mainPanel );
            this.Controls.Add( this.statusStrip );
            this.Controls.Add( this.mainMenu );
            this.Icon = ( (System.Drawing.Icon) ( resources.GetObject( "$this.Icon" ) ) );
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.Text = "Glyph Recognition Studio";
            this.Load += new System.EventHandler( this.MainForm_Load );
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.MainForm_FormClosing );
            this.mainMenu.ResumeLayout( false );
            this.mainMenu.PerformLayout( );
            this.statusStrip.ResumeLayout( false );
            this.statusStrip.PerformLayout( );
            this.mainPanel.ResumeLayout( false );
            this.splitContainer.Panel1.ResumeLayout( false );
            this.splitContainer.Panel2.ResumeLayout( false );
            this.splitContainer.ResumeLayout( false );
            this.groupBox2.ResumeLayout( false );
            this.glyphCollectionContextMenu.ResumeLayout( false );
            this.groupBox1.ResumeLayout( false );
            this.glyphCollectionsContextMenu.ResumeLayout( false );
            this.ResumeLayout( false );
            this.PerformLayout( );

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private AForge.Controls.VideoSourcePlayer videoSourcePlayer1;
        private AForge.Controls.VideoSourcePlayer videoSourcePlayer2;
        private AForge.Controls.VideoSourcePlayer videoSourcePlayer;
        private System.Windows.Forms.ToolStripMenuItem localVideoCaptureDeviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openVideofileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripStatusLabel fpsLabel;
        private System.Windows.Forms.ToolStripStatusLabel restStatusLabel;
        private System.Windows.Forms.ListView glyphCollectionsList;
        private System.Windows.Forms.ColumnHeader nameHeader;
        private System.Windows.Forms.ColumnHeader sizeHeader;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ContextMenuStrip glyphCollectionsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem activateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView glyphList;
        private System.Windows.Forms.ContextMenuStrip glyphCollectionContextMenu;
        private System.Windows.Forms.ToolStripMenuItem newGlyphToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editGlyphToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteGlyphToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem visualizationTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bordersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem namesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modelToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printPreviewToolStripMenuItem;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog;
        private System.Windows.Forms.PrintDialog printDialog;
        private System.Drawing.Printing.PrintDocument printDocument;
    }
}

