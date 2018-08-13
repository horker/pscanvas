using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Horker.Canvas
{
    public class ImageHandler : GridHandlerBase
    {
        public override Type[] GetPreferredTypes()
        {
            return new Type[] { typeof(Bitmap) };
        }

        public override string[] GetPreferredFileExtensions()
        {
            return new string[] { ".bmp", ".gif", ".png", ".jpg", ".jpeg" };
        }

        protected override string GetGridPaneName(IEnumerable<object> objects)
        {
            return "Images";
        }

        protected override IPane CreatePane(object obj)
        {
            var type = obj.GetType();

            if (type == typeof(Bitmap))
            {
                var bitmap = obj as Bitmap;
                return new ImagePane("Bitmap", bitmap);
            }
            else if (type == typeof(string))
            {
                var file = obj as string;
                return new ImagePane(file);
            }
            else if (type == typeof(FileInfo))
            {
                var file = (obj as FileInfo).FullName;
                return new ImagePane(file);
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
