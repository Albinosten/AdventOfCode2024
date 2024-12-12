using System.Collections.Generic;

namespace AdventOfCode2024
{


	internal class Day7 : IPuzzle<long>
	{
		public bool IsExample { get; set; }
		public long FirstResult => this.IsExample ? 3749 : 1545311493300;

		public long SecondResult => this.IsExample ? 11387 : 169122112716571;

		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";
		public long First()
		{
			return this.Solve();
		}

		public long Second()
		{
			return this.Solve(Part.Two);
		}
		private long Solve(Part part = Part.One)
		{
			var input = this.ParseInput();
			long result = 0;
			long trys = 0;
			foreach (var item in input)
			{
				var possibleCombinations = (int)Math.Pow(part == Part.One ? 2 : 3, item.numbers.Count - 1);
				for (var i = 0; i < possibleCombinations; i++)
				{
					trys++;
					long sum = item.numbers.First();
					var binary = ExtentionsAndHelpers
						.ToBase(i
							, part == Part.One
							? Base.Two
							: Base.Three)
						.PadLeft(item.numbers.Count - 1, '0')
						.ToString();
					for (var j = 0; j < item.numbers.Count - 1; j++)
					{
						if (binary[j] == '0')
						{
							sum += item.numbers[j + 1];
						}
						else if (binary[j] == '1')
						{
							sum *= item.numbers[j + 1];
						}
						else
						{
							sum = long.Parse(sum.ToString() + item.numbers[j + 1].ToString());
						}
					}
					if (sum == item.result)
					{
						result += sum;
						i = possibleCombinations;
					}
				}
			}
			Console.WriteLine("Number of trys: " + trys);
			return result;
		}

		private List<(long result, List<int> numbers)> ParseInput()
		{
			var allLines = File.ReadAllLines(this.path);
			var list = new List<(long result, List<int> numbers)>();

			foreach (var line in allLines)
			{
				var split = line.Split(": ");

				var result = long.Parse(split[0]);
				var numbers = split[1].Split(' ').Select(x => int.Parse(x)).ToList();
				list.Add((result, numbers));

			}

			return list;
		}
	}
}
