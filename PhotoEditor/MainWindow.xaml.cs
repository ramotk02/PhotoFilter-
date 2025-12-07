using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

namespace PhotoEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    

        BitmapImage CurrentImage;
        WriteableBitmap editableImage;


        /// Open Explorer to get a pic
        private void OpenImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Images|*.jpg;*.png;*.jpeg;*.bmp";

            if (dlg.ShowDialog() == true)
            {
                CurrentImage = new BitmapImage(new Uri(dlg.FileName));
                DisplayedImage.Source = CurrentImage;

                MessageBox.Show("Good choice !!!!");

            }
        }

        // Apply RGB Filter
        private void ApplyRGB(byte rFactor, byte gFactor, byte bFactor)
        {
            if (CurrentImage == null) return;

            editableImage = new WriteableBitmap(CurrentImage); 

            int width = editableImage.PixelWidth;
            int height = editableImage.PixelHeight;
            int stride = width * 4;

            byte[] pixels = new byte[height * stride];
            editableImage.CopyPixels(pixels, stride, 0);

            for (int i=0; i<pixels.Length; i += 4)
            {
                byte b = pixels[i];
                byte g = pixels[i + 1];
                byte r = pixels[i + 2];

                pixels[i] = (byte)Math.Min(255, b * (bFactor / 100.0));
                pixels[i + 1] = (byte)Math.Min(255, g * (gFactor / 100.0));
                pixels[i + 2] = (byte)Math.Min(255, r * (rFactor / 100.0));
            }
            editableImage.WritePixels(new Int32Rect(0, 0, width, height), pixels, stride, 0);
            DisplayedImage.Source = editableImage;



        }

    }
}