using WebCam_Capture;

namespace WPFCSharpWebCam
{
    using System.Drawing;

    using Image = System.Windows.Controls.Image;

    //Design by Pongsakorn Poosankam
    class WebCam
    {
        private WebCamCapture webcam;
        private Image videoOutput;
        private Image diffOutput;

        private const int FrameNumber = 30;

        private System.Drawing.Bitmap backgroundFrame;

        private bool storeBackground;

        public void InitializeWebCam(Image videoOutput, Image diffOutput)
        {
            webcam = new WebCamCapture
            {
                FrameNumber = 0,
                TimeToCapture_milliseconds = FrameNumber
            };

            webcam.ImageCaptured += this.webcam_ImageCaptured;
            this.videoOutput = videoOutput;
            this.diffOutput = diffOutput;
        }

        void webcam_ImageCaptured(object source, WebcamEventArgs e)
        {
            var webCamImage = (Bitmap)e.WebCamImage;

            if (this.storeBackground)
            {
                this.backgroundFrame = webCamImage;
                this.storeBackground = false;
            }

            if (this.backgroundFrame != null)
            {
                using (var bitmap = new Bitmap(webCamImage))
                {
                    var pixelDiff = Diff.PixelDiff(this.backgroundFrame, bitmap);
                    this.diffOutput.Source = Helper.LoadBitmap(pixelDiff);
                }
            }

            this.videoOutput.Source = Helper.LoadBitmap(webCamImage);
        }

        public void Start()
        {
            webcam.TimeToCapture_milliseconds = FrameNumber;
            webcam.Start(0);
        }

        public void Stop()
        {
            webcam.Stop();
        }

        public void Continue()
        {
            // change the capture time frame
            webcam.TimeToCapture_milliseconds = FrameNumber;

            // resume the video capture from the stop
            webcam.Start(this.webcam.FrameNumber);
        }

        public void ResolutionSetting()
        {
            webcam.Config();
        }

        public void AdvanceSetting()
        {
            webcam.Config2();
        }

        public void StoreBackgroung()
        {
            this.storeBackground = true;
        }
    }
}
