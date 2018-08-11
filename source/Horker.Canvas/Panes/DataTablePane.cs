using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Horker.Canvas
{
    public class DataGridPane : IPane
    {
        private string _name;
        private DataGrid _dataGrid;
        private DataTable _dataTable;

        public string Name { get => _name; }
        public UIElement Content { get => _dataGrid; }

        public DataTable DataTable { get => _dataTable; }

        public DataGridPane(string name, DataTable dataTable, IDictionary<string, object> props = null)
        {
            _name = name;

            _dataTable = dataTable;

            WpfWindow.RootWindow.Dispatcher.Invoke(() => {
                _dataGrid = new DataGrid();
                _dataGrid.ItemsSource = _dataTable.DefaultView;
                _dataGrid.AutoGenerateColumns = true;
                _dataGrid.CanUserAddRows = false;

                Helpers.SetProperties(_dataGrid, props);
            });
        }
    }
}
