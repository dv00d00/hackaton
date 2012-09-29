namespace AirHockey.Recognition.Client.ImageProcessing
{
    using System;
    using System.IO;
    using System.Windows.Media.Imaging;
    using System.Windows;
    using System.Windows.Interop;

    //Design by Pongsakorn Poosankam
    public class PictureHelper
    {
        //Block Memory Leak
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr handle);
        
        public static BitmapSource bitmapSource;
        
        public static IntPtr handle;
        
        public static BitmapSource LoadBitmap(System.Drawing.Bitmap source)
        {
            handle = source.GetHbitmap();
            bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            DeleteObject(handle);
            return bitmapSource;
        }

        public static void SaveImageCapture(BitmapSource bitmap)
        {
            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.QualityLevel = 100;

            // Configure save file dialog box
            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "Image", 
                DefaultExt = ".Jpg",
                Filter = "Image (.jpg)|*.jpg"
            };

            // Show save file dialog box
            var result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save Image
                string filename = dlg.FileName;
                var fstream = new FileStream(filename, FileMode.Create);
                encoder.Save(fstream);
                fstream.Close();
            }

        }
    }
}
