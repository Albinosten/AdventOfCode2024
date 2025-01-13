using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
	internal class Day23 : IPuzzle<int, string>
	{
		public bool IsExample { get; set; }
		public int FirstResult => this.IsExample ? 7 : 1304;

		public string SecondResult => this.IsExample ? "co,de,ka,ta" : "ao,es,fe,if,in,io,ky,qq,rd,rn,rv,vc,vl";

		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";

		public int First()
		{
			return this.Solve()
				.Where(x => x.Split(',').Any(a => a.StartsWith('t')))
				.Count();
		}

		public string Second()
		{
			return this.Solve(Part.Two).OrderByDescending(x => x.Length).FirstOrDefault() ?? "";
		}
		public List<string> Solve(Part part = Part.One)
		{
			var connections = this.ParseInput();

			var map = new MultiValueDictionary<string, string>();
			foreach (var connection in connections)
			{
				map.Add(connection.Item1, connection.Item2);
				map.Add(connection.Item2, connection.Item1);
			}

			var groupings = new List<string>();
			foreach (var key in map.Keys)
			{
				var values = new List<List<string>>();
				var computers = map.Get(key);

				if (part == Part.One) 
				{
					values = computers.GetPermutations(2);
				}
				else
				{
					values = computers.GetVariations();
				}
				
				foreach (var value in values)
				{
					value.Add(key);

					var shouldAdd = true;
					for (var i = 0; i < value.Count; i++)
					{
						var temp = value.ToList();
						temp.Remove(value[i]);
						if (!temp.All(x => map.Get(value[i]).Contains(x)))
						{
							shouldAdd = false;
						}
					}

					if (shouldAdd)
					{
						groupings.Add(string.Join(',', value.OrderBy(x => x)));
					}

				}
			}
			return groupings.DistinctList();
		}
		private List<(string,string)> ParseInput()
		{
			var allLines = File.ReadAllLines(this.path);
			var list = new List<(string,string)>();

			foreach (var line in allLines)
			{
				var numbers = line.Split('-');
				list.Add((numbers[0], numbers[1]));
			}

			return list;
		}
	}
}
