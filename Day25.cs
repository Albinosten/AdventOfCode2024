namespace AdventOfCode2024
{
	
	internal class Day25 : IPuzzle
	{
		public bool IsExample { get; set; }
		public int FirstResult => this.IsExample ? 3 : 3090;

		public long SecondResult => this.IsExample ? 0 : 0;

		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";
		public int First()
		{
			var (locks,keys) = ParseInput();
			var result = 0;
			for(int i = 0; i < locks.Count; i++)
			{
				for (int j = 0; j < keys.Count; j++)
				{
					var isMatch = true;
					for (int pin = 0; pin < 5; pin++)
					{
						if (locks[i][pin] + keys[j][pin] > 5)
						{
							isMatch = false;
						}
					}
					if (isMatch)
					{
						result++;
					}
				}
			}

			return result;
		}

		public long Second()
		{
			return 0;
		}

		private (List<List<int>>locks, List<List<int>> keys) ParseInput()
		{
			var allLines = File.ReadAllLines(this.path).Chunk(8);
			var locks = new List<List<int>>();
			var keys = new List<List<int>>();

			foreach (var line in allLines)
			{
				var _lock = new List<int>();
				var key = new List<int>();
				var isLock = line[0][0] == '#';
				for (int i = 0; i < 5; i++)
				{
					var number = line
						.Where(x => !string.IsNullOrEmpty(x))
						.Select(x => x[i])
						.Count(x => x == '#')
						-1;
					if(isLock)
					{
						_lock.Add(number);
					}
					else
					{
						key.Add(number);
					}
				}
				if (isLock)
				{
					locks.Add(_lock);
				}
				else
				{
					keys.Add(key);
				}
			}

			return (locks,keys);
		}
	}
}
