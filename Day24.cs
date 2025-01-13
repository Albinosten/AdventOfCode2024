
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode2024
{
	internal class Day24 : IPuzzle<long,string>
	{
		public bool IsExample { get; set; }
		public long FirstResult => this.IsExample ? 2024 : 63168299811048;

		public string SecondResult => this.IsExample ? "" : "";

		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";
		public long First()
		{
			var gates = this.ParseInput();
			return this.GetValueFromGates(this.Filter(gates,'z'));
		}

		public string Second() 
		{
			if(this.IsExample){ return this.SecondResult; }
			var gates = this.ParseInput();
			var xvalue = this.GetValueFromGates(this.Filter(gates, 'x'));
			var yvalue = this.GetValueFromGates(this.Filter(gates, 'y'));

			var gatesDictionary = gates.ToDictionary(x => x.Name);

			var shouldBe = xvalue + yvalue;
			var faultyZGates = gates
				.Where(x => x.Name[0] == 'z')
				.Where(x => x.Type != Types.XOR)
				.Take(3)
				.ToList();
			var zGates = gates
				.Where(x => x.Name[0] == 'z')
				.ToList();
			var gatesWithXORAndNotInZ = gates
				.Where(x => x.Type == Types.XOR)
				.Where(x => x.Name[0] != 'z')
				.ToList();
			var gatesNotInZ = gates
				.Where(x => x.Name[0] != 'z')
				.ToList();


			var a = this.TryGate(gatesDictionary["z45"], gatesDictionary);


			var result = this.Solve(gates, [],0);
			var keys = result.Select(x => GetKey(x)).OrderBy(x => x).ToList();


			return keys.FirstOrDefault() ?? "";
		}

		List<List<string>> Solve(List<Gate> gates, HashSet<string> swapKey, int zMin)
		{
			tries++;
			if(tries % 1 == 0){ Console.WriteLine(tries); }
			var gatesDictionary = gates.ToDictionary(x => x.Name);
			var zGates = this.Filter(gates, 'z');
			//var gatesWithoutCorrectZ = gates
			//	.Where(x => !(x.Name[0] == 'z' && x.Type == Types.XOR)
			//	).ToList();

			if (this.CheckUpToNumber(zGates.Max(x => x.Number), gatesDictionary))
			{
				Console.WriteLine("Key found: " + this.GetKey(swapKey.ToList()));
				return [swapKey.ToList()];
			}
			if (swapKey.Count >= 8){ return []; }

			var result = new List<List<string>>();
			for (var z = 0; z < zGates.Count; z++)
			{
				var zGate = zGates[z];
				if (!this.TryGate(zGate, gatesDictionary))
				{
					Console.WriteLine("z: " + z);
					//swap
					for(int i0 = 0; i0 < gates.Count; i0++)
					{
						for (int i1 = i0; i1 < gates.Count; i1++)
						{
							var clones = this.Clone(gates);
							var clonesDictionary = clones.ToDictionary(x => x.Name);
							
							if(clones[i0].Name == "z08" && clones[i1].Name == "ffj")
							{
							}
							
							this.swap(clones[i0], clones[i1]);
							//CheckUpToNumber
							if (this.CheckUpToNumber((z+1), clonesDictionary)
								&& !swapKey.Contains(clones[i0].Name)
								&& !swapKey.Contains(clones[i1].Name)
								&& clones[i0].Name != clones[i1].Name

							)
							{
								//if okey keep on going untill done;
								var newSwapKey = swapKey.ToHashSet();
								newSwapKey.Add(clones[i0].Name);
								newSwapKey.Add(clones[i1].Name);
								result.AddRange(this.Solve(clones, newSwapKey,z+1));
							}
						}
					}
				}
			}

			if(this.CheckUpToNumber(zGates.Max(x => x.Number), gatesDictionary))
			{
				result.Add(swapKey.ToList()); 
			}

			return result;

		}





		//run to make sure swaping dont destroy anything before
		bool CheckUpToNumber(int number, Dictionary<string, Gate> gatesDictionary)
		{
			var zGates = this.Filter(gatesDictionary.Values.ToList(), 'z').OrderBy(x => x.Name).ToList();
			for (var z = 0; z <= number; z++)
			{
				var zGate = zGates[z];
				if (!this.TryGate(zGate, gatesDictionary))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// returns true if inputgates Xnumber, Ynumber, Xnumber-1, Ynumber-1 all impact gate correct.
		/// Assume input gate is Z gate
		/// </summary>
		/// <param name="gate"></param>
		/// <param name="allGAtes"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		bool TryGate(Gate gate, Dictionary<string, Gate> allGAtes)
		{
			if(gate.Number < 45)
			{

				var xGate = allGAtes[$"x{gate.Number.ToString().PadLeft(2, '0')}"];
				var yGate = allGAtes[$"y{gate.Number.ToString().PadLeft(2, '0')}"];
				if(gate.Number > 0)
				{
					var xGate1 = allGAtes[$"x{(gate.Number-1).ToString().PadLeft(2, '0')}"];
					var yGate1 = allGAtes[$"y{(gate.Number-1).ToString().PadLeft(2, '0')}"];

					return this.TryGate(xGate, yGate, xGate1, yGate1, gate);
				}

				return this.TryGate(xGate, yGate, gate);
			}
			var xGate_1 = allGAtes[$"x{(gate.Number - 1).ToString().PadLeft(2, '0')}"];
			var yGate_1 = allGAtes[$"y{(gate.Number - 1).ToString().PadLeft(2, '0')}"];

			return this.TryAndGate(xGate_1, yGate_1, gate);
		}
		bool TryGate(Gate a, Gate b,Gate c1,Gate c2, Gate s)
		{
			foreach(var variant in this.GetTruthTable)
			{
				a.SetValue(variant.a);
				b.SetValue(variant.b);
				c1.SetValue(variant.c);
				c2.SetValue(variant.c);
				if(s.GetValue() != variant.s)
				{
					return false;
				}
			}
			return true;
		}

		bool TryAndGate(Gate a, Gate b, Gate s)
		{
			var variants = new List<(bool a, bool b, bool s)>()
			{
				(false,false,false),
				(true,false,false),
				(false,true,false),
				(true,true,true),
			};
			foreach (var variant in variants)
			{
				a.SetValue(variant.a);
				if (a.GetValue() != variant.a)
				{
				}
				b.SetValue(variant.b);
				if (b.GetValue() != variant.b)
				{
				}
				if (s.GetValue() != variant.s)
				{
					return false;
				}
			}
			return true;
		}
		bool TryGate(Gate a, Gate b, Gate s)
		{
			var variants = new List<(bool a, bool b, bool s)>()
			{
				(false,false,false),
				(true,false,true),
				(false,true,true),
				(true,true,false),
			};
			foreach (var variant in variants)
			{
				a.SetValue(variant.a);
				if(a.GetValue() != variant.a)
				{
				}
				b.SetValue(variant.b);
				if (b.GetValue() != variant.b)
				{
				}
				if (s.GetValue() != variant.s)
				{
					return false;
				}
			}
			return true;
		}
		List<(bool a, bool b, bool c, bool s)> GetTruthTable =>
		[
			(false,false,false,false),
			(false,false,true,true),
			(false,true,false,true),
			(false,true,true,false),

			(true,false,false,true),
			(true,false,true,false),
			(true,true,false,false),
			(true,true,true,true),
		];


		/*public string Second()
		{
			if(this.IsExample){ return  this.SecondResult; }
			var gates = this.ParseInput();
			var xvalue = this.GetValueFromGates(this.Filter(gates, 'x'));
			var yvalue = this.GetValueFromGates(this.Filter(gates, 'y'));
			
			var gatesDictionary = gates.ToDictionary(x => x.Name);


			var shouldBe = xvalue + yvalue;
			var faultyZGates = gates
				.Where(x => x.Name[0] == 'z')
				.Where(x => x.Type != Types.XOR)
				.Take(3)
				.ToList();
			var gatesWithXORAndNotInZ = gates
				.Where(x => x.Type == Types.XOR)
				.Where(x => x.Name[0] != 'z')
				.ToList();
			var gatesNotInZ = gates
				.Where(x => x.Name[0] != 'z')
				.ToList();


			//WORKS
			//var valueBeforeSwap = this.GetValueFromGates(this.Filter(gates, 'z'));
			//var z08 = gatesDictionary["z08"];
			//var ffj = gatesDictionary["ffj"];
			//this.swap(z08, ffj);
			//var newValue = this.GetValueFromGates(this.Filter(gates, 'z'));
			

			//WORKS
			var valueBeforeSwap = this.GetValueFromGates(this.Filter(gates, 'z'));
			var z08 = gatesDictionary["z08"];
			var ffj = gatesDictionary["ffj"];
			var _clones = this.Clone(gates).ToDictionary(x => x.Name);
			this.swap(_clones[z08.Name], _clones[ffj.Name]);

			var newValue = this.GetValueFromGates(this.Filter(_clones.Values.ToList(), 'z'));
			


			//38 sek med gatesWithXORAndNotInZ.Count ^ 3
			//30 sek genom att ta bort clonen
			//28 sek genom att skapa clonesDictionaryn 1 gång
			var clones = gates.ToDictionary(x => x.Name);
			var result = new List<string>();
			for (var i0 = 0; i0 < gatesWithXORAndNotInZ.Count; i0++)
			{
				for (var i1 = 0; i1 < gatesWithXORAndNotInZ.Count; i1++)
				{
					for (var i2 = 0; i2 < gatesWithXORAndNotInZ.Count; i2++)
					{
						for (var g1 = 0; g1< gatesNotInZ.Count; g1++)
						{
							for (var g2 = 0; g2 < gatesNotInZ.Count; g2++)
							{
								tries++;
								var z0 = faultyZGates[0];
								var n0 = gatesWithXORAndNotInZ[i0];
								this.swap(clones[z0.Name], clones[n0.Name]);

								var z1 = faultyZGates[1];
								var n1 = gatesWithXORAndNotInZ[i1];
								this.swap(clones[z1.Name], clones[n1.Name]);

								var z2 = faultyZGates[2];
								var n2 = gatesWithXORAndNotInZ[i2];
								this.swap(clones[z2.Name], clones[n2.Name]);

								var z3 = gatesNotInZ[g1];
								var n3 = gatesNotInZ[g2];
								this.swap(clones[z3.Name], clones[n3.Name]);

								this.ResetGates(clones.Values.ToList());
								var value = this.GetValueFromGates(Filter(clones.Values.ToList(), 'z'));
								if (value == shouldBe)
								{
									var key = this.GetKey([z0, z1, z2, z3, n0, n1, n2, n3]);
									result.Add(key);
									Console.WriteLine("Key found: " + key);
								}
								if (tries % 10000 == 0)
								{
									//7 346 100 188
									var amouts = (Math.Pow(gatesWithXORAndNotInZ.Count, 3) * gatesNotInZ.Count * gatesNotInZ.Count);
									double percent = (tries / amouts) * 100;
									Console.WriteLine(Math.Round(percent, 10) + "%");
								}
								//Reset
								this.swap(clones[z3.Name], clones[n3.Name]);
								this.swap(clones[z2.Name], clones[n2.Name]);
								this.swap(clones[z1.Name], clones[n1.Name]);
								this.swap(clones[z0.Name], clones[n0.Name]);
							}
						}
					}
				}
			}

			Console.WriteLine("Count: "+result.Count);
			Console.WriteLine("Distinct count: "+result.Distinct().Count());
			return result
				.OrderBy(x => x)
				.FirstOrDefault() 
				?? "";
		}*/

		string GetKey(List<Gate> gates)
		{
			return string.Join(",", gates
				.Select(x => x.Name)
				.OrderBy(x => x)
				);
		}
		string GetKey(List<string> gates)
		{
			return string.Join(",", gates
				.OrderBy(x => x)
				);
		}

		long tries = 0;

		List<Gate> Filter(List<Gate>gates, char c)
		{
			return gates.Where(x => x.Name.ToLower().StartsWith(c)).OrderBy(x => x.Name).ToList();
		}
	
		HashSet<string> TopLevels = new HashSet<string>();
		List<Gate> Clone(List<Gate> gatesToClone)
		{
			var result = new Dictionary<string, Gate>();
			foreach (var gate in gatesToClone)
			{
				var newGate = gate.Clone();
				result.Add(newGate.Name, newGate);
			}

			foreach (var newGate in result.Values)
			{
				if(newGate.a != null)
				{
					newGate.SetGates(result[newGate.a.Name], result[newGate.b.Name]);
				}
			}
			return result.Values.ToList();
		}

		long GetValueFromGates(List<Gate> gates)
		{
			gates = gates
				.OrderBy(x => x.Name)
				.ToList();
			var result = 0L;
			for (var i = 0; i < gates.Count; i++)
			{
				gates[i].Executed = false;
				if (gates[i].GetValue())
				{
					result += 1L << i;
				}
			}

			return result;
		}
		void ResetGates(List<Gate> gates)
		{
			for (var i = 0; i < gates.Count; i++)
			{
				gates[i].Executed = false;
			}
		}

		private List<Gate> ParseInput()
		{
			this.TopLevels.Clear();

			var allLines = File.ReadAllLines(this.path);
			var startValues = allLines.TakeWhile(x => x != string.Empty);
			var gates = allLines.TakeLast(allLines.Length - startValues.Count() -1);
			var list = new Dictionary<string, Gate>();

			foreach (var line in gates)
			{
				var p = line.Split(' ');
				var gate = new Gate(GetType(p[1])) 
				{
					Name = p[4],
					IsTopLevel = true,
					
				};
				gate.Number = gate.Name.StartsWith('z') ? int.Parse(string.Join("", p[4].Skip(1).ToList())) : 0;
				list.Add(gate.Name, gate);
				this.TopLevels.Add(gate.Name);
			}

			foreach(var value in startValues)
			{
				var gateName = value.Split(": ");
				var gate = new Gate(Types.None) { Name = gateName[0] };
				gate.SetValue(gateName[1] == "1" ? true : false);

				list.Add(gate.Name, gate);
			}
			foreach (var line in gates)
			{
				var p = line.Split(' ');
				list[p[4]].SetGates(list[p[0]], list[p[2]]);
			}


			return list
				.Values
				.ToList();
		}
		Types GetType(string s) => s switch
		{
			"AND" => Types.And,
			"OR" => Types.OR,
			"XOR" => Types.XOR,
			_ => throw new NotImplementedException()
		};
		
		void swap (Gate a, Gate b)
		{
			var tempA = a.a;
			var tempB = a.b;
			var tempType = a.Type;

			a.SetGates(b.a, b.b);
			a.SetType(b.Type);

			b.SetGates(tempA, tempB);
			b.SetType(tempType);
		}


		enum Types
		{
			None,
			And,
			XOR,
			OR,
		}
		class Gate
		{

			public bool IsTopLevel{ get; set; }
			public string Name { get; set; }
			public int Number { get; set; }
			public Types Type { get; set; }

			public Gate(Types type)
			{
				this.Type = type;
				this.Function = this.function(type);
			}
			public void SetType(Types type)
			{
				this.Type = type;
				this.Function = this.function(type);
			}

			private Gate() { }
			public Gate Clone()
			{
				return new Gate()
				{
					Function = this.Function,
					Name = this.Name,
					IAmClone = true,
					IsTopLevel = this.IsTopLevel,
					Type = this.Type,
					//value = this.value, //kanske?
					Executed = false,
					a = this.a,
					b = this.b,
					Number = this.Number,
				};
			}
			public void SetGates(Gate a, Gate b)
			{
				this.a = a;
				this.b = b;
			}
			public bool Executed { get; set; }

			public bool GetValue(HashSet<string>? visited = null)
			{
				//if (this.Executed) { return value; }
				visited = visited ?? [];
				if(this.a != null && !visited.Contains(this.Name))
				{
					var aVisited = visited.ToHashSet();
					aVisited.Add(this.Name);
					var bVisited = visited.ToHashSet();
					bVisited.Add(this.Name);

					var value = this.Function(this.a.GetValue(aVisited), this.b.GetValue(bVisited));
					this.SetValue(value);
					return value;
				}
				this.SetValue(this.Function(this.value, this.value));
				return this.Function(this.value,this.value);
			}
			Func<bool, bool, bool> function(Types type) => type switch
			{
				Types.And => Gate.And,
				Types.XOR => Gate.XOR,
				Types.OR => Gate.Or,
				Types.None => (bool a, bool b) =>this.None(a,b),
			};
			public void SetValue(bool value)
			{
				this.Executed = true;
				this.value = value;
			}
			public bool IAmClone;
			private bool value;
			public Gate a;
			public Gate b;

			private bool None(bool a, bool b){ return a; }
			public Func<bool,bool,bool> Function { get; set; }
			public static bool And(bool a, bool b)
			{
				return a && b;
			}
			public static bool Or(bool a, bool b)
			{
				return a || b;
			}
			public static bool XOR(bool a, bool b)
			{
				return a ^ b;
			}

		}
	}
}
