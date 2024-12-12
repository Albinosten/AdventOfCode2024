namespace AdventOfCode2024
{


	internal class Day8 : IPuzzle
	{
		public bool IsExample { get; set; }
		public int FirstResult => this.IsExample ? 14 : 390;

		public long SecondResult => this.IsExample ? 34 : 1246;

		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";
		public int First()
		{
			var map = this.ParseInput();

			var antennas = new Dictionary<char, List<(int x, int y)>>();
			var antinodes = new List<(int x, int y)>();

			for (var y = 0; y < map.Count; y++) 
			{
				for (var x = 0; x < map.Count; x++)
				{
					var antennaKey = map[y][x];
					if(antennaKey != '.')
					{
						if(!antennas.ContainsKey(antennaKey))
						{
							antennas.Add(antennaKey, []);
						}
						foreach(var antenna in antennas[antennaKey])
						{
							var delta = this.GetDelta((x,y), antenna);
							var antinode = delta.Add((x, y));
							if(this.WithinBounds(antinode, map))
							{
								antinodes.Add(antinode);
							}

							delta = this.GetDelta(antenna, (x, y));
							antinode = delta.Add(antenna);
							if (this.WithinBounds(antinode, map))
							{
								antinodes.Add(antinode);
							}
						}
						antennas[antennaKey].Add((x, y));
					}
				}
			}

			return antinodes
				.Distinct()
				.Count();
		}

		public long Second()
		{
			var map = this.ParseInput();

			var antennas = new Dictionary<char, List<(int x, int y)>>();
			var antinodes = new List<(int x, int y)>();

			for (var y = 0; y < map.Count; y++)
			{
				for (var x = 0; x < map.Count; x++)
				{
					var antennaKey = map[y][x];
					if (antennaKey != '.')
					{
						antinodes.Add((x, y));
						if (!antennas.ContainsKey(antennaKey))
						{
							antennas.Add(antennaKey, []);
						}
						foreach (var antenna in antennas[antennaKey])
						{
							var delta = this.GetDelta((x, y), antenna);
							var antinode = delta.Add((x, y));

							while(this.WithinBounds(antinode, map))
							{
								antinodes.Add(antinode);
								antinode = antinode.Add(delta);
							}
							
							
							delta = this.GetDelta(antenna, (x, y));
							antinode = delta.Add(antenna);
							while(this.WithinBounds(antinode, map))
							{
								antinodes.Add(antinode);
								antinode = antinode.Add(delta);
							}
						}
						antennas[antennaKey].Add((x, y));
					}
				}
			}

			return antinodes
				.Distinct()
				.Count();
		}

		private bool WithinBounds((int x, int y) pos, List<List<char>> map)
		{
			return pos.x >= 0 
				&& pos.y >= 0 
				&& pos.y < map.Count
				&& pos.x < map[0].Count
				;
		}
		private (int x, int y) GetDelta((int x, int y) first, (int x, int y) second)
		{
			return (first.x - second.x, first.y - second.y);
		}

		private List<List<char>> ParseInput()
		{
			var allLines = File.ReadAllLines(this.path);
			var result = new List<List<char>>();

			foreach (var line in allLines)
			{
				var row = new List<char>();
				foreach(char c in line)
				{
					row.Add(c);
				}
				result.Add(row);
			}

			return result;
		}
	}
}

