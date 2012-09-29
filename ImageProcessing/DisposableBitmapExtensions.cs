namespace AirHockey.Recognition.Client.ImageProcessing
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    static class DisposableBitmapExtensions
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
                this._bitmap = bitmap;
                this._data = bitmap.LockBits(rect, flags, format);
            }

            public void Dispose()
            {
                this._bitmap.UnlockBits(this._data);
            }

            public IntPtr Scan0
            {
                get { return this._data.Scan0; }
            }

            public int Stride
            {
                get { return this._data.Stride; }
            }

            public int Width
            {
                get { return this._data.Width; }
            }

            public int Height
            {
                get { return this._data.Height; }
            }

            public PixelFormat PixelFormat
            {
                get { return this._data.PixelFormat; }
            }

            public int Reserved
            {
                get { return this._data.Reserved; }
            }
        }
    }
}