using System.Diagnostics;
using AdventOfCode.Extensions;
using AdventOfCode.Interfaces;
using AdventOfCode.Models;

namespace AdventOfCode.Challenges.Day09
{
	public partial class Day09
		: IPartTwo, IPartTwoTest
	{
		/// <inheritdoc />
		bool IPartTwo.Run()
		{
			var result = SolveForPartTwo(InputFileLines);
			PartTwoResult = $"Largest tiled area is {result}.";
			return true;
		}

		/// <inheritdoc />
		void IPartTwoTest.Test()
		{
			var actual = SolveForPartTwo(TestInput);
			Debug.Assert(actual == PartTwoExpected);
		}

		/// <summary>
		/// Solve the problem for part two: the red tiles in the corners must have red/green tiles within the enclosed
		/// rectangle that they form
		/// </summary>
		/// <param name="input">The coordinates of the red tiles</param>
		/// <returns>The area of the largest rectangle</returns>
		private static long SolveForPartTwo(IEnumerable<string> input)
		{
			var grid = new TheatreGrid(100000, 100000);
			grid.LoadGrid(input);

			//	According to the requirements, the rectangle MUST have at least two corners that are red tiles, so
			//	those are the starting points for searching
			var redTiles = grid.RedTileCells.ToList();
			//	Create pairs of red tiles to search
			var areas = redTiles[..^1]
				.SelectMany((c1, i) => redTiles[(i + 1)..]
					.Select(c2 => new
					{
						c1, c2, Area = CoordinateExtensions.AreaEnclosedBy(c1.Location, c2.Location),
					}))
				.OrderByDescending(o => o.Area)
				.FirstOrDefault(q => grid.RangesContain(q.c1.Location, q.c2.Location));

			return areas?.Area ?? 0;
		}
	}
}