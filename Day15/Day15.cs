using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day15
{
    public class Day15 : AdventDay<Day15, List<Day15.Reading>>
    {
        protected override string SolvePart1()
        {
            var linePos = 2_000_000;

            var maxInRange = Input.Max(reading => reading.Sensor.X - Math.Abs(linePos - reading.Sensor.Y) + reading.Distance);
            var minInRange = Input.Min(reading => reading.Sensor.X + Math.Abs(linePos - reading.Sensor.Y) - reading.Distance);

            return (maxInRange - minInRange).ToString();
        }

        protected override string SolvePart2()
        {
            var max = 4_000_000L;
            var rotated = Input
                .SelectMany(r1 => Input.Select(r2 => (r1, r2)))
                .Select(pair => (
                    X: (pair.r2.Sensor.X + pair.r2.Sensor.Y + pair.r2.Distance + pair.r1.Sensor.X - pair.r1.Sensor.Y - pair.r1.Distance) / 2,
                    Y: (pair.r2.Sensor.X + pair.r2.Sensor.Y + pair.r2.Distance - pair.r1.Sensor.X + pair.r1.Sensor.Y + pair.r1.Distance) / 2 + 1
                ))
                .Select(pos => new Position(pos.X, pos.Y));

            foreach (var rot in rotated)
            {
                if (rot.X > 0 && rot.X < max && rot.Y > 0 && rot.Y < max)
                {
                    if (Input.All(reading => reading.Sensor.DistanceTo(rot) > reading.Distance))
                    {
                        return (max * rot.X + rot.Y).ToString();
                    }
                }
            }

            return string.Empty;
        }

        protected override List<Reading> LoadInput()
        {
            var regex = new Regex(@"x=(-?\d+), y=(-?\d+).*x=(-?\d+), y=(-?\d+)", RegexOptions.Compiled);
            return GetInputLines()
                .Select(line => regex.Match(line))
                .Select(match => match.Groups.Values.Skip(1).Select(g => g.Value).ToArray())
                .Select(match => new Reading(match))
                .ToList();
        }

        public class Reading
        {
            public Position Sensor { get; set; }

            public Position Beacon { get; set; }

            private int? _distance;
            public int Distance => _distance ??= Sensor.DistanceTo(Beacon);

            public Reading(string[] values)
            {
                Sensor = new Position(values[0], values[1]);
                Beacon = new Position(values[2], values[3]);
            }
        }

        public struct Position : IEquatable<Position>
        {
            public int X { get; set; }

            public int Y { get; set; }

            public Position(string x, string y) : this(int.Parse(x), int.Parse(y))
            {
            }

            public Position(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int DistanceTo(Position another) => Math.Abs(another.X - X) + Math.Abs(another.Y - Y);

            public bool Equals(Position other)
            {
                return X == other.X && Y == other.Y;
            }
        }
    }
}
