namespace AdventOfCode.Interfaces
{
	public interface ICell<out TCell, out TCoord>
	{
		/// <summary>
		/// The location of the cell within the parent entity
		/// </summary>
		TCoord Location { get; }

		/// <summary>
		/// The cell's state
		/// </summary>
		TCell State { get; }
	}
}