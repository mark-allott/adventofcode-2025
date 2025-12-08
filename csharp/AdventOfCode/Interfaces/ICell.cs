namespace AdventOfCode.Interfaces
{
	public interface ICell<out TState, out TCoord>
	{
		/// <summary>
		/// The location of the cell within the parent entity
		/// </summary>
		TCoord Location { get; }

		/// <summary>
		/// The cell's state
		/// </summary>
		TState State { get; }
	}
}