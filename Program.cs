// See https://aka.ms/new-console-template for more information
using AdventOfCode2024;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;

Console.WriteLine("Hello, World!");

var days = AppDomain
	.CurrentDomain
	.GetAssemblies()
	.SelectMany(x => x.GetTypes())
	.Where(x => typeof(IPuzzle).IsAssignableFrom(x))
	.Where(x => x.IsClass)
	.Select(x => Create(x))
	.ToList();

foreach (var day in days)
{
	OutputResult(day);
}


static void OutputResult<T, J>(IPuzzle<T, J> puzzle)
{
	var watch = new Stopwatch();
	watch.Start();

	Console.WriteLine("Now solving: " + puzzle.GetType().Name);
	Console.WriteLine("First: " + puzzle.First() + " Should be: " + puzzle.FirstResult);
	Console.WriteLine("Second: " + puzzle.Second() + " Should be: " + puzzle.SecondResult);

	watch.Stop();
	Console.WriteLine($"Time for {puzzle.GetType().Name}: " + watch.Elapsed);
}
static IPuzzle Create(Type type)
{
	return (IPuzzle)type.GetConstructor(Type.EmptyTypes).Invoke(null);
}