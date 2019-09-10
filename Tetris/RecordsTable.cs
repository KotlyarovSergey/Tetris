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
    public partial class RecordsTable : Form
    {
        private int MaxLines;
        private List<Records> RecordsLst;

        public RecordsTable(int RecordsCount, List<Records> RecordsList)
        {
            InitializeComponent();
            MaxLines = RecordsCount;
            RecordsLst = RecordsList;
        }


        private void RecordsTable_Load(object sender, EventArgs e)
        {
            this.Height = MaxLines * 27 + 30;
            this.Top = this.Top - (MaxLines * 27 / 2);

            Label lbl;
            for (int i = 0; i < MaxLines; i++)
            {
                lbl = new Label();
                lbl.Parent = this;
                lbl.Location = new Point(12, 18 + i * 27);
                lbl.Size = new Size(183, 32);
                lbl.AutoSize = false;
                lbl.Font = new Font("Haettenschweiler", 15.75F, FontStyle.Regular);
                if (i % 2 == 0)
                    lbl.ForeColor = Color.OrangeRed;
                else
                    lbl.ForeColor = Color.DarkGreen;
                if (i < RecordsLst.Count)
                    lbl.Text = RecordsLst[i].Gamer;
                else
                    lbl.Text = "Пусто";

                lbl = new Label();
                lbl.Parent = this;
                lbl.Location = new Point(207, 18 + i * 27);
                lbl.Size = new Size(115, 23);
                lbl.AutoSize = false;
                lbl.TextAlign = ContentAlignment.MiddleRight;
                lbl.Font = new Font("Haettenschweiler", 15.75F, FontStyle.Regular);
                if (i % 2 == 0)
                    lbl.ForeColor = Color.OrangeRed;
                else
                    lbl.ForeColor = Color.DarkGreen;
                if (i < RecordsLst.Count)
                    lbl.Text = LongToString(RecordsLst[i].Score);
                else
                    lbl.Text = "0";


            }
        }

        private string LongToString(long value)
        {
            string s = value.ToString();
            int ln = s.Length;

            string rez = string.Empty;
            int kr = ln % 3;

            rez = s.Substring(0, kr) + " ";

            int t = 0;
            for (int i = kr; i < ln; i++)
            {
                rez += s[i];

                t++;
                if (t % 3 == 0)
                {
                    rez += " ";
                    t = 0;
                }
            }

            return rez.Trim();
        }

        private void lblExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RecordsTable_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27 || e.KeyChar == ' ')
                this.Close();
        }

        private void RecordsTable_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawRectangle(new Pen(new SolidBrush(Color.DarkCyan)),
                            new Rectangle(new Point(1, 1),
                                          new Size(this.Width - 2, this.Height - 2)));


        }
    }
}
