using AdventOfCode2022;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
await RunAll();

async Task RunAll()
{
    var days = Assembly.GetExecutingAssembly()
        .GetTypes()
        .Where(t => t.IsClass && !t.IsAbstract && t.BaseType?.IsGenericType == true && t.BaseType?.GetGenericTypeDefinition() == typeof(AdventDay<,>))
        .OrderBy(t => t.Name);

    foreach (var day in days)
    {
        await RunSpecificType(day);
    }
}

async Task RunSpecific<TDay>()
{
    await RunSpecificType(typeof(TDay));
}

async Task RunSpecificType(Type dayType)
{
    var instance = Activator.CreateInstance(dayType);

    var part1Method = dayType.GetMethod("SolvePart1", BindingFlags.Instance | BindingFlags.NonPublic, null, Array.Empty<Type>(), default)!;
    var timer = Stopwatch.StartNew();
    var result = await (part1Method.Invoke(instance, null) as Task<string>)!;
    timer.Stop();
    Console.WriteLine($"{dayType.Name} Part 1: {result} - {1d * timer.ElapsedTicks / TimeSpan.TicksPerMillisecond}ms");

    var part2Method = dayType.GetMethod("SolvePart2", BindingFlags.Instance | BindingFlags.NonPublic, null, Array.Empty<Type>(), default)!;
    timer = Stopwatch.StartNew();
    result = await (part2Method.Invoke(instance, null) as Task<string>)!;
    timer.Stop();
    Console.WriteLine($"{dayType.Name} Part 2: {result} - {1d * timer.ElapsedTicks / TimeSpan.TicksPerMillisecond}ms");
}