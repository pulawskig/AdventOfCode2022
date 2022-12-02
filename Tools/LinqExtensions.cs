﻿using System;
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
    }
}