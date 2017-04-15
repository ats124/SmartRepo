using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softentertainer.SmartRepo.Utils
{
    public static class CollectionExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, TValue def = default(TValue))
        {
            TValue val;
            if (@this.TryGetValue(key, out val))
            {
                return val;
            }
            else
            {
                return def;
            }
        }
    }
}
