namespace AdventOfCode.Models
{
	public record TheatreCoordinate
		: Coordinate<int>
	{
		public TheatreCoordinate(int x, int y)
			: base(x, y)
		{
			ArgumentOutOfRangeException.ThrowIfNegative(x);
			ArgumentOutOfRangeException.ThrowIfNegative(y);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(x, 100000);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(y, 100000);
		}
	}
}