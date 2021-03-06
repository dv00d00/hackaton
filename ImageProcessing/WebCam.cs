﻿namespace AirHockey.Recognition.Client.ImageProcessing
{
    using AForge.Imaging.Filters;

    using WebCam_Capture;

    using System.Drawing;

    using Image = System.Windows.Controls.Image;

    class WebCam
    {
        private WebCamCapture webcam;
        private Image videoOutput;
        private Image diffOutput;

        private const int FrameNumber = 30;

        private System.Drawing.Bitmap backgroundFrame;

        private bool storeBackground;

        private BlobsScanner blobsScanner;

        private Image rects;

        private Rectangle? selection;

        public WebCam()
        {
            this.blobsScanner = new BlobsScanner();
        }

        public void InitializeWebCam(Image videoOutput, Image diffOutput, Image rects)
        {
            this.webcam = new WebCamCapture
            {
                FrameNumber = 0,
                TimeToCapture_milliseconds = FrameNumber
            };

            this.webcam.ImageCaptured += this.webcam_ImageCaptured;
            this.videoOutput = videoOutput;
            this.diffOutput = diffOutput;

            this.rects = rects;
        }

        void webcam_ImageCaptured(object source, WebcamEventArgs e)
        {
            var webCamImage = (Bitmap)e.WebCamImage;

            if (this.storeBackground)
            {
                this.backgroundFrame = new Bitmap(webCamImage);
                this.storeBackground = false;
            }

            if (this.backgroundFrame != null)
            {
                using (var bitmap = new Bitmap(webCamImage))
                {
                    var pixelDiff = DiffCalculator.PixelDiff(this.backgroundFrame, bitmap);

                    var cropped = CropDiff(pixelDiff);

                    this.blobsScanner.ScanImage(cropped);
                    var bitmapWithRects = new Bitmap(webCamImage.Width, webCamImage.Height);
                    this.blobsScanner.Draw(bitmapWithRects);

                    this.diffOutput.Source = PictureHelper.LoadBitmap(cropped);
                    this.rects.Source = PictureHelper.LoadBitmap(bitmapWithRects);
                }
            }

            this.videoOutput.Source = PictureHelper.LoadBitmap(webCamImage);
        }

        private Bitmap CropDiff(Bitmap pixelDiff)
        {
            if (selection != null)
            {
                var crop = new Crop(selection.Value);

                return crop.Apply(pixelDiff);
            }

            return pixelDiff;
        }

        public void Start()
        {
            this.webcam.TimeToCapture_milliseconds = FrameNumber;
            this.webcam.Start(0);
        }

        public void Stop()
        {
            this.webcam.Stop();
        }

        public void Continue()
        {
            // change the capture time frame
            this.webcam.TimeToCapture_milliseconds = FrameNumber;

            // resume the video capture from the stop
            this.webcam.Start(this.webcam.FrameNumber);
        }

        public void ResolutionSetting()
        {
            this.webcam.Config();
        }

        public void AdvanceSetting()
        {
            this.webcam.Config2();
        }

        public void StoreBackgroung()
        {
            this.storeBackground = true;
        }

        public void SetSelection(Rectangle result)
        {
            this.selection = result;
        }
    }
}
