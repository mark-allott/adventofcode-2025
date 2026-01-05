using System.Diagnostics;
using AdventOfCode.Interfaces;
using AdventOfCode.Models;

namespace AdventOfCode.Challenges.Day08
{
	public partial class Day08
		: IPartTwo, IPartTwoTest
	{
		/// <inheritdoc />
		bool IPartTwo.Run()
		{
			var result = SolvePartTwo(InputFileLines);
			PartTwoResult = $"Multiplication of X-coords = {result}";
			return true;
		}

		/// <inheritdoc />
		void IPartTwoTest.Test()
		{
			var actual = SolvePartTwo(TestInput);
			Debug.Assert(actual == PartTwoExpected);
		}

		private static long SolvePartTwo(IEnumerable<string> input)
		{
			var x = CalculateJunctionDistances(input);
			var circuits = x.circuits.ToList();
			var junctions = x.junctionsAndDistances.ToList();
			var iterations = circuits.Count;

			//	Loop around the shortest distances we discovered earlier
			foreach (var junction in junctions)
			{
				var circuitA = circuits.Single(s => s.HasJunction(junction.PointA));
				var circuitB = circuits.Single(s => s.HasJunction(junction.PointB));

				if (circuitA == circuitB)
					continue;

				circuitA.MergeWith(circuitB);
				iterations--;

				if (iterations == 1)
					return (long)(junction.PointA.X * junction.PointB.X);
				circuits.Remove(circuitB);
			}

			return 0;
		}

		private static (IEnumerable<Circuit> circuits, IEnumerable<JunctionsAndDistance> junctionsAndDistances) CalculateJunctionDistances(IEnumerable<string> input)
		{
			//	Convert input into junctions
			var strings = (input ?? Enumerable.Empty<string>()).ToList();
			var junctions = CreateJunctionBoxes(strings);
			//	Assert there are matching numbers of junctions to input lines
			Debug.Assert(junctions.Count == strings.Count);

			//	Assign each junction
			var circuits = junctions.Select(s => Circuit.Create(s))
				.ToList();

			//	Make sure the inputs are a list to stop multiple enumeration warnings
			var distances = junctions[..^1]
				.SelectMany((j1, o) => junctions[(o + 1)..].Select(j2 => new JunctionsAndDistance(j1, j2)))
				.OrderBy(o => o.Distance2);

			//	return the circuit list and junction/distance list
			return (circuits, distances);
		}
	}
}