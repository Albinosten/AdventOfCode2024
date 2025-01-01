
using System.Runtime.InteropServices.Marshalling;

namespace AdventOfCode2024
{

	internal class Day19 : IPuzzle
	{
		public bool IsExample { get; set; }
		public int FirstResult => this.IsExample ? 6 : 317;

		public long SecondResult => this.IsExample ? 16 : 883443544805484;

		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";
		
		public int First()
		{
			return (int)this.Solve();
		}

		//42419 too low
		//75621
		//121447
		//223673
		//1824528 too low
		//33090504 too low
		//1785557865242 incorrect
		//1907706144215 incorrect
		//883443544805484 ?correct
		public long Second()
		{
			return this.Solve(Part.Two);
		}


		long SolvePart2(string pattern, List<string>stripes)
		{
			var q = new List<(string pattern, long count)>();
			var count = 0;
			while (q.Count > 0) 
			{
				var nextBatch = new List<(string pattern, long count)>();
				foreach (var n in q) 
				{

					var firstLetter = pattern[n.pattern.Length];
					var moves = stripes
						.Where(x => x[0] == firstLetter) //make sure it starts with same letter
						.Where(x => x.Length + n.pattern.Length <= pattern.Length) //no too long numbers
						.Where(x => this.SubstringAreEqual(x, pattern.Substring(n.pattern.Length, x.Length)))
						.OrderBy(x => x.Length)
						.ToList();
						foreach (var move in moves)
						{
							//nextBatch.Add()
						}
				}
				if( stripes.Max(x => x.Length) < count)
				{
					
				}
				//groome list before assigning
				q = nextBatch;
				count++;
			}

			return 0;
		}









		private class Pattern
		{
			public string P { get; set; }
			public long Value { get; set; }
		}
		public long Solve(Part part = Part.One)
		{
			var patterns = this.ParseInput().Select(x => new Pattern { P = x, Value = 0 }).ToList();

			var a = 0;
			var possiblePatterns = 0L;
			var stripes = this.Getstripes();
			var singleMoves = stripes.Where(x => x.Length == 1).ToList();
			var moves = stripes/*.Where(x => x.Length> 1)*/.ToList();

			Parallel.ForEach(patterns, pattern =>
			//foreach (var pattern in patterns)
			{
				if (part == Part.One)
				{
					pattern.Value = this.SolveForPAttern(pattern.P, part);
				}
				else if (this.SolveForPAttern(pattern.P, Part.One) != 0)
				{
					var cache = new Dictionary<string, long>();
					pattern.Value = this.countWays(pattern.P, singleMoves, moves, cache);
					//Console.WriteLine("Done with:" + pattern.P);
					//Console.WriteLine("Value" + pattern.Value);
				}
			});
			//}


			return patterns.Sum(x => x.Value);
		}
		long countWays(string pattern, List<string> singleMoves, List<string> moves, Dictionary<string, long> cache)
		{
			if (cache.ContainsKey(pattern)) { return cache[pattern]; }

			if (pattern.Length == 1)
			{
				var contains = (singleMoves.Contains(pattern) == true);
				return contains ? 1 : 0;
			}
			long ways = 0L;

			foreach (var move in moves)
			{
				if (!pattern.StartsWith(move)) { continue; }

				var remain = pattern.Substring(move.Length);

				if (remain == "") { ways += 1; continue; }

				ways += this.countWays(remain, singleMoves, moves, cache);
			}

			cache[pattern] = ways;

			return ways;
		}
		private long SolveForPAttern(string pattern, Part part)
		{
			var stripes = this.Getstripes();
			var stripesMaxLength = stripes.Max(x => x.Length);
			var q = new Stack<(string pattern, int minIndex)>();
			var possiblePAtterns = 0;

			foreach (var stripe in stripes)
			{
				q.Push((stripe, 0));
			}
			var minIndex = 0;
			var a = 0;
			while (q.Count > 0)
			{
				var n = q.Pop();

				if (this.SubstringAreEqual(pattern, n.pattern))
				{
					possiblePAtterns++;
					if (part == Part.One)
					{
						break;
					}
				}

				if (!this.SubstringAreEqual(n.pattern, pattern, n.pattern.Length))
				{
					continue;
				}
				minIndex = Math.Max(minIndex, (n.pattern.Length - (stripesMaxLength + 1)));//works for part1 with a stack
				if (part == Part.One && n.pattern.Length < minIndex)
				{
					continue;
				}
				a++;
				if (a > 5000000)
				{
					Console.WriteLine(n.pattern);
					Console.WriteLine(pattern);
					Console.WriteLine(possiblePAtterns);
					a = 0;
				}
				if (n.pattern.Length == pattern.Length) { continue; }

				var firstLetter = pattern[n.pattern.Length];
				var moves = stripes
					.Where(x => x[0] == firstLetter) //make sure it starts with same letter
					.Where(x => x.Length + n.pattern.Length <= pattern.Length) //no too long numbers
					.Where(x => this.SubstringAreEqual(x, pattern.Substring(n.pattern.Length, x.Length)))
					.OrderBy(x => x.Length)
					.ToList();
				;
				//q.Clear();
				foreach (var move in moves)
				{
					//q.Enqueue((n.pattern + move, n.minIndex++));
					q.Push((n.pattern + move, n.minIndex++));
				}
			}
			return possiblePAtterns;
		}

		
		private bool SubstringAreEqual(string a, string b)
		{
			if (a.Length != b.Length) { return false; }

			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
				{
					return false;
				}
			}
			return true;
		}
		private bool SubstringAreEqual(string a, string b, int count)
		{
			if (a.Length > b.Length) { return false; }

			for (int i = 0; i < count; i++)
			{
				if (a[i] != b[i])
				{
					return false;
				}
			}
			return true;
		}

		private List<string> Getstripes()
		{
			return File.ReadAllLines(this.path)[0].Split(", ").ToList();
		}
		private List<string> ParseInput()
		{
			var allLines = File.ReadAllLines(this.path).Skip(2);
			return allLines.ToList();

		}
	}
}
