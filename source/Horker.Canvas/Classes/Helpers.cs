using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Horker.Canvas
{
    public class Helpers
    {
        public static void SetProperties(object obj, IDictionary<string, object> props)
        {
            if (props == null)
                return;

            var type = obj.GetType();
            foreach (var entry in props)
            {
                var prop = type.GetProperty((string)entry.Key);
                prop.SetValue(obj, entry.Value);
            }
        }

        public static IDictionary<string, object> ToDictionary(Hashtable hash)
        {
            if (hash == null)
                return null;

            var dict = new Dictionary<string, object>();
            foreach (DictionaryEntry entry in hash)
            {
                dict[entry.Key.ToString()] = entry.Value;
            }

            return dict;
        }

        public static void InvokeInWindowLoop(Action action)
        {
            WpfWindow.RootWindow.Dispatcher.Invoke(action);
        }

        // ref.
        // https://stackoverflow.com/questions/26260654/wpf-converting-bitmap-to-imagesource

        public static BitmapSource BitmapToBitmapSource(Bitmap bitmap)
        {
            var handle = bitmap.GetHbitmap();

            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                Win32Api.DeleteObject(handle);
            }
        }

        // Platform neutral version
        public static BitmapImage BitmapToImageSource2(Bitmap bitmap)
        {
            var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Bmp);

            var image = new BitmapImage();

            image.BeginInit();
            stream.Seek(0, SeekOrigin.Begin);
            image.StreamSource = stream;
            image.EndInit();

            return image;
        }

    }
}
