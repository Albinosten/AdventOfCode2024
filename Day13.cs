using System.Reflection.PortableExecutable;

namespace AdventOfCode2024
{
	internal class Day13 : IPuzzle<long>
	{
		public bool IsExample { get; set; }
		public long FirstResult => this.IsExample ? 480 : 37686;

		public long SecondResult => this.IsExample ? 875318608908 : 77204516023437;

		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";
		public Day13()
		{
			Day14.Foo = 13;
		}
		public long First()
		{
			return this.Solve();
		}

		public long Second()
		{
			return this.Solve(Part.Two);
		}

		/*
			//solve for A
			8400 = A*94 + B*22
			8400 / 94 = A + (B*22  / 94)
			8400 / 94 - (B*22  / 94) = A
			(8400 - B*22) / 94 = A


			//Solve for B
			5400 = A*34 + B*67
			(5400)/67 = (A*34)/67 + B
			(5400)/67 - (A*34)/67  = B
			(5400 - A*34) / 67  = B


			//replace variable A with definition of A
			(5400 - A*34) / 67  = B
			(5400 - ((8400 - B*22) / 94)*34) / 67 = B
			(5400 - ((8400 - B*22) / 94)*34) = B * 67
			5400 - ((8400 - B*22) / 94) * 34 = B * 67
			5400 - ((8400 *34 - B*22 * 34) / 94)  = B * 67
			5400 * 94 - (8400 *34 - B*22 * 34) = B * 67 * 94
			5400 * 94 - 8400*34 + B*22 * 34 = B * 67 * 94
			5400 * 94 - 8400*34 = B * 67 * 94 - B*22 * 34
			5400 * 94 - 8400*34 = B * (67 * 94 - 22 * 34)


			(5400 * 94 - 8400*34) / (67 * 94 - 22 * 34) = B  //använd denna för B

			(507600 - 285600) / (6298 - 748) = B
			222000 / 5550 = B
			40=B

			(8400 - B*22) / 94 = A
			(8400 - 40*22) / 94 = A
			(8400 - 880) / 94 = A
			7520 / 94 = A
			80 = A
		 */
		private long Solve(Part part = Part.One)
		{

			var clawMachines = this.ParseInput(part);
			foreach (var machine in clawMachines)
			{
				//Button A: X+94, Y+34
				//Button B: X+22, Y+67
				//Prize: X = 8400, Y = 5400

				//(5400 * 94 - 8400*34) / (67 * 94 - 22 * 34) = B  //använd denna för B
				decimal B = (machine.Prize.y * machine.ButtonA.x - machine.Prize.x * machine.ButtonA.y)
					/ (machine.ButtonB.y * machine.ButtonA.x - machine.ButtonB.x * machine.ButtonA.y);
				//(8400 - B*22) / 94 = A
				decimal A = (machine.Prize.x - B * machine.ButtonB.x) / machine.ButtonA.x;

				var X = ((long)A) * machine.ButtonA.x + ((long)B) * machine.ButtonB.x;
				var Y = ((long)A) * machine.ButtonA.y + ((long)B) * machine.ButtonB.y;
				if (X == machine.Prize.x
					&& Y == machine.Prize.y)
				{
					machine.CanWin = true;
					machine.CheapestWay = (long)(A * 3 + B);
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
				var priceXValue = string.Concat(chunk[2].Split("X=")[1].TakeWhile(x => x != ','));
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
