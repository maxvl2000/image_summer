using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace image_summer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (of.ShowDialog() != DialogResult.OK)
                return;
            try
            {
                picture.Image = new Bitmap(of.FileName);
            }
            catch { return; }
            }
        int x0 = 0, y0 = 0, x1=0, y1=0;

        private void picture_Paint(object sender, PaintEventArgs e)
        {
            if (picture.Image == null)
                return;

            Rectangle ee = new Rectangle(x0, y0, x1 - x0, y1 - y0);
            using (Pen pen = new Pen(Color.Red, 2))
            {
                e.Graphics.DrawRectangle(pen, ee);
            }

            //calc
            Bitmap b = picture.Image as Bitmap;
            long t = 0;
            for (int x = x0; x <= x1 && x < b.Width; x++)
                for (int y = y0; y <= y1 && y < b.Height; y++)
                    t += b.GetPixel(x, y).G;
            total.Text = t.ToString();
        }

        private void picture_MouseUp(object sender, MouseEventArgs e)
        {
            draw = false;
        }

        bool draw = false;

        private void picture_MouseMove(object sender, MouseEventArgs e)
        {
            if (!draw)
                return;
            if (picture.Image == null)
                return;
            x1 = e.X;
            y1 = e.Y;

            picture.Refresh();
        }

        private void picture_MouseDown(object sender, MouseEventArgs e)
        {
            x0 = e.X;
            y0 = e.Y;
            draw = true;
        }
    }
}
