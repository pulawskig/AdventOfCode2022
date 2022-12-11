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

        public bool ReloadForPart2 { get; protected set; }

        public AdventDay()
        {
            Input = LoadInput();
        }

        protected abstract string SolvePart1();

        protected abstract string SolvePart2();

        protected abstract TInput LoadInput();

        protected string[] GetInputLines()
        {
            return File.ReadAllLines(GetInputFilePath());
        }

        protected virtual string GetInputFilePath()
        {
            return $"{typeof(TDay).Name}/Input.txt";
        }
    }
}
