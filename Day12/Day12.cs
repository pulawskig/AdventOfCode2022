using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day12
{
    public class Day12 : AdventDay<Day12, Day12.Area[][]>
    {
        public Day12() : base()
        {
            ReloadForPart2 = true;
        }

        protected override string SolvePart1()
        {
            var flatInput = Input.SelectMany(x => x).ToArray();
            var start = flatInput.First(x => x.IsStart);
            var end = flatInput.First(x => x.IsEnd);

            var heuristic = (Area current) => Math.Sqrt(Math.Pow(current.X - end.X, 2) + Math.Pow(current.Y - end.Y, 2));

            var path = AStar(start, heuristic, a => a.IsEnd, height => height < -1);
            var stepCount = path.Count - 1;
            return stepCount.ToString();
        }

        protected override string SolvePart2()
        {
            var end = Input.SelectMany(x => x).First(x => x.IsEnd);

            var heuristic = (Area current) => (double) current.Height;

            var path = AStar(end, heuristic, a => a.Height == 0, height => height > 1);
            var stepCount = path.Count - 1;
            return stepCount.ToString();
        }

        protected override Area[][] LoadInput()
        {
            var areas = GetInputLines()
                .Select((line, i) => line
                    .Select((c, j) => new Area(i, j, c))
                    .ToArray())
                .ToArray();

            for (var i = 0; i < areas.Length; i++)
            {
                for (var j = 0; j < areas[i].Length; j++)
                {
                    if (i > 0)
                    {
                        areas[i][j].Neighbours.Add(areas[i - 1][j]);
                    }

                    if (j > 0)
                    {
                        areas[i][j].Neighbours.Add(areas[i][j - 1]);
                    }

                    if (i < areas.Length - 1)
                    {
                        areas[i][j].Neighbours.Add(areas[i + 1][j]);
                    }

                    if (j < areas[i].Length - 1)
                    {
                        areas[i][j].Neighbours.Add(areas[i][j + 1]);
                    }
                }
            }

            return areas;
        }

        private List<Area> AStar(Area start, Func<Area, double> heuristic, Func<Area, bool> foundPredicate, Func<int, bool> heightPredicate)
        {
            var queue = new Dictionary<Area, double>();
            var previous = new Dictionary<Area, Area>();
            var currentScore = new Dictionary<Area, double>();

            currentScore[start] = 0;

            queue[start] = heuristic(start);

            while (queue.Count > 0)
            {
                var current = queue.MinBy(x => x.Value).Key;
                queue.Remove(current);

                current.Visited = true;

                if (foundPredicate(current))
                {
                    return ReconstructPath(previous, current);
                }

                foreach (var neighbour in current.Neighbours)
                {
                    if (neighbour.Visited || heightPredicate(current.Height - neighbour.Height))
                    {
                        continue;
                    }

                    var proposedScore = currentScore[current] + 1;
                    if (!currentScore.TryGetValue(neighbour, out var score) || proposedScore < score)
                    {
                        previous[neighbour] = current;
                        currentScore[neighbour] = proposedScore;
                        queue[neighbour] = proposedScore + heuristic(neighbour);
                    }
                }
            }

            throw new InvalidOperationException();
        }

        private List<Area> ReconstructPath(Dictionary<Area, Area> previous, Area end)
        {
            var list = new List<Area>
            {
                end
            };

            var current = end;
            while (true)
            {
                if (!previous.ContainsKey(current))
                {
                    break;
                }

                current = previous[current];
                list.Add(current);
            }

            list.Reverse();
            return list;
        }

        public class Area
        {
            public int X { get; set; }

            public int Y { get; set; }

            public int Height { get; set; }

            public bool IsStart { get; set; }

            public bool IsEnd { get; set; }

            public bool Visited { get; set; }

            public List<Area> Neighbours { get; set; }

            public override bool Equals(object? obj)
            {
                return obj is Area area && area.X == X && area.Y == Y && area.Height == Height;
            }

            public override int GetHashCode()
            {
                return (X, Y, Height).GetHashCode();
            }

            public Area(int i, int j, char height)
            {
                X = j;
                Y = i;
                Height = height switch
                {
                    'S' => 0,
                    'E' => 'z' - 'a',
                    (>= 'a' and <= 'z') => height - 'a',
                    _ => throw new NotImplementedException(),
                };
                IsStart = height == 'S';
                IsEnd = height == 'E';
                Neighbours = new List<Area>();
            }
        }
    }
}
