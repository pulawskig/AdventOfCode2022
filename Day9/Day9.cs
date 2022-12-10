using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day9
{
    public class Day9 : AdventDay<Day9, List<Day9.Command>>
    {
        protected override string SolvePart1()
        {
            var visited = new HashSet<(int x, int y)>();
            var head = new RopeKnot();
            var tail = new RopeKnot();

            visited.Add((0, 0));

            foreach (var command in Input)
            {
                for (var i = 0; i < command.Count; i++)
                {
                    MoveHead(command, head);

                    CalculateKnotLocation(head, tail);

                    visited.Add(tail.Position);
                }
            }

            return visited.Count.ToString();
        }

        protected override string SolvePart2()
        {
            var visited = new HashSet<(int x, int y)>();
            var knots = Enumerable.Repeat(0, 10)
                .Select(_ => new RopeKnot())
                .ToArray();

            visited.Add((0, 0));

            foreach (var command in Input)
            {
                for (var i = 0; i < command.Count; i++)
                {
                    MoveHead(command, knots[0]);

                    for (var j = 1; j < knots.Length; j++)
                    {
                        CalculateKnotLocation(knots[j - 1], knots[j]);
                    }

                    visited.Add(knots[9].Position);
                }
            }

            return visited.Count.ToString();
        }

        protected override List<Command> LoadInput()
        {
            return GetInputLines()
                .Select(line => line.Split(' '))
                .Select(split => new Command(split[0], split[1]))
                .ToList();
        }

        private static void MoveHead(Command command, RopeKnot head)
        {
            head.X = command.Direction switch
            {
                Direction.Right => head.X + 1,
                Direction.Left => head.X - 1,
                _ => head.X,
            };

            head.Y = command.Direction switch
            {
                Direction.Down => head.Y + 1,
                Direction.Up => head.Y - 1,
                _ => head.Y,
            };
        }

        private static void CalculateKnotLocation(RopeKnot first, RopeKnot second)
        {
            if (Math.Abs(first.X - second.X) > 1 || Math.Abs(first.Y - second.Y) > 1)
            {
                if (first.X == second.X || first.Y == second.Y)
                {
                    if (Math.Abs(first.X - second.X) > 1)
                    {
                        second.X = first.X - second.X > 0 ? second.X + 1 : second.X - 1;
                    }
                    else if (Math.Abs(first.Y - second.Y) > 1)
                    {
                        second.Y = first.Y - second.Y > 0 ? second.Y + 1 : second.Y - 1;
                    }
                }
                else
                {
                    second.X = first.X - second.X > 0 ? second.X + 1 : second.X - 1;
                    second.Y = first.Y - second.Y > 0 ? second.Y + 1 : second.Y - 1;
                }
            }
        }

        public enum Direction
        {
            Right, Left, Up, Down
        }

        public struct Command
        {
            public Direction Direction { get; set; }

            public int Count { get; set; }

            public Command(string direction, string count)
            {
                Count = int.Parse(count);
                Direction = direction switch
                {
                    "R" => Direction.Right,
                    "L" => Direction.Left,
                    "U" => Direction.Up,
                    "D" => Direction.Down,
                    _ => throw new NotImplementedException(),
                };
            }
        }

        public class RopeKnot
        {
            public int X { get; set; }

            public int Y { get; set; }

            public (int x, int y) Position => (X, Y);
        }
    }
}
