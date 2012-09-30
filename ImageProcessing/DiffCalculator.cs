namespace AirHockey.Recognition.Client.ImageProcessing
{
    using System.Drawing;
    using System.Linq;

    using AForge.Imaging.Filters;

    public class DiffCalculator
    {
        private static FillHoles fillHoles;
        private static Threshold threshold;

        private static GaussianBlur gaussianBlur;

        private const byte THERSHOLD = 40;
        
        public static byte Max(params byte[] points)
        {
            return points.Max();
        }
        
        public static int Sum(params int[] points)
        {
            return points.Sum();
        }

        static DiffCalculator()
        {
            threshold = new Threshold(THERSHOLD);
            fillHoles = new FillHoles();
            fillHoles.MaxHoleHeight = 125;
            fillHoles.MaxHoleWidth = 125;
            fillHoles.CoupledSizeFiltering = true;

            gaussianBlur = new GaussianBlur(3, 11);
        }

        public static Bitmap PixelDiff(Bitmap a, Bitmap b)
        {
            var difference = new Difference(a);
            var dif =  difference.Apply(b);

            gaussianBlur.ApplyInPlace(dif);

            Bitmap clone = Grayscale.CommonAlgorithms.Y.Apply(dif);

            threshold.ApplyInPlace(clone);
            fillHoles.ApplyInPlace(clone);

            return clone;
        }
    }
}