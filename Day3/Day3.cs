using AdventOfCode2022.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day3
{
    public class Day3 : AdventDay<Day3, string[]>
    {
        protected override string SolvePart1()
        {
            return Input
                .Select(line =>
                {
                    var middle = line.Length / 2;
                    return (line.Substring(0, middle), line.Substring(middle));
                })
                .Select(line => line.Item1.First(c => line.Item2.Contains(c)))
                .Select(SetPriority)
                .Sum()
                .ToString();
        }

        protected override string SolvePart2()
        {
            return Input
                .SplitBy(3)
                .Select(group => group.First().First(@char => group.Skip(1).All(x => x.Contains(@char))))
                .Select(SetPriority)
                .Sum()
                .ToString();
        }

        protected override string[] LoadInput()
        {
            return GetInputLines();
        }

        private int SetPriority(char character)
        {
            return character >= 'a' && character <= 'z'
                ? character - 'a' + 1
                : character - 'A' + 27;
        }
    }
}
