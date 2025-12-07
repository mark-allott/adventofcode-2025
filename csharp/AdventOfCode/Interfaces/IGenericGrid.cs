namespace AdventOfCode.Interfaces
{
	internal interface IGenericGrid<TCell, TState, in TCoord, TRange>
		where TCell : class, ICell<TState, TCoord>
		where TState : struct
		where TCoord : class, ICoordinate<TRange>
		where TRange : struct, IComparable<TRange>, IEquatable<TRange>
	{
		/// <summary>
		/// Indexer property to access the underlying cell at the coordinates
		/// </summary>
		/// <param name="x">The x-axis coordinate</param>
		/// <param name="y">The y-axis coordinate</param>
		TCell this[int x, int y] { get; set; }

		/// <summary>
		/// Indexer property to access the underlying cell at the specified coordinate
		/// </summary>
		/// <param name="coord">The coordinate of the cell</param>
		TCell this[TCoord coord] { get; set; }
	}
}