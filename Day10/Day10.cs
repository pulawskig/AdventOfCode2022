using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day10
{
    public class Day10 : AdventDay<Day10, List<Day10.Command>>
    {
        protected override string SolvePart1()
        {
            var x = 1;
            var cycles = 0;
            var strength = 0;

            foreach (var command in Input)
            {
                var nextCycles = command.CycleCount;
                cycles += nextCycles;

                if (CheckCycles(cycles))
                {
                    strength += cycles * x;
                }
                else if (nextCycles > 1 && CheckCycles(cycles -  1))
                {
                    strength += (cycles - 1) * x;
                }

                if (command.Value.HasValue)
                {
                    x += command.Value.Value;
                }
            }

            return strength.ToString();
        }

        protected override string SolvePart2()
        {
            var x = 1;
            var cycles = 0;
            var pixelBuilder = new StringBuilder();

            foreach (var command in Input)
            {
                AppendPixel(x, cycles, pixelBuilder);
                cycles++;

                if (command.Type == CommandType.AddX)
                {
                    AppendPixel(x, cycles, pixelBuilder);
                    cycles++;
                    x += command.Value!.Value;
                }
            }

            var lines = pixelBuilder
                .ToString()
                .Chunk(40)
                .Select(charArray => new string(charArray))
                .Prepend(string.Empty);

            return string.Join("\n", lines);
        }

        protected override List<Command> LoadInput()
        {
            return GetInputLines()
                .Select(x => new Command(x))
                .ToList();
        }

        private bool CheckCycles(int cycleCount) => cycleCount == 20 || (cycleCount - 20) % 40 == 0;

        private void AppendPixel(int x, int cycleCount, StringBuilder pixelBuilder)
        {
            var pixel = Math.Abs(x - (cycleCount % 40)) <= 1
                ? '#'
                : '.';
            pixelBuilder.Append(pixel);
        }

        public enum CommandType
        {
            Noop,
            AddX,
        }

        public struct Command
        {
            public CommandType Type { get; set; }

            public int? Value { get; set; }

            public int CycleCount => Type switch
            {
                CommandType.Noop => 1,
                CommandType.AddX => 2,
                _ => throw new NotImplementedException(),
            };

            public Command(string line)
            {
                if (line == "noop")
                {
                    Type = CommandType.Noop;
                }
                else
                {
                    Type = CommandType.AddX;
                    Value = int.Parse(line.Split(' ')[1]);
                }
            }
        }
    }
}
