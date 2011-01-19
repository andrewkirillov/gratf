namespace Xna3DViewer
{
    partial class ModelSelectorForm
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
            this.cancelButton = new System.Windows.Forms.Button( );
            this.okButton = new System.Windows.Forms.Button( );
            this.modelViewerControl = new Xna3DViewer.ModelViewerControl( );
            this.modelsCombo = new System.Windows.Forms.ComboBox( );
            this.SuspendLayout( );
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point( 220, 250 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point( 135, 250 );
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size( 75, 23 );
            this.okButton.TabIndex = 3;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // modelViewerControl
            // 
            this.modelViewerControl.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.modelViewerControl.Location = new System.Drawing.Point( 11, 40 );
            this.modelViewerControl.Name = "modelViewerControl";
            this.modelViewerControl.Size = new System.Drawing.Size( 283, 204 );
            this.modelViewerControl.TabIndex = 5;
            // 
            // modelsCombo
            // 
            this.modelsCombo.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.modelsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modelsCombo.FormattingEnabled = true;
            this.modelsCombo.Location = new System.Drawing.Point( 10, 10 );
            this.modelsCombo.Name = "modelsCombo";
            this.modelsCombo.Size = new System.Drawing.Size( 283, 21 );
            this.modelsCombo.TabIndex = 7;
            this.modelsCombo.SelectedIndexChanged += new System.EventHandler( this.modelsCombo_SelectedIndexChanged );
            // 
            // ModelSelectorForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size( 304, 282 );
            this.Controls.Add( this.modelsCombo );
            this.Controls.Add( this.modelViewerControl );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.okButton );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModelSelectorForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Model";
            this.Load += new System.EventHandler( this.ModelSelectorForm_Load );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private ModelViewerControl modelViewerControl;
        private System.Windows.Forms.ComboBox modelsCombo;
    }
}