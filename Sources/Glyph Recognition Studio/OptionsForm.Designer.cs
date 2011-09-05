namespace GlyphRecognitionStudio
{
    partial class OptionsForm
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
            this.autoDetectFocalLengthCheck = new System.Windows.Forms.CheckBox( );
            this.groupBox1 = new System.Windows.Forms.GroupBox( );
            this.focalLengthBox = new System.Windows.Forms.TextBox( );
            this.label1 = new System.Windows.Forms.Label( );
            this.groupBox2 = new System.Windows.Forms.GroupBox( );
            this.glyphSizeBox = new System.Windows.Forms.TextBox( );
            this.label2 = new System.Windows.Forms.Label( );
            this.okButton = new System.Windows.Forms.Button( );
            this.cancelButton = new System.Windows.Forms.Button( );
            this.errorProvider = new System.Windows.Forms.ErrorProvider( this.components );
            this.label3 = new System.Windows.Forms.Label( );
            this.groupBox1.SuspendLayout( );
            this.groupBox2.SuspendLayout( );
            ( (System.ComponentModel.ISupportInitialize) ( this.errorProvider ) ).BeginInit( );
            this.SuspendLayout( );
            // 
            // autoDetectFocalLengthCheck
            // 
            this.autoDetectFocalLengthCheck.AutoSize = true;
            this.autoDetectFocalLengthCheck.Location = new System.Drawing.Point( 10, 25 );
            this.autoDetectFocalLengthCheck.Name = "autoDetectFocalLengthCheck";
            this.autoDetectFocalLengthCheck.Size = new System.Drawing.Size( 275, 17 );
            this.autoDetectFocalLengthCheck.TabIndex = 0;
            this.autoDetectFocalLengthCheck.Text = "&Autodetect camera\'s focal length (set to image width)";
            this.autoDetectFocalLengthCheck.UseVisualStyleBackColor = true;
            this.autoDetectFocalLengthCheck.CheckedChanged += new System.EventHandler( this.autoDetectFocalLengthCheck_CheckedChanged );
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add( this.focalLengthBox );
            this.groupBox1.Controls.Add( this.label1 );
            this.groupBox1.Controls.Add( this.autoDetectFocalLengthCheck );
            this.groupBox1.Location = new System.Drawing.Point( 10, 10 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 305, 80 );
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Camera\'s effective focal length";
            // 
            // focalLengthBox
            // 
            this.focalLengthBox.Location = new System.Drawing.Point( 105, 50 );
            this.focalLengthBox.Name = "focalLengthBox";
            this.focalLengthBox.Size = new System.Drawing.Size( 70, 20 );
            this.focalLengthBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 25, 53 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 79, 13 );
            this.label1.TabIndex = 1;
            this.label1.Text = "&Manual setting:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add( this.label3 );
            this.groupBox2.Controls.Add( this.glyphSizeBox );
            this.groupBox2.Controls.Add( this.label2 );
            this.groupBox2.Location = new System.Drawing.Point( 10, 95 );
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size( 305, 70 );
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Glyphs\' properties";
            // 
            // glyphSizeBox
            // 
            this.glyphSizeBox.Location = new System.Drawing.Point( 120, 25 );
            this.glyphSizeBox.Name = "glyphSizeBox";
            this.glyphSizeBox.Size = new System.Drawing.Size( 70, 20 );
            this.glyphSizeBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 10, 28 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 110, 13 );
            this.label2.TabIndex = 0;
            this.label2.Text = "Glyph\'s real size (mm):";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point( 160, 180 );
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size( 75, 23 );
            this.okButton.TabIndex = 2;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler( this.okButton_Click );
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point( 240, 180 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font( "Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte) ( 204 ) ) );
            this.label3.Location = new System.Drawing.Point( 65, 50 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 234, 13 );
            this.label3.TabIndex = 2;
            this.label3.Text = "If set to 0, 3D augmented reality will be disabled.";
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size( 326, 215 );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.okButton );
            this.Controls.Add( this.groupBox2 );
            this.Controls.Add( this.groupBox1 );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler( this.OptionsForm_Load );
            this.groupBox1.ResumeLayout( false );
            this.groupBox1.PerformLayout( );
            this.groupBox2.ResumeLayout( false );
            this.groupBox2.PerformLayout( );
            ( (System.ComponentModel.ISupportInitialize) ( this.errorProvider ) ).EndInit( );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.CheckBox autoDetectFocalLengthCheck;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox focalLengthBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox glyphSizeBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label label3;
    }
}