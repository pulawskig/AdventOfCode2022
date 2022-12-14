using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day14
{
    public class Day14 : AdventDay<Day14, Day14.Point[][]>
    {
        public Day14() : base()
        {
            ReloadForPart2 = true;
        }

        protected override string SolvePart1()
        {
            var source = Input[0][200];
            var finish = false;
            var count = 0L;

            while (!finish)
            {
                var current = (source.X, source.Y);
                var settled = false;

                while (current.Y < 199)
                {
                    if (!Input[current.Y + 1][current.X].IsSolid)
                    {
                        current = (current.X, current.Y + 1);
                    }
                    else if (!Input[current.Y + 1][current.X - 1].IsSolid)
                    {
                        current = (current.X - 1, current.Y + 1);
                    }
                    else if (!Input[current.Y + 1][current.X + 1].IsSolid)
                    {
                        current = (current.X + 1, current.Y + 1);
                    }
                    else
                    {
                        Input[current.Y][current.X].Type = PointType.Sand;
                        count++;
                        settled = true;
                        break;
                    }
                }

                finish = !settled;
            }

            return count.ToString();
        }

        protected override string SolvePart2()
        {
            var floor = Input.SelectMany(x => x).Where(x => x.IsSolid).Max(x => x.Y) + 2;
            foreach (var point in Input[floor])
            {
                point.Type = PointType.Rock;
            }

            var source = Input[0][200];
            var count = 0L;

            while (true)
            {
                var current = (source.X, source.Y);
                var settled = false;

                while (true)
                {
                    if (!Input[current.Y + 1][current.X].IsSolid)
                    {
                        current = (current.X, current.Y + 1);
                    }
                    else if (!Input[current.Y + 1][current.X - 1].IsSolid)
                    {
                        current = (current.X - 1, current.Y + 1);
                    }
                    else if (!Input[current.Y + 1][current.X + 1].IsSolid)
                    {
                        current = (current.X + 1, current.Y + 1);
                    }
                    else
                    {
                        Input[current.Y][current.X].Type = PointType.Sand;
                        count++;
                        settled = true;
                        break;
                    }
                }

                if (settled && current.X == source.X && current.Y == source.Y)
                {
                    break;
                }
            }

            return count.ToString();
        }

        protected override Point[][] LoadInput()
        {
            var grid = new int[200].Select((_, y) => new int[400].Select((_, x) => new Point(x, y)).ToArray()).ToArray();

            foreach (var line in GetInputLines())
            {
                var points = line
                    .Split(" -> ")
                    .Select(point => point.Split(','))
                    .Select(split => (X: int.Parse(split[0]) - 300, Y: int.Parse(split[1])))
                    .ToArray();

                var previous = points[0];
                for (var i = 1; i < points.Length; i++)
                {
                    var next = points[i];
                    var isHorizontal = previous.Y == next.Y;
                    var difference = isHorizontal
                        ? next.X - previous.X
                        : next.Y - previous.Y;

                    Action<int> setRock = (j) => (isHorizontal ? grid[previous.Y][previous.X + j] : grid[previous.Y + j][previous.X]).Type = PointType.Rock;

                    if (difference >= 0)
                    {
                        for (var j = 0; j <= difference; j++)
                        {
                            setRock(j);
                        }
                    }
                    else
                    {
                        for (var j = 0; j >= difference; j--)
                        {
                            setRock(j);
                        }
                    }

                    previous = next;
                }
            }

            return grid;
        }

        public enum PointType
        {
            Air, Rock, Sand
        }

        public class Point
        {
            public int X { get; set; }

            public int Y { get; set; }

            public PointType Type { get; set; }

            public bool IsSolid => Type != PointType.Air;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
                Type = PointType.Air;
            }
        }
    }
}
