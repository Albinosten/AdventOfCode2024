using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace AdventOfCode2024
{
	internal class Day21 : IPuzzle<long>
	{
		public bool IsExample { get; set; }
		public long FirstResult => this.IsExample ? 126384 : 184716;

		public long SecondResult => this.IsExample ? 154115708116294 : 229403562787554;
		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";

		public long First()
		{
			var codes = this.ParseInput();
			var result = 0L;
			foreach (var code in codes)
			{
				var numericKeyPad = new KeyPad();
				var temp = new List<string>();

				var pathsOnNumericKeyPad = numericKeyPad.GetPaths(code, this.NumericKeyPad());

				foreach (var item in pathsOnNumericKeyPad)
				{
					temp.AddRange(this.Solve([
						new KeyPad(),
						new KeyPad(),
						]
					, item));
				}

				var shortest = temp.OrderBy(x => x.Length).First();
				var value = int.Parse(string.Join("",code.SkipLast(1)));
				result += (value * shortest.Length);
			}

			return result;
		}
		public long Second()
		{
			var codes = this.ParseInput();
			var result = 0L;
			foreach (var code in codes)
			{
				var numericKeyPad = new KeyPad();
				var temp = new List<long>();

				var pathsOnNumericKeyPad = numericKeyPad.GetPaths(code, this.NumericKeyPad());

				foreach (var item in pathsOnNumericKeyPad)
				{
					var pads = new List<KeyPad>();
					for(int i = 0;i<25;i++)
					{
						pads.Add(new KeyPad());
					}
					temp.Add(this.SolvePart2(pads, item));
				}

				var shortest = temp.Min();
				var value = int.Parse(string.Join("", code.SkipLast(1)));
				result += (value * shortest);
			}

			return result;
		}


		public long SolvePart2(List<KeyPad> keypads, string path)
		{
			var result = 0L;

			if (keypads.Count > 0)
			{
				var next = keypads.ToList();
				var keyPad = keypads[0];

				if(cache.ContainsKey((keypads.Count, path)))
				{
					return cache.Get((keypads.Count, path)).Max();
				}

				next.RemoveAt(0);

				var start = keyPad.CurrentPos;
				for (int i = 0; i < path.Length; i++)
				{
					var paths = keyPad.GetPaths(start, path[i], DirectionalKeyPad());
					start = path[i];

					var temp = new List<long>();
					foreach (var p in paths)
					{
						temp.Add(this.SolvePart2(next.CloneList(), p));
					}
					result += temp.Min();
				}
				cache.Add((keypads.Count, path), result);
			}
			else { return path.Length; }

			return result;
		}
		MultiValueDictionary<(int, string), long> cache = new();

		public List<string> Solve(List<KeyPad>keypads, string path)
		{
			var result = new List<string>();

			if (keypads.Count > 0) 
			{
				var next = keypads.ToList();
				var keyPad = keypads[0];

				var paths = keyPad.GetPaths(path, this.DirectionalKeyPad());
				next.RemoveAt(0);

				foreach (var nextPath in paths)
				{
					result.AddRange(this.Solve(next.CloneList(), nextPath));
				}
			}
			else { return [path]; }

			return result;
		}
		
		private List<List<char>> NumericKeyPad()
		{
			return new List<List<char>>()
			{
				{['7','8','9'] },
				{['4','5','6'] },
				{['1','2','3'] },
				{['x','0','A'] },
			};
		}
		private List<List<char>> DirectionalKeyPad()
		{
			return new List<List<char>>()
			{
				{['x', '^', 'A'] },
				{['<','v','>'] },
			};
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
	class KeyPad : Clonable<KeyPad>
	{
		public char CurrentPos { get; set; }
		static MultiValueDictionary<(char, char), string> smallCache = new MultiValueDictionary<(char, char), string>();
		public List<string> GetPaths(string path, List<List<char>> map)
		{
			var values = new List<List<string>>();
			for (var i = 0; i < path.Length; i++)
			{
				var paths = this.GetPaths(this.CurrentPos, path[i], map);
				this.CurrentPos = path[i];

				values.Add(paths.DistinctList());
			}
			return Helper.CartesianProduct(values);
		}
		
		public List<string> GetPaths(char start, char end, List<List<char>> map)
		{
			if(smallCache.ContainsKey((start,end)))
			{
				return smallCache.Get((start, end));
			}

			var startPos = (0, 0);
			var endPos = (0, 0);
			for (var y = 0; y < map.Count; y++)
			{
				for (var x = 0; x < map[y].Count; x++)
				{
					if (map[y][x] == start)
					{
						startPos = (x, y);
					}
					if (map[y][x] == end)
					{
						endPos = (x, y);
					}
				}
			}

			var q = new Queue<((int x, int y) pos, List<((int x, int y) positions, Direction d)> visited, Direction d, int score)>();
			q.Enqueue((startPos, new List<((int x, int y) positions, Direction d)>(), Direction.Up, 0));
			q.Enqueue((startPos, new List<((int x, int y) positions, Direction d)>(), Direction.Down, 0));
			q.Enqueue((startPos, new List<((int x, int y) positions, Direction d)>(), Direction.Left, 0));
			q.Enqueue((startPos, new List<((int x, int y) positions, Direction d)>(), Direction.Right, 0));

			List<(int score, List<((int x, int y) ppositions, Direction d)> path)> paths = [];
			var shortestPath = int.MaxValue;
			while (q.Count > 0)
			{
				var n = q.Dequeue();

				if (n.pos == endPos && n.score <= shortestPath)
				{
					shortestPath = n.score;
					paths.Add((n.score, n.visited));
					continue;
				}

				var moves = Helper.GetAllDirections()
					.Select(x => new
					{
						Pos = Helper.GetNextPosition(x, n.pos),
						Dir = x
					})
					.Where(x => Helper.WithinBounds(x.Pos, map))
					.Where(x => ValidMove(x.Pos, map))
					.Where(x => !n.visited.Select(x => x.positions).Contains(x.Pos))
					.ToList();
				foreach (var move in moves)
				{
					var visited = n.visited.ToList();
					visited.Add((move.Pos, move.Dir));

					var score = n.score;
					if (n.d != move.Dir)
					{
						score = score + 100;
					}
					q.Enqueue((move.Pos, visited, move.Dir, score++));
				}
			}
			var results = paths
				.Where(x => x.score == shortestPath)
				.Select(x => string.Join("", x.path.Select(x => TranslateDirection(x.d))))
				.Select(x => x + "A")
				.DistinctList();
			foreach (var path in results)
			{
				smallCache.Add((start, end), path);
			}
			return results;
		}
		bool ValidMove((int x, int y) pos, List<List<char>> map)
		{
			return map[pos.y][pos.x] != 'x';
		}
		char TranslateDirection(Direction direction) => direction switch
		{
			Direction.Up => '^',
			Direction.Down => 'v',
			Direction.Left => '<',
			Direction.Right => '>',
			_ => throw new InvalidOperationException(),
		};
		public KeyPad Clone()
		{
			return new KeyPad() { CurrentPos = this.CurrentPos };
		}
		static int number = 0;
		public KeyPad()
		{
			CurrentPos = 'A';
			number = ++number;
		}
	}
}
