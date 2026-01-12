namespace AdventOfCode.Models
{
	internal record TheatreCoordinateRange
		: TheatreCoordinate
	{
		/// <summary>
		/// The end coordinate of the range for X
		/// </summary>
		public int EndX { get; init; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="startX">The starting X coordinate of the range</param>
		/// <param name="endX">The ending X coordinate of the range</param>
		/// <param name="y">The Y coordinate for the range</param>
		public TheatreCoordinateRange(int startX, int endX, int y)
			: base(startX, y)
		{
			var minX = Math.Min(startX, endX);
			var maxX = Math.Max(startX, endX);
			X = minX;
			EndX = maxX;
		}

		/// <summary>
		/// Alternate constructor
		/// </summary>
		/// <param name="startLocation">The first location in the range</param>
		/// <param name="endLocation">The second location in the range</param>
		public TheatreCoordinateRange(TheatreCoordinate startLocation, TheatreCoordinate endLocation)
			: this(startLocation.X, endLocation.X, startLocation.Y)
		{
			//	Must be on same row
			ArgumentOutOfRangeException.ThrowIfNotEqual(startLocation.Y, endLocation.Y);
		}

		/// <summary>
		/// Determines whether the specified location is contained within this range
		/// </summary>
		/// <param name="location">The location to check</param>
		/// <returns>True if the location is within the range</returns>
		public bool Contains(TheatreCoordinate location)
		{
			return location.Y == Y &&
			       location.X >= X &&
			       location.X <= EndX;
		}

		/// <summary>
		/// Determines whether the specified range is contained within this range
		/// </summary>
		/// <param name="range">The range to check</param>
		/// <returns>True if the range is within this range</returns>
		public bool Contains(TheatreCoordinateRange range)
		{
			return range.Y == Y &&
			       range.X >= X &&
			       range.EndX <= EndX;
		}

#if DEBUG
		/// <summary>
		/// Override the ToString method for debugging purposes.
		/// </summary>
		/// <returns>Coordinates used by the range</returns>
		public override string ToString()
		{
			return $"Y: {Y}, StartX: {X}, EndX: {EndX}";
		}
#endif
	}
}