using System.Diagnostics;
using System.Numerics;
using AdventOfCode.Extensions;
using AdventOfCode.Interfaces;
using AdventOfCode.Models;

namespace AdventOfCode.Challenges.Day08
{
	public partial class Day08
		: IPartOne, IPartOneTest
	{
		/// <inheritdoc />
		bool IPartOne.Run()
		{
			var result = SolvePartOneAlt(InputFileLines, 1000);
			PartOneResult = $"Multiplication of 3 largest circuits = {result}";
			return true;
		}

		/// <inheritdoc />
		void IPartOneTest.Test()
		{
			var actual = SolvePartOneAlt(TestInput, 10);
			Debug.Assert(actual == PartOneExpected);
		}

		/// <summary>
		/// Solution finder for part one of challenge
		/// </summary>
		/// <param name="input">The list of 3D coordinates that represent the individual junctions</param>
		/// <param name="maxConnections">The maximum number of connections to be used to solve the problem</param>
		/// <returns>The value that represents the solution to the problem</returns>
		/// <exception cref="ArgumentException"></exception>
		private static long SolvePartOne(IEnumerable<string> input, int maxConnections)
		{
			//	Convert input into junctions
			var strings = (input ?? Enumerable.Empty<string>()).ToList();
			var junctions = CreateJunctionBoxes(strings);
			//	Assert there are matching numbers of junctions to input lines
			Debug.Assert(junctions.Count == strings.Count);
			//	Calculate the distances between points, order by shortest and use the number of connections we need
			var shortestDistances = CalculateDistances(junctions)
				.OrderBy(o => o.Value)
				.Take(maxConnections)
				.ToDictionary();

			//	Make a store for our circuit lists
			var circuitList = new List<PlaygroundCircuit>();

			//	Loop around the shortest distances we discovered earlier
			foreach (var connection in shortestDistances)
			{
				//	Find any existing circuits we can add to
				var circuits = circuitList
					.Where(q => q.CanAddJunction(connection.Key))
					.ToList();

				PlaygroundCircuit currentCircuit;
				switch (circuits.Count)
				{
					//	No existing circuits found, so create a new one and add to the list
					case 0:
						currentCircuit = new PlaygroundCircuit();
						circuitList.Add(currentCircuit);
						break;
					//	One match found
					case 1:
						currentCircuit = circuits[0];
						break;
					//	There are 2 circuits that need merging, create a new merged circuit and remove the 2 separate ones
					case 2:
						currentCircuit = PlaygroundCircuit.MergeCircuits(circuits[0], circuits[1]);
						circuitList.Remove(circuits[0]);
						circuitList.Remove(circuits[1]);
						circuitList.Add(currentCircuit);
						break;
					default: throw new ArgumentException($"Too many matches: {circuits.Count}");
				}

				//	Add the new junction to the circuit
				currentCircuit.AddJunction(connection.Key);
			}

			//	Grab the top 3 most connected circuits and multiply their junction counts together
			var sum = circuitList.OrderByDescending(o => o.JunctionCount)
				.Take(3)
				.Aggregate(1, (a, v) => a * v.JunctionCount);
			return sum;
		}

		/// <summary>
		/// Converts the <paramref name="input"/> values into <see cref="Vector3"/> coordinates
		/// </summary>
		/// <param name="input">The list of x, y and z locations for the junctions</param>
		/// <returns>The junctions as <see cref="Vector3"/> equivalents</returns>
		private static List<SimpleLongVector3> CreateJunctionBoxes(IEnumerable<string> input)
		{
			var coords = input.ParseEnumerableOfStringToListOfListOfInt();
			Debug.Assert(coords.All(c => c.Count == 3));
			return coords.Select(coord => new SimpleLongVector3(coord[0], coord[1], coord[2]))
				.ToList();
		}

		/// <summary>
		/// Determines the distance between pairs of junctions
		/// </summary>
		/// <param name="junctions">The list of individual junction locations</param>
		/// <returns>A dictionary of paired junctions (as a tuple key) and the Euclidean distance between them</returns>
		private static Dictionary<(SimpleLongVector3, SimpleLongVector3), long> CalculateDistances(
			IEnumerable<SimpleLongVector3> junctions)
		{
			//	Make sure the inputs are a list to stop multiple enumeration warnings
			var junctionPoints = (junctions ?? Enumerable.Empty<SimpleLongVector3>()).ToList();

			//	Shortcut the creation of the points using LINQ to iterate over them
			return junctionPoints[..^1].SelectMany((j1, o) =>
					junctionPoints[(o + 1)..].Select(j2 =>
						new KeyValuePair<(SimpleLongVector3, SimpleLongVector3), long>((j1, j2),
							SimpleLongVector3.Distance2(j1, j2))))
				.ToDictionary();
		}

		/// <summary>
		/// Alternate solver for part one that uses different classes to provide teh answer
		/// </summary>
		/// <param name="input">The list of 3D coordinates that represent the individual junctions</param>
		/// <param name="maxConnections">The maximum number of connections to be used to solve the problem</param>
		/// <returns>The value that represents the solution to the problem</returns>
		private static long SolvePartOneAlt(IEnumerable<string> input, int maxConnections)
		{
			var x = CalculateJunctionDistances(input);
			var circuits = x.circuits.ToList();
			var distances = x.junctionsAndDistances.Take(maxConnections).ToList();

			foreach (var distance in distances)
			{
				var circuitA = circuits.Single(s => s.HasJunction(distance.PointA));
				var circuitB = circuits.Single(s => s.HasJunction(distance.PointB));

				if (circuitA == circuitB)
					continue;

				circuitA.MergeWith(circuitB);
				circuits.Remove(circuitB);
			}

			//	Grab the top 3 most connected circuits and multiply their junction counts together
			var sum = circuits.OrderByDescending(o => o.JunctionCount)
				.Take(3)
				.Aggregate(1, (a, v) => a * v.JunctionCount);
			return sum;
		}
	}
}