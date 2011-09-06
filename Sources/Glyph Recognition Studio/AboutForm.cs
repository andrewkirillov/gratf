using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;

namespace GlyphRecognitionStudio
{
    public partial class AboutForm : Form
    {
        public AboutForm( )
        {
            InitializeComponent( );

            // get version
            versionLabel.Text = "Version: " + Assembly.GetExecutingAssembly( ).GetName( ).Version.ToString( );

            // set links
            mailLabel.Links.Add( 0, mailLabel.Text.Length, "mailto:andrew.kirillov@aforgenet.com" );
            aforgeLabel.Links.Add( 0, aforgeLabel.Text.Length, "http://www.aforgenet.com/framework/" );
            gratfLabel.Links.Add( 0, gratfLabel.Text.Length, "http://www.aforgenet.com/projects/gratf/" );
        }

        // On URL label click
        private void label_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
        {
            System.Diagnostics.Process.Start( e.Link.LinkData.ToString( ) );
        }
    }
}
