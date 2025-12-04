using System.Diagnostics;
using AdventOfCode.Interfaces;

namespace AdventOfCode.Challenges.Day04
{
	public partial class Day04
		: IPartOne, IPartOneTest
	{
		/// <inheritdoc />
		bool IPartOne.Run()
		{
			var result = SolvePartOne(InputFileLines);
			PartOneResult = $"Accessible rolls is {result}";
			return true;
		}

		/// <inheritdoc />
		void IPartOneTest.Test()
		{
			var actual = SolvePartOne(_partOneTestData);
			Debug.Assert(actual == PartOneExpected);
		}

		private readonly List<(int yOffset, int xOffset)> _gridOffsets =
		[
			(-1, -1),
			(-1, 0),
			(-1, 1),
			(0, -1),
			(0, 1),
			(1, -1),
			(1, 0),
			(1, 1)
		];

		private List<(int row, int col, int neighbours)> GetNeighbours(List<string> input)
		{
			var rows = input.Count;
			var cols = input[0].Length;

			var gridNeighbours = new List<(int row, int col, int neighbours)>();
			for (var r = 0; r < rows; r++)
			{
				for (var c = 0; c < cols; c++)
				{
					if (input[r][c] == '.')
						continue;
					var neighboursCount = 0;
					_gridOffsets.ForEach(o =>
						{
							var y = r + o.yOffset;
							var x = c + o.xOffset;
							if (y < 0 || y >= rows || x < 0 || x >= cols)
								return;
							if (input[y][x] == '.')
								return;
							neighboursCount++;
						}
					);
					gridNeighbours.Add((r, c, neighboursCount));
				}
			}

			return gridNeighbours;
		}

		private int SolvePartOne(List<string> input)
		{
			return GetNeighbours(input).Count(c => c.neighbours < 4);
		}

		private readonly List<string> _partOneTestData =
		[
			"..@@.@@@@.",
			"@@@.@.@.@@",
			"@@@@@.@.@@",
			"@.@@@@..@.",
			"@@.@@@@.@@",
			".@@@@@@@.@",
			".@.@.@.@@@",
			"@.@@@.@@@@",
			".@@@@@@@@.",
			"@.@.@@@.@."
		];

		private const int PartOneExpected = 13;
	}
}