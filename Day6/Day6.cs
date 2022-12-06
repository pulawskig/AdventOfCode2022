using AdventOfCode2022.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day6
{
    public class Day6 : AdventDay<Day6, string>
    {
        protected override string SolvePart1()
        {
            return FindUniqueSubstring(4).ToString();
        }

        protected override string SolvePart2()
        {
            return FindUniqueSubstring(14).ToString();
        }

        protected override string LoadInput()
        {
            return GetInputLines().First();
        }

        private int FindUniqueSubstring(int length)
        {
            var buffer = new OrderedSet<char>();
            var counter = 0;
            foreach (var c in Input)
            {
                var success = buffer.Add(c);
                counter++;

                if (success)
                {
                    if (buffer.Count == length)
                    {
                        break;
                    }

                    continue;
                }

                do
                {
                    buffer.Remove(buffer.First());
                    success = buffer.Add(c);
                }
                while (!success);
            }

            return counter;
        }
    }

    public class Day6v2 : AdventDay<Day6v2, string>
    {
        protected override string SolvePart1()
        {
            return FindUniqueSubstring(4).ToString();
        }

        protected override string SolvePart2()
        {
            return FindUniqueSubstring(14).ToString();
        }

        protected override string LoadInput()
        {
            return GetInputLines().First();
        }

        private int FindUniqueSubstring(int length)
        {
            var span = Input.AsSpan();
            var counter = 0;

            while (true)
            {
                var x = span[counter..(counter + length)];

                if (x.Length == new HashSet<char>(x.ToArray()).Count)
                {
                    break;
                }

                counter++;
            }

            return counter + length;
        }

        protected override string GetInputFilePath()
        {
            return $"Day6/Input.txt";
        }
    }
}
