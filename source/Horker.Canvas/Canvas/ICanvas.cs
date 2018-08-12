using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.Canvas
{
    public interface ICanvas
    {
        IReadOnlyList<IPane> Panes { get; }

        void AddPane(IPane pane);
        void ReplacePane(IPane oldPane, IPane newPane);
        void RemovePaneAt(int index);

        void Close();
        bool IsClosed();

        void SetFocusAt(int index);
    }
}
