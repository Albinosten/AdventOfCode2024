using System.Runtime.InteropServices;

namespace AdventOfCode2024
{
	internal class Day20 : IPuzzle<long>
	{
		public bool IsExample { get; set; }
		public long FirstResult => this.IsExample ? 44 : 1321;

		public long SecondResult => this.IsExample ? 285 : 971737;

		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";

		public long First()
		{
			return this.SolvePartTwo(2,Part.One); 
		}
		
		public long Second()
		{
			return this.SolvePartTwo(20,Part.Two);
		}

		long SolvePartTwo(int cheatStepps, Part part)
		{
			var distances = this.GetDistanceToAllNodesFromEnd()
				.Where(x => x.Value < int.MaxValue)
				.ToDictionary(x => x.Key, x => x.Value)
				;

			var allCheats = new Dictionary<int,int>();
			foreach (var i in distances)
			{
				foreach (var j in distances)
				{
					var cheatDistance = Helper.GetManhattanDistance(i.Key, j.Key);
					if(cheatDistance <= cheatStepps)
					{
						var normalDistance = i.Value - j.Value;
						this.SetValue(allCheats, normalDistance - cheatDistance);
					}
				}
			}

			return allCheats
				.Where(x => x.Key >= (this.IsExample ? (part == Part.One ? 1 : 50) : 100))
				.Sum(x => x.Value);
		}

		Dictionary<(int x, int y), int> GetDistanceToAllNodesFromEnd()
		{
			var (map, startPos, endPos) = this.ParseInput();
			var q = new List<(int x, int y)>
			{
				endPos
			};

			var closesDistanceVector = Helper.InitalizeShortestPathVector(map[0].Count, map.Count);
			var visited = new HashSet<(int x, int y)>();
			int count = 0;
			while (q.Count > 0)
			{
				var nextBatch = new List<(int x, int y)>();
				foreach (var n in q)
				{
					closesDistanceVector[n] = count;
					visited.Add(n);

					var moves = Helper.GetAllDirections()
						.Select(x => Helper.GetNextPosition(x, n))
						.Where(x => Helper.WithinBounds(x, map))
						.Where(x => this.PossibleMove(x, map))
						.Where(x => !visited.Contains(x))
						.ToList();
					;
					foreach (var move in moves)
					{
						var value = map[move.y][move.x];
						nextBatch.Add(move);
					}
				}
				count++;
				q = nextBatch;
			}
			return closesDistanceVector;
		}

		void SetValue(Dictionary<int, int> dictionary, int key)
		{
			if (dictionary.ContainsKey(key))
			{
				dictionary[key]++; 
			}
			else
			{
				dictionary.Add(key, 1);
			}
		}
		bool PossibleMove((int x, int y) pos, List<List<char>> map)
		{
			return map[pos.y][pos.x] != '#';
		}
		private (List<List<char>> map, (int x, int y) startPos, (int x, int y) endPos) ParseInput()
		{
			var allLines = File.ReadAllLines(this.path).ToList();
			var list = new List<List<char>>();
			var startPos = (0, 0);
			var endPos = (0, 0);

			for (int y = 0; y < allLines.Count; y++)
			{
				var row = new List<char>();
				for (int x = 0; x < allLines[y].Count(); x++)
				{
					var value = allLines[y][x];
					if (value == 'S'){
						startPos = (x, y);
					}
					if (value == 'E')
					{
						endPos = (x, y);
					}
					row.Add(value);
				}
				list.Add(row);
			}

			return (list,startPos,endPos);
		}
	}
}
