using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Tools
{
    public static class LinqExtensions
    {
        public static IEnumerable<IEnumerable<T>> SplitBy<T> (this IEnumerable<T> source, Func<T, bool> predicate)
        {
            var list = new List<T>();
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return list;
                    list = new List<T>();
                    continue;
                }

                list.Add(item);
            }
            yield return list;
        }

        public static IEnumerable<IEnumerable<T>> SplitBy<T>(this IEnumerable<T> source, int count)
        {
            var list = new List<T>();
            var i = 0;

            foreach (var item in source)
            {
                list.Add(item);

                if ((i + 1) % count == 0)
                {
                    yield return list;
                    list = new List<T>();
                }

                i++;
            }

            if (list.Any())
            {
                yield return list;
            }
        }
    }
}
