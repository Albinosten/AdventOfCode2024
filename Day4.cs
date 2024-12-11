using System.Drawing;

namespace AdventOfCode2024
{


	internal class Day4 : IPuzzle
	{
		public bool IsExample => false;
		public int FirstResult => this.IsExample ? 18 : 2613;

		public long SecondResult => this.IsExample ? 9 : 1905;

		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";
		public int First()
		{
			var count = 0;
			var input = this.ParseInput();
			for(int y = 0; y < input.Count; y++)
			{
				var row = input[y];
				for (int x = 0; x < row.Count; x++)
				{
					var directions = this.GetXMASMoves(x, y);
					foreach (var direction in directions)
					{
						var positions = direction
							.Where(d => WithinRange(d.x, d.y, row.Count, input.Count))
							.ToList();
						string word = "";
						foreach (var position in positions)
						{
							word += input[position.y][position.x];
						}
						if (IsValid(word))
						{
							count++;
						}
					}
				}
			}

			return count;
		}

		//1968 too high
		//980 too low
		public long Second()
		{
			var count = 0;
			var input = this.ParseInput();
			for (int y = 0; y < input.Count; y++)
			{
				var row = input[y];
				for (int x = 0; x < row.Count; x++)
				{
					if(input[y][x] == 'A')
					{
						var positions = this.GetX_MASMoves(x, y)
							.Where(d => WithinRange(d.x, d.y, row.Count, input.Count))
							.ToList();
						var word = new List<char>();
						foreach (var position in positions)
						{
							word.Add(input[position.y][position.x]);
						}
						if (IsValid(word))
						{
							count++;
						}
					}
					
				}
			}

			return count;
		}

		private bool IsValid(List<char> input)
		{
			return input.Count == 5
				&& input.Count(x => x == 'A') == 1
				&& input.Count(x => x == 'M') == 2
				&& input.Count(x => x == 'S') == 2
				&& input[1] != input[4]
				&& input[3] != input[2]
			;
		}
		private bool IsValid(string input)
		{
			return input == "XMAS" || input == "SAMX";
		}
		private (int x, int y)[] GetX_MASMoves(int x, int y)
		{
			return
			[
				(x,y),
				(x+1,y+1),//right down
				(x+1,y-1),//right up
				(x-1,y+1),//left down
				(x-1,y-1),//left up
			];
		}
		private (int x, int y)[][] GetXMASMoves(int x, int y)
		{
			return
			[
				[(x,y),(x+1,y),(x+2,y),(x+3,y)], //Right
				[(x, y),(x,y+1),(x,y+2),(x,y+3)], //down
				
				[(x, y),(x+1, y-1), (x+2, y-2), (x+3, y-3)],//Right + Upp
				[(x, y),(x+1, y+1), (x+2, y+2), (x+3, y+3)]
			];
		}
		private bool WithinRange(int x, int y, int xMax, int Ymax)
		{
			return x >= 0
				&& x < xMax
				&& y >= 0
				&& y < Ymax
				
				;

		}

		private List<List<char>> ParseInput()
		{
			var allLines = File.ReadAllLines(this.path);
			var list = new List<List<char>>();

			foreach (var line in allLines)
			{
				var characters = new List<char>();
				foreach (var c in line)
				{
					characters.Add(c);
				}
				list.Add(characters);
			}

			return list;
		}
	}
	/*
	m s
	 a
	m s

	s m
	 a 
	s m

	s s
	 a
	m m

	s m
	 a
	m s
	 */
}
