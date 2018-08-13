using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Horker.Canvas
{
    public abstract class GridHandlerBase : IHandler
    {
        private List<object> _objects;

        public abstract Type[] GetPreferredTypes();

        protected abstract string GetGridPaneName(IEnumerable<object> objects);

        protected abstract IPane CreatePane(object obj);

        public IEnumerable<IPane> BeginProcessing()
        {
            _objects = new List<object>();

            return null;
        }

        public IEnumerable<IPane> ProcessRecord(object inputObject)
        {
            _objects.Add(inputObject);

            return null;
        }

        public IEnumerable<IPane> EndProcessing()
        {
            if (_objects.Count == 1)
                return new IPane[] { CreatePane(_objects[0]) };

            var columns = (int)Math.Ceiling(Math.Sqrt(_objects.Count));
            var rows = (int)Math.Ceiling((double)_objects.Count / columns);

            var name = GetGridPaneName(_objects);
            var grid = new GridPane(name, rows, columns, true);

            foreach (var obj in _objects)
            {
                var pane = CreatePane(obj);
                grid.AddContent(pane);
            }

            return new IPane[] { grid };
        }
    }
}
