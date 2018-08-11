using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.Canvas
{
    public class PaneContainerManager
    {
        // Global instance
        public static PaneContainerManager Instance = new PaneContainerManager();

        private List<IPaneContainer> _containers;

        public PaneContainerManager()
        {
            _containers = new List<IPaneContainer>();
        }

        public void Add(IPaneContainer paneContainer)
        {
            _containers.Add(paneContainer);
        }

        public void Remove(IPaneContainer painContainer)
        {
            _containers.Remove(painContainer);
        }

        public IReadOnlyList<IPaneContainer> GetLiveContainers()
        {
            RemoveClosedContainer();
            return _containers;
        }

        private void RemoveClosedContainer()
        {
            _containers.RemoveAll(x => x.IsClosed());
        }
    }
}
