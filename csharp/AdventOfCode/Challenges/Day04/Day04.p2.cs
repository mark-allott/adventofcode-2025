using System.Diagnostics;
using AdventOfCode.Interfaces;

namespace AdventOfCode.Challenges.Day04
{
	public partial class Day04
		: IPartTwo, IPartTwoTest
	{
		/// <inheritdoc />
		bool IPartTwo.Run()
		{
			var result = SolvePartTwo(InputFileLines);
			PartTwoResult = $"Removed rolls is {result}";
			return true;
		}

		/// <inheritdoc />
		void IPartTwoTest.Test()
		{
			var actual = SolvePartTwo(_partOneTestData);
			Debug.Assert(actual == PartTwoExpected);
		}

		private int SolvePartTwo(List<string> input)
		{
			var removed = 0;

			var removedCount = 0;
			do
			{
				var removable = GetNeighbours(input)
					.Where(c => c.neighbours < 4)
					.ToList();
				removable.ForEach(r =>
				{
					var row = input[r.row];
					input[r.row] = $"{row[..r.col]}.{row[(r.col + 1)..]}";
				});
				removedCount = removable.Count;
				removed += removedCount;
			} while (removedCount > 0);
			return removed;
		}

		private const int PartTwoExpected = 43;
	}
}