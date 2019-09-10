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
    public partial class frPause : Form
    {
        public frPause()
        {
            InitializeComponent();
        }

        private void frPause_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ' || e.KeyChar == 27 )
                this.Close();
        }

        private void frPause_Load(object sender, EventArgs e)
        {
            //this.ShowInTaskbar = false;
        }
    }
}
