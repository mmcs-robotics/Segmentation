using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Nails
{
    public partial class Form1 : Form
    {

        private static List<Bitmap> imageComplex;
        private static List<Bitmap> depthComplex;
        private static List<List<Point3>> depthPointsArrays;
        List<Bitmap> images;
        List<Color[,]> imagesColor;
        Point Center;
        Color TargetColor;

        public Form1()
        {
            InitializeComponent();
            images = new List<Bitmap>();
            imagesColor = new List<Color[,]>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Load();
        }

        void Load()
        {
            depthPointsArrays = new List<List<Point3>>();
            imageComplex = new List<Bitmap>();
            depthComplex = new List<Bitmap>();
            OpenFileDialog opf = new OpenFileDialog();
            if (opf.ShowDialog() == DialogResult.OK)
            {
                var fileLines = System.IO.File.ReadLines(opf.FileName);
                int i = 0;
                foreach (var row in fileLines)
                {
                    string[] complex = row.Split(';');
                    
                    Bitmap origin = new Bitmap(complex[0]);
                    Bitmap scale = new Bitmap(origin, pictureBox1.Width, pictureBox1.Height);
                    imageComplex.Add(scale);
                    origin = new Bitmap(complex[1]);
                    scale = new Bitmap(origin, pictureBox1.Width, pictureBox1.Height);
                    depthComplex.Add(scale);
                    depthPointsArrays.Add(Racurs.Get3DPoints(imageComplex[imageComplex.Count - 1], depthComplex[depthComplex.Count - 1], new Point3(complex[2]), new Point3(complex[3])));
                    
                }
            }
            if (images.Count == 1)
            {
                UpdatePicture(images[0]);
            }
            else
            {

                Bitmap origin = imageComplex[0];
                Bitmap scale = new Bitmap(origin, pictureBox1.Width, pictureBox1.Height);
                UpdatePicture(scale);
                AddImage(scale);
                Center = new Point(pictureBox1.Width / 2, pictureBox1.Height / 2);
                Bitmap origin2 = imageComplex[1];
                Bitmap scale2 = new Bitmap(origin2, pictureBox2.Width, pictureBox2.Height);
                pictureBox2.Image = scale2;
                pictureBox2.Invalidate();
            }
        }

        void UpdatePicture(Bitmap bmp)
        {
            pictureBox1.Image = bmp;
            pictureBox1.Invalidate();
        }

        void ToBlack()
        {
            if (images.Count < 2)
            {
                Mask();
            }
            if (images.Count == 3)
            {
                UpdatePicture(images[2]);
            }
            else
            {
                Bitmap Black = HelperImage.ToBlackImage(imagesColor[1]);
                UpdatePicture(Black);
                AddImage(Black);
            }
        }
        
        void AddImage(Bitmap img)
        {
            images.Add(img);
            imagesColor.Add(HelperImage.ImageToArray(img));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ToBlack();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Filter();
        }

        void Filter()
        {
            if (images.Count < 3)
            {
                ToBlack();
            }
            if (images.Count == 4)
            {
                UpdatePicture(images[3]);
            }
            else
            {
                int[,] img = HelperImage.ColorToValueArray(imagesColor[2]);
                img = HelperImage.FilterValues(img);
                Bitmap bmp = HelperImage.ValuesToImage(img);
                UpdatePicture(bmp);
                AddImage(bmp);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (images.Count < 4)
            {
                Filter();
            }
            int size = (int)numericUpDown1.Value;
            Eye.Generate(size, 3);
            //double[,] img = Eye.SeeAll(HelperImage.ColorToValueArray(imagesColor[2]));
            double[,] img = Eye.SeeHand(HelperImage.ColorToValueArray(imagesColor[2]), Center, (int)numericUpDown3.Value);
            Bitmap result = HelperImage.FromDoubleToBitmap(img);
            UpdatePicture(result);
            AddImage(result);
            #region old
            /*


            Graphics g = Graphics.FromImage(pictureBox1.Image);
            //g.Clear(Color.Black);
            List<Point> Forming = Eye.FormingArrCV();
            List<Point> Hull = Eye.GetHull(Forming);
          //  g.DrawPolygon(new Pen(Color.Red), Hull.ToArray());
            List<double> dists = Eye.GetDefect(Forming, Hull);
            g.DrawLines(new Pen(Color.Blue), Forming.ToArray());

            int local = 10;
            int localRadius = 10;
            int countRight = 0;
            int countFalse = 0;
            int deltaIndex = local * 2;

            List<int> zeroPointIndex = new List<int>();
            List<int> maxPointIndex = new List<int>();
            for (int i = local; i < dists.Count - local - 1; i++)
            {
                //if(dists[i] < dists[i - local] && dists[i] < dists[i + local])

                if (dists[i] < 2 && dists[Math.Max(0, i - deltaIndex)] > local && dists[Math.Min(dists.Count - 1, i + deltaIndex)] > local)
                {
                    countRight++;
                    i += deltaIndex;
                }
                else
                {
                    if (countRight > 0)
                    {
                        int rightIndex = (i - deltaIndex) - countRight / 2;
                        zeroPointIndex.Add(rightIndex);
                        g.DrawEllipse(new Pen(Color.Green), new Rectangle(new Point(Forming[rightIndex].X - localRadius, Forming[rightIndex].Y - localRadius), new Size(localRadius * 2, localRadius * 2)));
                    }
                    countRight = 0;
                }
            }

            for (int i = 0; i < zeroPointIndex.Count - 1; i++)
            {
                double maxZero = 0;
                int tempIndex = -1;
                for (int j = zeroPointIndex[i]; j < zeroPointIndex[i + 1]; j++)
                {
                    if (dists[j] > maxZero && dists[j] > 20)
                    {
                        maxZero = dists[j];
                        tempIndex = j;
                    }
                }
                if (tempIndex != -1)
                {
                    maxPointIndex.Add(tempIndex);
                }
            }
            foreach (var item in maxPointIndex)
            {
                g.DrawEllipse(new Pen(Color.Aqua), new Rectangle(new Point(Forming[item].X - localRadius, Forming[item].Y - localRadius), new Size(localRadius * 2, localRadius * 2)));
            }

            */
            #endregion
            pictureBox1.Invalidate();

            //string[] seriesArray = { "Dists" };
            //int[] pointsArray = { 1, 2 };
            //this.chart1.Palette = ChartColorPalette.SeaGreen;

            //// Set title.
            //this.chart1.Titles.Add("Dists");

            //// Add series.
            //Series series = this.chart1.Series.Add(seriesArray[0]);
            //series.ChartType = SeriesChartType.FastLine;
            //for (int i = 0; i < dists.Count; i++)
            //{
            //    series.Points.AddY(dists[i]);
            //}

        }

        private void numericUpDown1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Bitmap img = (Bitmap)pictureBox1.Image;
            Graphics g = Graphics.FromImage(img);

            //g.DrawRectangle(new Pen(Color.Red), new Rectangle(e.X + 20, e.Y - 40, 40, 40));
            pictureBox1.Image = img;
            pictureBox1.Invalidate();
            int count = 20;
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
            Center = new Point(e.X, e.Y);
            TargetColor = Color.FromArgb(R, G, B);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Mask();
        }

        void Mask()
        {
            if (images.Count < 1)
            {
                Load();
            }
            Bitmap bmp = new Bitmap(images[0]);
            //int distColor = (int)numericUpDown2.Value;
            //for (int i = 0; i < bmp.Width; i++)
            //{
            //    for (int j = 0; j < bmp.Height; j++)
            //    {
            //        Color temp = bmp.GetPixel(i, j);
            //        int RealDist = HelperImage.GetDistanceColor(temp, TargetColor);
            //        bmp.SetPixel(i, j, RealDist > distColor ? Color.Black : TargetColor);
            //    }
            //}
            AddImage(bmp);
            UpdatePicture(bmp);

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //(pictureBox1.Image as Bitmap).Save(@"C:\Img\result\chudo.png");
            pictureBox1.Image.Save(@"E:\projects\Nails\Example\111.jpg");
            pictureBox2.Image.Save(@"E:\projects\Nails\Example\112.jpg");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Svert.sver().ToString());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            List<Point3> tempPoint3s = depthPointsArrays[0];
            var t = tempPoint3s.Where(a => Eye.LastPoints.FirstOrDefault(b => a.originalX == b.X && a.originalY == b.Y) != new Point()).ToList();
           /*var tt = t.Where(
                a =>
                    depthPointsArrays[1].FirstOrDefault(
                        b => Math.Sqrt((a.X - b.X)*(a.X - b.X) + (a.Y - b.Y)*(a.Y - b.Y) + (a.Z - b.Z)*(a.Z - b.Z)) < 50) != null).ToList();*/
            var tt =
                depthPointsArrays[1].Where(
                    b =>
                        t.FirstOrDefault(
                            a =>
                                Math.Sqrt((a.X - b.X)*(a.X - b.X) + (a.Y - b.Y)*(a.Y - b.Y) + (a.Z - b.Z)*(a.Z - b.Z)) <
                                10) != null).ToList();
            var k = tt.Count;
            Bitmap bb = (Bitmap)pictureBox2.Image;
            foreach (var po in tt)
            {
                bb.SetPixel(po.originalX, po.originalY, Color.SeaGreen);
            }
            int maxX = int.MinValue;
            int minX = int.MaxValue;
            int maxY = int.MinValue;
            int minY = int.MaxValue;
            foreach (var point3 in tt)
            {
                if (point3.originalX > maxX)
                    maxX = point3.originalX;
                if (point3.originalY > maxY)
                    maxY = point3.originalY;
                if (point3.originalX < minX)
                    minX = point3.originalX;
                if (point3.originalY < minY)
                    minY = point3.originalY;
            }

            Bitmap Black = HelperImage.ToBlackImage(HelperImage.ImageToArray((Bitmap)pictureBox2.Image));


            HelperImage.ImageToArray(Black);
            Point ppp = new Point((minX + maxX) / 2, (minY + maxY) / 2);
            double[,] img = Eye.SeeHand(HelperImage.ColorToValueArray(HelperImage.ImageToArray(Black)), ppp, (int)numericUpDown3.Value);
            Bitmap result = HelperImage.FromDoubleToBitmap(img);
            pictureBox2.Image = result;
            pictureBox2.Invalidate();
            
            int x = 0;
        }
    }
}
