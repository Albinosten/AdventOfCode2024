using AdventOfCode2024;
using System.Runtime.InteropServices;

internal class Day18 : IPuzzle<int,string>
{
	public bool IsExample { get; set; }
	public int FirstResult => this.IsExample ? 22 : 432;
	public string SecondResult => this.IsExample ? "1,0" : "";
	private int startBytes => this.IsExample ? 12 : 1024;
	private (int width, int height) space => this.IsExample ? (6, 6) : (70, 70);

	public string path => this.IsExample
		? $"Inputs/{this.GetType().Name}_TEMP.txt"
		: $"Inputs/{this.GetType().Name}.txt";
	
	//430 too low
	public int First()
	{
		return this.Solve(this.ReadBytes(this.startBytes));
	}

		int Solve(List<(int x, int y)>bytes)
		{
			var map = this.CreateMap(bytes);

			var q = new Queue<(List<(int x, int y)> visited, (int x, int y) pos, int score)>();
			q.Enqueue((new List<(int x, int y)>(), (0,0), 0));

			var closesDistanceVector = this.InitalizeShortestPathVector();
			var minScore = int.MaxValue;
			while (q.Count > 0)
			{
				var n = q.Dequeue();

				if (!closesDistanceVector.ContainsKey(n.pos))
				{
					closesDistanceVector.Add(n.pos, n.score);
				}
				if (closesDistanceVector[n.pos]  <= n.score)
				{
					continue;
				}
				closesDistanceVector[n.pos] = n.score;

				if (n.pos == this.space && minScore >= n.score)
				{
					minScore = n.score;
					Helper.PrintMap(map,n.visited);

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
					.Where(x => !map[x.Pos.y][x.Pos.x])
					.Where(x => !n.visited.Contains(x.Pos))
					.ToList();
				;
				foreach (var move in moves)
				{
					var visited = n.visited.ToList();
					visited.Add(n.pos);
					var localScore = n.score;
					localScore++;
					q.Enqueue((visited, move.Pos, localScore));
				}
			}

			return minScore;
		}
	public string Second()
	{
		var bytes = this.ReadBytes(int.MaxValue);
		for(int i = startBytes; i < bytes.Count;i++)
		{
			var score = this.Solve(bytes.Take(i).ToList());
			if(score == int.MaxValue)
			{
				return string.Join(',',  bytes[i]);
			}
		}

		return "";
	}
	private Dictionary<(int x, int y), int> InitalizeShortestPathVector()
	{
		var v = new Dictionary<(int x, int y), int>();
		for (int y = 0; y < space.height + 1; y++)
		{
			for (int x = 0; x < space.width + 1; x++)
			{
				v.Add((x, y), int.MaxValue);
			}
		}
		return v;
	}
	void printDistanceVector(Dictionary<(int x, int y), int> vector, List<List<bool>> map)
	{
		for (var y = 0; y < map.Count; y++)
		{
			for (var x = 0; x < map[y].Count(); x++)
			{
				var value = vector[(x, y)];
				if (value != int.MaxValue)
				{
					Console.Write('*');
				}
				else
				{
					Console.Write(' ');

				}
			}
			Console.WriteLine();
		}
		Console.WriteLine();
	}
	
	private List<List<bool>> CreateMap(List<(int x, int y)>bytes)
	{
		var map = new List<List<bool>>();
		for(int y= 0;y<space.height+1;y++)
		{
			var row = new List<bool>();
			for(int x= 0;x<space.width+1;x++)
			{
				row.Add(false);
			}
			map.Add(row);
		}
		foreach(var b in bytes)
		{
			map[b.y][b.x]  = true;
		}
		return map;
	}
	public List<(int x, int y)>ReadBytes(int count)
	{
		var allLines = File.ReadAllLines(this.path);
		var bytes = new List<(int x, int y)>();
		foreach (var line in allLines.Take(count))
		{
			var numbers = line
				.Split(',')
				.Select(x => int.Parse(x))
				.ToList();
			bytes.Add((numbers[0], numbers[1]));
		}

		return bytes;
	}
}