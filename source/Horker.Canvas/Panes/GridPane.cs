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
    public class GridPane : IPane
    {
        private string _name;
        private Grid _grid;

        public string Name { get => _name; }
        public UIElement Content { get => _grid; }

        public Grid Grid { get => _grid; }

        public GridPane(string name, string[] rowDefinitions, string[] columnDefinitions)
        {
            _name = name;

            WpfWindow.RootWindow.Dispatcher.Invoke(() => {
                _grid = new Grid();
                _grid.HorizontalAlignment = HorizontalAlignment.Stretch;
                _grid.VerticalAlignment = VerticalAlignment.Stretch;

                _grid.ShowGridLines = true;

                foreach (var def in rowDefinitions)
                {
                    var converter = new GridLengthConverter();
                    var length = (GridLength)converter.ConvertFromString(def);

                    var rowDef = new RowDefinition();
                    rowDef.Height = length;
                    _grid.RowDefinitions.Add(rowDef);
                }

                foreach (var def in columnDefinitions)
                {
                    var converter = new GridLengthConverter();
                    var length = (GridLength)converter.ConvertFromString(def);

                    var columnDef = new ColumnDefinition();
                    columnDef.Width = length;
                    _grid.ColumnDefinitions.Add(columnDef);
                }
            });
        }

        public void SetContent(UIElement content, int row, int column, int rowSpan = 1, int columnSpan = 1)
        {
            WpfWindow.RootWindow.Dispatcher.Invoke(() => {
                _grid.Children.Add(content);
                Grid.SetRow(content, row);
                Grid.SetColumn(content, column);
                Grid.SetRowSpan(content, rowSpan);
                Grid.SetColumnSpan(content, columnSpan);
            });
        }

        public void SetContent(IPane pane, int row, int column, int span = 1)
        {
            UIElement content = pane.Content;
            SetContent(content, row, column, span);
        }

        public void AddContent(UIElement content)
        {
            int row = 0;
            int column = 0;
            WpfWindow.RootWindow.Dispatcher.Invoke(() => {
                var count = _grid.Children.Count;
                var columns = _grid.ColumnDefinitions.Count;

                row = count / columns;
                column = count % columns;
            });

            SetContent(content, row, column);
        }

        public void AddContent(IPane pane)
        {
            UIElement content = (UIElement)pane.Content;
            AddContent(content);
        }

        public void RemoveContent(UIElement content)
        {
            _grid.Children.Remove(content);
        }
    }
}
