// See https://aka.ms/new-console-template for more information
using AdventOfCode2024;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;

Console.WriteLine("Hello, World!");

//var puzzles = AppDomain
//	.CurrentDomain
//	.GetAssemblies()
//	.SelectMany(x => x.GetTypes())
//	.Where(x => typeof(IPuzzle).IsAssignableFrom(x))
//	.Where(x => x.IsClass)
//	.Select(x => Create(x))
//	.ToList();
var watch = new Stopwatch();
watch.Start();

//var a = new Day20()
//{
//	IsExample = true
//};
//Console.WriteLine(a.First());
//Console.WriteLine(a.Second());

Print(new Day22());
Print(new Day21());
Print(new Day20());
Print(new Day19());
Print(new Day18());
Print(new Day17());
Print(new Day16());
Print(new Day15());
Print(new Day14());
Print(new Day13());
Print(new Day12());
Print(new Day11());
Print(new Day10());
Print(new Day9());
Print(new Day8());
Print(new Day7());
Print(new Day6());
Print(new Day5());
Print(new Day4());
Print(new Day3());
Print(new Day2());
Print(new Day1());


watch.Stop();
Console.WriteLine($"Total time: " + watch.Elapsed);
static void Print<T,J>(IPuzzle<T,J> puzzle)	
{
	var puzzleName = puzzle.GetType().Name;
	puzzle.IsExample = true;

	Console.WriteLine($"Now solving {(puzzle.IsExample ? "Example: " : "")}: " + puzzleName);
	OutputResult(puzzle, puzzleName);
	puzzle.IsExample = false;
	OutputResult(puzzle, puzzleName);
	Console.WriteLine("***************************");
}

static void OutputResult<T, J>(IPuzzle<T, J> puzzle, string puzzleName)
{
	var watch = new Stopwatch();
	watch.Start();

	var firstValue = puzzle.First();
	Console.WriteLine("First: " + firstValue + " Should be: " + puzzle.FirstResult);

	ThrowIfNotSame(firstValue, puzzle.FirstResult,puzzleName);

	var secondValue = puzzle.Second();
	Console.WriteLine("Second: " + secondValue + " Should be: " + puzzle.SecondResult);
	ThrowIfNotSame(secondValue, puzzle.SecondResult, puzzleName);

	watch.Stop();
	Console.WriteLine($"Time for {puzzle.GetType().Name}: " + watch.Elapsed);
}

static void ThrowIfNotSame<T>(T actual, T expected, string puzzleName)
{
	if (!actual.Equals(expected))
	{
		//Console.Clear();
		throw new Exception("Expected value: " + expected + " Actual value: " + actual + " On day: " + puzzleName);
	}
}

static IPuzzle Create(Type type)
{
	return (IPuzzle)type.GetConstructor(Type.EmptyTypes).Invoke(null);
}