using System.Globalization;

namespace AdventOfCode2024
{
internal class Day9 : IPuzzle<long>
	{
		public bool IsExample { get; set; }
		public long FirstResult => this.IsExample ? 1928 : 6241633730082;

		public long SecondResult => this.IsExample ? 2858 : 6265268809555;

		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";
		public long First()
		{
			var input = this.ParseInput();

			var firstIndex = 0;
			var lastIndex = input.Count-1;
			
			while(firstIndex < lastIndex)
			{
				if(input[firstIndex] == -1
					&& input[lastIndex] != -1
				)
				{
					//swap
					var temp = input[firstIndex];
					input[firstIndex] = input[lastIndex];
					input[lastIndex] = temp;
					lastIndex--;
				}
				else if(input[lastIndex] == -1) 
				{
					lastIndex--; 
				}
				else
				{
					firstIndex++;
				}
			}

			return this.CalculateChecksum(input);
		}

		public long Second()
		{
			var input = this.ParseInput();

			var lastId = input[input.Count - 1];
			var firstIndexOfLastId = input.Count - 1;
			var lastIndexOfLastId = input.Count - 1;

			for(int id = lastId; id >=0; id--)
			{
				while (input[lastIndexOfLastId] != id)
				{
					lastIndexOfLastId--;
				}
				firstIndexOfLastId = lastIndexOfLastId;
				while (firstIndexOfLastId > 0 
					&& input[firstIndexOfLastId-1] == id)
				{
					firstIndexOfLastId--;
				}
				var lenghtOfId = lastIndexOfLastId - firstIndexOfLastId+1;

				//find a open spot
				for(int potentialStartIndex = 0; potentialStartIndex < firstIndexOfLastId; potentialStartIndex++)
				{
					//var gap = input.GetRange(potentialStartIndex, lenghtOfId);
					var gap = input[potentialStartIndex..(potentialStartIndex+lenghtOfId)];

					if(gap.All(x => x == -1))
					{
						//swap
						for(int j = 0; j < lenghtOfId; j++)
						{
							var temp = input[potentialStartIndex + j];
							input[potentialStartIndex + j] = input[firstIndexOfLastId + j];
							input[firstIndexOfLastId + j] = temp;
						}
						potentialStartIndex = firstIndexOfLastId;

					}
				}
			}

			return this.CalculateChecksum(input);
		}
		private long CalculateChecksum(List<int> input)
		{
			return input
					.Select((id, index) => (long)(GetValueForCheckSum(id) * index))
					.Sum();
		}
		private int GetValueForCheckSum(int value)
		{
			return value > 0 ? value : 0;
		}

		private List<int> ParseInput()
		{
			var allLines = File.ReadAllLines(this.path)[0];
			var list = new List<int>();

			for(int i = 0; i < allLines.Count(); i++)
			{
				var useId = i % 2 == 0;
				var number = int.Parse(allLines[i].ToString());
				for (int j = 0; j < number; j++)
				{
					if(useId)
					{
						list.Add(i / 2);
					}
					else 
					{
						list.Add(-1);
					}
				}
				
			}

			return list;
		}
		private void PrintStep(List<int> list)
		{
			Console.WriteLine();
			for(int i = 0; i < list.Count; i++)
			{
				if(list[i] == -1)
				{
					Console.Write(".");
				}
				else
				{
					Console.Write(list[i]);
				}
			}
		}
	}
}
