using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nails
{
    class HelperImage
    {

        #region Image Convert

        public static Color[,] ImageToArray(Bitmap bmp)
        {
            Color[,] Result = new Color[bmp.Width, bmp.Height];
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Result[i, j] = bmp.GetPixel(i, j);
                }
            }
            return Result;
        }

        public static int[,] ColorToValueArray(Color[,] img)
        {
            int[,] Result = new int[img.GetLength(0), img.GetLength(1)];
            for (int i = 0; i < img.GetLength(0); i++)
            {
                for (int j = 0; j < img.GetLength(1); j++)
                {
                    Result[i, j] = ColorToValue(img[i, j]);
                    if (Result[i,j] < 200)
                    {
                        int x = Result[i, j];
                    }
                }
            }

            return Result;
        }

        static int ColorToValue(Color c)
        {
            return (int)(0.299 * c.R + 0.587 * c.G + 0.114 * c.B);
        }

        public static Bitmap ValuesToImage(int[,] values)
        {
            Bitmap Result = new Bitmap(values.GetLength(0), values.GetLength(1));
            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int j = 0; j < values.GetLength(1); j++)
                {
                    Result.SetPixel(i, j, Color.FromArgb(values[i, j], values[i, j], values[i, j]));
                }
            }
            return Result;
        }

        public static Bitmap FromDoubleToBitmap(double[,] img)
        {
            Bitmap Result = new Bitmap(img.GetLength(0), img.GetLength(1));
            int[,] tempArr = ToNormalInt(img, 255);
            for (int i = 0; i < img.GetLength(0); i++)
            {
                for (int j = 0; j < img.GetLength(1); j++)
                {
                    Result.SetPixel(i, j, Color.FromArgb(tempArr[i, j], tempArr[i, j], tempArr[i, j]));
                }
            }
            return Result;
        }

        static Bitmap FromIntToBitmap(int[,] img)
        {
            Bitmap Result = new Bitmap(img.GetLength(0), img.GetLength(1));
            int[,] tempArr = ToNormalInt(img, 255);
            for (int i = 0; i < img.GetLength(0); i++)
            {
                for (int j = 0; j < img.GetLength(1); j++)
                {
                    if (tempArr[i,j] < 200)
                    {
                        int x = tempArr[i, j];
                    }
                    Result.SetPixel(i, j, Color.FromArgb(tempArr[i, j], tempArr[i, j], tempArr[i, j]));
                }
            }
            return Result;
        }

        #endregion

        public static Bitmap ToBlackImage(Color[,] img)
        {
            return FromIntToBitmap(ColorToValueArray(img));
        }
        

        #region Normal

        static int[,] ToNormalInt(double[,] img, int size)
        {
            int[,] Result = new int[img.GetLength(0), img.GetLength(1)];
            double Min = img.Cast<double>().Min();
            double Max = img.Cast<double>().Max();
            for (int i = 0; i < img.GetLength(0); i++)
            {
                for (int j = 0; j < img.GetLength(1); j++)
                {
                    Result[i, j] = Math.Max(0, Math.Min(255, ((int)((img[i, j] - Min) / (Max - Min) * size))));
                }
            }
            return Result;
        }

        static int[,] ToNormalInt(int[,] img, int size)
        {
            int[,] Result = new int[img.GetLength(0), img.GetLength(1)];
            int Min = img.Cast<int>().Min();
            int Max = img.Cast<int>().Max();
            for (int i = 0; i < img.GetLength(0); i++)
            {
                for (int j = 0; j < img.GetLength(1); j++)
                {
                    Result[i, j] = (int)((double)img[i, j] / (double)Max * 255);
                }
            }
            return Result;
        }
        #endregion

        #region Filter

        static int FilterPixel(int a, int b, int c)
        {
            int middle;
            if ((a <= b) && (a <= c))
            {
                middle = (b <= c) ? b : c;
            }
            else
            {
                if ((b <= a) && (b <= c))
                {
                    middle = (a <= c) ? a : c;
                }
                else
                {
                    middle = (a <= b) ? a : b;
                }
            }

            return middle;
        }

        public static int[,] FilterValues(int[,] img)
        {
            int[,] Result = new int[img.GetLength(0), img.GetLength(1)];
            for (int i = 0; i < img.GetLength(0); i++)
            {
                for (int j = 0; j < img.GetLength(1); j++)
                {
                    if (i == 0 || j == 0 || i == img.GetLength(0) - 1 || j == img.GetLength(1) - 1)
                    {
                        Result[i, j] = img[i, j];
                    }
                    else
                    {
                        int[] arr = new int[9] { img[i - 1, j - 1], img[i - 1, j], img[i - 1, j + 1], img[i, j - 1], img[i, j], img[i, j + 1], img[i + 1, j - 1], img[i + 1, j], img[i + 1, j + 1] };
                        Array.Sort<int>(arr);
                        Result[i, j] = arr[4];
                        if (Result[i, j] < 200)
                        {
                            int xx = Result[i, j];
                            int x1 = img[i, j];
                        }
                    }
                }
            }
            int err = 0;

            for (int i = 0; i < img.GetLength(0); i++)
            {
                for (int j = 0; j < img.GetLength(1); j++)
                {
                    err += img[i, j] != Result[i, j] ? 1 : 0;
                }
            }
            return Result;
        }

        #endregion

        #region Color
        
        public static int GetDistanceColor(Color col, Color colTarget)
        {
            int dR = Math.Abs(col.R - colTarget.R);
            int dG = Math.Abs(col.G - colTarget.G);
            int dB = Math.Abs(col.B - colTarget.B);

            return dR + dG + dB;
        }

        #endregion
    }
}
