using AdventOfCode.Interfaces;

namespace AdventOfCode.Models
{
	public record Coordinate<T>
		: ICoordinate<T>
		where T : struct, IComparable<T>, IEquatable<T>
	{
		#region Constructors

		public Coordinate(T x, T y)
		{
			X = x;
			Y = y;
		}

		#endregion

		#region ICoordinate implementation

		/// <inheritdoc/>
		public T X { get; init; }

		/// <inheritdoc/>
		public T Y { get; init; }

		#endregion
	}
}