using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nails
{
    class Racurs
    {
        static public List<Point3> Get3DPoints(Bitmap OriginalImage, Bitmap DepthScene, Point3 CameraLoc, Point3 CameraVect)
        {
            List<Point3> Result = new List<Point3>();
            for (int i = 0; i < OriginalImage.Height; i++)
            {
                for (int j = 0; j < OriginalImage.Width; j++)
                {
                    //Point3 p = new Point3(DepthScene.GetPixel(j,i).R, j,i);
                    Point3 p = new Point3(0, j, i);
                    
                    p.Move(-CameraLoc.X, -CameraLoc.Y, -CameraLoc.Z);
                    //p.RotateY(j / OriginalImage.Width * 90 - 45);
                    //p.RotateZ(-CameraVect.Z);


                    //p.RotateX(CameraVect.X);
                    //p.RotateY(CameraVect.Y);
                    
                    p.SetOriginalCoord(j,i);
                    Result.Add(p);

                }
            }
            return Result;
        }
    }
}
