using AdventOfCode.Models;

namespace AdventOfCode.Extensions
{
	internal static class CoordinateExtensions
	{
		/// <summary>
		/// Returns the number of cells contained in the rectangular area described by the two points
		/// </summary>
		/// <param name="coordA">Point A</param>
		/// <param name="coordB">Point B</param>
		/// <returns>The number of cells enclosed</returns>
		public static long AreaEnclosedBy(Coordinate<int> coordA, Coordinate<int> coordB)
		{
			long dx = 1 + Math.Abs(coordA.X - coordB.X);
			long dy = 1 + Math.Abs(coordA.Y - coordB.Y);
			return dx * dy;
		}

		/// <summary>
		/// Returns the coordinates enclosed between two points forming a rectangle
		/// </summary>
		/// <param name="coordA">Location 1 for the described rectangle</param>
		/// <param name="coordB">Location 2 for the described rectangle</param>
		/// <returns>All coordinates in the rectangular area</returns>
		public static IEnumerable<TheatreCoordinate> CoordinatesInAreaEnclosedBy(TheatreCoordinate coordA,
			TheatreCoordinate coordB)
		{
			var minX = Math.Min(coordA.X, coordB.X);
			var maxX = Math.Max(coordA.X, coordB.X);
			var minY = Math.Min(coordA.Y, coordB.Y);
			var maxY = Math.Max(coordA.Y, coordB.Y);

			return Enumerable.Range(minY, 1 + (maxY - minY))
				.SelectMany(y => Enumerable.Range(minX, 1 + (maxX - minX)).Select(x => new TheatreCoordinate(x, y)))
				.OrderBy(o => o.Y)
				.ThenBy(o => o.X)
				.ToList();
		}

		/// <summary>
		/// Determines the coordinates in the rectangle described by <paramref name="coordA"/> and <paramref name="coordB"/>,
		/// converting the large number of coordinates into a smaller "range" descriptor for a row of coordinates
		/// </summary>
		/// <param name="coordA">Location 1 for the described rectangle</param>
		/// <param name="coordB">Location 2 for the described rectangle</param>
		/// <returns>A range of coordinates describing the rectangle</returns>
		public static IEnumerable<TheatreCoordinateRange> CoordinateRangesInAreaEnclosedBy(TheatreCoordinate coordA,
			TheatreCoordinate coordB)
		{
			var minX = Math.Min(coordA.X, coordB.X);
			var maxX = Math.Max(coordA.X, coordB.X);
			var minY = Math.Min(coordA.Y, coordB.Y);
			var maxY = Math.Max(coordA.Y, coordB.Y);

			return Enumerable.Range(minY, 1 + (maxY - minY))
				.Select(y => new TheatreCoordinateRange(minX, maxX, y))
				.OrderBy(o => o.Y)
				.ThenBy(o => o.X);
		}
	}
}