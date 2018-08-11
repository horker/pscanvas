using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Horker.Canvas
{
    public interface IPane
    {
        string Name { get; }
        UIElement Content { get; }
    }

    public interface IPaneContainer
    {
        IReadOnlyList<IPane> Panes { get; }

        void AddPane(IPane pane);
        void ReplacePane(IPane oldPane, IPane newPane);
        void RemovePane(IPane pane);

        void Close();
        bool IsClosed();
    }
}
