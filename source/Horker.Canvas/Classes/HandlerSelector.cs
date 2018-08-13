using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.Canvas
{
    public class HandlerSelector
    {
        public static HandlerSelector Instance { get; } = new HandlerSelector();

        public Dictionary<string, IHandler> Handlers { get; private set; }
        public Dictionary<Type, IHandler> TypeHandlerMap { get; private set; }
        public IHandler FallbackHandler { get; private set; }

        public HandlerSelector()
        {
            Handlers = new Dictionary<string, IHandler>();
            TypeHandlerMap = new Dictionary<Type, IHandler>();

            // Preset handlers
            RegisterHandler("image", new ImageHandler());
            RegisterHandler("browser", new BrowserHandler());

            FallbackHandler = null;
        }

        public void RegisterHandler(string name, IHandler handler)
        {
            Handlers[name] = handler;
            AddHandlerTypes(handler);
        }

        public void AddHandlerTypes(IHandler handler)
        {
            var types = handler.GetPreferredTypes();
            foreach (var type in types)
                TypeHandlerMap[type] = handler;
        }

        public IHandler SelectByName(string name)
        {
            IHandler handler = null;
            Handlers.TryGetValue(name.ToLower(), out handler);

            if (handler == null)
                handler = FallbackHandler;

            if (handler == null)
                throw new ApplicationException(string.Format("No handler found for type name {0}", name));

            return handler;
        }

        public IHandler SelectByType(Type type)
        {
            IHandler handler = null;
            TypeHandlerMap.TryGetValue(type, out handler);

            if (handler == null)
                handler = FallbackHandler;

            if (handler == null)
                throw new ApplicationException(string.Format("No handler found for type {0}", type.FullName));

            return handler;
        }
    }
}
