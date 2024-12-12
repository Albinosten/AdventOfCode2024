using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
	public static class ExtentionsAndHelpers
	{
		public static List<T> ExceptElementAt<T>(this List<T> list, int element)
		{
			var result = new List<T>(list);

			result.RemoveAt(element);

			return result;
		}
		public static (int x, int y) Add(this (int x, int y) first, (int x, int y) second)
		{
			return (first.x + second.x, first.y + second.y);
		}

		public static string ToBase(int value, Base toBase) => toBase switch
		{
			Base.Two => IntToString(value, ['0', '1']),
			Base.Three => IntToString(value, ['0', '1', '2']),
			Base.Ten => value.ToString(),
			_ => throw new NotImplementedException(),
		};
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
	}
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
}
