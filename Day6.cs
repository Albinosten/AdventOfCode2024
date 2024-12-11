using System.Collections.Generic;

namespace AdventOfCode2024
{
	internal class Day6 : IPuzzle
	{
		public bool IsExample { get; set; }
		public int FirstResult => this.IsExample ? 41 : 5030;

		public long SecondResult => this.IsExample ? 6 : 1928;

		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";

		public int First()
		{
			this.Run(this.ParseInput(), out var pathOfGuard);
			return pathOfGuard
			.Select(x => x.position)
				.Distinct()
				.Count()
				;
		}

		public long Second()
		{
			this.Run(this.ParseInput(), out var pathOfGuard);
			var result = 0;
			var possibleBlocks = pathOfGuard
				.Select(x => x.position)
				.Distinct()
				.Skip(1)
				.ToList();
				;
			foreach (var position in possibleBlocks)
			{
				var map = this.ParseInput();
				map[position.y][position.x] = '#';

				if(this.Run(map, out var _))
				{
					result++;
				}
			}


			return result;
		}
		private bool Run(List<List<char>> map, out List<(Direction direction, (int x, int y) position)> pathOfGuard)
		{
			pathOfGuard = new List<(Direction direction, (int x, int y) position)>
			{
				(Direction.Up, (this.Location.x, this.Location.y))
			};
			
			var position = this.Location;
			var direction = Direction.Up;
			while (true)
			{
				var nextPos = this.GetNextPosition(direction, position);

				if (nextPos.x < 0
					|| nextPos.y < 0
					|| nextPos.x >= map[0].Count
					|| nextPos.y >= map.Count
					)
				{
					return false;
				}

				var value = map[nextPos.y][nextPos.x];
				if (value == '#')
				{
					direction = this.GetNextDirection(direction);
				}
				else
				{
					position = nextPos;
					if(pathOfGuard.Contains((direction, nextPos)))
					{
						return true;
					}
					pathOfGuard.Add((direction,position));
				}
			}
		}

		private (int x, int y) Location { get; set; }

		private Direction GetNextDirection(Direction direction) => direction switch
		{
			Direction.Up => Direction.Right,
			Direction.Right => Direction.Down,
			Direction.Down => Direction.Left,
			Direction.Left => Direction.Up,
		};
		private (int x, int y) GetNextPosition(Direction direction, (int x, int y) position) => direction switch
		{
			Direction.Up => (position.x, position.y - 1),
			Direction.Down => (position.x, position.y + 1),
			Direction.Left => (position.x - 1, position.y),
			Direction.Right => (position.x + 1, position.y),
		};
		
		private List<List<char>> ParseInput()
		{
			var allLines = File
				.ReadAllLines(this.path)
				.ToList();
			var map = new List<List<char>>();

			for(int y = 0; y < allLines.Count; y++)
			{
				var line = new List<char>();
				for (int x = 0; x < allLines[y].Count(); x++)
				{
					var value = allLines[y][x];
					line.Add(value);
					if (value == '^')
					{
						this.Location = (x, y);
					}
				}
				map.Add(line);
			}

			return map;
		}
		private enum Direction
		{
			Up,
			Down,
			Left,
			Right
		}
	}
}
