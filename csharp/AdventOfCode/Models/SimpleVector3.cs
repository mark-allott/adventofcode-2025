namespace AdventOfCode.Models
{
	/// <summary>
	/// A simplified version of the <see cref="System.Numerics.Vector3"/> class, specifically for long values and
	/// returning the Euclidean distance as its squared value
	/// </summary>
	public record SimpleLongVector3
	{
		/// <summary>
		/// The X co-ord in 3D space
		/// </summary>
		public long X { get; init; }

		/// <summary>
		/// The Y co-ord in 3D space
		/// </summary>
		public long Y { get; init; }

		/// <summary>
		/// The Z co-ord in 3D space
		/// </summary>
		public long Z { get; init; }

		/// <summary>
		/// Constructor that accepts the co-ordinates for the <paramref name="x"/>, <paramref name="y"/> and <paramref name="z"/> values of the vector
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public SimpleLongVector3(long x, long y, long z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>
		/// Calculates the distance squared value for the Euclidean distance between two vectors
		/// </summary>
		/// <param name="a">The first vector location</param>
		/// <param name="b">The second vector location</param>
		/// <returns>The squared distance between the points</returns>
		public static long Distance2(SimpleLongVector3 a, SimpleLongVector3 b)
		{
			var dx = Math.Abs(a.X - b.X);
			var dy = Math.Abs(a.Y - b.Y);
			var dz = Math.Abs(a.Z - b.Z);
			var x2 = dx * dx;
			var y2 = dy * dy;
			var z2 = dz * dz;
			return x2 + y2 + z2;
		}
	}
}