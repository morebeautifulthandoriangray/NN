using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tensorflow.Keras.Engine;

namespace Kursovaya
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow() => InitializeComponent();

        private void OpenImageButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Выбор картинки",
                Filter = "Картинки| *.png; *.jpeg; *.bpm; *.jpg|Все файлы (*.*)|*.*",
                CheckFileExists = true,
            };

            if (dialog.ShowDialog() != true) return;

            var file = dialog.FileName;


            /*
            if (Image.Source != null)
            {
                Image.Source = null;
            }
            */


            BitmapImage image = new BitmapImage(new Uri(file));
            Image.Source = new BitmapImage(new Uri(file));
            byte[] data;

            
            using (MemoryStream ms = new MemoryStream())
            {
                BitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(ms);
                data = ms.ToArray();
            }
            


            var result = AnimalClassification.Predict(new AnimalClassification.ModelInput
            {
               ImageSource = data

            }); 


            ResultText.Text = $"{ result.PredictedLabel} - {result.Score.Max():p0}";
        }

        private static byte[] ImageToBytes(BitmapImage image)
        {
            byte[] Data;
            JpegBitmapEncoder JpegEncoder = new JpegBitmapEncoder();
            JpegEncoder.Frames.Add(BitmapFrame.Create(image));
            using (System.IO.MemoryStream MS = new System.IO.MemoryStream())
            {
                JpegEncoder.Save(MS);
                Data = MS.ToArray();
            }
            return Data;
        }



    }
}
