using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tetris
{
    public partial class YourName : Form
    {
        public string GamerName = string.Empty;
        
        private bool TextBoxFirstClick = true;
        private DialogResult DialRez = DialogResult.No;

        public YourName()
        {
            InitializeComponent();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (TextBoxFirstClick == true)
            {
                textBox1.Text = "";
                TextBoxFirstClick = false;
            }
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            GamerName = textBox1.Text;
            if (GamerName.Length > 100)
                GamerName = GamerName.Substring(0, 100);
            DialRez = System.Windows.Forms.DialogResult.Yes;
            this.Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            GamerName = string.Empty;
            DialRez = System.Windows.Forms.DialogResult.No;
            this.Close();
        }

        private void YourName_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = DialRez;
        }


    }
}
