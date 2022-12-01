using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public abstract class AdventDay<TDay, TInput>
    {
        protected TInput Input { get; init; }

        public AdventDay()
        {
            Input = LoadInput();
        }

        protected abstract TInput LoadInput();

        protected abstract Task<string> SolvePart1();

        protected abstract Task<string> SolvePart2();

        protected string[] GetInputLines()
        {
            return File.ReadAllLines($"{typeof(TDay).Name}/Input.txt");
        }
    }
}
