using System.Collections.Generic;

namespace AdventOfCode2024
{


	internal class Day7 : IPuzzle<long>
	{
		public bool IsExample { get; set; }
		public long FirstResult => this.IsExample ? 3749 : 0;

		public long SecondResult => this.IsExample ? 0 : 0;

		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";
		public long First()
		{
			var input = this.ParseInput();
			long result = 0;
			foreach (var item in input) 
			{
				var possibleCombinations = (int)Math.Pow(2, item.numbers.Count - 1);
				for (var i = 0; i < possibleCombinations; i++)
				{
					long sum = item.numbers.First();
					var binary = Convert
						.ToString(i, 2)
						.PadLeft(item.numbers.Count-1,'0')
						.ToString();
					for (var j = 0; j < item.numbers.Count-1; j++)
					{
						if(binary[j]== '0')
						{
							sum += item.numbers[j+1];
						}
						else
						{
							sum *= item.numbers[j+1];
						}
					}
					if(sum == item.result)
					{
						result += sum;
						i = possibleCombinations;
					}
				}
			}
			return result;
		}

		public long Second()
		{
			return 0;
		}

		private List<(long result , List<int> numbers)> ParseInput()
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
