using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace Nails
{
    internal class Point3
    {
        public double X;
        public double Y;
        public double Z;
        public int originalX, originalY;

        public Point3(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Point3(string content)
        {
            string[] con = content.Split('/');
            this.X = int.Parse(con[0]);
            this.Y = int.Parse(con[1]);
            this.Z = int.Parse(con[2]);
        }

        public void SetOriginalCoord(int x, int y)
        {
            originalX = x;
            originalY = y;
        }

        public Point ToPoint2()
        {
            return new Point(Convert.ToInt32(X), Convert.ToInt32(Y));
        }

        private void Change(double[,] Transform)
        {
            var temp = Matrix.ChangePoint3(this, Transform);
            X = temp.X;
            Y = temp.Y;
            Z = temp.Z;
        }

        public void Move(double dx, double dy, double dz)
        {
            Change(Matrix.Move3(dx, dy, dz));
        }

        public void Scale(double sx, double sy, double sz)
        {
            Change(Matrix.Scale3(sx, sy, sz));
        }

        public void RotateX(double angle)
        {
            Change(Matrix.Rotate3X(angle));
        }

        public void RotateY(double angle)
        {
            Change(Matrix.Rotate3Y(angle));
        }

        public void RotateZ(double angle)
        {
            Change(Matrix.Rotate3Z(angle));
        }

        public void Rotate(double angle, char C)
        {
            if (C == 'x' || C == 'X')
            {
                RotateX(angle);
                RotateX(angle);
            }
            else if (C == 'y' || C == 'Y')
            {
                RotateY(angle);
                RotateY(angle);
            }
            else if (C == 'z' || C == 'Z')
            {
                RotateZ(angle);
                RotateZ(angle);
            }
        }


        public Point3 Clone()
        {
            return new Point3(X, Y, Z);
        }
    }
}
