namespace WPFCSharpWebCam
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    static class Extensions
    {
        public static DisposableImageData LockBitsDisposable(this Bitmap bitmap, Rectangle rect, ImageLockMode flags, PixelFormat format)
        {
            return new DisposableImageData(bitmap, rect, flags, format);
        }

        public class DisposableImageData : IDisposable
        {
            private readonly Bitmap _bitmap;
            private readonly BitmapData _data;

            internal DisposableImageData(Bitmap bitmap, Rectangle rect, ImageLockMode flags, PixelFormat format)
            {
                _bitmap = bitmap;
                _data = bitmap.LockBits(rect, flags, format);
            }

            public void Dispose()
            {
                _bitmap.UnlockBits(_data);
            }

            public IntPtr Scan0
            {
                get { return _data.Scan0; }
            }

            public int Stride
            {
                get { return _data.Stride; }
            }

            public int Width
            {
                get { return _data.Width; }
            }

            public int Height
            {
                get { return _data.Height; }
            }

            public PixelFormat PixelFormat
            {
                get { return _data.PixelFormat; }
            }

            public int Reserved
            {
                get { return _data.Reserved; }
            }
        }
    }

    public class Diff
    {
        private const byte Thershold = 10;

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
                    byte diff = (byte)(Math.Abs(aPtr[1] + aPtr[2] + aPtr[3] - bPtr[1] - bPtr[2] - bPtr[3]) / 3);

                    byte result  = (byte)(diff > Thershold ? 255 : 0);

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