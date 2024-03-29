﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace image_summer      //Calculates a mean value of a custom area on a picture / set of pictures
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
                Bitmap src = new Bitmap(of.FileName);
                Bitmap newBmp = new Bitmap(src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                using (Graphics gfx = Graphics.FromImage(newBmp))
                {
                    gfx.DrawImage(src, 0, 0, src.Width,src.Height);
                }

            picture.Image = newBmp;
            }
            catch { return; }
        }
        int x0 = 0, y0 = 0, x1=0, y1=0, X=0, Y=0;

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
            X = x1 - x0 + 1;
            Y = y1 - y0 + 1;
            double t = 0;
            double s = 0;
            if (X > 0 && Y > 0)
            {
                var g = new int[X, Y];
                Bitmap b = picture.Image as Bitmap;

                for (int x = x0; x <= x1 && x < b.Width; x++)
                    for (int y = y0; y <= y1 && y < b.Height; y++)
                    {
                        g[x - x0, y - y0] = b.GetPixel(x, y).G;
                        t += g[x - x0, y - y0];
                    }
                t /= X * Y;

                for (int x = 0; x < X; x++)
                    for (int y = 0; y < Y; y++)
                        s += (t - g[x, y]) * (t - g[x, y]);
                s /= X * Y;
                s = Math.Sqrt(s);
            }
            total.Text = string.Format("mean = {0:0.00}, stddev = {1:0.00}, stderr = {2:0.00}", t, s, s / t * 100);
        }

        private void picture_MouseUp(object sender, MouseEventArgs e)
        {
            draw = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (X > 0 && Y > 0)
            {
                try
                {
                    double m = double.Parse(mul.Text);
                    Bitmap b = picture.Image as Bitmap;
                    
                    for (int x = x0; x <= x1 && x < b.Width; x++)
                        for (int y = y0; y <= y1 && y < b.Height; y++)
                        {
                            double c = b.GetPixel(x, y).G;
                            c *= m;
                            int d = c < 255 ? ((int)c) : 255;
                            b.SetPixel(x, y, Color.FromArgb(d,d,d));
                        }
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
            }
            picture.Refresh();
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
