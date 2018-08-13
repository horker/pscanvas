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
    public class MultipleCanvas : ICanvas
    {
        private List<Window> _windows;
        private List<IPane> _panes;
        private IDictionary<string, object> _windowProps;

        public IReadOnlyList<IPane> Panes { get => _panes; }

        public MultipleCanvas(IDictionary<string, object> windowProps = null)
        {
            _windows = new List<Window>();
            _panes = new List<IPane>();

            if (windowProps == null)
                _windowProps = new Dictionary<string, object>();
            else
                _windowProps = new Dictionary<string, object>(windowProps);
        }

        public void AddPane(IPane pane)
        {
            Window window = WpfWindow.CreateWindow(null, _windowProps, w => {
                w.Title = pane.Name;
                w.Content = pane.Content;

                w.Closed += (sender, e) => {
                    RemovePane(pane);
                };
            });

            _windows.Add(window);
            _panes.Add(pane);
        }

        public void ReplacePane(IPane oldPane, IPane newPane)
        {
            var index = _panes.IndexOf(oldPane);
            _windows[index].Content = newPane.Content;
            _panes[index] = newPane;
        }

        public void RemovePane(IPane pane)
        {
            var index = _panes.IndexOf(pane);
            RemovePaneAt(index);
        }

        public void RemovePaneAt(int index)
        {
            if (index < 0)
                index = _panes.Count + index;

            if (index < 0 || _panes.Count <= index)
                throw new ArgumentOutOfRangeException("index");

            Helpers.InvokeInWindowLoop(() => {
                _windows[index].Close();
            });
            _windows.RemoveAt(index);
            _panes.RemoveAt(index);
        }

        public void Close()
        {
            Helpers.InvokeInWindowLoop(() => {
                // You can't use foreach because _windows are updated in the Window.OnClose() event.
                var w = _windows.ToArray();
                for (var i = 0; i < w.Length; ++i)
                    w[i].Close();
            });
        }

        public bool IsClosed()
        {
            return _windows.Count == 0;
        }

        public void SetFocusAt(int index)
        {
            if (index < 0)
                index = _windows.Count + index;

            if (index < 0 || _windows.Count <= index)
                throw new ArgumentOutOfRangeException("index");

            Helpers.InvokeInWindowLoop(() => {
                _windows[index].Focus();
            });
        }

        public void Activate(int index)
        {
            if (index < 0)
                index = _windows.Count + index;

            if (index < 0 || _windows.Count <= index)
                throw new ArgumentOutOfRangeException("index");

            Helpers.InvokeInWindowLoop(() => {
                if (_windows[index].IsVisible)
                    _windows[index].Show();

                if (_windows[index].WindowState == WindowState.Minimized)
                    _windows[index].WindowState = WindowState.Normal;

                _windows[index].Activate();
            });
        }

        public void MoveToForeground(int index)
        {
            if (index < 0)
                index = _windows.Count + index;

            if (index < 0 || _windows.Count <= index)
                throw new ArgumentOutOfRangeException("index");

            Helpers.InvokeInWindowLoop(() => {
                if (_windows[index].IsVisible)
                    _windows[index].Show();

                if (_windows[index].WindowState == WindowState.Minimized)
                    _windows[index].WindowState = WindowState.Normal;

                if (_windows[index].Topmost)
                    return;

                _windows[index].Topmost = true;
                _windows[index].Topmost = false;
            });
        }
    }
}
