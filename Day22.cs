using System;

namespace AdventOfCode2024
{
	internal class Day22 : IPuzzle<long>
	{
		public bool IsExample { get; set; }
		public long FirstResult => this.IsExample ? 37327623 : 19458130434;

		public long SecondResult => this.IsExample ? 0 : 0;

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
			//var prune_mask = (1 << 24) - 1;
			//var a = secret;
			//a ^= (a << 6) & prune_mask;

			//a ^= (a >> 5);

			//a ^= (a << 11) & prune_mask;

			//return a;

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
			return 0;
		}

		private List<long> ParseInput()
		{
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
