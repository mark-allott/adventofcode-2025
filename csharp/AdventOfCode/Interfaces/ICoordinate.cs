namespace AdventOfCode.Interfaces
{
	public interface ICoordinate<out T>
		where T : struct, IComparable<T>, IEquatable<T>
	{
		/// <summary>
		/// X-coord in a graph
		/// </summary>
		T X { get; }

		/// <summary>
		/// Y-coord in a graph
		/// </summary>
		T Y { get; }
	}
}