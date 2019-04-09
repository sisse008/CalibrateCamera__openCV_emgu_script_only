using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using Emgu.CV.CvEnum;
using System.Runtime.InteropServices;

namespace CalibrateCameraInvoke
{

    public static class MatExtension
    {
        public static dynamic GetValue(this Mat mat, int row, int col)
        {
            var value = CreateElement(mat.Depth);
            Marshal.Copy(mat.DataPointer + (row * mat.Cols + col) * mat.ElementSize, value, 0, 1);
            return value[0];
        }

        public static void SetValue(this Mat mat, int row, int col, dynamic value)
        {
            var target = CreateElement(mat.Depth, value);
            Marshal.Copy(target, 0, mat.DataPointer + (row * mat.Cols + col) * mat.ElementSize, 1);
        }
        private static dynamic CreateElement(DepthType depthType, dynamic value)
        {
            var element = CreateElement(depthType);
            element[0] = value;
            return element;
        }

        private static dynamic CreateElement(DepthType depthType)
        {
            if (depthType == DepthType.Cv8S)
            {
                return new sbyte[1];
            }
            if (depthType == DepthType.Cv8U)
            {
                return new byte[1];
            }
            if (depthType == DepthType.Cv16S)
            {
                return new short[1];
            }
            if (depthType == DepthType.Cv16U)
            {
                return new ushort[1];
            }
            if (depthType == DepthType.Cv32S)
            {
                return new int[1];
            }
            if (depthType == DepthType.Cv32F)
            {
                return new float[1];
            }
            if (depthType == DepthType.Cv64F)
            {
                return new double[1];
            }
            return new float[1];
        }
    }


    class Program
    {
        static void Main(string[] args)
        {

            MCvPoint3D32f objectp_1 = new MCvPoint3D32f(1f, 1f, 1f);
            MCvPoint3D32f objectp_2 = new MCvPoint3D32f(1f, 1f, 1f);
            MCvPoint3D32f objectp_3 = new MCvPoint3D32f(1f, 1f, 1f);

            //List<MCvPoint3D32f> objectPoints = new List<MCvPoint3D32f> { objectp_1, objectp_2, objectp_3 };
            MCvPoint3D32f[][] objectPoints = new MCvPoint3D32f[][] { new MCvPoint3D32f[]
                { objectp_1,objectp_1,objectp_1,objectp_1 } };




            PointF imagep_1 = new PointF(1f, 1f);
            PointF imagep_2 = new PointF(1f, 1f);
            PointF imagep_3 = new PointF(1f, 1f);

            //List<PointF> imagePoints = new List<PointF> { imagep_1, imagep_2, imagep_3 };
            PointF[][] imagePoints = new PointF[][] { new PointF[] { imagep_1, imagep_1, imagep_1, imagep_1 } };

            Size imageSize = new Size(500, 500);

            Mat cameraMat = new Mat(new Size(3, 3), DepthType.Cv32F, 1);

            cameraMat.SetValue(0, 0, 302);
            cameraMat.SetValue(0, 1, 0);
            cameraMat.SetValue(0, 2, 101);
            cameraMat.SetValue(1, 0, 0);
            cameraMat.SetValue(1, 1, 411);
            cameraMat.SetValue(1, 2, 106);
            cameraMat.SetValue(2, 0, 0);
            cameraMat.SetValue(2, 1, 0);
            cameraMat.SetValue(2, 1, 1);

            Matrix<double> cameraMatrix = new Matrix<double>(new double[,] { { 302, 0, 101 }, { 0, 411, 106 }, { 0, 0, 1 } });

            cameraMat.ToImage<Gray, byte>().Save("test.jpg");
            //CvInvoke.cvSetReal2D(cameraMat.DataPointer, 0, 0, 302);
            //CvInvoke.cvSetReal2D(cameraMat.DataPointer, 0, 1, 0);
            //CvInvoke.cvSetReal2D(cameraMat.DataPointer, 0, 2, 101);
            //CvInvoke.cvSetReal2D(cameraMat.DataPointer, 1, 0, 0);
            //CvInvoke.cvSetReal2D(cameraMat.DataPointer, 1, 1, 411);
            //CvInvoke.cvSetReal2D(cameraMat.DataPointer, 1, 2, 106);
            //CvInvoke.cvSetReal2D(cameraMat.DataPointer, 2, 0, 0);
            //CvInvoke.cvSetReal2D(cameraMat.DataPointer, 2, 1, 0);
            //CvInvoke.cvSetReal2D(cameraMat.DataPointer, 2, 2, 1);

            Emgu.CV.CvEnum.CalibType calibrationType = Emgu.CV.CvEnum.CalibType.UseIntrinsicGuess;

            Emgu.CV.Structure.MCvTermCriteria termCriteria = new Emgu.CV.Structure.MCvTermCriteria(50);

            Mat _distortionCoeffs = new Mat(new Size(1, 5), DepthType.Cv32F, 1);


            Emgu.CV.ExtrinsicCameraParameters[] extrinsicParams;

            Mat[] rotation;// = new Mat(new Size(3, 3), DepthType.Cv32F, 1);
            Mat[] translation; //= new Mat(new Size(3, 3), DepthType.Cv32F, 1);

            var result = CvInvoke.CalibrateCamera(objectPoints, imagePoints, imageSize, cameraMatrix, _distortionCoeffs, calibrationType, termCriteria, out rotation, out translation);
            double t = rotation[0].GetValue(0,0);
            double t2 = rotation[0].GetValue(2,0);

        }
    }
}
