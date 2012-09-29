namespace AirHockey.Recognition.Client
{
    using System.Windows;
    using System.Windows.Media.Imaging;

    using AirHockey.Recognition.Client.ImageProcessing;

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            this.InitializeComponent();
        }

        WebCam webcam;
        private void mainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            this.webcam = new WebCam();
            this.webcam.InitializeWebCam(this.imgVideo, this.diffImage, this.rects);
        }

        private void bntStart_Click(object sender, RoutedEventArgs e)
        {
            this.webcam.Start();
        }

        private void bntStop_Click(object sender, RoutedEventArgs e)
        {
            this.webcam.Stop();
        }

        private void bntContinue_Click(object sender, RoutedEventArgs e)
        {
            this.webcam.Continue();
        }

        private void bntCapture_Click(object sender, RoutedEventArgs e)
        {
            this.webcam.StoreBackgroung();

            this.imgCapture.Source = this.imgVideo.Source;
        }

        private void bntSaveImage_Click(object sender, RoutedEventArgs e)
        {
            PictureHelper.SaveImageCapture((BitmapSource)this.imgCapture.Source);
        }

        private void bntResolution_Click(object sender, RoutedEventArgs e)
        {
            this.webcam.ResolutionSetting();
        }

        private void bntSetting_Click(object sender, RoutedEventArgs e)
        {
            this.webcam.AdvanceSetting();
        }
    }
}
