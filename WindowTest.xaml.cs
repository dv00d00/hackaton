using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AirHockey.Recognition.Client
{
    using System.Drawing;

    using Point = System.Windows.Point;

    /// <summary>
    /// Interaction logic for WindowTest.xaml
    /// </summary>
    public partial class WindowTest : Window
    {
        private bool mooving;

        public WindowTest()
        {
            InitializeComponent();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(canvas);
            
            Canvas.SetLeft(selection, position.X);
            Canvas.SetTop(selection, position.Y);

            mooving = true;
        }

        private void canvas_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(canvas);

            double X = Canvas.GetLeft(selection);
            double Y = Canvas.GetTop(selection);

            selection.Width = Math.Abs(position.X - X);
            selection.Height = Math.Abs(position.Y - Y);

            mooving = false;

            var result = GetSelectedPixels();
        }

        private void canvas_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (!mooving)
            {
                return;
            }

            Point position = e.GetPosition(canvas);

            double X = Canvas.GetLeft(selection);
            double Y = Canvas.GetTop(selection);

            selection.Width = Math.Abs(position.X - X);
            selection.Height = Math.Abs(position.Y - Y);
        }

        private Rectangle GetSelectedPixels()
        {
            double screenX = Canvas.GetLeft(selection);
            double screenY = Canvas.GetTop(selection);

            double screenWidth = selection.Width;
            double screenHeight = selection.Height;

            double sourceWidth = image.Source.Width;
            double sourceHeight = image.Source.Width;

            double xScale = canvas.Width / sourceWidth;
            double yScale = canvas.Height / sourceHeight;

            int rectX = (int)(screenX / xScale);
            int rectY = (int)(screenY / yScale);

            int rectWidth = (int)(screenWidth / xScale);
            int rectHeight = (int)(screenHeight / yScale);

            return new Rectangle(rectX, rectY, rectWidth, rectHeight);
        }
    }
}
