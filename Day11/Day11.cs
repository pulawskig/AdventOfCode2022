using AdventOfCode2022.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day11
{
    public class Day11 : AdventDay<Day11, Day11.Monkey[]>
    {
        public Day11() : base()
        {
            ReloadForPart2 = true;
        }

        protected override string SolvePart1()
        {
            PerformInspections(20, val => val / 3);

            return Input
                .Select(m => m.InspectionCount)
                .OrderByDescending(m => m)
                .Take(2)
                .Aggregate((a, b) => a * b)
                .ToString();
        }

        protected override string SolvePart2()
        {
            var divisor = Input.Select(m => m.TestCheck).Aggregate((a, b) => a * b);

            PerformInspections(10_000, val => val % divisor);

            return Input
                .Select(m => m.InspectionCount)
                .OrderByDescending(m => m)
                .Take(2)
                .Aggregate((a, b) => a * b)
                .ToString();
        }

        protected override Monkey[] LoadInput()
        {
            var monkeyInputs = GetInputLines()
                .SplitBy(string.IsNullOrWhiteSpace)
                .Select(x => x.ToArray())
                .ToArray();

            var numberRegex = new Regex(@"(\d{1,2})", RegexOptions.Compiled);
            var operationRegex = new Regex(@"(\+|\*) (old|\d{1,2})", RegexOptions.Compiled);

            var monkeys = new List<Monkey>();

            foreach (var input in monkeyInputs)
            {
                var number = int.Parse(numberRegex.Match(input[0]).Groups[1].Value);
                var items = numberRegex.Matches(input[1]).Select(g => long.Parse(g.Value));
                var test = int.Parse(numberRegex.Match(input[3]).Groups[1].Value);

                var operationMatch = operationRegex.Match(input[2]);
                var operation = new Operation(operationMatch.Groups[1].Value, operationMatch.Groups[2].Value);

                monkeys.Add(new Monkey
                {
                    Number = number,
                    Items = new Queue<long>(items),
                    Operation = operation,
                    TestCheck = test,
                    InspectionCount = 0,
                });
            }

            foreach (var input in monkeyInputs)
            {
                var number = int.Parse(numberRegex.Match(input[0]).Groups[1].Value);
                var truthy = int.Parse(numberRegex.Match(input[4]).Groups[1].Value);
                var falsy = int.Parse(numberRegex.Match(input[5]).Groups[1].Value);

                monkeys[number].TruthyTarget = monkeys[truthy];
                monkeys[number].FalsyTarget = monkeys[falsy];
            }

            return monkeys.ToArray();
        }

        private void PerformInspections(int count, Func<long, long> relaxation)
        {
            for (var i = 0; i < count; i++)
            {
                foreach (var monkey in Input)
                {
                    while (monkey.Items.Count > 0)
                    {
                        var item = monkey.Items.Dequeue();
                        item = monkey.Operation.Perform(item);
                        item = relaxation(item);

                        if (item % monkey.TestCheck == 0L)
                        {
                            monkey.TruthyTarget.Items.Enqueue(item);
                        }
                        else
                        {
                            monkey.FalsyTarget.Items.Enqueue(item);
                        }

                        monkey.InspectionCount++;
                    }
                }
            }
        }

        public class Monkey
        {
            public int Number { get; set; }

            public Queue<long> Items { get; set; }

            public Operation Operation { get; set; }

            public int TestCheck { get; set; }

            public Monkey TruthyTarget { get; set; }

            public Monkey FalsyTarget { get; set; }

            public long InspectionCount { get; set; }
        }

        public enum OperationType
        {
            Sum, Product
        }

        public struct Operation
        {
            public OperationType Type { get; set; }

            public int? Value { get; set; }

            public Operation(string type, string value)
            {
                Type = type switch
                {
                    "+" => OperationType.Sum,
                    "*" => OperationType.Product,
                    _ => throw new NotImplementedException(),
                };

                Value = value switch
                {
                    "old" => null,
                    _ => int.Parse(value),
                };
            }

            public long Perform(long firstValue)
            {
                return (Type, Value) switch
                {
                    (OperationType.Sum, null) => firstValue + firstValue,
                    (OperationType.Sum, not null) => firstValue + Value.Value,
                    (OperationType.Product, null) => firstValue * firstValue,
                    (OperationType.Product, not null) => firstValue * Value.Value,
                    _ => throw new NotImplementedException(),
                };
            }
        }
    }
}
