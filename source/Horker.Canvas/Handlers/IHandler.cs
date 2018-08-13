using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Horker.Canvas
{
    public interface IHandler
    {
        Type[] GetPreferredTypes();

        IEnumerable<IPane> BeginProcessing();
        IEnumerable<IPane> ProcessRecord(object inputObject);
        IEnumerable<IPane> EndProcessing();
    }
}
