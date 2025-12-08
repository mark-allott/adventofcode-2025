namespace AdventOfCode.Models
{
	public record GridCoordinate
		: Coordinate<int>
	{
		public GridCoordinate(int x, int y)
			: base(x, y)
		{
			ArgumentOutOfRangeException.ThrowIfNegative(x);
			ArgumentOutOfRangeException.ThrowIfNegative(y);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(x, 1000);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(y, 1000);
		}
	}
}