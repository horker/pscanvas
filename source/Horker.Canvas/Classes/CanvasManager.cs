using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.Canvas
{
    public class CanvasManager
    {
        // Global instance
        public static CanvasManager Instance = new CanvasManager();

        private List<ICanvas> _canvases;

        public CanvasManager()
        {
            _canvases = new List<ICanvas>();
        }

        public void Add(ICanvas canvas)
        {
            _canvases.Add(canvas);
        }

        public void Remove(ICanvas canvas)
        {
            _canvases.Remove(canvas);
        }

        public IReadOnlyList<ICanvas> GetLiveCanvases()
        {
            RemoveClosedCanvases();
            return _canvases;
        }

        private void RemoveClosedCanvases()
        {
            _canvases.RemoveAll(x => x.IsClosed());
        }

        public ICanvas GetActiveCanvas()
        {
            var containers = GetLiveCanvases();
            if (containers.Count > 0)
                return containers.Last();

            return null;
        }
    }
}
