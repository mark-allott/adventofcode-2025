using System.Diagnostics;
using AdventOfCode.Extensions;
using AdventOfCode.Interfaces;
using AdventOfCode.Models;

namespace AdventOfCode.Challenges.Day05
{
	public partial class Day05
		: IPartTwo, IPartTwoTest
	{
		/// <inheritdoc />
		bool IPartTwo.Run()
		{
			var result = SolvePartTwo(InputFileLines);
			PartTwoResult = $"Number of fresh IDs = {result}";
			return true;
		}

		/// <inheritdoc />
		void IPartTwoTest.Test()
		{
			var actual = SolvePartTwo(_partOneTestInput);
			Debug.Assert(actual == PartTwoExpected);
		}

		private static long SolvePartTwo(List<string> input)
		{
			var sections = input.ParseEnumerableOfStringToListOfListOfString();
			Debug.Assert(sections.Count == 2);
			var ranges = sections[0].Select(s => new NumberRange(s))
				.OrderBy(r => r.Start)
				.ToList();

			var freshRanges = new List<NumberRange>();
			NumberRange? current = null;
			foreach (var range in ranges)
			{
				current ??= range;
				if (current == range)
					continue;

				if (current.CanMergeWith(range))
				{
					current = new NumberRange(Math.Min(range.Start, current.Start), Math.Max(range.End, current.End));
					continue;
				}

				freshRanges.Add(current);
				current = range;
			}

			if (current is not null)
				freshRanges.Add(current);

			return freshRanges.Sum(r => r.Count);
		}

		private const long PartTwoExpected = 14;
	}
}