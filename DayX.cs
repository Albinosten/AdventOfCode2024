namespace AdventOfCode2024
{


	internal class DayX 
	//: IPuzzle
	{
		public bool IsExample { get; set; }
		public int FirstResult => this.IsExample ? 0 : 0;

		public long SecondResult => this.IsExample ? 0 : 0;

		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";
		public int First()
		{
			return 0;
		}

		public long Second()
		{
			return 0;
		}

		private List<string> ParseInput()
		{
			var allLines = File.ReadAllLines(this.path);
			var list = new List<string>();

			foreach (var line in allLines)
			{
				list.Add(line);
			}

			return list;
		}
	}
}
