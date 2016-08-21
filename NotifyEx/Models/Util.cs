using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper;
using MetroTrilithon.Mvvm;

namespace NotifyEx.Models
{
    public static class Util
    {
        public static bool IsInEvent
            => KanColleClient.Current.Master?.MapAreas.Values
                .Any(area => area.RawData.api_type == 1)
               ?? false;

        public static int FirstIndex<T>(this IEnumerable<T> source, Func<T, bool> selector)
        {
            return source
                .Select((item, index) => new {index, result = selector(item)})
                .First(pair => pair.result)
                .index;
        }
    }
}
