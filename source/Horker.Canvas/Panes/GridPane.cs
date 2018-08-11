using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Horker.Canvas
{
    public class GridPane : IPane
    {
        private string _name;
        private Grid _grid;
        private int _splitterCount;

        public string Name { get => _name; }
        public UIElement Content { get => _grid; }

        public Grid Grid { get => _grid; }

        public GridPane(string name, string[] rowDefinitions, string[] columnDefinitions, bool resizable = true)
        {
            _name = name;

            WpfWindow.RootWindow.Dispatcher.Invoke(() => {
                _grid = new Grid();
                _grid.HorizontalAlignment = HorizontalAlignment.Stretch;
                _grid.VerticalAlignment = VerticalAlignment.Stretch;
                _grid.ShowGridLines = false;

                for (var i = 0; i < rowDefinitions.Length; ++i)
                {
                    var def = rowDefinitions[i];
                    var converter = new GridLengthConverter();
                    var length = (GridLength)converter.ConvertFromString(def);

                    var rowDef = new RowDefinition();
                    rowDef.Height = length;
                    _grid.RowDefinitions.Add(rowDef);
                }

                for (var i = 0; i < columnDefinitions.Length; ++i)
                {
                    var def = columnDefinitions[i];
                    var converter = new GridLengthConverter();
                    var length = (GridLength)converter.ConvertFromString(def);

                    var columnDef = new ColumnDefinition();
                    columnDef.Width = length;
                    _grid.ColumnDefinitions.Add(columnDef);
                }

                // Add grid splitters

                if (resizable)
                {
                    for (var i = 0; i < rowDefinitions.Length - 1; ++i)
                    {
                        var splitter = new GridSplitter()
                        {
                            Height = 5,
                            VerticalAlignment = VerticalAlignment.Bottom,
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            Background = Brushes.Transparent
                        };
                        _grid.Children.Add(splitter);
                        Grid.SetRow(splitter, i);
                        Grid.SetColumn(splitter, 0);
                        Grid.SetColumnSpan(splitter, columnDefinitions.Length);
                        Grid.SetZIndex(splitter, 99);
                    }

                    for (var i = 0; i < columnDefinitions.Length - 1; ++i)
                    {
                        var splitter = new GridSplitter()
                        {
                            Width = 5,
                            VerticalAlignment = VerticalAlignment.Stretch,
                            HorizontalAlignment = HorizontalAlignment.Right,
                            Background = Brushes.Transparent
                        };
                        _grid.Children.Add(splitter);
                        Grid.SetRow(splitter, 0);
                        Grid.SetColumn(splitter, i);
                        Grid.SetRowSpan(splitter, rowDefinitions.Length);
                        Grid.SetZIndex(splitter, 99);
                    }
                }

                _splitterCount = _grid.Children.Count;
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
            SetContent(pane.Content, row, column, span);
        }

        public void AddContent(UIElement content)
        {
            int row = 0;
            int column = 0;
            WpfWindow.RootWindow.Dispatcher.Invoke(() => {
                var count = _grid.Children.Count - _splitterCount;
                var columns = _grid.ColumnDefinitions.Count;

                row = count / columns;
                column = count % columns;
            });

            SetContent(content, row, column);
        }

        public void AddContent(IPane pane)
        {
            AddContent(pane.Content);
        }

        public void RemoveContent(UIElement content)
        {
            _grid.Children.Remove(content);
        }
    }
}
