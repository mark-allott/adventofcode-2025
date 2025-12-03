using System.Diagnostics;
using AdventOfCode.Interfaces;
using AdventOfCode.Models;

namespace AdventOfCode.Challenges.Day02
{
	public partial class Day02
		: IPartTwo, IPartTwoTest
	{
		/// <inheritdoc />
		bool IPartTwo.Run()
		{
			var result = SolvePartTwo(InputFileLines);
			PartTwoResult = $"Sum of invalid IDs = {result}";
			return true;
		}

		/// <inheritdoc />
		void IPartTwoTest.Test()
		{
			var actual = SolvePartTwo(_partOneTestData);
			Debug.Assert(actual == PartTwoExpected);
		}

		/// <summary>
		/// Detects repeated patterns within a given number by splitting the number into smaller sections and testing
		/// whether all parts match the first
		/// </summary>
		/// <param name="number">The number to check</param>
		/// <returns>True if a pattern is detected</returns>
		private static bool IsInvalid(long number)
		{
			//	Convert to text value
			var text = $"{number}";
			var halfLength = text.Length / 2;
			var groups = new List<List<string>>();

			for (var i = 1; i <= halfLength; i++)
			{
				if (i > 1 && text.Length % i != 0)
					continue;

				var groupings = new List<string>();
				for (var j = 0; j < text.Length; j += i)
				{
					groupings.Add(text.Substring(j, i));
				}

				if (groupings.Count > 0)
					groups.Add(groupings);
			}

			return groups.Any(g => g.All(a => a == g[0]));
		}

		private static long SolvePartTwo(List<string> input)
		{
			var invalidIds = input
				.SelectMany(r => r.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(e => new NumberRange(e)))
				.Select(r => new
				{
					Range = r,
					Values = r.Numbers
						.Select(n => new { Number = n, Invalid = IsInvalid(n) })
						.ToList()
				})
				.SelectMany(m => m.Values.Where(v => v.Invalid).Select(q => q.Number))
				.ToList();
			var result = 0L;
			invalidIds.ForEach(i => result += i);
			return result;
		}

		private const long PartTwoExpected = 4174379265L;
	}
}