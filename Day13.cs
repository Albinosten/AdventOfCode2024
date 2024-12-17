namespace AdventOfCode2024
{
	internal class Day13 : IPuzzle<long>
	{
		public bool IsExample { get; set; }
		public long FirstResult => this.IsExample ? 480 : 0;

		public long SecondResult => this.IsExample ? 0 : 0;

		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";
		public long First()
		{
			var clawMachines = this.ParseInput();
			long tries = 0;
			var result = new List<(int A, int B)>();
			foreach (var machine in clawMachines) 
			{
				for (int A = 0;A < 100;A++)
				{
					for (int B = 0; B < 100; B++) 
					{
						tries++;
						var x = machine.ButtonA.x * A + machine.ButtonB.x * B;
						var y = machine.ButtonA.y * A + machine.ButtonB.y * B;
						if(x == machine.Prize.x && y == machine.Prize.y)
						{
							machine.CanWin = true;
							var cost = A * 3 + B ;
							if(cost < machine.CheapestWay)
							{
							result.Add((A, B));
								machine.CheapestWay = cost;
							}
						}
					}
				}
			}

			Console.WriteLine("Tries: " + tries);
			return clawMachines.Where(x => x.CanWin).Sum(x => x.CheapestWay);
		}

		public long Second()
		{
			var clawMachines = this.ParseInput(Part.One);
			foreach (var machine in clawMachines)
			{
				var s = new Stack<(long A, long B)>();
				var visited = new HashSet<(long A, long B)>();
				s.Push((0, 0));

				while (s.Count > 0)
				{
					var n = s.Pop();
					visited.Add(n);
					long x = machine.ButtonA.x * n.A + machine.ButtonB.x * n.B;
					long y = machine.ButtonA.y * n.A + machine.ButtonB.y * n.B;
					if(n.A > 40 && n.B > 39)
					{

					}
					Console.WriteLine("A: " + n.A + " B: " + n.B);
					
					if (x == machine.Prize.x && y == machine.Prize.y) //win
					{
						machine.CanWin = true;
						var cost = n.A * 3 + n.B;
						if (cost < machine.CheapestWay)
						{
							machine.CheapestWay = cost;
						}
					}


					var nextX = machine.ButtonA.x * (n.A + 1) + machine.ButtonB.x * n.B;
					var nextY = machine.ButtonA.y * (n.A + 1) + machine.ButtonB.y * n.B;
					if (!visited.Contains((n.A+1, n.B))
						&& nextX < machine.Prize.x && nextY < machine.Prize.y
					)
					{
						s.Push((n.A+1, n.B));
					}

					nextX = machine.ButtonA.x * n.A + machine.ButtonB.x * (n.B + 1);
					nextY = machine.ButtonA.y * n.A + machine.ButtonB.y * (n.B + 1);
					if (!visited.Contains((n.A, n.B+1))
						&& nextX < machine.Prize.x && nextY < machine.Prize.y
					)
					{
						s.Push((n.A, n.B+1));
					}
				}
			}
			return clawMachines.Where(x => x.CanWin).Sum(x => x.CheapestWay);

		}

		private List<ClawMachine> ParseInput(Part part = Part.One)
		{
			var chunks = File.ReadAllLines(this.path).Chunk(4);
			var list = new List<ClawMachine>();

			foreach (var chunk in chunks)
			{
				var clawMachine = new ClawMachine();
				//Button A: X+31, Y+71
				var buttonAXValue = chunk[0][12].ToString() + chunk[0][13].ToString();
				var buttonAYValue = chunk[0][18].ToString() + chunk[0][19].ToString();
				clawMachine.ButtonA = (int.Parse(buttonAXValue), int.Parse(buttonAYValue));

				//Button B: X+22, Y+67
				var buttonBXValue = chunk[1][12].ToString() + chunk[1][13].ToString();
				var buttonBYValue = chunk[1][18].ToString() + chunk[1][19].ToString();
				clawMachine.ButtonB = (int.Parse(buttonBXValue), int.Parse(buttonBYValue));

				//Prize: X=8400, Y=5400
				var priceXValue = string.Concat( chunk[2].Split("X=")[1].TakeWhile(x => x != ','));
				var priceYValue = chunk[2].Split("Y=")[1];

															//12616466338
				long conversionError = part == Part.One ? 0 : 10000000000000;

				clawMachine.Prize = (long.Parse(priceXValue) + conversionError, long.Parse(priceYValue) + conversionError);
				list.Add(clawMachine);
			}

			return list;
		}
	}
	public class ClawMachine
	{
	public ClawMachine() { CheapestWay = long.MaxValue; }
		public (int x, int y) ButtonA { get; set; }
		public (int x, int y) ButtonB { get; set; }
		public (long x, long y) Prize { get; set; }
		public long CheapestWay { get; set; }
		public bool CanWin { get; set; }
	}
}
