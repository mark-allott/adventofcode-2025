using AdventOfCode.Interfaces;

namespace AdventOfCode.Models
{
	public record Coordinate<T>
		: ICoordinate<T>
		where T : struct, IComparable<T>, IEquatable<T>
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		private Coordinate()
		{
		}

		/// <summary>
		/// Alternate constructor; permits direct setting of coordinate values from constructor, rather than init properties
		/// </summary>
		/// <param name="x">The location in X for the coordinate</param>
		/// <param name="y">The location in Y for the coordinate</param>
		protected Coordinate(T x, T y)
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