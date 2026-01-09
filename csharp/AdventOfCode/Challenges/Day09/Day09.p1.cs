using System.Diagnostics;
using AdventOfCode.Extensions;
using AdventOfCode.Interfaces;
using AdventOfCode.Models;

namespace AdventOfCode.Challenges.Day09
{
	public partial class Day09
		: IPartOne, IPartOneTest
	{
		/// <inheritdoc />
		bool IPartOne.Run()
		{
			var result = SolveForPartOne(InputFileLines);
			PartOneResult = $"Largest area is {result}.";
			return true;
		}

		/// <inheritdoc />
		void IPartOneTest.Test()
		{
			var actual = SolveForPartOne(TestInput);
			Debug.Assert(actual == PartOneExpected);
		}

		private long SolveForPartOne(IEnumerable<string> input)
		{
			var strings = (input ?? Enumerable.Empty<string>()).ToList();
			var inputList = strings.ParseEnumerableOfStringToListOfListOfInt();
			if (inputList.Any(x => x.Count != 2))
				throw new ArgumentException("Incorrect number of values in input");

			var coords = inputList
				.Select(m => new TheatreCoordinate(m[0], m[1]))
				.ToList();
			var results = coords[..^1]
				.SelectMany((c1, o) => coords[(o + 1)..]
					.Select(c2 => new { c1, c2, Area = CoordinateExtensions.AreaEnclosedBy(c1, c2) }))
				.OrderByDescending(o => o.Area)
				.ToList();

			return results[0].Area;
		}
	}
}