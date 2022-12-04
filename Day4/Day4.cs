using AdventOfCode2022.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day4
{
    public class Day4 : AdventDay<Day4, List<(Day4.ElfRange First, Day4.ElfRange Second)>>
    {
        protected override string SolvePart1()
        {
            return Input
                .Where(x => x.First.IsInside(x.Second))
                .Count()
                .ToString();
        }

        protected override string SolvePart2()
        {
            return Input
                .Where(x => x.First.IsOverlap(x.Second))
                .Count()
                .ToString();
        }

        protected override List<(ElfRange First, ElfRange Second)> LoadInput()
        {
            return GetInputLines()
                .Select(line => line.Split(','))
                .Select(pair => pair.Select(r => new ElfRange(r)))
                .Select(pair => (pair.First(), pair.Last()))
                .ToList();
        }

        public struct ElfRange
        {
            public int Start { get; init; }

            public int End { get; init; }

            public ElfRange(string range)
            {
                var split = range.Split('-');
                Start = int.Parse(split[0]);
                End = int.Parse(split[1]);
            }

            public bool IsInside(ElfRange another)
            {
                return (Start <= another.Start && End >= another.End)
                || (another.Start <= Start && another.End >= End);
            }

            public bool IsOverlap(ElfRange another)
            {
                return End >= another.Start && Start <= another.End;
            }
        }
    }
}
