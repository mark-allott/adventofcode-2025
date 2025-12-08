using AdventOfCode.Enums;
using AdventOfCode.Extensions;

namespace AdventOfCode.Models
{
	internal record TachyonSplitterGridCell
		: Cell<TachyonSplitterState, GridCoordinate, int>
	{
		public TachyonSplitterGridCell(int column, int row, char state)
			: base(new GridCoordinate(column, row), state.ToState())
		{
		}
	}
}