using System.Collections;

namespace AdventOfCode2024
{
	public static class Extentions
	{
		public static List<List<T>> GetPermutations<T>(this IEnumerable<T> list, int length)
		{
			if (length == 1) return list.Select(t => new T[] { t }.ToList()).ToList();

			return GetPermutations(list, length - 1)
				.SelectMany(t => list.Where(e => !t.Contains(e)),
					(t1, t2) => t1.Concat(new T[] { t2 }).ToList()).ToList();
		}
		public static List<List<T>>GetVariations<T>(this List<T> list)
		{
			var result = new List<List<T>>();

			for (int i = 0; i < Math.Pow(2, list.Count); i++)
			{
				var row  = new List<T>();
				BitArray b = new BitArray(new int[] { i });
				bool[] bits = new bool[b.Count];
				b.CopyTo(bits, 0);

				for(int j = 0; j < list.Count; j++)
				{
					if(bits[j])
					{
						row.Add(list[j]);
					}
				}
				result.Add(row);
			}

			
			return result;
		}
		public static IEnumerable<T> TakeWhile<T>(this IEnumerable<T> source, Func<T, bool> predicate, bool inclusive)
		{
			foreach (T item in source)
			{
				if (predicate(item))
				{
					yield return item;
				}
				else
				{
					if (inclusive) yield return item;

					yield break;
				}
			}
		}
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
		public static List<T> CloneList<T>(this List<T> list)
			where T : Clonable<T>
		{
			return list.Select(x => x.Clone()).ToList();
		}
		public static List<T> DistinctList<T>(this List<T> list)
		{
			return list.Distinct().ToList();
		}
		public static List<T> DistinctList<T>(this IEnumerable<T> list)
		{
			return list.Distinct().ToList();
		}


	}
}
