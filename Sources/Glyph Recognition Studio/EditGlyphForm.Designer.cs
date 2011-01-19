namespace GlyphRecognitionStudio
{
    partial class EditGlyphForm
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
            this.label1 = new System.Windows.Forms.Label( );
            this.nameBox = new System.Windows.Forms.TextBox( );
            this.groupBox1 = new System.Windows.Forms.GroupBox( );
            this.glyphEditor = new GlyphRecognitionStudio.GlyphEditorControl( );
            this.okButton = new System.Windows.Forms.Button( );
            this.cancelButton = new System.Windows.Forms.Button( );
            this.errorProvider = new System.Windows.Forms.ErrorProvider( this.components );
            this.groupBox2 = new System.Windows.Forms.GroupBox( );
            this.pictureBox = new System.Windows.Forms.PictureBox( );
            this.label3 = new System.Windows.Forms.Label( );
            this.colorButton = new System.Windows.Forms.Button( );
            this.label2 = new System.Windows.Forms.Label( );
            this.colorDialog = new System.Windows.Forms.ColorDialog( );
            this.modelBox = new System.Windows.Forms.PictureBox( );
            this.label4 = new System.Windows.Forms.Label( );
            this.groupBox1.SuspendLayout( );
            ( (System.ComponentModel.ISupportInitialize) ( this.errorProvider ) ).BeginInit( );
            this.groupBox2.SuspendLayout( );
            ( (System.ComponentModel.ISupportInitialize) ( this.pictureBox ) ).BeginInit( );
            ( (System.ComponentModel.ISupportInitialize) ( this.modelBox ) ).BeginInit( );
            this.SuspendLayout( );
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 10, 23 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 38, 13 );
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point( 55, 20 );
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size( 195, 20 );
            this.nameBox.TabIndex = 1;
            this.nameBox.TextChanged += new System.EventHandler( this.nameBox_TextChanged );
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add( this.glyphEditor );
            this.groupBox1.Location = new System.Drawing.Point( 10, 53 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 250, 260 );
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Glyph Image";
            // 
            // glyphEditor
            // 
            this.glyphEditor.GlyphData = null;
            this.glyphEditor.Location = new System.Drawing.Point( 10, 20 );
            this.glyphEditor.Name = "glyphEditor";
            this.glyphEditor.Size = new System.Drawing.Size( 230, 230 );
            this.glyphEditor.TabIndex = 0;
            // 
            // okButton
            // 
            this.okButton.Enabled = false;
            this.okButton.Location = new System.Drawing.Point( 55, 440 );
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size( 75, 23 );
            this.okButton.TabIndex = 3;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler( this.okButton_Click );
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point( 140, 440 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add( this.modelBox );
            this.groupBox2.Controls.Add( this.label4 );
            this.groupBox2.Controls.Add( this.pictureBox );
            this.groupBox2.Controls.Add( this.label3 );
            this.groupBox2.Controls.Add( this.colorButton );
            this.groupBox2.Controls.Add( this.label2 );
            this.groupBox2.Location = new System.Drawing.Point( 10, 320 );
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size( 250, 100 );
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Visualization";
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point( 100, 35 );
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size( 50, 50 );
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 3;
            this.pictureBox.TabStop = false;
            this.pictureBox.Click += new System.EventHandler( this.pictureBox_Click );
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 105, 20 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 39, 13 );
            this.label3.TabIndex = 2;
            this.label3.Text = "Image:";
            // 
            // colorButton
            // 
            this.colorButton.Location = new System.Drawing.Point( 20, 35 );
            this.colorButton.Name = "colorButton";
            this.colorButton.Size = new System.Drawing.Size( 50, 50 );
            this.colorButton.TabIndex = 1;
            this.colorButton.UseVisualStyleBackColor = true;
            this.colorButton.Click += new System.EventHandler( this.colorButton_Click );
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 10, 20 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 78, 13 );
            this.label2.TabIndex = 0;
            this.label2.Text = "Highlight &Color:";
            // 
            // modelBox
            // 
            this.modelBox.BackColor = System.Drawing.SystemColors.ControlDark;
            this.modelBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.modelBox.Location = new System.Drawing.Point( 180, 35 );
            this.modelBox.Name = "modelBox";
            this.modelBox.Size = new System.Drawing.Size( 50, 50 );
            this.modelBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.modelBox.TabIndex = 5;
            this.modelBox.TabStop = false;
            this.modelBox.Click += new System.EventHandler( this.modelBox_Click );
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 177, 19 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 56, 13 );
            this.label4.TabIndex = 4;
            this.label4.Text = "3D Model:";
            // 
            // EditGlyphForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size( 271, 472 );
            this.Controls.Add( this.groupBox2 );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.okButton );
            this.Controls.Add( this.groupBox1 );
            this.Controls.Add( this.nameBox );
            this.Controls.Add( this.label1 );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditGlyphForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Glyph";
            this.groupBox1.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize) ( this.errorProvider ) ).EndInit( );
            this.groupBox2.ResumeLayout( false );
            this.groupBox2.PerformLayout( );
            ( (System.ComponentModel.ISupportInitialize) ( this.pictureBox ) ).EndInit( );
            ( (System.ComponentModel.ISupportInitialize) ( this.modelBox ) ).EndInit( );
            this.ResumeLayout( false );
            this.PerformLayout( );

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private GlyphEditorControl glyphEditor;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button colorButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox modelBox;
        private System.Windows.Forms.Label label4;
    }
}