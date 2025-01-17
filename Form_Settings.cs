﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace OMRON_IFZ_Viewer
{
    public partial class Form_Settings : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );

        public Form_Settings()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            this.ShowInTaskbar = false;
            this.TopMost = true; //permet d'avoir cette fenêtre non modale toujours devant.
            lblVersion.Text =Application.ProductVersion;
            Translation();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private const int CS_DROPSHADOW = 0x00020000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        private void Form_Settings_Load(object sender, EventArgs e)
        {
            switch (Properties.Settings.Default.LangueSoft)
            {
                case "fr-FR":
                    comboBox1.SelectedIndex = 0;
                    break;
                case "it-IT":
                    comboBox1.SelectedIndex = 2;
                    break;
                default:
                    comboBox1.SelectedIndex = 1;
                    break;

            }
            comboBox2.SelectedIndex = Properties.Settings.Default.ZoomMode;
           
                // lblVersion.Text= Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string language;
            switch (comboBox1.SelectedItem)
            {
                case "Français":
                    language = "fr-FR";
                    break;
                case "Italiano":
                    language = "it-IT";
                    break;
                default:
                    language = "en-US";
                    break;

            }
            Properties.Settings.Default.LangueSoft = language;
            Translation();
        }

        private void Translation()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.LangueSoft);
            lblTitle.Text = Properties.strings.Settings_Title;
            lbl1.Text = Properties.strings.Settings_lbl1;
            lbl2.Text = Properties.strings.Settings_lbl2;
            lbl3.Text = Properties.strings.Settings_lbl3;
            lbl4.Text = Properties.strings.Settings_lbl4;
            lbl5.Text = Properties.strings.Settings_lbl5;
            
            comboBox2.Items.Clear();
            comboBox2.Items.Add(Properties.strings.Settings_cb2_opt1);
            comboBox2.Items.Add(Properties.strings.Settings_cb2_opt2);
        
            btnClose.Text = Properties.strings.Settings_btnClose;

            Refresh();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ZoomMode = comboBox2.SelectedIndex;
            Console.WriteLine(comboBox2.SelectedIndex);
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            Process process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = @".\ServerRegistrationManager.exe";
            process.StartInfo.Arguments = "install IfzThumbnailHandler.dll -codebase";
            process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            process.Start();
            process.WaitForExit();// Waits here for the process to exit.
        }

        private void email_Click(object sender, EventArgs e)
        {
            var url = "mailto:jerome.pinard@omron.com";
            Process.Start(url);
        }
    }
}
