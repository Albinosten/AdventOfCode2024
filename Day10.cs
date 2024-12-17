using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;

namespace AdventOfCode2024
{
	internal class Day10 : IPuzzle
	{
		public bool IsExample { get; set; }
		public int FirstResult => this.IsExample ? 36 : 776;

		public long SecondResult => this.IsExample ? 81 : 1657;
		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";
		public int First()
		{
			var input = this.ParseInput();

			var result = 0;
			foreach (var startPos in input.StartIndexes)
			{
				result += this.GetResultFromStartPos(input.map.Select(x => x.ToList()).ToList(), startPos);
			}
			return result;
		}

		public long Second()
		{
			var input = this.ParseInput();

			var result = 0;
			foreach (var startPos in input.StartIndexes)
			{
				result += this.GetResultFromStartPos(input.map.Select(x => x.ToList()).ToList(), startPos, Part.Two);
			}
			return result;
		}

		public int GetResultFromStartPos(List<List<int>> map
			, (int x, int y) startPos
			, Part part = Part.One
			)
		{
			var result = 0;
			var q = new Queue<((int x, int y)pos, List<(int x, int y)> path)>();
			var path = new List<(int x, int y)>();
			q.Enqueue((startPos, path));

			while (q.Count > 0) 
			{
				var n = q.Dequeue();
				var neighbours = this.GetPossibleMove(n.pos)
					.Where(x => IsMovePossible(n.pos,x,map))
					.ToList();
				foreach (var neighbour in neighbours) 
				{
					var p = n.path.ToList();
					p.Add(n.pos);
					q.Enqueue((neighbour, p));
				}
				if(map[n.pos.y][n.pos.x] == 9)
				{
					if(part == Part.One)
					{
						map[n.pos.y][n.pos.x] *= -1;
					}
					result++;
				}
			}

			return result;
		}
		private bool IsMovePossible((int x, int y) currentPos, (int x, int y) nextPos, List<List<int>> map)
		{
			return this.WithinBounds(nextPos, map) 
				&& map[currentPos.y][currentPos.x] + 1 == map[nextPos.y][nextPos.x];
		}
		private bool WithinBounds((int x, int y) pos, List<List<int>> map)
		{
			return pos.x >= 0
				&& pos.y >= 0
				&& pos.y < map.Count
				&& pos.x < map[0].Count;
		}
		private List<(int x, int y)> GetPossibleMove((int x, int y) startPos)
		{
			return 
			[
				(startPos.x+1, startPos.y),
				(startPos.x-1, startPos.y),
				(startPos.x, startPos.y+1),
				(startPos.x, startPos.y-1),
			];
		}

		private (List<List<int>> map, List<(int x, int y)> StartIndexes) ParseInput()
		{
			var allLines = File.ReadAllLines(this.path);
			var map = new List<List<int>>();
			var startIndexes = new List<(int x, int y)>();

			for (int y = 0; y < allLines.Length; y++)
			{
				var row = new List<int>();
				for (int x = 0; x < allLines[y].Length; x++) 
				{
					char col = allLines[y][x];
					if(col == '0')
					{
						startIndexes.Add((x, y));
					}
					if (col == '.')
					{
						row.Add(-6);
					}
					else
					{
						row.Add(int.Parse(col.ToString()));
					}

				}
				map.Add(row);
			}

			return (map,startIndexes);
		}
	}
}
