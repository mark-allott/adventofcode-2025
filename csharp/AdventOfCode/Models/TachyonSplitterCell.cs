using AdventOfCode.Enums;
using AdventOfCode.Extensions;

namespace AdventOfCode.Models
{
	internal record TachyonSplitterCell
		: Cell<TachyonSplitterState, GridCoordinate, int>
	{
		public TachyonSplitterCell(int row, int column, char state)
			: base(new GridCoordinate(column, row) { X = column, Y = row }, state.ToState())
		{
		}
	}
}