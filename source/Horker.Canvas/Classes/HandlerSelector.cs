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
        public Dictionary<string, IHandler> FileExtensionHandlerMap { get; private set; }
        public IHandler FallbackHandler { get; private set; }

        public HandlerSelector()
        {
            Handlers = new Dictionary<string, IHandler>();
            TypeHandlerMap = new Dictionary<Type, IHandler>();
            FileExtensionHandlerMap = new Dictionary<string, IHandler>();

            // Preset handlers
            RegisterHandler("image", new ImageHandler());
            RegisterHandler("browser", new BrowserHandler());

            FallbackHandler = null;
        }

        public void RegisterHandler(string name, IHandler handler)
        {
            Handlers[name] = handler;
            AddHandlerTypes(handler);
            AddHandlerFileExtensions(handler);
        }

        public void AddHandlerTypes(IHandler handler)
        {
            var types = handler.GetPreferredTypes();
            foreach (var type in types)
                TypeHandlerMap[type] = handler;
        }

        public void AddHandlerFileExtensions(IHandler handler)
        {
            var extensions = handler.GetPreferredFileExtensions();
            foreach (var ext in extensions)
                FileExtensionHandlerMap[ext.ToLower()] = handler;
        }

        public IHandler SelectByName(string name)
        {
            IHandler handler = null;
            Handlers.TryGetValue(name.ToLower(), out handler);

            return handler;
        }

        public IHandler SelectByType(Type type)
        {
            IHandler handler = null;
            TypeHandlerMap.TryGetValue(type, out handler);

            return handler;
        }

        public IHandler SelectByFileExtension(string path)
        {
            IHandler handler = null;

            var i = path.LastIndexOf(".");
            if (i != -1)
            {
                var ext = path.Substring(i).ToLower();

                foreach (var entry in FileExtensionHandlerMap)
                {
                    if (ext == entry.Key)
                    {
                        handler = entry.Value;
                        break;
                    }
                }
            }

            return handler;
        }
    }
}
