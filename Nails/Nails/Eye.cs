using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nails
{
    class Eye
    {
        static public List<double[,]> Original;
        static public List<Point> LastPoints; 

        static bool[,] MaskCont;

        public static void Generate(int size, int count)
        {
            Original = new List<double[,]>();
            for (int k = 0; k < count; k++)
			{
                double Trad = (Double)(((180d / count) * (double) k) / 180d * Math.PI);
                double[,] eye = new double[size, size];
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        Double x1 = (Double)(2.0 * (j - (size - 1) / 2) / (size - 1));
                        Double y1 = (Double)(2.0 * (i - (size - 1) / 2) / (size - 1));
                        Double xx = (x1 * Math.Cos(Trad) - y1 * Math.Sin(Trad));
                        Double yy = (x1 * Math.Sin(Trad) + y1 * Math.Cos(Trad));
                        eye[j, i] = Math.Sign(xx) * Math.Exp(-6 * Math.Pow(xx * xx + yy * yy, 4));
                        //eye[j, i] = Math.Sign(xx) * Math.Exp(-1 * (Math.Pow(xx, 8)+ yy * yy));
                    }
                }
                Original.Add(eye);
            }
		}

        public static double[,] SeeAll(int[,] img)
        {
            double[,] Result = new double[img.GetLength(0), img.GetLength(1)];
            DateTime t1 = DateTime.Now;
            for (int i = 0; i < img.GetLength(0); i++)
            {
                for (int j = 0; j < img.GetLength(1); j++)
                {
                    if (img[i, j] < 40 || img[i,j] > 220)
                        continue;
                    double TempSqrt = 0;
                    int k = 0;
                    foreach (var item in Original)
                    {
                        k++;
                        double temp = 0;
                        if (i - item.GetLength(0) >= 0 && i + item.GetLength(0) < img.GetLength(0) && j - item.GetLength(0) >= 0 && j + item.GetLength(0) < img.GetLength(1))
                        {
                            double m1 = img[i - item.GetLength(0) / 2, j - item.GetLength(1) / 2];
                            double m2 = img[i - item.GetLength(0) / 2, j - item.GetLength(1) / 2 + item.GetLength(0)];
                            double m3 = img[i - item.GetLength(0) / 2 + item.GetLength(0), j - item.GetLength(1) / 2];
                            double m4 = img[i - item.GetLength(0) / 2 + item.GetLength(0), j - item.GetLength(1) / 2 + item.GetLength(0)];
                            double sz = 0.001 ;
                            if (Math.Abs(m2 - m3) < sz && Math.Abs(m1 - m4) < sz && Math.Abs(m1 - m3) < sz && Math.Abs(m4 - m2) < sz)
                            {
                               // break;
                            }
                        }
                        for (int ki = 0; ki < item.GetLength(0); ki++)
                        {                            
                            if (i - item.GetLength(0) / 2 + ki < 0 || i - item.GetLength(0) / 2 + ki > img.GetLength(0) - 1)
                                continue;
                            for (int kj = 0; kj < item.GetLength(1); kj++)
                            {
                                if (j - item.GetLength(1) / 2 + kj < 0 || j - item.GetLength(1) / 2 + kj > img.GetLength(1) - 1)
                                    continue;
                                
                                temp += img[i - item.GetLength(0) / 2 + ki, j - item.GetLength(1) / 2 + kj] * item[ki, kj];

                            }
                        }
                        TempSqrt += temp * temp;
                        if (k == 1 && TempSqrt < 10)
                        {
                          //  break;
                        }
                        else if (k == 2 && TempSqrt < 100)
                        {
                          //  break;
                        }
                        
                        
                    }

                    Result[i, j] = Math.Sqrt(TempSqrt);
                }
            }
            DateTime t2 = DateTime.Now;
            MessageBox.Show((t2 - t1).TotalMilliseconds.ToString());
            return Result;
        }


        public static double[,] SeeHand(int[,] img, Point p, int Delta)
        {
            int steps = 0;
            double[,] Result = new double[img.GetLength(0), img.GetLength(1)];
            LastPoints = new List<Point>();
            MaskCont = new bool[img.GetLength(0), img.GetLength(1)];
            for (int i = 0; i < Result.GetLength(0); i++)
            {
                for (int j = 0; j < Result.GetLength(1); j++)
                {
                    Result[i, j] = 0;
                    MaskCont[i, j] = false;
                }
            }
            //int[] dx = new int[8] { 1, 1, 1, 0, 0, -1, -1, -1 };
            //int[] dy = new int[8] { 1, 0, -1, 1, -1, 1, 0, -1 };
            int[] dx = new int[4] { 1, 0, 0, -1 };
            int[] dy = new int[4] { 0, 1, -1, 0 };

            DateTime t1 = DateTime.Now;
            Queue<Point> points = new Queue<Point>();
            points.Enqueue(p);
            while (points.Count != 0)
            {
                p = points.Dequeue();
                bool flag = false;
                if (Result[p.X, p.Y] != 0)
                    continue;
                double TempSqrt = 0;
                int k = 0;
                foreach (var item in Original)
                {
                    k++;
                    double temp = 0;
                    if (p.X - item.GetLength(0) >= 0 && p.X + item.GetLength(0) < img.GetLength(0) && p.Y - item.GetLength(0) >= 0 && p.Y + item.GetLength(0) < img.GetLength(1))
                    {
                        double m1 = img[p.X - item.GetLength(0) / 2, p.Y - item.GetLength(1) / 2];
                        double m2 = img[p.X - item.GetLength(0) / 2, p.Y - item.GetLength(1) / 2 + item.GetLength(0)];
                        double m3 = img[p.X - item.GetLength(0) / 2 + item.GetLength(0), p.Y - item.GetLength(1) / 2];
                        double m4 = img[p.X - item.GetLength(0) / 2 + item.GetLength(0), p.Y - item.GetLength(1) / 2 + item.GetLength(0)];
                        double sz = 65;
                        if (Math.Abs(m2 - m3) < sz && Math.Abs(m1 - m4) < sz && Math.Abs(m1 - m3) < sz && Math.Abs(m4 - m2) < sz)
                        {
                         //   break;
                        }
                    }

                    for (int ki = 0; ki < item.GetLength(0); ki++)
                    {
                        if (p.X - item.GetLength(0) / 2 + ki < 0 || p.X - item.GetLength(0) / 2 + ki > img.GetLength(0) - 1)
                            continue;
                        for (int kj = 0; kj < item.GetLength(1); kj++)
                        {
                            if (p.Y - item.GetLength(1) / 2 + kj < 0 || p.Y - item.GetLength(1) / 2 + kj > img.GetLength(1) - 1)
                                continue;
                            if (Math.Abs(item[ki, kj]) < 0.1)
                                continue;
                            temp += img[p.X - item.GetLength(0) / 2 + ki, p.Y - item.GetLength(1) / 2 + kj] * item[ki, kj];
                            steps++;

                        }
                    }
                    TempSqrt += temp * temp;
                    if (k == 1 && TempSqrt < 50)
                    {
                    //    break;
                    }
                    else if (k == 2 && TempSqrt < 200)
                    {
                      //  break;
                    }

                }
                Result[p.X, p.Y] = Math.Sqrt(TempSqrt);
                LastPoints.Add(p);
                if (Result[p.X, p.Y] < Delta)
                {
                    Result[p.X, p.Y] = 35;
                    for (int i = 0; i < dx.Length; i++)
                    {
                        if (p.X + dx[i] < Result.GetLength(0) && p.X + dx[i] > 0 && p.Y + dy[i] < Result.GetLength(1) && p.Y + dy[i] > 0)
                        {
                            if (Result[p.X + dx[i], p.Y + dy[i]] == 0)
                            {
                                points.Enqueue(new Point(p.X + dx[i], p.Y + dy[i]));
                                
                                //if (p.X + 2 * dx[i] < Result.GetLength(0) && p.X + 2 * dx[i] > 0 && p.Y + 2 * dy[i] < Result.GetLength(1) && p.Y + 2 * dy[i] > 0)
                                //{
                                //    if (Result[p.X + 2 * dx[i], p.Y + 2 * dy[i]] == 0)
                                //    {
                                        
                                //    }
                                //}
                            }
                        }
                    }
                }
                else
                {
                    MaskCont[p.X, p.Y] = true;
                }

            }
            Result = InnerCont();
            DateTime t2 = DateTime.Now;
            MessageBox.Show((t2 - t1).TotalMilliseconds.ToString() + " " + steps.ToString());
            return Result;
        }

        public static List<Point> FormingArrCV()
        {
            List<Point> result = new List<Point>();


            Point FirstPoint = new Point();
            bool[,] points = MaskCont;
            bool[,] matr = new bool[MaskCont.GetLength(0), MaskCont.GetLength(1)];
            for (int i = 0; i < matr.GetLength(0); i++)
            {
                for (int j = 0; j < matr.GetLength(1); j++)
                {
                    matr[i, j] = false;
                }
            }
            for (int i = MaskCont.GetLength(1) - 10; i >= 10; i--)
            {
                for (int j = MaskCont.GetLength(0) - 10; j >= 10; j--)
                {
                    if (points[j, i])
                    {
                        FirstPoint = new Point(j, i);
                        goto FindFirst;
                    }
                }
            }
            
            
        FindFirst:
            int[] dx = new int[8] { 1, 0, -1, -1, -1, 0, 1, 1 };
            int[] dy = new int[8] { -1, -1, -1, 0, 1, 1, 1, 0 };
            result.Add(FirstPoint);
            int lastDelta = 6;
            matr[FirstPoint.X, FirstPoint.Y] = true;
            int Radius = 9;
            for (int i = 0; i < dx.Length; i++)
            {
                Point p = new Point(FirstPoint.X + dx[(i + lastDelta) % dx.Length], FirstPoint.Y + dy[(i + lastDelta) % dy.Length]);
                if (p.X < 0 || p.Y < 0 || p.X > matr.GetLength(0) - 1 || p.Y > matr.GetLength(1) - 1)
                    continue;
                if (matr[p.X, p.Y])
                    continue;
                if (points[p.X, p.Y])
                {
                    lastDelta = i + 4;
                    FirstPoint = p;
                    goto FindFirst;
                    matr[p.X, p.Y] = true;
                }

            }

            for (int i = 0; i < dx.Length; i++)
            {
                Point p = new Point(FirstPoint.X + 2 * dx[(i + lastDelta) % dx.Length], FirstPoint.Y + 2 * dy[(i + lastDelta) % dy.Length]);
                if (p.X < 0 || p.Y < 0 || p.X > matr.GetLength(0) - 1 || p.Y > matr.GetLength(1) - 1)
                    continue;
                if (matr[p.X, p.Y])
                    continue;
                if (points[p.X, p.Y])
                {
                    lastDelta = i + 4;
                    FirstPoint = p;
                    goto FindFirst;
                    matr[p.X, p.Y] = true;
                }

            }

            return result;
        }

        public static List<Point> FormingArr()
        {
            List<Point> result = new List<Point>();


            Point FirstPoint = new Point();
            bool[,] points = MaskCont;
            bool[,] matr = new bool[MaskCont.GetLength(0), MaskCont.GetLength(1)];
            for (int i = 0; i < matr.GetLength(0); i++)
            {
                for (int j = 0; j < matr.GetLength(1); j++)
                {
                    matr[i, j] = false;
                }
            }
            for (int i = MaskCont.GetLength(1) - 10; i >= 10; i--)
            {
                for (int j = MaskCont.GetLength(0) - 10; j >= 10; j--)
                {
                    if (points[j, i])
                    {
                        FirstPoint = new Point(j, i);
                        goto FindFirst;
                    }
                }
            }
        FindFirst:
            result.Add(FirstPoint);
            matr[FirstPoint.X, FirstPoint.Y] = true;
            int Radius = 9;
            while (true)
            {
                double tempAngle = 0;
                double tempDist = double.MaxValue;
                double MinCoef = double.MaxValue;

                double Dist = 100;
                double Angle = 0;
                int maxSect = 100;
                for (int i = Math.Max(0, result.Last().X - Radius); i < Math.Min(MaskCont.GetLength(0) - 1, result.Last().X + Radius + 1); i++)
                {
                    for (int j = Math.Max(0, result.Last().Y - Radius); j < Math.Min(MaskCont.GetLength(1) - 1, result.Last().Y + Radius + 1); j++)
                    {
                        if (result.Count == 702)
                        {
                            int xx1 = 1;
                            Console.Write((points[i, j]) + "; ");
                        }
                        if (!points[i, j] || matr[i, j])
                            continue;
                        if (result.Count <= 1)
                        {
                            tempDist = GetDist(result[result.Count - 1], new Point(i, j));
                            if (tempDist < MinCoef)
                            {
                                MinCoef = tempDist;
                                FirstPoint = new Point(i, j);
                            }
                        }
                        else
                        {

                            double coefAngle = 1;
                            Point A = result[result.Count - 1];
                            Point B = result[result.Count - 2];
                            Point C = new Point(i, j);

                            //                            tempAngle = (GetAngle(result[result.Count - 1], result[result.Count - 2], new Point(i, j)) * 180 / Math.PI);
                            tempAngle = (GetAngle(result[result.Count - 2], result[result.Count - 1], new Point(i, j)) * 180 / Math.PI);

                            int sign = (B.X - A.X) * (C.Y - A.Y) - (B.Y - A.Y) * (C.X - A.X);
                            if (sign < 0)
                            {
                                //tempAngle += 180;
                            }

                            tempAngle *= coefAngle;
                            //(bx-ax)*(py-ay)-(by-ay)*(px-ax) 
                            if (result.Count > 2)
                            {
                                double tempAngle2 = (GetAngle(result[result.Count - 2], result[result.Count - 3], new Point(i, j)) * 180 / Math.PI) * coefAngle;
                                //tempAngle = (tempAngle + tempAngle + tempAngle2) / 3d;
                            }
                            tempDist = GetDist(result[result.Count - 1], new Point(i, j));

                            if (tempAngle == 0)
                            {
                                //tempAngle = 180d;
                            }
                            int intAngle = (int)Math.Abs(180 - tempAngle) / 12;

                            if (maxSect > intAngle)
                            {
                                maxSect = intAngle;
                                Dist = tempDist;
                                FirstPoint = new Point(i, j);
                            }
                            else if (maxSect == intAngle && tempDist < Dist)
                            {
                                Dist = tempDist;
                                FirstPoint = new Point(i, j);
                            }
                            if (tempDist / (tempAngle * tempAngle) < MinCoef && tempDist < 70)
                            {
                                MinCoef = tempDist / (tempAngle * tempAngle);
                                //FirstPoint = new Point(i, j);
                            }
                        }
                        Console.WriteLine();
                    }
                }

                if (MinCoef < 11110)
                {
                    matr[FirstPoint.X, FirstPoint.Y] = true;
                    result.Add(FirstPoint);
                }
                else
                {
                    break;
                }
            }

            return result;
        }

        public static double GetDist(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        public static double GetAngle(Point p1, Point p2, Point p3)
        {
            double x1 = p1.X - p2.X, x2 = p3.X - p2.X;
            double y1 = p1.Y - p2.Y, y2 = p3.Y - p2.Y;
            double d1 = Math.Sqrt(x1 * x1 + y1 * y1);
            double d2 = Math.Sqrt(x2 * x2 + y2 * y2);
            return Math.Acos((x1 * x2 + y1 * y2) / (d1 * d2));
        }

        public static List<Point> GetHull(List<Point> points)
        {
            List<Point> result = new List<Point>();


            Point[] pointsArr = points.ToArray();
            Point FirstPoint = pointsArr[0];
            for (int i = 0; i < pointsArr.Length; i++)
            {
                if (pointsArr[i].Y < FirstPoint.Y || pointsArr[i].Y == FirstPoint.Y && pointsArr[i].X > FirstPoint.X)
                {
                    FirstPoint = pointsArr[i];
                }
            }
            result.Add(new Point(0, FirstPoint.Y));
            result.Add(FirstPoint);

            while (true)
            {

                int MinInd = -1;
                int MaxInd = -1;
                double MinAngle = 0;
                double MaxAngle = 0;
                for (int i = 0; i < pointsArr.Length; i++)
                {

                    double angle = GetAngle(result[result.Count - 2], result[result.Count - 1], pointsArr[i]);
                    if (angle >= MaxAngle)
                    {
                        MaxAngle = angle;
                        MaxInd = i;
                    }
                }
                if (MaxInd == -1 || result.Contains(pointsArr[MaxInd]))
                    break;
                result.Add(pointsArr[MaxInd]);
            }
            result.Remove(result[0]);
            return result;
        }


        public static List<double> GetDefect(List<Point> points, List<Point> Hull)
        {
            Point pHull1 = new Point();
            Point pHull2 = new Point();
            List<double> Dists = new List<double>();
            int countHull = 0;
            int ind = 0;
            for (int i = 0; i < points.Count && countHull < 2; i++)
            {
                if (Hull.Contains(points[i]))
                {
                    if (countHull == 0)
                    {
                        pHull1 = points[i];
                        countHull++;
                    }
                    else if (countHull == 1)
                    {
                        pHull2 = points[i];
                        countHull++;
                    }
                }
                ind = i;
            }

            for (int i = 0; i < points.Count; i++)
            {
                if (i > ind)
                {
                    if (Hull.Contains(points[i]))
                    {
                        pHull1 = pHull2;
                        pHull2 = points[i];
                    }
                }
                double d1 = GetDist(points[i], pHull1);
                double d2 = GetDist(points[i], pHull2);
                //Dists.Add((d1 + d2) / 2);
                //Dists.Add(Math.Min(d1, d2));
                Dists.Add(DistLineAbs(points[i], pHull1, pHull2));
            }
            return Dists;
        }

        static double DistLine(Point p, Point p1, Point p2)
        {
            return ((p1.Y - p2.Y) * p.X + (p2.X - p1.X) * p.Y + (p1.X * p2.Y - p2.X * p1.Y)) / Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        static double DistLineAbs(Point p, Point p1, Point p2)
        {
            return Math.Abs(DistLine(p, p1, p2));
        }

        public static double[,] InnerCont()
        {
            Point p = new Point(5, 5);
            Queue<Point> pointQueue = new Queue<Point>();
            pointQueue.Enqueue(p);

            //int[] dx = new int[8] { 1, 1, 1, 0, 0, -1, -1, -1 };
            //int[] dy = new int[8] { 1, 0, -1, 1, -1, 1, 0, -1 };
            int[] dx = new int[4] { 1, 0, 0, -1 };
            int[] dy = new int[4] { 0, 1, -1, 0 };
            bool[,] Result = new bool[MaskCont.GetLength(0), MaskCont.GetLength(1)];
            double[,] ResultInt = new double[MaskCont.GetLength(0), MaskCont.GetLength(1)];
            bool[,] TempMask = new bool[MaskCont.GetLength(0), MaskCont.GetLength(1)];
            for (int i = 0; i < Result.GetLength(0); i++)
			{
                for (int j = 0; j < Result.GetLength(1); j++)
                {
                    Result[i, j] = TempMask[i, j] = false;
                    ResultInt[i, j] = 0;
                }
			}
            while(pointQueue.Count > 0)
            {
                p = pointQueue.Dequeue();
                for (int i = 0; i < dy.Length; i++)
                {
                    Point p1 = new Point(p.X + dx[i], p.Y + dy[i]);
                    if (p1.X < 1 || p1.Y < 1 || p1.X > TempMask.GetLength(0) - 1 || p1.Y > TempMask.GetLength(1) - 1)
                        continue;

                    if (!TempMask[p1.X, p1.Y])
                    {
                        if (MaskCont[p1.X, p1.Y])
                        {
                            Result[p1.X, p1.Y] = true;
                            ResultInt[p1.X, p1.Y] = 255;
                        }
                        else
                        {                 
                            pointQueue.Enqueue(p1);
                        }
                        TempMask[p1.X, p1.Y] = true;
                    }
                }
            }
            MaskCont = Result;
            return ResultInt;
        }
    }
}
