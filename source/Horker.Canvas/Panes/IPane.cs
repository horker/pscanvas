using System.Windows;

namespace Horker.Canvas
{
    public enum ImageFileType
    {
        BMP,
        PNG,
        GIF,
        JPEG
    };

    public interface IPane
    {
        string Name { get; }
        UIElement Content { get; }

        void SaveToFile(string path, ImageFileType type, double dpiX = 96.0, double dpiY = 96.0);
    }
}
