using System;

namespace AdventOfCode2024
{
	internal class Day22 : IPuzzle<long>
	{
		public bool IsExample { get; set; }
		public long FirstResult => this.IsExample ? 37327623 : 19458130434;

		public long SecondResult => this.IsExample ? 23 : 2130;

		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";
		public long First()
		{
			var secrets = this.ParseInput();

			long result = 0L;
			foreach (var secret in secrets) 
			{
				var number = secret;
				for (int i = 0; i < 2000; i++)
				{
					number = this.calculate(number);
				}
				result += number;
			}
			return result;
		}
		long calculate(long secret)
		{
			//step 1
			//Calculate the result of multiplying the secret number by 64.
			//Then, mix this result into the secret number.
			//Finally, prune the secret number.

			secret = this.Mix(secret * 64, secret);
			secret = this.prune(secret);

			//step 2
			//Calculate the result of dividing the secret number by 32.
			//Round the result down to the nearest integer.
			//Then, mix this result into the secret number.
			//Finally, prune the secret number.
			secret = this.Mix((int)(secret / 32), secret);
			secret = this.prune(secret);

			//step 3
			//Calculate the result of multiplying the secret number by 2048.
			//Then, mix this result into the secret number.
			//Finally, prune the secret number.
			secret = this.Mix(secret * 2048, secret);
			secret = this.prune(secret);

			return secret;
		}

		long Mix(long number, long secret)
		{
			return number ^ secret;
		}
		long prune(long number)
		{
			return number % 16777216; //2^24
		}
		public long Second()
		{
			var secrets = this.ParseInput(Part.Two);
			var diffDictionary = new List<Dictionary<string, int>>();

			foreach (var secret in secrets)
			{
				var d = new Dictionary<string, int>();
				int lastValue = (int)secret % 10;
				var number = secret;

				int v1 = 0, v2 = 0, v3 = 0, v4;
				for (int i = 0; i < 2000; i++)
				{
					number = this.calculate(number);
					int bananas = (int)number % 10;

					v4 = v3;
					v3 = v2;
					v2 = v1;
					v1 = bananas-lastValue;
					lastValue = bananas;

					if(i>3)
					{
						var key = string.Join("",v4, v3, v2, v1);
						d.TryAdd(key, bananas);
					}
				}
				diffDictionary.Add(d);
			}

			var result = 0L;
			var tries = 0;
			var runs = 0L;

			//130321 tries standard
			//40951 optimized tries took 4 min
			//added diffDictionary and got down to 11 sek
			for (int i0 = -9;i0 <10; i0++)
			{
				for (int i1 = -9; i1 < 10; i1++)
				{
					if (this.OutsideBounds(i0,i1,0,0)) { continue; }

					for (int i2 = -9; i2 < 10; i2++)
					{

						if(this.OutsideBounds(i0,i1,i2,0)) { continue; }

						for (int i3 = -9; i3 < 10; i3++)
						{
							if (this.OutsideBounds(i0,i1,i2,i3)) { continue; }

							tries++;
							var localResult = 0L;
							var key = string.Join("", i0, i1, i2, i3);
							foreach (var d in diffDictionary)
							{
								runs++;
								if(d.TryGetValue(key, out int r))
								{
									localResult += r;
								}
							}
							result = Math.Max(result, localResult);
						}
					}
				}
			}
			Console.WriteLine("Tries:" + tries);
			Console.WriteLine("Runs:" + runs);
			return result;
		}
		bool OutsideBounds(int a, int b, int c, int d)
		{
			if (Math.Abs(a + b) > 9) { return true; }
			if (Math.Abs(b + c) > 9) { return true; }
			if (Math.Abs(c + d) > 9) { return true; }
			
			if (Math.Abs(a + b + c) > 9) { return true; }
			if (Math.Abs(b + c + d) > 9) { return true; }
			if (Math.Abs(a + b + c + d) > 9) { return true; }


			return false;
		}



		private List<long> ParseInput(Part part = Part.One)
		{
			if(this.IsExample && part == Part.Two){ return [1, 2, 3, 2024]; }
			var allLines = File.ReadAllLines(this.path);
			var list = new List<long>();

			foreach (var line in allLines)
			{
				list.Add(long.Parse(line));
			}

			return list;
		}
	}
}
