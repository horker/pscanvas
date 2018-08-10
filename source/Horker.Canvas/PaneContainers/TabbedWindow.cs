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
    public class TabbedWindow : IPaneContainer
    {
        private Window _window;
        private List<IPane> _panes;

        public Window Window { get => _window; }

        public TabControl TabControl { get => (TabControl)_window.Content; }

        public IReadOnlyList<IPane> Panes { get => _panes; }

        public TabbedWindow()
        {
            Window window = WpfWindow.CreateWindow(null, w => {
                w.Title = "pscanvas (tabbed)";

                var tab = new ScrollableTabControl.ScrollableTabControl();
                tab.HorizontalAlignment = HorizontalAlignment.Stretch;
                tab.VerticalAlignment = VerticalAlignment.Stretch;

                w.Content = tab;
            });

            _window = window;

            _panes = new List<IPane>();
        }

        public void AddPane(IPane pane)
        {
            _window.Dispatcher.Invoke(() => {
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
            TabControl.Items.Remove(tabItem);
        }
    }
}
