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
    public class TabbedCanvas : ICanvas
    {
        private Window _window;
        private List<IPane> _panes;

        public Window Window { get => _window; }

        public TabControl TabControl { get => (TabControl)_window.Content; }

        public IReadOnlyList<IPane> Panes { get => _panes; }

        public TabbedCanvas(IDictionary<string, object> windowProps = null, IDictionary<string, object> tabProps = null)
        {
            Window window = WpfWindow.CreateWindow(null, windowProps, w => {
                if (string.IsNullOrEmpty(w.Title))
                    w.Title = "pscanvas (tabbed)";

                var tab = new Vortexwolf.ScrollableTabControl.ScrollableTabControl();
                tab.HorizontalAlignment = HorizontalAlignment.Stretch;
                tab.VerticalAlignment = VerticalAlignment.Stretch;

                w.Content = tab;

                Helpers.SetProperties(tab, tabProps);
            });

            _window = window;
            _panes = new List<IPane>();
        }

        public void AddPane(IPane pane)
        {
            Helpers.InvokeInWindowLoop(() => {
                var name = "Tab" + (TabControl.Items.Count + 1);

                // tab display text
                var header = new TextBlock()
                {
                    Text = pane.Name
                };

                // close button
                var closeButton = new Button()
                {
                    Content = "X",
                    VerticalAlignment = VerticalAlignment.Bottom,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Padding = new Thickness(0),
                    IsTabStop = false
                };

                // ViewBox
                var viewBox = new Viewbox()
                {
                    Child = closeButton,
                    VerticalAlignment = VerticalAlignment.Center,
                    Stretch = Stretch.Fill,
                    Margin = new Thickness(3, 0, 0, 0),
                    Height = 12,
                    Width = 12
                };

                // tab template
                var tabTemplate = new Grid();
                tabTemplate.ColumnDefinitions.Add(new ColumnDefinition());
                tabTemplate.ColumnDefinitions.Add(new ColumnDefinition());
                tabTemplate.Children.Add(header);
                Grid.SetColumn(header, 0);
                tabTemplate.Children.Add(viewBox);
                Grid.SetColumn(viewBox, 1);

                // tab item
                var tabItem = new TabItem();
                tabItem.Name = name;
                tabItem.Header = tabTemplate;
                tabItem.Content = pane.Content;

                tabItem.AddHandler(Button.ClickEvent, new RoutedEventHandler(CloseButton_OnClick));

                TabControl.Items.Add(tabItem);
            });

            _panes.Add(pane);
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            var tabItem = (TabItem)sender;
            var index = TabControl.Items.IndexOf(tabItem);
            RemovePaneAt(index);
        }

        public void ReplacePane(IPane oldPane, IPane newPane)
        {
            var index = _panes.IndexOf(oldPane);
            TabControl.Items[index] = newPane.Content;
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

            TabControl.Items.RemoveAt(index);
            _panes.RemoveAt(index);
        }

        public void Close()
        {
            Helpers.InvokeInWindowLoop(() => {
                _window.Close();
            });
        }

        public bool IsClosed()
        {
            return WpfWindow.IsWindowClosed(_window);
        }

        public void SetFocusAt(int index)
        {
            if (index < 0)
                index = _panes.Count + index;

            if (index < 0 || _panes.Count <= index)
                throw new ArgumentOutOfRangeException("index");

            Helpers.InvokeInWindowLoop(() => {
                (TabControl.Items[index] as TabItem).Focus();
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
            // ref.
            // https://stackoverflow.com/questions/257587/bring-a-window-to-the-front-in-wpf

            Helpers.InvokeInWindowLoop(() => {
                if (_window.IsVisible)
                    _window.Show();

                if (_window.WindowState == WindowState.Minimized)
                    _window.WindowState = WindowState.Normal;

                if (_window.Topmost)
                    return;

                _window.Topmost = true;
                _window.Topmost = false;
            });
        }
    }
}
