using System.Diagnostics;
using System.Text;
using AdventOfCode.Interfaces;

namespace AdventOfCode.Challenges.Day03
{
	public partial class Day03
		: IPartTwo, IPartTwoTest
	{
		/// <inheritdoc />
		bool IPartTwo.Run()
		{
			var result = SolvePartTwo(InputFileLines);
			PartTwoResult = $"Total joltage is {result}";
			return true;
		}

		/// <inheritdoc />
		void IPartTwoTest.Test()
		{
			var actual = SolvePartTwo(PartOneTestInput);
			Debug.Assert(actual == PartTwoExpected);
		}

		private static long MaxJoltageFrom12(char[] digits)
		{
			var allValues = digits.Select(d => int.Parse($"{d}")).ToList();
			var selected = new List<int>();
			var startIndex = 0;
			var ignoredCount = digits.Length - 12;
			while (selected.Count < 12)
			{
				if (ignoredCount > 0)
				{
					var maxSelection = allValues[startIndex..(1 + startIndex + ignoredCount)].Max();
					selected.Add(maxSelection);
					var offsetIndex = allValues.IndexOf(maxSelection, startIndex);
					ignoredCount -= (offsetIndex - startIndex);
					startIndex = offsetIndex + 1;
				}
				else
				{
					selected.Add(allValues[startIndex++]);
				}
			}

			var sb = new StringBuilder();
			selected.ForEach(x => sb.Append($"{x}"));
			return long.Parse(sb.ToString());
		}

		private static long SolvePartTwo(List<string> input)
		{
			var maxJolts = input
				.Select(l => MaxJoltageFrom12(l.ToCharArray()))
				.ToList();
			return maxJolts.Sum();
		}

		private const long PartTwoExpected = 3121910778619;
	}
}