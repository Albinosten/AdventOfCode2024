using Microsoft.Win32;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Reflection.Emit;

namespace AdventOfCode2024
{
	internal class Day17 : IPuzzle<string,long>
	{
		public bool IsExample { get; set; }
		public string FirstResult => this.IsExample ? "4,6,3,5,6,3,5,2,1,0" : "6,0,6,3,0,2,3,1,6";

		public long SecondResult => this.IsExample ? 117440 : 236539226447469;

		public string path => this.IsExample
			? $"Inputs/{this.GetType().Name}_TEMP.txt"
			: $"Inputs/{this.GetType().Name}.txt";

		public Day17()
		{
			this.Output = new List<int>();
		}
		private long RegisterA { get; set; }
		private long RegisterB { get; set; }
		private long RegisterC { get; set; }
		private List<int> Output{ get; set; }
		private int instructionPointer { get; set; }
		public string First()
		{
			var program = this.ParseInput().program;
			this.Run(program);

			return string.Join(',', this.Output);
		}


		//more than 68904704
		//402709344563600 correkt?
		public long Second()
		{
			var (program,ra,rb,rc ) = this.ParseInput(Part.Two);

			if (this.IsExample) 
			{
				//For the example i could convert the program from base 8 to 10 to get the correct value. but that only worked for my example.
				var reverse = program.ToList();
				reverse.Reverse();
				var base8String = string.Join("", reverse).TrimStart('0');
				base8String += "0";
				return Helper.FromBase8ToBase10(base8String);
			}

			var min = (long)Math.Pow(8, 15);
			var max = (long)Math.Pow(8, 16);

			var diff = max - min;
			var stepps = 1000000;
			var count = diff / stepps;

			var compareNumber = 7;

			var span = this.GetSpan(min, max, stepps,compareNumber);

			while(span.last - span.first > Math.Pow(10, 7))
			{
				span = this.GetSpan(span.first, span.last, stepps, ++compareNumber);
			}

			for (long i = span.first; i <= span.last; i++)
			{
				this.Output.Clear();
				this.RegisterA = (long)i;
				this.RegisterB = rb;
				this.RegisterC = rc;
				this.Run(program);
				if (Helper.ListsAreEqual(program, this.Output))
				{
					return (long)i;
				}
			}

			throw new Exception("No match found");
		}

		/// <summary>
		/// finds start-stop values where the program and output matches for giving length.
		/// return new start-stop values that are more correct
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="stepps"></param>
		/// <param name="compareNumber"></param>
		/// <returns></returns>
		private (long first, long last) GetSpan(long min, long max, long stepps, int compareNumber)
		{
			var (program, ra, rb, rc) = this.ParseInput(Part.Two);

			var diff = max - min;
			var count = diff / stepps;

			long first = long.MaxValue;
			long last = 0L;

			for (long i = min; i < max; i += count)
			{
				//var i = min + j;
				this.Output.Clear();
				this.RegisterA = (long)i;
				this.RegisterB = rb;
				this.RegisterC = rc;
				this.Run(program);

				var length = program.Count();
				if (this.Compare(this.Output, program, Math.Min(length-2, compareNumber)))
				{
					first = (long)Math.Min(i, first);
					last = (long)Math.Max(i, last);
				}
			}

			return (first-count, last+count);
		}
		public bool Compare<T>(List<T> a, List<T> b, int count)
		{
			if (a.Count != b.Count) { return false; }
			{
				for (int i = (a.Count-1); i > (a.Count - 1) - Math.Min(a.Count, count); i--)
				{
					if (!a[i].Equals(b[i]))
					{
						return false;
					}
				}
			}
			return true;
		}
		private void Run(List<int> instructions)
		{
			this.instructionPointer = 0;
			while (this.instructionPointer < instructions.Count)
			{
				var instructionNumber = instructions[this.instructionPointer];
				var instruction = this.GetInstruction(instructionNumber);

				this.instructionPointer++;
				var instructionValue = instructions[this.instructionPointer];

				this.instructionPointer++;
				instruction(instructionValue);

			}
		}
		void Case1()
		{
			this.RegisterA = 0;
			this.RegisterB = 0;
			this.RegisterC = 9;
			this.Output.Clear();



			//var instruction = this.GetInstruction(2);
			//instruction(6);

			this.Run([2, 6]);

			if(this.RegisterA != 0
			&& this.RegisterB != 1
			&& this.RegisterC != 9
			)
			{
				throw new InvalidDataException();
			}
		}
		void Case2()
		{
			this.RegisterA = 10;
			this.RegisterB = 0;
			this.RegisterC = 0;
			this.Output.Clear();

			//var instruction = this.GetInstruction(5);
			//instruction(0);
			//instruction = this.GetInstruction(5);
			//instruction(1);
			//instruction = this.GetInstruction(5);
			//instruction(4);
			this.Run([5, 0, 5, 1, 5, 4]);

			if (this.Output[0] != 0
			&& this.Output[1] != 1
			&& this.Output[2] != 2
			)
			{
				throw new InvalidDataException();
			}
		}
		void Case3()
		{
			this.RegisterA = 2024;
			this.RegisterB = 0;
			this.RegisterC = 0;
			this.Output.Clear();

			//var instruction = this.GetInstruction(0);
			//instruction(1);
			//instruction = this.GetInstruction(5);
			//instruction(4);
			//instruction = this.GetInstruction(3);
			//instruction(0);
			this.Run([0, 1, 5, 4, 3, 0]);

			//4,2,5,6,7,7,7,7,3,1,0
			if (this.RegisterA != 0
			&& this.Output[0] != 4
			&& this.Output[1] != 2
			&& this.Output[2] != 5
			&& this.Output[3] != 6
			&& this.Output[4] != 7
			&& this.Output[5] != 7
			&& this.Output[6] != 7
			&& this.Output[7] != 7
			&& this.Output[8] != 3
			&& this.Output[9] != 1
			&& this.Output[10] != 0
			)
			{
				throw new InvalidDataException();
			}
		}
		void Case4()
		{
			this.RegisterA = 0;
			this.RegisterB = 29;
			this.RegisterC = 0;
			this.Output.Clear();

			//var instruction = this.GetInstruction(1);
			//instruction(7);
			this.Run([1,7]);

			if (this.RegisterB != 26)
			{
				throw new InvalidDataException();
			}
		}
		void Case5()
		{
			this.RegisterA = 0;
			this.RegisterB = 2024;
			this.RegisterC = 43690;
			this.Output.Clear();

			//var instruction = this.GetInstruction(4);
			//instruction(0);
			this.Run([4,0]);

			if (this.RegisterB != 44354)
			{
				throw new InvalidDataException();
			}
		}
		Action<long> GetInstruction(int i) => i switch
		{
			0 => value => this.adv(value),
			1 => value => this.bxl(value),
			2 => value => this.bst(value),
			3 => value => this.jnz(value),
			4 => value => this.bxc(value),
			5 => value => this.out_(value),
			6 => value => this.bdv(value),
			7 => value => this.cdv(value),
			_ => throw new InvalidOperationException(),
		};
		long GetComboValue(long i) => i switch
		{
			0 => 0,
			1 => 1,
			2 => 2,
			3 => 3,
			4 => this.RegisterA,
			5 => this.RegisterB, 
			6 => this.RegisterC,
			_ => throw new InvalidOperationException(),
		};


		/// <summary>
		/// The adv instruction (opcode 0) performs division. 
		/// The numerator is the value in the A register. 
		/// The denominator is found by raising 2 to the power of the instruction's combo operand.
		/// (So, an operand of 2 would divide A by 4 (2^2); an operand of 5 would divide A by 2^B.) 
		/// The result of the division operation is truncated to an integer and then written to the A register.
		/// </summary>
		/// <param name="value"></param>
		void adv(long value)//0
		{
			long numerator = this.RegisterA;
			long denominator = (long) Math.Pow(2, GetComboValue(value));
			long v = (numerator / denominator);
			this.RegisterA = (long)v;
		}
		/// <summary>
		/// The bxl instruction (opcode 1) calculates the bitwise XOR of register B and the instruction's literal operand, then stores the result in register B.
		/// </summary>
		/// <param name="value"></param>
		void bxl(long value)//1
		{
			var rB = this.RegisterB;
			long v = rB ^ value;
			this.RegisterB = v;
		}

		/// <summary>
		///The bst instruction(opcode 2) calculates the value of its combo operand modulo 8 
		///(thereby keeping only its lowest 3 bits), then writes that value to the B register.
		/// </summary>
		/// <param name="value"></param>
		void bst(long value)//2
		{
			var comboValue = this.GetComboValue(value);
			var v = comboValue % 8;
			this.RegisterB = v;
		}
		/// <summary>
		/// The jnz instruction (opcode 3) does nothing if the A register is 0.
		/// However, if the A register is not zero, it jumps by setting the instruction pointer to the value of its literal operand; 
		/// if this instruction jumps, the instruction pointer is not increased by 2 after this instruction.
		/// </summary>
		/// <param name="value"></param>
		void jnz(long value)//3
		{
			if(this.RegisterA == 0){ return; }

			this.instructionPointer = (int)value;
		}
		/// <summary>
		/// The bxc instruction (opcode 4) calculates the bitwise XOR of register B and register C, then stores the result in register B. (For legacy reasons, this instruction reads an operand but ignores it.)
		/// </summary>
		/// <param name="value"></param>
		void bxc(long value)//4
		{
			var rB = this.RegisterB;
			var rC = this.RegisterC;
			long v = rB ^ rC;
			this.RegisterB = v;
		}

		/// <summary>
		/// 		/// The out instruction (opcode 5) calculates the value of its combo operand modulo 8, then outputs that value.
		/// (If a program outputs multiple values, they are separated by commas.)
		/// </summary>
		/// <param name="value"></param>
		void out_(long value)//5
		{
			var comboValue = this.GetComboValue(value);
			long v = comboValue % 8;
			this.Output.Add((int)v);

		}

		/// <summary>
		/// The bdv instruction (opcode 6) works exactly like the adv instruction except that the result is stored in the B register. (The numerator is still read from the A register.)
		/// </summary>
		/// <param name="value"></param>
		void bdv(long value)//6
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// The cdv instruction (opcode 7) works exactly like the adv instruction except that the result is stored in the C register. (The numerator is still read from the A register.)
		/// </summary>
		/// <param name="value"></param>
		void cdv(long value)//7
		{
			long numerator = this.RegisterA;
			long denominator = (long) Math.Pow(2, GetComboValue(value));
			long v = (numerator / denominator);
			this.RegisterC = (long)v;
		}
		

		private (List<int> program, long rA, long rB, long rC) ParseInput(Part part = Part.One)
		{
			var startIndex = (part == Part.Two && this.IsExample) ? 5 : 0;

			var allLines = File.ReadAllLines(this.path);
			this.RegisterA = long.Parse(allLines[startIndex+0].Split(": ")[1]);
			this.RegisterB = long.Parse(allLines[startIndex + 1].Split(": ")[1]);
			this.RegisterC = long.Parse(allLines[startIndex + 2].Split(": ")[1]);
			this.instructionPointer = 0;
			this.Output.Clear();

			var result = new List<int>();
			foreach (var line in allLines[startIndex + 4].Split(": ")[1].Split(','))
			{
				result.Add(int.Parse(line));
			}
			return (result, this.RegisterA, this.RegisterB, this.RegisterC);
		}


	}
}
