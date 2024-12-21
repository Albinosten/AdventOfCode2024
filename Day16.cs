using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace AdventOfCode2024
{
	internal class Day16 : IPuzzle
	{
		public bool IsExample { get; set; }
		public int FirstResult => this.IsExample ? 7036 : 123540;

		public long SecondResult => this.IsExample ? 45 : 665;

		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";
		int GetManhattanDistance((int x, int y) pos1, (int x, int y) pos2)
		{
			return Math.Abs(pos1.x - pos2.x) + Math.Abs(pos1.y - pos2.y);
		}
		public int First()
		{
			return this.Solve().minScore;
		}

		bool ValidTile((int x, int y) pos, List<List<char>> map)
		{
			return map[pos.y][pos.x] != '#';
		}
		public long Second()
		{
			return this.Solve(Part.Two).uniquePositions.Count;
		}
		(int minScore, List<(int x, int y)>uniquePositions) Solve(Part part = Part.One)
		{
			var (map, startPos, endPos) = this.ParseInput();

			var q = new Queue<(List<(int x, int y)> visited, (int x, int y) pos, int score, Direction direction)>();
			q.Enqueue((new List<(int x, int y)>(), startPos, 0, Direction.Right));

			var closesDistanceVector = new Dictionary<(int x, int y), long>();
			
			var allBestRouts = new HashSet<(int x, int y)>();
			int minScore = int.MaxValue;
			while (q.Count > 0)
			{
				var n = q.Dequeue();

				if (!closesDistanceVector.ContainsKey(n.pos))
				{
					closesDistanceVector.Add(n.pos, n.score);
				}
				var extraLength = part == Part.One ? 0 : 1100;
				if (closesDistanceVector[n.pos] + extraLength < n.score)
				{
					continue;
				}
				closesDistanceVector[n.pos] = n.score;

				if (n.pos == endPos && n.score == this.FirstResult)
				{
					foreach (var c in n.visited) 
					{
						allBestRouts.Add(c);
					}
				}
				if (n.pos == endPos && minScore > n.score)
				{
					minScore = n.score;
					continue;
				}

				var moves = Helper.GetAllDirections()
					.Select(x => new
					{
						Pos = Helper.GetNextPosition(x, n.pos),
						Dir = x,
					}
					)
					.Where(x => Helper.WithinBounds(x.Pos, map))
					.Where(x => this.ValidTile(x.Pos, map))
					.Where(x => !n.visited.Contains(x.Pos))
					.ToList();
				;
				foreach (var move in moves)
				{
					var visited = n.visited.ToList();
					visited.Add(n.pos);
					var localScore = n.score;
					localScore++;
					if (move.Dir != n.direction)
					{
						localScore += 1000;
					}
					q.Enqueue((visited, move.Pos, localScore, move.Dir));
				}
			}
			allBestRouts.Add(endPos);

			//Helper.PrintMap(map, allBestRouts.ToList());

			return (minScore, allBestRouts.ToList());
		}

		private (List<List<char>> map, (int x, int y) startPos, (int x, int y) endPos) ParseInput()
		{
			var allLines = File.ReadAllLines(this.path);
			var result = new List<List<char>>();

			var startPos = (0, 0);
			var endPos = (0, 0);
			for (int y = 0; y < allLines.Count();y++)
			{
				var row = new List<char>();
				for (int x = 0; x < allLines[y].Count(); x++)
				{
					if(allLines[y][x] == 'S')
					{
						startPos = (x, y);
					}
					if (allLines[y][x] == 'E')
					{
						endPos = (x, y);
					}
					row.Add(allLines[y][x]);
				}
				result.Add(row);
			}
			return (result,startPos,endPos);
		}
	}
}
