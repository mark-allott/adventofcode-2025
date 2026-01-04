namespace AdventOfCode.Models
{
	internal record Circuit
	{
		/// <summary>
		/// Store for all locations in the circuit
		/// </summary>
		private List<SimpleLongVector3> _junctions = [];

		/// <summary>
		/// Don't permit public constructor usage - use the static constructor instead
		/// </summary>
		private Circuit()
		{
		}

		/// <summary>
		/// Determines whether the circuit contains the specified <paramref name="junction"/>
		/// </summary>
		/// <param name="junction">The junction to find</param>
		/// <returns>True if the circuit contains the <paramref name="junction"/></returns>
		public bool HasJunction(SimpleLongVector3 junction)
		{
			return _junctions.Contains(junction);
		}

		/// <summary>
		/// Returns the number of junctions within the circuit
		/// </summary>
		public int JunctionCount
		{
			get
			{
				return _junctions.Count;
			}
		}

		/// <summary>
		/// Merges the current circuit with another
		/// </summary>
		/// <param name="circuit">The circuit to be merged with this one</param>
		public void MergeWith(Circuit circuit)
		{
			_junctions = _junctions.Concat(circuit._junctions)
				.Distinct()
				.ToList();
		}

		/// <summary>
		/// static constructor that ensures the specified <paramref name="junction"/> is added to the circuit
		/// </summary>
		/// <param name="junction">The junction which represents the initial location for the circuit</param>
		/// <returns></returns>
		public static Circuit Create(SimpleLongVector3 junction)
		{
			var circuit = new Circuit();
			circuit._junctions.Add(junction);
			return circuit;
		}
	}
}