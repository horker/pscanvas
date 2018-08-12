using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Horker.Canvas
{
    public class ImagePane : PaneBase
    {
        private string _name;
        private System.Windows.Controls.Image _image;

        public override string Name { get => _name; }
        public override UIElement Content { get => _image; }

        public ImagePane(string file, IDictionary<string, object> props = null)
        {
            Helpers.InvokeInWindowLoop(() => {
                var bitmapSource = new BitmapImage();
                bitmapSource.BeginInit();
                bitmapSource.UriSource = new Uri(file);
                bitmapSource.EndInit();

                CreateImagePane(file, bitmapSource, props);
            });
        }

        public ImagePane(string name, Bitmap bitmap, IDictionary<string, object> props = null)
        {
            Helpers.InvokeInWindowLoop(() => {
                var bitmapSource = Helpers.BitmapToBitmapSource(bitmap);
                CreateImagePane(name, bitmapSource, props);
            });
        }

        private void CreateImagePane(string name, BitmapSource bitmapSource, IDictionary<string, object> props = null)
        {
            _name = name;
            _image = new System.Windows.Controls.Image();
            _image.Source = bitmapSource;
            Helpers.SetProperties(_image, props);
        }
    }
}
