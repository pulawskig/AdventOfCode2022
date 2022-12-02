using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day2
{
    public class Day2 : AdventDay<Day2, List<string[]>>
    {
        protected override async Task<string> SolvePart1()
        {
            return LoadInput()
                .Select(Match.PrepareForPart1)
                .Sum(m => m.Calculate())
                .ToString();
        }

        protected override async Task<string> SolvePart2()
        {
            return LoadInput()
                .Select(Match.PrepareForPart2)
                .Sum(m => m.Calculate())
                .ToString();
        }

        protected override List<string[]> LoadInput()
        {
            return GetInputLines()
                .Select(l => l.Split(' '))
                .ToList();
        }

        public enum RPS
        {
            Rock = 1,
            Paper = 2,
            Scissors = 3,
        }

        public enum MatchResult
        {
            Loss = 0,
            Draw = 3,
            Win = 6,
        }

        public class Match
        {
            public RPS OpponentsHand { get; set; }

            public RPS YourHand { get; set; }

            public MatchResult Result => (OpponentsHand, YourHand) switch
            {
                (RPS.Rock, RPS.Rock) => MatchResult.Draw,
                (RPS.Rock, RPS.Paper) => MatchResult.Win,
                (RPS.Rock, RPS.Scissors) => MatchResult.Loss,
                (RPS.Paper, RPS.Rock) => MatchResult.Loss,
                (RPS.Paper, RPS.Paper) => MatchResult.Draw,
                (RPS.Paper, RPS.Scissors) => MatchResult.Win,
                (RPS.Scissors, RPS.Rock) => MatchResult.Win,
                (RPS.Scissors, RPS.Paper) => MatchResult.Loss,
                (RPS.Scissors, RPS.Scissors) => MatchResult.Draw,
                _ => throw new NotImplementedException(),
            };

            public static Match PrepareForPart1(string[] input)
            {
                return new Match
                {
                    OpponentsHand = input[0] switch
                    {
                        "A" => RPS.Rock,
                        "B" => RPS.Paper,
                        "C" => RPS.Scissors,
                        _ => throw new NotImplementedException(),
                    },
                    YourHand = input[1] switch
                    {
                        "X" => RPS.Rock,
                        "Y" => RPS.Paper,
                        "Z" => RPS.Scissors,
                        _ => throw new NotImplementedException(),
                    },
                };
            }

            public static Match PrepareForPart2(string[] input)
            {
                return new Match
                {
                    OpponentsHand = input[0] switch
                    {
                        "A" => RPS.Rock,
                        "B" => RPS.Paper,
                        "C" => RPS.Scissors,
                        _ => throw new NotImplementedException(),
                    },
                    YourHand = (input[0], input[1]) switch
                    {
                        ("A", "X") => RPS.Scissors,
                        ("B", "X") => RPS.Rock,
                        ("C", "X") => RPS.Paper,
                        ("A", "Y") => RPS.Rock,
                        ("B", "Y") => RPS.Paper,
                        ("C", "Y") => RPS.Scissors,
                        ("A", "Z") => RPS.Paper,
                        ("B", "Z") => RPS.Scissors,
                        ("C", "Z") => RPS.Rock,
                        _ => throw new NotImplementedException(),
                    },
                };
            }

            public int Calculate()
            {
                return (int)YourHand + (int)Result;
            }
        }
    }
}
