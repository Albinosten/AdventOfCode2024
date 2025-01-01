using System;
using System.Numerics;

namespace AdventOfCode2024
{
	internal class Day14 : IPuzzle
	{
		public bool IsExample { get; set; }
		public int FirstResult => this.IsExample ? 12 : 225810288;
		public static int Foo { get; set; }
		public long SecondResult => this.IsExample ? 0 : 6752;
		private (int width, int height) space => this.IsExample ? (11, 7) : (101, 103);

		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";

		public bool HasStraightLine(int lenght, List<(int x, int y)> guardPoses)
		{
			var poses = guardPoses.ToList();
			return guardPoses.Any(x => this.HasStraightLine(lenght, x, poses));
		}
		public bool HasStraightLine(int lenght, (int x, int y) pos, List<(int x, int y)>guardPoses)
		{
			for (int i = 0; i < lenght; i++) 
			{
				if(!guardPoses.Contains((pos.x+i, pos.y)))
				{
					return false;
				}
			}
			return true;
		}
		public int First()
		{
			var guards = this.ParseInput();

			for (int time = 0; time < 100; time++) 
			{

				for (int g = 0; g < guards.Count; g++)
				{
					var guard = guards[g];
					guard.pos = this.Teleport(this.Add(guard.pos, guard.vector));

					guards[g] = guard;
					
				}

				
			}
			//Console.WriteLine();
			//Print(guards);

			var result = new int[4] { 0,0,0,0};
			var temp = new List<(int x, int y)>();
			foreach (var guard in guards)
			{
				if(guard.pos.x == this.space.width/2)
				{
					continue;
				}
				if (guard.pos.y == this.space.height/ 2)
				{
					continue;
				}
				if (guard.pos.x < this.space.width / 2)
				{
					if (guard.pos.y < this.space.height / 2) 
					{
						result[0]++;//topleft
					}
					else
					{
						temp.Add(guard.pos);
						result[2]++;//bottom left
					}
				}
				else
				{
					if (guard.pos.y < this.space.height / 2)
					{
						result[1]++;//top right

					}
					else
					{
						result[3]++;//bottom right

					}
				}
			}
			return result[0]* result[1]* result[2]* result[3];
		}

		private (int x, int y) Add((int x, int y) pos, (int x, int y) vector)
		{
			return (pos.x + vector.x, pos.y + vector.y);
		}
		private (int x, int y) Teleport((int x, int y) pos)
		{
			if (pos.x > this.space.width-1) 
			{
				pos.x -= this.space.width;
			}
			if (pos.x < 0)
			{
				pos.x += this.space.width;
			}
			if (pos.y > this.space.height-1)
			{
				pos.y -= this.space.height;
			}
			if (pos.y < 0)
			{
				pos.y += this.space.height;
			}

			return pos;
		}
		private void Print(List<((int x, int y) pos, (int x, int y) vector)> guards)
		{
		//if(!this.IsExample){ return; }
			var positions = new int[this.space.height, this.space.width];
			foreach (var guard in guards) 
			{
				positions[guard.pos.y,guard.pos.x]++;
			}
			for (int y = 0; y < this.space.height; y++) 
			{
				Console.WriteLine();
				for (int x = 0; x < this.space.width; x++)
				{
					var count = positions[y, x];
					var value = count > 0 ? count.ToString() : ".";
					Console.Write(value);
				}
			}
		}
		public long Second()
		{
			var guards = this.ParseInput();

			for (int time = 0; time < this.space.width * this.space.height; time++)
			{
				if (this.HasStraightLine(10, guards.Select(x => x.pos).ToList()))
				{
					return time;
				}
				for (int g = 0; g < guards.Count; g++)
				{
					var guard = guards[g];
					guard.pos = this.Teleport(this.Add(guard.pos, guard.vector));

					guards[g] = guard;

				}
			}
			return 0;
		}

		private List<((int x, int y) pos , (int x, int y) vector)> ParseInput()
		{
			var allLines = File.ReadAllLines(this.path);
			var list = new List<((int x, int y) pos, (int x, int y) vector)>();

			foreach (var line in allLines)
			{
				var numbers = ParseValues(line);
				list.Add(((numbers.p1, numbers.p2), (numbers.v1, numbers.v2)));
			}

			return list;
		}
		public static (int p1, int p2, int v1, int v2) ParseValues(string input)
		{
			// Split the input string into p and v components
			string[] parts = input.Split(' '); // Split by space
			string pPart = parts[0]; // "p=10,22"
			string vPart = parts[1]; // "v=50,-21"

			// Parse p values
			string[] pValues = pPart.Split('=')[1].Split(',');
			int p1 = int.Parse(pValues[0]);
			int p2 = int.Parse(pValues[1]);

			// Parse v values
			string[] vValues = vPart.Split('=')[1].Split(',');
			int v1 = int.Parse(vValues[0]);
			int v2 = int.Parse(vValues[1]);

			// Return the parsed values as a tuple
			return (p1, p2, v1, v2);
		}
	}
}
