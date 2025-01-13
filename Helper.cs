using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
	public enum Part
	{
		One,
		Two,
	}
	public enum Base
	{
		Two,
		Three,
		Ten,
	}
	public interface Clonable<T>
	{
		T Clone();
	}
	public enum Direction { Up, Down, Left, Right }

	public class Helper
	{
		
		public static string ToBase(long value, Base toBase) => toBase switch
		{
			Base.Two => IntToString(value, ['0', '1']),
			Base.Three => IntToString(value, ['0', '1', '2']),
			Base.Ten => value.ToString(),
			_ => throw new NotImplementedException(),
		};
		public static string ToBase(int value, Base toBase) => toBase switch
		{
			Base.Two => IntToString(value, ['0', '1']),
			Base.Three => IntToString(value, ['0', '1', '2']),
			Base.Ten => value.ToString(),
			_ => throw new NotImplementedException(),
		};
		public static long FromBase8ToBase10(string octalNumber)
		{
			long decimalNumber = 0;

			// Iterate through each digit of the octal number
			for (int i = 0; i < octalNumber.Length; i++)
			{
				// Convert the character to a digit (0-7)
				int digit = octalNumber[i] - '0';

				// Validate if the digit is valid for base 8
				if (digit < 0 || digit > 7)
				{
					throw new ArgumentException("Invalid octal number: Contains digits outside the range 0-7.");
				}

				// Calculate the power of 8 for the current position
				int position = octalNumber.Length - 1 - i;
				decimalNumber += digit * (long)Math.Pow(8, position);
			}

			return decimalNumber;
		}
		private static string IntToString(int value, char[] baseChars)
		{
			// 32 is the worst cast buffer size for base 2 and int.MaxValue
			int i = 32;
			char[] buffer = new char[i];
			int targetBase = baseChars.Length;

			do
			{
				buffer[--i] = baseChars[value % targetBase];
				value = value / targetBase;
			}
			while (value > 0);

			char[] result = new char[32 - i];
			Array.Copy(buffer, i, result, 0, 32 - i);

			return new string(result);
		}
		private static string IntToString(long value, char[] baseChars)
		{
			int i = 64;
			char[] buffer = new char[i];
			int targetBase = baseChars.Length;

			do
			{
				buffer[--i] = baseChars[value % targetBase];
				value = value / targetBase;
			}
			while (value > 0);

			char[] result = new char[64 - i];
			Array.Copy(buffer, i, result, 0, 64 - i);

			return new string(result);
		}
		public static bool ListsAreEqual<T>(List<T> a, List<T> b, int subCount = int.MaxValue)
		{
			if (a.Count != b.Count
			&& a.Count < subCount
			&& b.Count < subCount
			) { return false; }
			{
				for (int i = 0; i < Math.Min( a.Count,subCount); i++)
				{
					if (!a[i].Equals(b[i]))
					//if (!EqualityComparer<T>.Default.Equals(a[i], b[i]))
					{
						return false;
					}
				}
			}
			return true;
		}
		public static bool ListsAreEqualFromEnd<T>(List<T> a, List<T> b, int subCount = int.MaxValue)
		{
			if (a.Count != b.Count
			&& a.Count < subCount
			&& b.Count < subCount
			) { return false; }
			{
				for (int i = 1; i < Math.Min(b.Count, Math.Min(a.Count,subCount)); i++)
				{
					if (!a[a.Count-i].Equals(b[b.Count- i]))
					//if (!EqualityComparer<T>.Default.Equals(a[i], b[i]))
					{
						return false;
					}
				}
			}
			return true;
		}
		public static Direction[] GetAllDirections() =>
		[	
			Direction.Up,
			Direction.Down,
			Direction.Left,
			Direction.Right,
		];
		public static bool WithinBounds<T>((int x, int y) pos, List<List<T>> map)
		{
			//return WithinBounds(pos, (map.Count, map[0].Count));
			return WithinBounds(pos, (map[0].Count, map.Count));
		}
		public static int GetManhattanDistance((int x, int y) pos1, (int x, int y) pos2)
		{
			return Math.Abs(pos1.x - pos2.x) + Math.Abs(pos1.y - pos2.y);
		}
		public static bool WithinBounds((int x, int y) pos, (int x, int y) limit)
		{
			return pos.x >= 0
				&& pos.y >= 0
				&& pos.y < limit.y
				&& pos.x < limit.x;
		}
		public static (int x, int y) GetNextPosition(Direction direction, (int x, int y) position) => direction switch
		{
			Direction.Up => (position.x, position.y - 1),
			Direction.Down => (position.x, position.y + 1),
			Direction.Left => (position.x - 1, position.y),
			Direction.Right => (position.x + 1, position.y),
			_ => throw new InvalidOperationException(),
		};

		public static void PrintMap(List<List<bool>> map, List<(int x, int y)> pos)
		{
			Console.Clear();
			for (var y = 0; y < map.Count; y++)
			{
				for (var x = 0; x < map[y].Count(); x++)
				{
					if (pos.Contains((x, y)))
					{
						Console.Write('O');
					}
					else{

						var value = map[y][x];
						Console.Write(value ? '#' : '.');
					}
				}
				Console.WriteLine();
			}
			Console.WriteLine();
		}
		public static void PrintMap(List<List<char>> map, List<(int x, int y)> pos)
		{
			for (var y = 0; y < map.Count; y++)
			{
				for (var x = 0; x < map[y].Count(); x++)
				{
					var value = map[y][x];
					if (pos.Contains((x, y)))
					{
						Console.Write('*');

					}
					else if(value == '.')
					{
						Console.Write(' ');
					}
					//else if (value == '#')
					//{
					//	Console.Write(' ');
					//}
					else
					{
						Console.Write(value);
						//Console.Write(' ');
					}
				}
				Console.WriteLine();
			}
		}
		public static void PrintMap<T>(List<List<T>> map)
		{
			for (var y = 0; y < map.Count; y++)
			{
				for (var x = 0; x < map[y].Count(); x++)
				{
					Console.Write(map[y][x]);
				}
				Console.WriteLine();
			}
		}
		public static Dictionary<(int x, int y), int> InitalizeShortestPathVector(int xMax, int yMax)
		{
			var v = new Dictionary<(int x, int y), int>();
			for (int y = 0; y < yMax + 1; y++)
			{
				for (int x = 0; x < xMax + 1; x++)
				{
					v.Add((x, y), int.MaxValue);
				}
			}
			return v;
		}
		public static List<string> CartesianProduct(List<List<string>> lists)
		{
			List<string> result = [""];
			foreach (var list in lists)
			{
				result = result.SelectMany(prefix => list.Select(item => prefix + item)).ToList();
			}
			return result;
		}
		
	}
	class MultiValueDictionary<TKey, TValue>  // no (collection) base class
	{
		private readonly bool prune;
		private Dictionary<TKey, List<TValue>> _data = new Dictionary<TKey, List<TValue>>();
		public MultiValueDictionary(bool prune = false)
		{
			this.prune = prune;
		}
		public IEnumerable<TKey> Keys => _data.Keys;
		public void Add(TKey k, TValue v)
		{
			if (_data.ContainsKey(k))
				_data[k].Add(v);
			else
				_data.Add(k, [v]);
		}
		public bool ContainsKey(TKey k)
		{
			return _data.ContainsKey(k);
		}
		public List<TValue> Get(TKey k)
		{
			if(this.prune){
				var temp = _data[k].DistinctList();
				_data.Remove(k);
				_data.Add(k, temp);
			}

			return _data[k];
		}
	}
}
