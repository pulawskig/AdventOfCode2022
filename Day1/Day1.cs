using AdventOfCode2022.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day1
{
    public class Day1 : AdventDay<Day1, List<List<long>>>
    {
        protected override string SolvePart1()
        {
            var result = Input.Select(group => group.Sum()).Max();

            return result.ToString();
        }

        protected override string SolvePart2()
        {
            var result = Input
                .Select(group => group.Sum())
                .OrderByDescending(sum => sum)
                .Take(3)
                .Sum();

            return result.ToString();
        }

        protected override List<List<long>> LoadInput()
        {
            return GetInputLines()
                .SplitBy(line => string.IsNullOrEmpty(line))
                .Select(group => group.Select(line => long.Parse(line)).ToList())
                .ToList();
        } 
    }
}
