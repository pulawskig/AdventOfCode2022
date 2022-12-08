using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day8
{
    public class Day8 : AdventDay<Day8, Day8.Tree[][]>
    {
        protected override string SolvePart1()
        {
            var maxUp = Enumerable.Repeat(-1, Input[0].Length).ToArray();
            for (var i = 0; i < Input.Length; i++)
            {
                var maxLeft = -1;
                for (var j = 0; j < Input[i].Length; j++)
                {
                    var current = Input[i][j];
                    if (maxUp[j] < current.Height)
                    {
                        maxUp[j] = current.Height;
                        current.Visible = true;
                    }

                    if (maxLeft < current.Height)
                    {
                        maxLeft = current.Height;
                        current.Visible = true;
                    }
                }
            }

            var maxDown = Enumerable.Repeat(-1, Input[0].Length).ToArray();
            for (var i = Input.Length - 1; i >= 0; i--)
            {
                var maxRight = -1;
                for (var j = Input[i].Length - 1; j >= 0; j--)
                {
                    var current = Input[i][j];
                    if (maxDown[j] < current.Height)
                    {
                        maxDown[j] = current.Height;
                        current.Visible = true;
                    }

                    if (maxRight < current.Height)
                    {
                        maxRight = current.Height;
                        current.Visible = true;
                    }
                }
            }

            return Input
                .SelectMany(x => x)
                .Where(x => x.Visible)
                .Count()
                .ToString();
        }

        protected override string SolvePart2()
        {
            var avg = Input
                .SelectMany(x => x)
                .Average(x => x.Height);

            var onlyAboveAverage = Input
                .SelectMany(x => x)
                .Where(x => x.Height > avg)
                .ToArray();

            foreach (var tree in onlyAboveAverage)
            {
                var scoreLeft = 0;
                for (var k = tree.Y - 1; k >= 0; k--)
                {
                    scoreLeft++;

                    if (Input[tree.X][k].Height >= tree.Height)
                    {
                        break;
                    }
                }

                var scoreRight = 0;
                for (var k = tree.Y + 1; k < Input[tree.X].Length; k++)
                {
                    scoreRight++;

                    if (Input[tree.X][k].Height >= tree.Height)
                    {
                        break;
                    }
                }

                var scoreUp = 0;
                for (var k = tree.X - 1; k >= 0; k--)
                {
                    scoreUp++;

                    if (Input[k][tree.Y].Height >= tree.Height)
                    {
                        break;
                    }
                }

                var scoreDown = 0;
                for (var k = tree.X + 1; k < Input.Length; k++)
                {
                    scoreDown++;

                    if (Input[k][tree.Y].Height >= tree.Height)
                    {
                        break;
                    }
                }

                tree.Score = scoreLeft * scoreRight * scoreUp * scoreDown;
            }

            return onlyAboveAverage
                .Max(x => x.Score)
                .ToString();
        }

        protected override Tree[][] LoadInput()
        {
            return GetInputLines()
                .Select((line, i) => line
                    .Select((c, j) => new Tree(c, i, j))
                    .ToArray())
                .ToArray();
        }

        public class Tree
        {
            public int X { get; set; }

            public int Y { get; set; }

            public int Height { get; set; }

            public bool Visible { get; set; }

            public int Score { get; set; }

            public Tree(char height, int x, int y)
            {
                Height = int.Parse(height.ToString());
                X = x;
                Y = y;
            }
        }
    }
}
