using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Nails
{
    class Svert
    {
        [DllImport(@"C:\Users\Vadim\Documents\Visual Studio 2013\Projects\Svert\Debug\Svert.dll", CharSet = CharSet.Unicode, EntryPoint = "fnsvertka2")]
        unsafe public static extern double fnsvertka2(float[] a, float[] b, long c);

        [DllImport(@"C:\Users\Vadim\Documents\Visual Studio 2013\Projects\Svert\Debug\Svert.dll", CharSet = CharSet.Unicode, EntryPoint = "svert")]
        unsafe public static extern float svert(float[] a, float[] b, int c, int count);

        static Random rand;

        public static float sver()
        {
            float[] r = new float[2] { 2.1f, 3.3f };
            //float[] aa = new float[8] { 7f, 2f, 2f, 2f, 2f, 2f, 2f, 2f};
            //float[] bb = new float[8] { 3f, 3f, 3f, 3f, 2f, 2f, 2f, 2f};
            DateTime d1 = DateTime.Now;
            float res = 0f;
            int count = 0;
            rand = new Random();
            for (int j = 0; j < 5000*500; j++)
            {

                float[] aa = new float[12 * 12];
                float[] bb = new float[12 * 12];
                for (int i = 0; i < 12 * 12; i++)
                {
                    //aa[i] = (float)rand.NextDouble();
                    //bb[i] = (float)rand.NextDouble();
                    aa[i] = 1f;
                    bb[i] = 1f;
                }

                //res = svert(aa, bb, 12 * 12, 5000*500);
                res = sv(aa, bb, 12 * 12);
                count += 12 * 12;
            }
            DateTime d2 = DateTime.Now;
            return (float)(d2 - d1).TotalMilliseconds;
            return (float)count;
        }

        static float sv(float[] a, float[] b, int c)
        {
            float temp = 0f;
            for (int i = 0; i < c; i++)
            {
                temp += a[i] * b[i];
            }

            return temp;
        }

        public static double sver2()
        {
            double res = 0;
            float[] aa = new float[4] {1f,1f,1f,1f};
            float[] bb = new float[4] {1f,1f,1f,1f};
            res = fnsvertka2(aa, bb, 4);
            return res;
        }

    }
}
