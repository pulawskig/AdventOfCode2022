using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day7
{
    public class Day7 : AdventDay<Day7, List<Day7.Directory>>
    {
        protected override string SolvePart1()
        {
            return Input
                .Select(x => x.Size)
                .Where(x => x <= 100_000)
                .Sum()
                .ToString();
        }

        protected override string SolvePart2()
        {
            var max = Input.First(x => x.Name == @"/").Size;
            var remainder = 30_000_000L - (70_000_000L - max);

            return Input
                .Select(x => x.Size)
                .Where(x => x >= remainder)
                .Min()
                .ToString();
        }

        protected override List<Directory> LoadInput()
        {
            var homeDirectory = new Directory
            {
                Name = @"/"
            };
            var currentDirectory = homeDirectory;
            var allDirectories = new List<Directory>
            {
                homeDirectory
            };

            var cdCommand = new Regex(@"^\$ cd (\/|\w+|\.\.)$");
            var lsCommand = new Regex(@"^\$ ls$");
            var lsDirectory = new Regex(@"^dir (\w+)$");
            var lsFile = new Regex(@"^(\d+) ([\w.]+)$");

            foreach (var line in GetInputLines())
            {
                var match = lsCommand.Match(line);
                if (match.Success)
                {
                    continue;
                }

                match = cdCommand.Match(line);
                if (match.Success)
                {
                    currentDirectory = match.Groups[1].Value switch
                    {
                        @"/" => homeDirectory,
                        ".." => currentDirectory.Parent,
                        _ => currentDirectory.Directories[match.Groups[1].Value]
                    };
                    continue;
                }

                match = lsDirectory.Match(line);
                if (match.Success)
                {
                    var name = match.Groups[1].Value;
                    var newDir = new Directory
                    {
                        Name = name,
                        Parent = currentDirectory
                    };
                    currentDirectory.Directories.Add(name, newDir);
                    allDirectories.Add(newDir);
                    continue;
                }

                match = lsFile.Match(line);
                if (match.Success)
                {
                    var size = long.Parse(match.Groups[1].Value);
                    var name = match.Groups[2].Value;

                    currentDirectory.Files.Add(new File
                    {
                        Size = size,
                        Name = name,
                        Parent = currentDirectory,
                    });
                }
            }

            return allDirectories;
        }

        public class Directory
        {
            public Dictionary<string, Directory> Directories { get; } = new Dictionary<string, Directory>();

            public List<File> Files { get; } = new List<File>();

            public Directory Parent { get; set; }

            public string Name { get; set; }

            private long? _size;
            public long Size => _size ??= Directories.Sum(x => x.Value.Size) + Files.Sum(x => x.Size);
        }

        public class File
        {
            public Directory Parent { get; set; }

            public string Name { get; set; }

            public long Size { get; set; }
        }
    }
}
