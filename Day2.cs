namespace AdventOfCode2024
{
	internal class Day2 : IPuzzle
	{
		public bool IsExample => false;
		public int FirstResult => this.IsExample ? 2 : 585;

		//624 too low
		public long SecondResult => this.IsExample ? 4 : 626;

		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";
		public int First()
		{
			var reports = this.ParseInput();
			var result = 0;
			foreach (var report in reports) 
			{
				if(this.AllLevelsIsInOrder(report)
					&& this.AllLevelsIsWithinBounds(report)
				)
				{
					result++;
				}
			}
			return result;
		}

		public long Second()
		{
			var reports = this.ParseInput();
			var result = 0;
			for (int r = 0; r < reports.Count; r++)
			{
				for (var i = 0; i < reports[r].Count; i++) 
				{
					var newReport = reports[r].ExceptElementAt(i);
					if (this.AllLevelsIsInOrder(newReport)
						&& this.AllLevelsIsWithinBounds(newReport)
					)
					{
						result++;
						i = reports[r].Count;
					}
				}
			}
			return result;
		}

		private List<List<int>> ParseInput()
		{
			var allLines = File.ReadAllLines(this.path);
			var reports = new List<List<int>>();

			foreach (var line in allLines)
			{
				var report = new List<int>();
				var levels = line.Split(' ');
				foreach (var level in levels) 
				{
					report.Add(int.Parse(level));
				}
				reports.Add(report);
			}

			return reports;
		}

		private bool AllLevelsIsWithinBounds(List<int> levels)
		{
			for (int i = 1; i < levels.Count; i++)
			{
				var diff = Math.Abs(levels[i] - levels[i-1]);
				if(diff < 1 || diff > 3)
				{
					return false;
				}
			}
			return true;
		}
		private bool AllLevelsIsInOrder(List<int> levels) 
		{
			var orderedLevels = 
				(levels[0] < levels[1] 
					? levels.OrderBy(x => x)
					: levels.OrderByDescending(x => x)
				).ToList();
			for ( int i = 0; i < orderedLevels.Count; i++ )
			{
				if(levels[i] != orderedLevels[i])
				{
					return false; 
				}
			}
			return true;
		}
	}
}
