using System.Diagnostics;

namespace AdventOfCode2024
{
	internal class Day11 : IPuzzle
	{
		public bool IsExample { get; set; }
		public int FirstResult => this.IsExample ? 55312 : 224529;

		public long SecondResult => this.IsExample ? 65601038650482 : 266820198587914;

		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";
		public int First()
		{
			return (int)this.Solve(25);
		}

		public long Second()
		{
			return this.Solve(75);
		}

		private long Solve(int times)
		{
			var dictionary = this.ParseInput()
					.ToDictionary(x => x, x => 1L);

			var tempDictionary = new Dictionary<string, long>();

			for (var i = 0; i < times; i++)//times
			{
				foreach (var key in dictionary.Keys.ToList())
				{
					var count = dictionary[key];
					if (key == "0")
					{
						tempDictionary.TryAdd("1", 0);//make sure number 1 exist
						tempDictionary["1"] += count;
					}
					else if (key.Length % 2 == 0)
					{
						var chunkSize = key.Length / 2;
						var split = Enumerable.Range(0, key.Length / chunkSize)
							.Select(i => key.Substring(i * chunkSize, chunkSize))
							.Select(x => long.Parse(x).ToString())
							.ToList();
						
						foreach (var splitKey in split)
						{
							tempDictionary.TryAdd(splitKey, 0);
							tempDictionary[splitKey] += count;
						}
					}
					else
					{
						var newKey = (long.Parse(key) * 2024).ToString();
						tempDictionary.TryAdd(newKey, 0);
						tempDictionary[newKey] += count;
					}
				}
				dictionary = tempDictionary.ToDictionary(x => x.Key, x=> x.Value);
				tempDictionary.Clear();
			}
			return dictionary.Sum(x => x.Value);
		}
		private List<string> ParseInput()
		{
			var allLines = File.ReadAllLines(this.path)[0].Split(' ');
			var list = new List<string>();

			foreach (var line in allLines)
			{
				list.Add(line);
			}

			return list;
		}
	}
}
