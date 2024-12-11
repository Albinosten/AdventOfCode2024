namespace AdventOfCode2024
{


	internal class Day1 : IPuzzle
	{
		public bool IsExample { get; set; }
		public int FirstResult => this.IsExample ? 11 : 3246517;

		public long SecondResult => this.IsExample ? 31 : 29379307;
		//public static string path => "Inputs/Day1.txt";
		public string path => this.IsExample 
			? $"Inputs/{this.GetType().Name}_TEMP.txt" 
			: $"Inputs/{this.GetType().Name}.txt";
		public int First()
		{
			var lists = this.ParseInput();

			var leftOrderedList = lists
				.leftList
				.OrderBy(x => x)
				.ToList();
			var rightOrderedList = lists
				.rightList
				.OrderBy(x => x)
				.ToList();

			var result = 0;
			for (var i = 0; i < leftOrderedList.Count; i++) 
			{
				result += Math.Abs(leftOrderedList[i] - rightOrderedList[i]);
			}
			return result;
		}

		public long Second()
		{
			var lists = this.ParseInput ();

			var result = 0;
			for (var i = 0; i < lists.leftList.Count; i++)
			{
				var leftItem = lists.leftList[i];
				var times = 0;
				for (var j = 0; j < lists.rightList.Count; j++)
				{
				var rightItem = lists.rightList[j];
					if (leftItem == rightItem)
					{
						times++;
					}
				}
				result += leftItem * times;
			}
			return result;
		}

		private (List<int> leftList, List<int> rightList) ParseInput()
		{
			var allLines = File.ReadAllLines(path);
			var leftList = new List<int>();
			var rightList = new List<int>();
			
			foreach (var line in allLines) 
			{
				var parcedLine = line.Split("   ");
				leftList.Add(int.Parse(parcedLine[0]));
				rightList.Add(int.Parse(parcedLine[1]));
			}

			return (leftList, rightList);
		}
	}
}
