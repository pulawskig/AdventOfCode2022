using AdventOfCode2022.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day5
{
    public class Day5 : AdventDay<Day5, (Stack<char>[], Day5.Instruction[])>
    {
        private static Regex _containerRegex = new Regex(@"(?:(?:\[(\w)\]|(   )) )?");

        protected override string SolvePart1()
        {
            var (containers, instructions) = Input;
            containers = containers.Select(x => new Stack<char>(x.Reverse())).ToArray();

            foreach (var instruction in instructions)
            {
                for (var i = 0; i < instruction.Count; i++)
                {
                    var sourceContainer = containers[instruction.Source];
                    if (!sourceContainer.Any())
                    {
                        break;
                    }

                    containers[instruction.Destination].Push(sourceContainer.Pop());
                }
            }

            return string.Join("", containers.Select(c => c.FirstOrDefault().ToString()));
        }

        protected override string SolvePart2()
        {
            var (containers, instructions) = Input;
            containers = containers.Select(x => new Stack<char>(x.Reverse())).ToArray();

            foreach (var instruction in instructions)
            {
                var sourceContainer = containers[instruction.Source];
                var take = Math.Min(instruction.Count, sourceContainer.Count);
                var temp = new Stack<char>();

                for (var j = 0; j < take; j++)
                {
                    temp.Push(sourceContainer.Pop());
                }

                for (var j = 0; j < take; j++)
                {
                    containers[instruction.Destination].Push(temp.Pop());
                }
            }

            return string.Join("", containers.Select(c => c.FirstOrDefault().ToString()));
        }

        protected override (Stack<char>[], Instruction[]) LoadInput()
        {
            var lines = GetInputLines();
            var containerLines = lines.TakeWhile(x => !string.IsNullOrWhiteSpace(x)).SkipLast(1).Select(l => l + ' ').Reverse().ToArray();
            var instructions = lines.Skip(containerLines.Length + 2).Select(l => new Instruction(l)).ToArray();

            var containerCount = int.Parse(lines
                .Skip(containerLines.Length)
                .First()
                .Replace(" ", "")
                .Last()
                .ToString());

            var containers = Enumerable.Range(0, containerCount)
                .Select(_ => new Stack<char>())
                .ToArray();

            foreach (var containerLine in containerLines)
            {
                var matches = _containerRegex.Matches(containerLine);
                for (var i = 0; i < containerCount; i++)
                {
                    var value = matches.ElementAt(i).Groups[1].Value;
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        continue;
                    }

                    containers[i].Push(value[0]);
                }
            }

            return (containers, instructions);
        }

        public struct Instruction
        {
            public int Count { get; set; }

            public int Source { get; set; }

            public int Destination { get; set; }

            private static Regex _regex = new Regex(@"^move (\d+) from (\d) to (\d)$");

            public Instruction(string source)
            {
                var match = _regex.Match(source);
                var values = match.Groups.Values.Skip(1).Select(g => int.Parse(g.Value)).ToArray();
                Count = values[0];
                Source = values[1] - 1;
                Destination = values[2] - 1;
            }
        }
    }
}
