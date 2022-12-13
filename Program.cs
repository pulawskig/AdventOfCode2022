using AdventOfCode2022;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
RunAll();

async void RunAll()
{
    var days = Assembly.GetExecutingAssembly()
        .GetTypes()
        .Where(t => t.IsClass && !t.IsAbstract && t.BaseType?.IsGenericType == true && t.BaseType?.GetGenericTypeDefinition() == typeof(AdventDay<,>))
        .OrderBy(t => t.Name);

    foreach (var day in days)
    {
        RunSpecificType(day);
    }
}

async void RunSpecific<TDay>()
{
    RunSpecificType(typeof(TDay));
}

async void RunSpecificType(Type dayType)
{
    var instance = Activator.CreateInstance(dayType);

    var part1Method = dayType.GetMethod("SolvePart1", BindingFlags.Instance | BindingFlags.NonPublic, null, Array.Empty<Type>(), default)!;

    string result;

    var timestamp = Stopwatch.GetTimestamp();
    result = (part1Method.Invoke(instance, null) as string)!;
    var time = Stopwatch.GetElapsedTime(timestamp);

    Console.WriteLine($"{dayType.Name} Part 1: {result ?? string.Empty} - {time.TotalMilliseconds:N4}ms");

    var reloadProperty = dayType.GetProperty("ReloadForPart2", BindingFlags.Instance | BindingFlags.Public, null, typeof(bool), Array.Empty<Type>(), default);

    if ((bool)reloadProperty!.GetValue(instance)!)
    {
        instance = Activator.CreateInstance(dayType);
    }

    var part2Method = dayType.GetMethod("SolvePart2", BindingFlags.Instance | BindingFlags.NonPublic, null, Array.Empty<Type>(), default)!;

    timestamp = Stopwatch.GetTimestamp();
    result = (part2Method.Invoke(instance, null) as string)!;
    time = Stopwatch.GetElapsedTime(timestamp);

    Console.WriteLine($"{dayType.Name} Part 2: {result ?? string.Empty} - {time.TotalMilliseconds:N4}ms");
}