using AdventOfCode2022.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day13
{
    public class Day13 : AdventDay<Day13, List<(Day13.Packet, Day13.Packet)>>
    {
        protected override string SolvePart1()
        {
            return Input
                .Select((pair, i) => (pair.Item1.CompareTo(pair.Item2), i + 1))
                .Where(x => x.Item1 < 0)
                .Sum(x => x.Item2)
                .ToString();
        }

        protected override string SolvePart2()
        {
            var dividerPackets = new[]
            {
                new Packet
                {
                    IsDivider = true,
                    SubPackets = new List<Packet>
                    {
                        new Packet
                        {
                            Value = 2,
                        },
                    },
                },
                new Packet
                {
                    IsDivider = true,
                    SubPackets = new List<Packet>
                    {
                        new Packet
                        {
                            Value = 6,
                        },
                    },
                },
            };

            return Input
                .SelectMany(x => new[] { x.Item1, x.Item2 })
                .Concat(dividerPackets)
                .OrderBy(x => x, new Packet())
                .Select((x, i) => (Packet: x, Index: i + 1))
                .Where(x => x.Packet.IsDivider)
                .Aggregate(1, (a, b) => a * b.Index)
                .ToString();
        }

        protected override List<(Packet, Packet)> LoadInput()
        {
            return GetInputLines()
                .SplitBy(string.IsNullOrWhiteSpace)
                .Select(pair => pair.Select(line => Packet.Parse(line, 1, out _)).ToArray())
                .Select(pair => (pair[0], pair[1]))
                .ToList();
        }

        public class Packet : IComparable<Packet>, IComparer<Packet>
        {
            public List<Packet> SubPackets { get; set; } = new List<Packet>();

            public int? Value { get; set; }

            public bool IsDivider { get; set; }

            public int CompareTo(Packet? another)
            {
                if (another == null)
                {
                    return -1;
                }

                var emptyWrapper = Wrap(-100);

                return (Value, another.Value) switch
                {
                    (not null, not null) => Value.Value - another.Value.Value,
                    (not null, null) => Wrap(this).CompareTo(another),
                    (null, not null) => CompareTo(Wrap(another)),
                    (null, null) => SubPackets
                        .ZipSafe(another.SubPackets, (a, b) => a.CompareTo(b), emptyWrapper, emptyWrapper)
                        .FirstOrDefault(x => x != 0)
                };
            }

            public static Packet Wrap(Packet another)
            {
                var packet = new Packet();
                packet.SubPackets.Add(another);
                return packet;
            }

            public static Packet Wrap(int value)
            {
                return new Packet
                {
                    Value = value,
                };
            }

            public static Packet Parse(string line, int startIndex, out int endIndex)
            {
                var packet = new Packet();
                endIndex = -1;

                for (var i = startIndex; i < line.Length; i++)
                {
                    if (line[i] == '[')
                    {
                        packet.SubPackets.Add(Parse(line, i + 1, out i));
                        continue;
                    }
                    if (line[i] == ']')
                    {
                        endIndex = i;
                        break;
                    }
                    if (char.IsDigit(line[i]))
                    {
                        var digits = line.Skip(i).TakeWhile(char.IsDigit).ToArray();
                        i += digits.Length - 1;
                        packet.SubPackets.Add(Wrap(int.Parse(digits)));

                        if (line[i + 1] == ',')
                        {
                            i++;
                        }
                    }
                }

                return packet;
            }

            public int Compare(Packet? x, Packet? y)
            {
                return x?.CompareTo(y) ?? 1;
            }
        }
    }
}
