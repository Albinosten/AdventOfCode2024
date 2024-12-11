namespace AdventOfCode2024
{

	public interface IPuzzle<T> : IPuzzle<T, T>
	{
	}
	public interface IPuzzle<T, P>
	{
		T First();
		P Second();

		T FirstResult { get; }
		P SecondResult { get; }
		bool IsExample { get; set; }
	}
	public interface IPuzzle : IPuzzle<int, long>
	{
	}
}
