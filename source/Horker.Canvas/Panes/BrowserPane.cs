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
    public class BrowserPane : PaneBase
    {
        private string _name;
        private string _uri;
        private WebBrowser _browser;

        public override string Name { get => _name; set { _name = value; } }

        public override UIElement Content { get => _browser; }

        public BrowserPane(string uri, IDictionary<string, object> props = null)
        {
            _uri = _name = uri;

            Helpers.InvokeInWindowLoop(() => {
                _browser = new WebBrowser();
                _browser.Source = new Uri(uri);
            });
        }
    }
}
