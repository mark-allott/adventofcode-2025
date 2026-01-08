using AdventOfCode.Models;

namespace AdventOfCode.Extensions
{
	internal class CoordinateExtensions
	{
		public static long AreaEnclosedBy(Coordinate<int> coordA, Coordinate<int> coordB)
		{
			long dx = 1 + Math.Abs(coordA.X - coordB.X);
			long dy = 1 + Math.Abs(coordA.Y - coordB.Y);
			return dx * dy;
		}
	}
}