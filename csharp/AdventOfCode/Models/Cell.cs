using AdventOfCode.Interfaces;

namespace AdventOfCode.Models
{
	internal record Cell<TState, TCoord, TCoordType>
		: ICell<TState, TCoord>
		where TState : notnull
		where TCoord : ICoordinate<TCoordType>
		where TCoordType : struct, IComparable<TCoordType>, IEquatable<TCoordType>
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		private Cell()
		{
		}

		/// <summary>
		/// Standard constructor
		/// </summary>
		/// <param name="location">The cell location</param>
		/// <param name="state">The cell state</param>
		protected Cell(TCoord location, TState state)
		{
			Location = location;
			State = state;
		}

		#endregion

		#region ICell implementation

		/// <inheritdoc/>
		public TCoord Location { get; init; }

		/// <summary>
		/// The state of the cell - could be an enum value, character, etc.
		/// </summary>
		public TState State { get; set; }

		#endregion
	}
}