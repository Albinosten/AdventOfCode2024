namespace AdventOfCode2024
{
	internal class Day5 : IPuzzle
	{
		public bool IsExample { get; set; }
		public int FirstResult => this.IsExample ? 143 : 4185;

		public long SecondResult => this.IsExample ? 123 : 4480;

		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";
		public int First()
		{
			return Solve();
		}
		public long Second()
		{
			return this.Solve(Part.Two);
		}
		public int Solve(Part part = Part.One)
		{
			var rules = this.ReadAllLines()
				.Select(x => new
				{
					Result = this.TryParseRule(x, out var value),
					rule = value,
				})
				.Where(x => x.Result)
				.Select(x => x.rule)
				.ToList();
			var updates = this.ReadAllLines()
				.Select(x => new
				{
					Result = this.TryParseUpdate(x, out var value),
					rule = value,
				})
				.Where(x => x.Result)
				.Select(x => x.rule)
				.ToList();
			var result = 0;
			foreach (var update in updates)
			{
				var isValid = true;
				for (var i = 0;i<update.Count;i++)
				{
					var number = update[i];
					for(var j = 0;j<i; j++)
					{
						var previousNumber = update[j];

						if(rules.Any(x => x.first == number && x.second == previousNumber))
						{
							isValid = false;
							if(part == Part.Two)
							{
								var temp = update[i];
								update[i] = update[j];
								update[j] = temp;
							}
						}
					}
				}
				if (part ==Part.One ? isValid : !isValid)
				{
					var value = GetMiddleValue(update);
					result += value;
				}
			}
			return result;
		}
		private int GetMiddleValue(List<int> numbers)
		{
			int index = numbers.Count / 2;
			return numbers[index];
		}

		private List<string> ReadAllLines()
		{
			return File
				.ReadAllLines(this.path)
				.ToList();
		}

		public bool TryParseUpdate(string input, out List<int> update)
		{
			update = new List<int>();
			var numbers = input.Split(',').Where(x => x.Length == 2);
			foreach (var number in numbers) 
			{
				update.Add(int.Parse(number));
			}
			return update.Count > 0;
		}
		private bool TryParseRule(string input, out (int first, int second) rule)
		{
			rule = (0, 0);
			var numbers = input.Split('|');
			if(numbers.Count() == 2)
			{
				rule = (int.Parse(numbers[0]), int.Parse(numbers[1]));
				return true;
			}

			return false;
		}
	}
}
