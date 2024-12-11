namespace AdventOfCode2024
{
	internal class Day3 : IPuzzle
	{
		public bool IsExample { get; set; }
		public int FirstResult => this.IsExample ? 161 : 175015740;

		public long SecondResult => this.IsExample ? 48 : 112272912;

		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";
		public int First()
		{
			var lines = this.ParseInput();
			var result = 0;
			foreach (var line in lines) 
			{
				result += this.Parse(line);
			}

			return result;
		}

		public long Second()
		{
			var lineSplitOnDo = this.ParseInput(Part.Two)
				.Aggregate((result, value) => result += value)
				.Split("do()");

			var result = 0;
			foreach (var does in lineSplitOnDo)
			{
				var doBeforeDont = does.Split("don't")[0];
				result += this.Parse(doBeforeDont);
			}

			return result;
		}

		private int Parse(string line)
		{
			var mulls = line.Split("mul(");
			var result = 0;
			foreach (var mullString in mulls) 
			{
				var mullSplitOnComma = mullString.Split(',');
				var leftOfComma = mullSplitOnComma[0];
				if (this.TryGetNumberInRange(leftOfComma, out var firstNumber))
				{
					var rightOfComma = mullSplitOnComma[1];
					var rightOfCommaAndLeftOfParentheses = rightOfComma.Split(')')[0];
					if(this.TryGetNumberInRange(rightOfCommaAndLeftOfParentheses, out var secondNumber))
					{
						result += firstNumber * secondNumber;
					}
				}
			}
			return result;
		}

		private bool TryGetNumberInRange(string value, out int number)
		{
			if(value.Length <= 3
				&& value.Length >= 1
				&& int.TryParse(value, out number))
			{
				return true;
			}
			number = 0;
			return false;
		}

		private List<string> ParseInput(Part part = Part.One)
		{
			var allLines = File.ReadAllLines(this.path);
			var list = new List<string>();
			if(IsExample)
			{
				var index = part == Part.One ? 0 : 1;
				return [allLines[index]];
			}
			foreach (var line in allLines)
			{
				list.Add(line);
			}

			return list;
		}
	}
}
