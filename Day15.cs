using System.Collections.Generic;

namespace AdventOfCode2024
{
	internal class Day15 : IPuzzle
	{
		public bool IsExample { get; set; }
		public int FirstResult => this.IsExample ? 10092 : 1414416;

		public long SecondResult => this.IsExample ? 9021 : 1386070;

		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";
		public int First()
		{
			return (int)this.Solve();
		}
		public long Second()
		{
			return this.Solve(Part.Two);
		}
		private long Solve(Part part = Part.One)
		{
			var (robotPos, map, moves) = this.ParseInput(part);

			for (var m = 0; m < moves.Length; m++)
			{
				var direction = moves[m];
				var nextPos = this.GetNextPos(robotPos, direction);
				if (part == Part.One 
					? CanMove(nextPos, direction, map) 
					: CanMovePartTwo(nextPos,direction,map))
				{
					if(part == Part.One)
					{
						this.ApplyMove(robotPos, direction, map);
					}
					else{
						this.ApplyMovePartTwo(robotPos, direction, map);
					}
					
					robotPos = nextPos;

				}
			}
			//Helper.PrintMap(map);
			var result = this.GetResult(map, part);
			return result;
		}
		long GetResult(List<List<char>> map, Part part = Part.One)
		{
			long result = 0;
			for (var y = 0; y < map.Count; y++)
			{
				for (var x = 0; x < map[y].Count(); x++)
				{
					var field = map[y][x];
					if (field == 'O')
					{
						result += 100 * y + x;
					}
					else if(field == '[')
					{
						//var a = Math.Min(y, map.Count -1 - y);
						var a = y;
						//var b = Math.Min(x, map[y].Count-1 - (x+1));
						var b = x;
						result += 100*a + b;
					}
				}
			}
			return result;
		}
		

		public void ApplyMovePartTwo((int x, int y) pos, char direction, List<List<char>> map)
		{
			var field = map[pos.y][pos.x];
			map[pos.y][pos.x] = '.';

			var nextPos = this.GetNextPos(pos, direction);
			var nextFieldValue = map[nextPos.y][nextPos.x];

			var boxCharacters = new[] { 'O', '[', ']' };
			if (boxCharacters.Contains(nextFieldValue))
			{
				if (nextPos.y != pos.y) 
				{
					var box = this.GetConnectedBox(nextPos, map);
					if(box != nextPos)
					{
						this.ApplyMovePartTwo(box, direction, map);

					}
				}
				this.ApplyMovePartTwo(nextPos, direction, map);
			}
			map[nextPos.y][nextPos.x] = field;

		}
		public void ApplyMove((int x, int y) pos, char direction, List<List<char>> map)
		{
			var field = map[pos.y][pos.x];
			map[pos.y][pos.x] = '.';

			var nextPos = this.GetNextPos(pos, direction);
			var nextFieldValue = map[nextPos.y][nextPos.x];

			var boxCharacters = new[] { 'O', '[', ']' };
			if (boxCharacters.Contains(nextFieldValue))
			{
				this.ApplyMove(nextPos, direction, map);
			}
			map[nextPos.y][nextPos.x] = field;
		}


		List<(int x, int y)> GetConnected((int x, int y) pos, char direction, List<List<char>> map, HashSet<(int x, int y)>visited)
		{
			var field = map[pos.y][pos.x];
			
			var boxCharacters = new[] { 'O', '[', ']' };
			if (boxCharacters.Contains(field)
				&& !visited.Contains(pos)
			)
			{
				visited.Add(pos);
				
				var nextPos = this.GetNextPos(pos, direction);
				this.GetConnected(nextPos, direction, map, visited);
				
				var connectedBox = this.GetConnectedBox(pos, map);
				this.GetConnected(connectedBox, direction, map, visited);


				//visited.Add(connectedBox);

				return visited.ToList();
				;
			}
			return [];
		}
		bool CanMovePartTwo((int x, int y) pos, char direction, List<List<char>> map)
		{
			var positions = this.GetConnected(pos, direction, map, new HashSet<(int x, int y)>()).Distinct().ToList();
			return positions.All(x => CanMove(x, direction, map)) 
				&& this.CanMove(pos,direction,map);
		}
		bool CanMove((int x, int y) pos, char direction, List<List<char>> map)
		{
			var field = map[pos.y][pos.x];
			if (field == '.')
			{
				return true;
			}
			var boxCharacters = new[] { 'O', '[', ']' };
			if (boxCharacters.Contains(field))
			{
				var nextPos = this.GetNextPos(pos, direction);
				return this.CanMove(nextPos, direction, map)
				;
			}
			return false;
		}
		(int x, int y) GetConnectedBox((int x, int y) pos, List<List<char>> map)
		{
			if(map[pos.y][pos.x] == '[')
			{
				return (pos.x + 1, pos.y);
			}
			if (map[pos.y][pos.x] == ']')
			{
				return (pos.x - 1, pos.y);
			}
			return pos;
		}
		(int x, int y) GetNextPos((int x, int y) current, char direction) => direction switch
		{
			'^' => (current.x, current.y - 1),
			'v' => (current.x, current.y + 1),
			'<' => (current.x - 1, current.y),
			'>' => (current.x + 1, current.y),
			_ => throw new InvalidOperationException(),
		};

		

		private ((int x, int y) robotPos, List<List<char>> map, string moves) ParseInput(Part part)
		{
			var robotPos = (0, 0);
			var map = new List<List<char>>();
			var allLines = File.ReadAllLines(this.path);
			var list = new List<string>();

			var mapInput = allLines.TakeWhile(x => !string.IsNullOrEmpty(x)).ToList();
			for (var y = 0; y < mapInput.Count(); y++)
			{
				var row = new List<char>();
				for (var x = 0; x < mapInput[y].Count(); x++)
				{
					
					var field = mapInput[y][x];
					if(field == 'O')
					{
						if (part == Part.Two)
						{
							row.Add('[');
							row.Add(']');
						}
						else{ row.Add(field); }
					}
					if (field == '@')
					{
						robotPos = (x, y);
						row.Add('@');
						if (part == Part.Two)
						{
							robotPos = (x*2, y);
							row.Add('.');
						}
					}
					if(field == '#')
					{
						row.Add(field);
						if (part == Part.Two)
						{
							row.Add(field);
						}
					}
					if (field == '.')
					{
						row.Add(field);
						if (part == Part.Two)
						{
							row.Add(field);
						}
					}
				}
				map.Add(row);
			}
			var moves = allLines.Skip(mapInput.Count).Aggregate((result, str) => result += str);

			return (robotPos, map, moves);
		}
	}
}
