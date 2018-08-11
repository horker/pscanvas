using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.Canvas
{
    internal class Helpers
    {
        public static void SetProperties(object obj, IDictionary<string, object> props)
        {
            if (props == null)
                return;

            var type = obj.GetType();
            foreach (var entry in props)
            {
                var prop = type.GetProperty((string)entry.Key);
                prop.SetValue(obj, entry.Value);
            }
        }

        public static IDictionary<string, object> ToDictionary(Hashtable hash)
        {
            if (hash == null)
                return null;

            var dict = new Dictionary<string, object>();
            foreach (DictionaryEntry entry in hash)
            {
                dict[entry.Key.ToString()] = entry.Value;
            }

            return dict;
        }
    }
}
