using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace Horker.Canvas
{
    public class SingleCanvas : ICanvas
    {
        private Window _window;
        private IPane _pane;
        private IDictionary<string, object> _windowProps;

        public IReadOnlyList<IPane> Panes { get => new IPane[] { _pane }; }

        public SingleCanvas(IDictionary<string, object> windowProps = null)
        {
            _window = null;
            _pane = null;

            if (windowProps == null)
                _windowProps = new Dictionary<string, object>();
            else
                _windowProps = new Dictionary<string, object>(windowProps);
        }

        public void AddPane(IPane pane)
        {
            if (_window == null)
            {
                _window = WpfWindow.CreateWindow(null, _windowProps, w => {
                    w.Title = pane.Name;
                    w.Content = pane.Content;

                    w.Closed += (sender, e) => {
                        _window = null;
                        _pane = null;
                    };
                });
            }
            else
            {
                Helpers.InvokeInWindowLoop(() => {
                    _window.Content = pane.Content;
                });
            }

            _pane = pane;
        }

        public void ReplacePane(IPane oldPane, IPane newPane)
        {
            _pane = newPane;
        }

        public void RemovePane(IPane pane)
        {
            Close();
        }

        public void RemovePaneAt(int index)
        {
            Close();
        }

        public void Close()
        {
            Helpers.InvokeInWindowLoop(() => {
                _window.Close();
            });
            _window = null;
            _pane = null;
        }

        public bool IsClosed()
        {
            return _window == null;
        }

        public void SetFocusAt(int index)
        {
            Helpers.InvokeInWindowLoop(() => {
                _pane.Content.Focus();
            });
        }

        public void Activate(int index)
        {
            Helpers.InvokeInWindowLoop(() => {
                if (_window.IsVisible)
                    _window.Show();

                if (_window.WindowState == WindowState.Minimized)
                    _window.WindowState = WindowState.Normal;

                _window.Activate();
            });
        }

        public void MoveToForeground(int index)
        {
            Helpers.InvokeInWindowLoop(() => {
                if (_window.Topmost)
                    return;

                _window.Topmost = true;
                _window.Topmost = false;
            });
        }
    }
}
