using System.Diagnostics;
using AdventOfCode.Interfaces;

namespace AdventOfCode.Challenges.Day03
{
	public partial class Day03
		: IPartOne, IPartOneTest
	{
		/// <inheritdoc />
		bool IPartOne.Run()
		{
			var result = SolvePartOne(InputFileLines);
			PartOneResult = $"Total joltage is {result}";
			return true;
		}

		/// <inheritdoc />
		void IPartOneTest.Test()
		{
			var actual = SolvePartOne(PartOneTestInput);
			Debug.Assert(actual == PartOneExpected);
		}

		private static int MaxJoltage(char[] digits)
		{
			var result = 0;
			var v = digits.Select(d => int.Parse($"{d}")).ToList();
			var maxStart = v[0..^1].Max();
			var startIndex = 1 + v.IndexOf(maxStart);
			var maxEnd = v[startIndex..].Max();
			result = int.Parse($"{maxStart}{maxEnd}");
			return result;
		}

		private static int SolvePartOne(List<string> input)
		{
			var maxJolts = input
				.Select(l => MaxJoltage(l.ToCharArray()))
				.ToList();
			return maxJolts.Sum();
		}

		private static readonly List<string> PartOneTestInput =
			["987654321111111", "811111111111119", "234234234234278", "818181911112111"];

		private const int PartOneExpected = 357;
	}
}