﻿using System.Linq;
using System.Net.WebSockets;

namespace AdventOfCode2024
{
	internal class Day12 : IPuzzle<long>
	{
		public bool IsExample { get; set; }
		public long FirstResult => this.IsExample ? 1930 : 1457298;

		public long SecondResult => this.IsExample ? 1206 : 921636;

		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";
		public long First()
		{
			var map = this.ParseInput();
			var unvisited = new HashSet<(int x, int y)>();
			for (var y = 0; y < map.Count; y++)
			{
				for (var x = 0; x < map[y].Count; x++)
				{
					unvisited.Add((x, y));
				}
			}
			var result = new List<(int size, int number)>();
			while (unvisited.Count > 0)
			{
				var startPoint = unvisited.First();
				var q = new Queue<(int x, int y)>();
				q.Enqueue(startPoint);

				var fenceNumber = 0;
				var cluster = new HashSet<(int x, int y)>
				{
					startPoint
				};

				while (q.Count > 0)
				{
					var n = q.Dequeue();
					unvisited.Remove(n);

					var neighbours = this.GetPossibleMove(n)
						.Where(x => !cluster.Contains(x))
						.ToList();

					foreach (var x in neighbours)
					{
						if (this.WithinBounds(x, map))
						{
							if (this.IsSamePlant(n, x, map))
							{
								cluster.Add(x);
								q.Enqueue(x);
							}
							else
							{
								//annat kluster
								fenceNumber++;
							}
						}
						else
						{
							//utanför kartan. 
							fenceNumber++;
						}
					}
				}
				result.Add((cluster.Count(), fenceNumber));

			}

			return result.Select(x => (long)(x.number * x.size)).Sum();
		}

		//915174 too low
		//921636 ??
		public long Second()
		{
			var map = this.ParseInput();
			var unvisited = new HashSet<(int x, int y)>();
			for (var y = 0; y < map.Count; y++)
			{
				for (var x = 0; x < map[y].Count; x++)
				{
					unvisited.Add((x, y));
				}
			}
			var result = new List<(string name, long size, long number)>();
			while (unvisited.Count > 0)
			{
				var startPoint = unvisited.First();
				var q = new Queue<(int x, int y)>();
				q.Enqueue(startPoint);

				var fenceNumber = 0;
				var fences = new List<(Direction d, int x, int y)>();
				var cluster = new HashSet<(int x, int y)>
				{
					startPoint
				};

				while (q.Count > 0)
				{
					var n = q.Dequeue();
					unvisited.Remove(n);

					var neighbours = this.GetPossibleMoveWithDirections(n)
						.Where(x => !cluster.Contains(x.pos))
						.ToList();

					foreach (var x in neighbours)
					{
						if (this.WithinBounds(x.pos, map)
							&& this.IsSamePlant(n, x.pos, map)
						)
						{
							cluster.Add(x.pos);
							q.Enqueue(x.pos);
						}
						else
						{
							//utanför kartan. 
							fenceNumber++;
							fences.Add((x.d, x.pos.x, x.pos.y));
						}
					}
				}
				var name = map[cluster.First().y][cluster.First().x].ToString();
				result.Add((name, cluster.Count(), GetNumberOfFences(fences, Part.Two)));
			}

			return result.Select(x => (long)(x.number * x.size)).Sum();
		}
		private int GetNumberOfFences(List<(Direction d, int x, int y)> fencesPoses, Part part = Part.One)
		{
			//this.Print(fencesPoses, fencesPoses);
			if (part == Part.One) { return fencesPoses.Count; }

			var unvisited = new List<(Direction d, (int x, int y) pos)>();
			var fencesLeft = new List<(Direction d, (int x, int y) pos)>();
			foreach (var fencePos in fencesPoses)
			{
				unvisited.Add((fencePos.d, (fencePos.x, fencePos.y)));
				fencesLeft.Add((fencePos.d, (fencePos.x, fencePos.y)));
			}

			var result = fencesPoses.Count;

			while (unvisited.Count > 0)
			{
				var startPoint = unvisited.First();
				var q = new Queue<(Direction d, (int x, int y) pos)>();
				var removed = new List<(int x, int y)>();
				q.Enqueue((startPoint));

				while (q.Count > 0)
				{
					var n = q.Dequeue();
					unvisited.Remove(n);

					removed.Add(n.pos);

					var neighbours = this.GetPossibleMove(n.pos)

						.Where(x => fencesPoses.Contains((n.d, x.x, x.y)))

						.Where(x => unvisited.Contains((n.d, (x.x, x.y))))
						.Where(x => !removed.Contains((x.x, x.y)))
						.ToList();

					foreach (var x in neighbours)
					{
						q.Enqueue((n.d, x));
						fencesLeft.Remove((n.d, (x.x, x.y)));
						//this.Print(fencesLeft.Select(x => x.pos).ToList(),fencesPoses.Select(x => (x.x, x.y)).ToList());
						removed.Add((x.x, x.y));
						result--;
					}
				}
			}

			return result;
		}

		public void Print(List<(int x, int y)> left, List<(int x, int y)> original)
		{
			Console.WriteLine();
			var yMax = left.Max(x => x.y);
			var yMin = left.Min(x => x.y);

			var xMax = left.Max(x => x.x);
			var xMin = left.Min(x => x.x);

			for (var y = yMin; y <= yMax; y++)
			{
				Console.WriteLine();
				for (var x = xMin; x <= xMax; x++)
				{
					var count = left.Where(l => l.x == x && l.y == y).Count().ToString();
					if (count == "0" && original.Contains((x, y)))
					{
						count = "*";
					}
					else if (count == "0") { count = " "; }
					Console.Write(count.ToString());
				}
			}
		}



		private bool IsSamePlant((int x, int y) currentPos, (int x, int y) nextPos, List<List<char>> map)
		{
			return map[currentPos.y][currentPos.x] == map[nextPos.y][nextPos.x];
		}
		private bool WithinBounds((int x, int y) pos, List<List<char>> map)
		{
			return pos.x >= 0
				&& pos.y >= 0
				&& pos.y < map.Count
				&& pos.x < map[0].Count;
		}
		
		private List<(Direction d, (int x, int y) pos)> GetPossibleMoveWithDirections((int x, int y) startPos)
		{
			return
			[
				(Direction.Right, (startPos.x+1, startPos.y)),
				(Direction.Left, (startPos.x-1, startPos.y)),
				(Direction.Down, (startPos.x, startPos.y+1)),
				(Direction.Up, (startPos.x, startPos.y-1)),
			];
		}
		private List<(int x, int y)> GetPossibleMove((int x, int y) startPos)
		{
			return
			[

				(startPos.x + 1, startPos.y),
				(startPos.x - 1, startPos.y),
				(startPos.x, startPos.y + 1),
				(startPos.x, startPos.y - 1),
			];
		}
		private List<List<char>> ParseInput()
		{
			var allLines = File.ReadAllLines(this.path);
			var list = new List<List<char>>();

			foreach (var line in allLines)
			{
				var newLine = new List<char>();
				foreach (var item in line)
				{
					newLine.Add(item);

				}
				list.Add(newLine);
			}

			return list;
		}
	}
}
