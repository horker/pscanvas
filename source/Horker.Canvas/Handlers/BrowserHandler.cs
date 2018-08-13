using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Horker.Canvas
{
    public class BrowserHandler: GridHandlerBase
    {
        public override Type[] GetPreferredTypes()
        {
            return new Type[] {};
        }

        protected override string GetGridPaneName(IEnumerable<object> objects)
        {
            return "Pages";
        }

        public override string[] GetPreferredFileExtensions()
        {
            return new string[] { ".html", ".htm" };
        }

        protected override IPane CreatePane(object obj)
        {
            var type = obj.GetType();

            if (type == typeof(string))
            {
                var uri = obj as string;
                return new BrowserPane(uri);
            }
            else if (type == typeof(FileInfo))
            {
                var file = (obj as FileInfo).FullName;
                return new BrowserPane(file);
            }
            else if (type == typeof(DirectoryInfo))
            {
                // TODO: show the Windows folder icon.
                throw new NotImplementedException();
            }

            throw new ArgumentException("Unknown object type");
        }
    }
}
