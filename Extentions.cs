using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
	public static class Extentions
	{
		public static List<T> ExceptElementAt<T>(this List<T> list, int element)
		{
			var result = new List<T>(list);

			result.RemoveAt(element);
			
			return result;
		}
	}
	public enum Part
	{
		One,
		Two,
	}
}
