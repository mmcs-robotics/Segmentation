using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContMask
{
    public partial class Form1 : Form
    {

        Bitmap Img;
        Bitmap MaskedImg;
        Color CenterColor;
        Point CenterPoint = new Point(-1, -1);

        public Form1()
        {
            InitializeComponent();
            Img = new Bitmap(@"C:\Img\qwe5.jpg");
            Img = new Bitmap(Img, pictureBox1.Size);
            UpdatePicture(Img);

            CenterColor = GetAvCenterColor(Img);
        }



        void UpdatePicture(Bitmap img)
        {
            pictureBox1.Image = img;
            pictureBox1.Invalidate();
        }

        Color GetCenterColor(Bitmap img)
        {
            return img.GetPixel(img.Width / 2, img.Width / 2);
        }

        Color GetAvCenterColor(Bitmap img, int count = 5)
        {
            int R = 0, G = 0, B = 0;
            for (int i = img.Width / 2 - count; i < img.Width / 2 + count; i++)
            {
                for (int j = img.Height / 2 - count; j < img.Height / 2 + count; j++)
                {
                    Color col = img.GetPixel(i, j);
                    R += col.R;
                    G += col.G;
                    B += col.B;
                }
            }
            R /= count * count * 4;
            G /= count * count * 4;
            B /= count * count * 4;
            return Color.FromArgb(R, G, B);
        }

        Color GetAvCenterColor(Bitmap img, Point p, int count = 5)
        {
            int R = 0, G = 0, B = 0;
            for (int i = p.X - count; i < p.X + count; i++)
            {
                for (int j = p.Y - count; j < p.Y + count; j++)
                {
                    Color col = img.GetPixel(i, j);
                    R += col.R;
                    G += col.G;
                    B += col.B;
                }
            }
            R /= count * count * 4;
            G /= count * count * 4;
            B /= count * count * 4;
            return Color.FromArgb(R, G, B);
        }

        int GetDistanceColor(Color col)
        {
            int dR = Math.Abs(col.R - CenterColor.R);
            int dG = Math.Abs(col.G - CenterColor.G);
            int dB = Math.Abs(col.B - CenterColor.B);

            return dR + dG + dB;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //int distCol = int.Parse(textBox1.Text);
            MaskedImg = new Bitmap(Img.Width, Img.Height);
            for (int i = 0; i < Img.Width; i++)
            {
                for (int j = 0; j < Img.Height; j++)
                {
                    Color Temp = Img.GetPixel(i, j);
                    Color col = !CheckYCbCr(Temp, CenterColor) ? Color.Black : Temp;
                    MaskedImg.SetPixel(i, j, col);
                }
            }
            UpdatePicture(MaskedImg);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image.Save(@"C:\Img\iphonemask.jpg");
        }


        bool CheckYCbCr(Color c, Color c2)
        {
            
            byte Y = (byte)((0.257 * c.R) + (0.504 * c.G) + (0.098 * c.B) + 16);
            byte Cb = (byte)(-(0.148 * c.R) - (0.291 * c.G) + (0.439 * c.B) + 128);
            byte Cr = (byte)((0.439 * c.R) - (0.368 * c.G) - (0.071 * c.B) + 128);

            byte Y2 = (byte)((0.257 * c2.R) + (0.504 * c2.G) + (0.098 * c2.B) + 16);
            byte Cb2 = (byte)(-(0.148 * c2.R) - (0.291 * c2.G) + (0.439 * c2.B) + 128);
            byte Cr2 = (byte)((0.439 * c2.R) - (0.368 * c2.G) - (0.071 * c2.B) + 128);

            int dist = int.Parse(textBox1.Text);
            int delta = 4;
            if (Math.Sqrt((Cb - (Cb2 + delta)) * (Cb - (Cb2 + delta)) + (Cr - (Cr2 + delta)) * (Cr - (Cr2 + delta))) < dist)
            {
                return true;
            }
            return false;

            if (Y > (int)numericUpDown2.Value || Y < (int)numericUpDown1.Value)
                return false;
            if (Cb > (int)numericUpDown3.Value || Cb < (int)numericUpDown4.Value)
                return false;
            if (Cr > (int)numericUpDown5.Value || Cr < (int)numericUpDown6.Value)
                return false;
            return true;
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            CenterPoint = new Point(e.X, e.Y);
            CenterColor = GetAvCenterColor(Img, CenterPoint);
        }

    }
}
