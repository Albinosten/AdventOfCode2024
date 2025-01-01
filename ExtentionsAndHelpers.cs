namespace AdventOfCode2024
{
	public static class ExtentionsAndHelpers
	{
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
