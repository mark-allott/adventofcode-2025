using AdventOfCode.Enums;

namespace AdventOfCode.Models
{
	internal record TheatreGridCell
		: Cell<TileColour, TheatreCoordinate, int>
	{
		public TheatreGridCell(TheatreCoordinate location, TileColour state)
			: base(location, state)
		{
		}
	}
}