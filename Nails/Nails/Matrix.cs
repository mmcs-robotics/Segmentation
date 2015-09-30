using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Nails
{
    class Matrix
    {

        static private double multStrToColumn(List<double> l1, List<double> l2)
        {
            double res = 0;
            for (int i = 0; i < l1.Count; ++i)
                res += l1[i] * l2[i];
            return res;
        }
        static public List<List<double>> multMatrix(List<List<double>> m1, List<List<double>> m2)
        {
            List<List<double>> result = new List<List<double>>();
            for (int i = 0; i < m1.Count; ++i)
            {
                result.Add(new List<double>());
                for (int j = 0; j < m1[i].Count; ++j)
                    result[i].Add(multStrToColumn(m1[i], m2[j]));
            }
            return result;
        }

        static public double[,] Move3(double x, double y, double z)
        {
            List<List<double>> T = new List<List<double>>();
            double[,] _T = { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { x, y, z, 1 } };
            return _T;
        }

        static public double[,] Scale3(double x, double y, double z)
        {
            List<List<double>> T = new List<List<double>>();
            double[,] _T = { { x, 0, 0, 0 }, { 0, y, 0, 0 }, { 0, 0, z, 0 }, { 0, 0, 0, 1 } };
            return _T;
        }

        static public Point ChangePoint(Point p, double[,] M)
        {
            List<List<double>> temp = new List<List<double>>();
            temp.Add(new List<double>());
            temp[0].Add(p.X);
            temp[0].Add(p.Y);
            temp[0].Add(1);
            double[,] _temp = { { p.X, p.Y, 1 } };
            var res = multMatrix(_temp, M);
            return new Point(Convert.ToInt32(res[0, 0]), Convert.ToInt32(res[0, 1]));
        }
        static public double[,] multMatrix(double[,] first, double[,] second)
        {
            double[,] array_temp = new double[first.GetLength(0), second.GetLength(0)];
            for (int i = 0; i < first.GetLength(0); i++)
                for (int j = 0; j < second.GetLength(0); j++)
                {
                    array_temp[i, j] = 0;
                    for (int k = 0; k < second.GetLength(0); k++)
                        array_temp[i, j] += first[i, k] * second[k, j];
                }
            return array_temp;
        }
        static public Point3 ChangePoint3(Point3 p, double[,] M)
        {
            List<List<double>> temp = new List<List<double>>();
            temp.Add(new List<double>());
            temp[0].Add(p.X);
            temp[0].Add(p.Y);
            temp[0].Add(p.Z);
            temp[0].Add(1);
            double[,] _temp = { { p.X, p.Y, p.Z, 1 } };
            var res = multMatrix(_temp, M);
            return new Point3(res[0, 0], res[0, 1], res[0, 2]);
        }

        static public double[,] Rotate3Y(double angle)
        {
            angle = angle * Math.PI / 180d;
            double[,] _T = { { Math.Cos(angle), 0, -Math.Sin(angle), 0 }, { 0, 1, 0, 0 }, { Math.Sin(angle), 0, Math.Cos(angle), 0 }, { 0, 0, 0, 1 } };
            return _T;
        }

        static public double[,] Rotate3X(double angle)
        {
            angle = angle * Math.PI / 180d;
            double[,] _T = { { 1, 0, 0, 0 }, { 0, Math.Cos(angle), -Math.Sin(angle), 0 }, { 0, Math.Sin(angle), Math.Cos(angle), 0 }, { 0, 0, 0, 1 } };
            return _T;
        }

        static public double[,] Rotate3Z(double angle)
        {
            angle = angle * Math.PI / 180d;
            double[,] _T = { { Math.Cos(angle), -Math.Sin(angle), 0, 0 }, { Math.Sin(angle), Math.Cos(angle), 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };
            return _T;
        }
    }
}
