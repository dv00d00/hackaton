namespace WPFCSharpWebCam
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    public class Diff
    {
        private const byte Thershold = 20;

        public static unsafe Bitmap PixelDiff(Bitmap a, Bitmap b)
        {
            Bitmap output = new Bitmap(a.Width, a.Height, PixelFormat.Format32bppArgb);
            Rectangle rect = new Rectangle(Point.Empty, a.Size);
            using (var aData = a.LockBitsDisposable(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb))
            using (var bData = b.LockBitsDisposable(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb))
            using (var outputData = output.LockBitsDisposable(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb))
            {
                byte* aPtr = (byte*)aData.Scan0;
                byte* bPtr = (byte*)bData.Scan0;
                byte* outputPtr = (byte*)outputData.Scan0;
                int len = aData.Stride * aData.Height;
                for (int i = 0; i < len; i += 4)
                {
                    // byte diff = (byte)(Math.Abs(aPtr[0] + aPtr[1] + aPtr[2] - bPtr[0] - bPtr[1] - bPtr[2]) / 3);
                    byte diff = (byte)(Math.Abs(aPtr[0] + aPtr[1] + aPtr[2] - bPtr[0] - bPtr[1] - bPtr[2]) / 3);

                    byte result = (byte)(diff > Thershold ? 255 : 0);

                    outputPtr[0] = outputPtr[1] = outputPtr[2] = result;
                    outputPtr[3] = 255;

                    outputPtr += 4;
                    aPtr += 4;
                    bPtr += 4;
                }
            }
            return output;
        }
    }
}