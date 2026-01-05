namespace AdventOfCode.Models
{
	internal record PlaygroundCircuit
	{
		/// <summary>
		/// Internal store for individual junction points
		/// </summary>
		private readonly List<SimpleLongVector3> _junctionPoints = [];

		/// <summary>
		/// Determines whether the two junction points can be added to this circuit
		/// </summary>
		/// <param name="pointA">The first junction</param>
		/// <param name="pointB">The second junction</param>
		/// <returns>True if empty, or one junction point can connect with another already in the circuit</returns>
		public bool CanAddJunction(SimpleLongVector3 pointA, SimpleLongVector3 pointB)
		{
			//	If no entries, then we can add both now
			if (_junctionPoints.Count == 0)
				return true;

			//	Check if either of the points is contained in the circuit
			var hasPointA = _junctionPoints.Contains(pointA);
			var hasPointB = _junctionPoints.Contains(pointB);
			return hasPointA || hasPointB;
		}

		/// <summary>
		/// Overload to determine whether two junction points can be added to the circuit
		/// </summary>
		/// <param name="junction">A tuple containing the two junction points</param>
		/// <returns>True if empty, or one junction point can connect with another already in the circuit</returns>
		public bool CanAddJunction((SimpleLongVector3 pointA, SimpleLongVector3 pointB) junction)
		{
			return CanAddJunction(junction.pointA, junction.pointB);
		}

		public bool CanAddJunction(JunctionsAndDistance jad)
		{
			return CanAddJunction(jad.PointA, jad.PointB);
		}

		/// <summary>
		/// Adds the junctions to the circuit
		/// </summary>
		/// <param name="pointA">The first junction</param>
		/// <param name="pointB">The second junction</param>
		public void AddJunction(SimpleLongVector3 pointA, SimpleLongVector3 pointB)
		{
			//	Check if either of the points is contained in the circuit
			var hasPointA = _junctionPoints.Contains(pointA);
			var hasPointB = _junctionPoints.Contains(pointB);

			if (!(hasPointA || hasPointB) && _junctionPoints.Count > 0)
				return;

			//	Add missing junctions to the circuit list
			if (!hasPointA)
				_junctionPoints.Add(pointA);
			if (!hasPointB)
				_junctionPoints.Add(pointB);
		}

		/// <summary>
		/// Overload to add junction points to the circuit
		/// </summary>
		/// <param name="junction">A tuple of the two junction points</param>
		public void AddJunction((SimpleLongVector3 pointA, SimpleLongVector3 pointB) junction)
		{
			AddJunction(junction.pointA, junction.pointB);
		}

		public void AddJunction(JunctionsAndDistance jad)
		{
			AddJunction(jad.PointA, jad.PointB);
		}

		/// <summary>
		/// Yields the number of junctions contained in the circuit
		/// </summary>
		public int JunctionCount
		{
			get
			{
				return _junctionPoints.Count;
			}
		}

		public int ConnectionCount
		{
			get
			{
				return JunctionCount - 1;
			}
		}

		/// <summary>
		/// Static constructor that can merge two different circuit collections
		/// </summary>
		/// <param name="first">The first collection of junctions</param>
		/// <param name="other">The other collection of junctions</param>
		/// <returns>The merged set of junctions</returns>
		public static PlaygroundCircuit MergeCircuits(PlaygroundCircuit first, PlaygroundCircuit other)
		{
			//	Merge the two collections into a temp list
			var circuits = new List<SimpleLongVector3>(first._junctionPoints);
			circuits.AddRange(other._junctionPoints);

			//	Create the new circuit collection and add the distinct elements
			var newCircuit = new PlaygroundCircuit();
			newCircuit._junctionPoints.AddRange(circuits);
			return newCircuit;
		}
	}
}