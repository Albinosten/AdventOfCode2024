﻿// See https://aka.ms/new-console-template for more information
using AdventOfCode2024;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;

Console.WriteLine("Hello, World!");

var puzzles = AppDomain
	.CurrentDomain
	.GetAssemblies()
	.SelectMany(x => x.GetTypes())
	.Where(x => typeof(IPuzzle).IsAssignableFrom(x))
	.Where(x => x.IsClass)
	.Select(x => Create(x))
	.ToList();

foreach (var puzzle in puzzles)
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
			Console.Clear();
			throw new Exception("Expected value: " + expected + " Actual value: " + actual + " On day: "+ puzzleName);
		}
}
static IPuzzle Create(Type type)
{
	return (IPuzzle)type.GetConstructor(Type.EmptyTypes).Invoke(null);
}