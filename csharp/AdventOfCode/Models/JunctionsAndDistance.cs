namespace AdventOfCode.Models
{
	/// <summary>
	/// A simple record that represents the connection between two junctions and the distance between them
	/// </summary>
	internal record JunctionsAndDistance
	{
		public SimpleLongVector3 PointA { get; init; }
		public SimpleLongVector3 PointB { get; init; }
		public long Distance2 { get; init; }

		public JunctionsAndDistance(SimpleLongVector3 pointA, SimpleLongVector3 pointB)
		{
			PointA = pointA;
			PointB = pointB;
			Distance2 = SimpleLongVector3.Distance2(PointA, PointB);
		}
	}
}