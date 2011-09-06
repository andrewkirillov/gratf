namespace GlyphRecognitionStudio
{
    partial class AboutForm
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
            this.logoPictureBox = new System.Windows.Forms.PictureBox( );
            this.panel1 = new System.Windows.Forms.Panel( );
            this.blackLine = new System.Windows.Forms.Label( );
            this.label1 = new System.Windows.Forms.Label( );
            this.label2 = new System.Windows.Forms.Label( );
            this.gratfLabel = new System.Windows.Forms.LinkLabel( );
            this.aforgeLabel = new System.Windows.Forms.LinkLabel( );
            this.label3 = new System.Windows.Forms.Label( );
            this.mailLabel = new System.Windows.Forms.LinkLabel( );
            this.label4 = new System.Windows.Forms.Label( );
            this.okButton = new System.Windows.Forms.Button( );
            this.pictureBox1 = new System.Windows.Forms.PictureBox( );
            this.versionLabel = new System.Windows.Forms.Label( );
            ( (System.ComponentModel.ISupportInitialize) ( this.logoPictureBox ) ).BeginInit( );
            this.panel1.SuspendLayout( );
            ( (System.ComponentModel.ISupportInitialize) ( this.pictureBox1 ) ).BeginInit( );
            this.SuspendLayout( );
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Image = global::GlyphRecognitionStudio.Properties.Resources.gratf_logo;
            this.logoPictureBox.Location = new System.Drawing.Point( 50, 0 );
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size( 274, 54 );
            this.logoPictureBox.TabIndex = 0;
            this.logoPictureBox.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add( this.blackLine );
            this.panel1.Controls.Add( this.logoPictureBox );
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point( 0, 0 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 374, 55 );
            this.panel1.TabIndex = 1;
            // 
            // blackLine
            // 
            this.blackLine.BackColor = System.Drawing.Color.Black;
            this.blackLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.blackLine.Location = new System.Drawing.Point( 0, 54 );
            this.blackLine.Name = "blackLine";
            this.blackLine.Size = new System.Drawing.Size( 374, 1 );
            this.blackLine.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte) ( 204 ) ) );
            this.label1.Location = new System.Drawing.Point( 112, 70 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 151, 13 );
            this.label1.TabIndex = 2;
            this.label1.Text = "Glyph Recognition Studio";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 40, 115 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 295, 13 );
            this.label2.TabIndex = 3;
            this.label2.Text = "GRATF project - Glyph Recognition and Tracking Framework";
            // 
            // gratfLabel
            // 
            this.gratfLabel.AutoSize = true;
            this.gratfLabel.Location = new System.Drawing.Point( 84, 130 );
            this.gratfLabel.Name = "gratfLabel";
            this.gratfLabel.Size = new System.Drawing.Size( 206, 13 );
            this.gratfLabel.TabIndex = 19;
            this.gratfLabel.TabStop = true;
            this.gratfLabel.Text = "http://www.aforgenet.com/projects/gratf/";
            this.gratfLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler( this.label_LinkClicked );
            // 
            // aforgeLabel
            // 
            this.aforgeLabel.AutoSize = true;
            this.aforgeLabel.Location = new System.Drawing.Point( 91, 230 );
            this.aforgeLabel.Name = "aforgeLabel";
            this.aforgeLabel.Size = new System.Drawing.Size( 192, 13 );
            this.aforgeLabel.TabIndex = 25;
            this.aforgeLabel.TabStop = true;
            this.aforgeLabel.Text = "http://www.aforgenet.com/framework/";
            this.aforgeLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler( this.label_LinkClicked );
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 104, 215 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 166, 13 );
            this.label3.TabIndex = 24;
            this.label3.Text = "Based on AForge.NET framework";
            // 
            // mailLabel
            // 
            this.mailLabel.ActiveLinkColor = System.Drawing.Color.MediumBlue;
            this.mailLabel.LinkColor = System.Drawing.Color.MediumBlue;
            this.mailLabel.Location = new System.Drawing.Point( 102, 180 );
            this.mailLabel.Name = "mailLabel";
            this.mailLabel.Size = new System.Drawing.Size( 171, 23 );
            this.mailLabel.TabIndex = 23;
            this.mailLabel.TabStop = true;
            this.mailLabel.Text = "andrew.kirillov@aforgenet.com";
            this.mailLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.mailLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler( this.label_LinkClicked );
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point( 81, 165 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 212, 16 );
            this.label4.TabIndex = 22;
            this.label4.Text = "Developed by Andrew Kirillov";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.okButton.Location = new System.Drawing.Point( 150, 270 );
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size( 75, 23 );
            this.okButton.TabIndex = 21;
            this.okButton.Text = "OK";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point( 15, 260 );
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size( 344, 2 );
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            // 
            // versionLabel
            // 
            this.versionLabel.Location = new System.Drawing.Point( 87, 85 );
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size( 200, 13 );
            this.versionLabel.TabIndex = 26;
            this.versionLabel.Text = "version";
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // AboutForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.okButton;
            this.ClientSize = new System.Drawing.Size( 374, 304 );
            this.Controls.Add( this.versionLabel );
            this.Controls.Add( this.aforgeLabel );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.mailLabel );
            this.Controls.Add( this.label4 );
            this.Controls.Add( this.okButton );
            this.Controls.Add( this.pictureBox1 );
            this.Controls.Add( this.gratfLabel );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.label1 );
            this.Controls.Add( this.panel1 );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            ( (System.ComponentModel.ISupportInitialize) ( this.logoPictureBox ) ).EndInit( );
            this.panel1.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize) ( this.pictureBox1 ) ).EndInit( );
            this.ResumeLayout( false );
            this.PerformLayout( );

        }

        #endregion

        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label blackLine;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel gratfLabel;
        private System.Windows.Forms.LinkLabel aforgeLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel mailLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label versionLabel;
    }
}