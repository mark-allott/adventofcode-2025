using System.Diagnostics;
using AdventOfCode.Interfaces;
using AdventOfCode.Models;

namespace AdventOfCode.Challenges.Day02
{
	public partial class Day02
		: IPartOne, IPartOneTest
	{
		/// <inheritdoc />
		bool IPartOne.Run()
		{
			var result = SolvePartOne(InputFileLines);
			PartOneResult = $"Sum of invalid IDs = {result}";
			return true;
		}

		/// <inheritdoc />
		void IPartOneTest.Test()
		{
			var actual = SolvePartOne(_partOneTestData);
			Debug.Assert(actual == _partOneExpected);
		}

		private static long SolvePartOne(List<string> input)
		{
			var ranges = input
				.SelectMany(r => r.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(e => new NumberRange(e)))
				.Select(range => new
				{
					Range = range,
					Values = range.Numbers
						.Select(n => new { Number = n, Text = $"{n}" })
						.Where(q => q.Text.Length % 2 == 0)
						.ToList()
				})
				.Select(s => new
				{
					s.Range,
					Values = s.Values.Select(v => new
						{
							v.Number, Left = v.Text[..(v.Text.Length / 2)], Right = v.Text[(v.Text.Length / 2)..]
						})
						.Where(q => q.Left == q.Right)
						.Select(q => q.Number)
						.ToList()
				})
				.Where(q => q.Values.Count > 0)
				.ToList();
			var result = 0L;
			ranges.SelectMany(m => m.Values).ToList().ForEach(i => result += i);
			return result;
		}

		private readonly List<string> _partOneTestData =
		[
			"11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124"
		];

		private readonly long _partOneExpected = 1227775554;
	}
}