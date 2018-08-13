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
                foreach (var w in _windows)
                    w.Close();
            });
            _windows.Clear();
            _panes.Clear();
        }

        public bool IsClosed()
        {
            return _windows.Count == 0;
        }

        public void SetFocusAt(int index)
        {
            if (index < 0)
                index = _panes.Count + index;

            if (index < 0 || _panes.Count <= index)
                throw new ArgumentOutOfRangeException("index");

            Helpers.InvokeInWindowLoop(() => {
                _windows[index].Focus();
            });
        }
    }
}
