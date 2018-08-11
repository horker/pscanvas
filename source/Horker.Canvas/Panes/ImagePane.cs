using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Horker.Canvas
{
    public class ImagePane : IPane
    {
        private string _file;
        private Image _image;

        public string Name { get => _file; }
        public UIElement Content { get => _image; }

        public ImagePane(string file, IDictionary<string, object> props = null)
        {
            _file = file;

            WpfWindow.RootWindow.Dispatcher.Invoke(() => {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(file);
                bitmap.EndInit();

                _image = new Image();
                _image.Source = bitmap;

                Helpers.SetProperties(_image, props);
            });
        }
    }
}
