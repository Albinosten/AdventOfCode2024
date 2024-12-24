using AdventOfCode2024;
using System.Runtime.InteropServices;

internal class Day18 : IPuzzle
{
	public bool IsExample { get; set; }
	public int FirstResult => this.IsExample ? 22 : 0;
	private (int width, int height) space => this.IsExample ? (6, 6) : (70, 70);
	private int Bytes => this.IsExample ? 12 : 1024;
	public long SecondResult => this.IsExample ? 0 : 0;

	public string path => this.IsExample
		? $"Inputs/{this.GetType().Name}_TEMP.txt"
		: $"Inputs/{this.GetType().Name}.txt";

	List<(int x, int y)> GetFakeVisited() {
		return new List<(int x, int y)>
		{
			(0,0),
			(0,1),
			(0,2),
			(0,3),
			(0,4),
			(1,4),
			(2,4),
			(3,4),
			(4,4),
			(5,4),
			(6,4),
			(7,4),
			(8,4),
			(9,4),

		};
	}
	public int First()
	{
		var map = this.ParseInput();

		var q = new Stack<((int x, int y) pos, List<(int x, int y)> visited, Direction d)>();
		//var q = new List<((int x, int y) pos, List<(int x, int y)> visited, Direction d)>();

		q.Push(((0, 0), new List<(int x, int y)>() { }, Direction.Down));
		//q.Push(((0, 0), new List<(int x, int y)>() { }));
		//q.Add(((0, 0), new List<(int x, int y)>() { }, Direction.Down));

		var closesDistanceVector = this.InitalizeShortestPathVector();

		var minPath = int.MaxValue;
		var i = 0;
		var print = false;
		while (q.Count > 0)
		{
			var n = q.Pop();
			//var n = q[0];
			//q.RemoveAt(0);

			if (closesDistanceVector[n.pos] < n.visited.Count-1)
			{
				continue;
			}
			closesDistanceVector[n.pos] = n.visited.Count;
			if (n.visited.Count > minPath || n.visited.Count > this.space.width * 3) { continue; }

			if (n.pos == this.space)
			{
				minPath = Math.Min(minPath, n.visited.Count);
				continue;
			}

			i++;
			if (i % 100000 == 0)
			{
				Helper.PrintMap(map, n.visited);
				//this.printDistanceVector(closesDistanceVector, map);
				print = false;
			}

			var moves = Helper.GetAllDirections()
					.Select(x => new
					{
						Pos = Helper.GetNextPosition(x, n.pos),
						Dir = x,
					}
					)
					.Where(x => Helper.WithinBounds(x.Pos, (this.space.height + 1, this.space.width + 1)))
					.Where(x => !map[x.Pos.y][x.Pos.x])
					.Where(x => !n.visited.Contains(x.Pos))
					//.OrderBy(x => Helper.GetManhattanDistance(x.Pos, this.space))
					//.OrderBy(x => Directions.Contains(x.Dir))
					.OrderBy(x => x.Dir == n.d)
					.ToList();
			;
			foreach (var move in moves)
			{

				var visited = n.visited.ToList();
				visited.Add(n.pos);
				this.UpdateVector(closesDistanceVector, n.pos, map);
				q.Push((move.Pos, visited, move.Dir));
				//q.Add((move.Pos, visited, move.Dir));
			}
		}

		return minPath;
	}
	private List<Direction> Directions => new List<Direction>() { Direction.Down, Direction.Right };
	public long Second()
	{
		return 0;
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
	private void UpdateVector(Dictionary<(int x, int y), int> vector
		, (int x, int y) pos
		, List<List<bool>> map

		)
	{
		this.UpdateVector(vector, pos, map,Direction.Up);
		this.UpdateVector(vector, pos, map,Direction.Down);
		this.UpdateVector(vector, pos, map,Direction.Left);
		this.UpdateVector(vector, pos, map,Direction.Right);
	}
	private void UpdateVector(Dictionary<(int x, int y), int> vector
	, (int x, int y) pos
	, List<List<bool>> map
	, Direction d
	
	)
	{
		int startValue = vector[pos];
		if(startValue == int.MaxValue){ return; }
		while (Helper.WithinBounds(pos,map) && !map[pos.y][pos.x])
		{
			vector[pos] = Math.Min(vector[pos],startValue++);
			pos = Helper.GetNextPosition(d, pos);

		}
		//while(Helper.WithinBounds(pos,map) && !map[pos.y][pos.x])
		//{
		//	pos = Helper.GetNextPosition(Direction.Right, pos);
		//}

	}

	private List<List<bool>> ParseInput()
	{
		var allLines = File.ReadAllLines(this.path);
		var map= new List<List<bool>>();
		for(int y= 0;y<space.height+1;y++)
		{
			var row = new List<bool>();
			for(int x= 0;x<space.width+1;x++)
			{
				row.Add(false);
			}
			map.Add(row);
		}

		foreach (var line in allLines.Take(this.Bytes))
		{
			var numbers = line
				.Split(',')
				.Select(x => int.Parse(x))
				.ToList();

			map[numbers[1]][numbers[0]] = true;
		}

		return map;
	}
}