using System.Diagnostics;
using AdventOfCode.Extensions;
using AdventOfCode.Interfaces;
using AdventOfCode.Models;

namespace AdventOfCode.Challenges.Day06
{
	public partial class Day06
		: IPartTwo, IPartTwoTest
	{
		/// <inheritdoc />
		bool IPartTwo.Run()
		{
			var result = SolvePartTwo(InputFileLines);
			PartTwoResult = $"Homework total = {result}";
			return true;
		}

		/// <inheritdoc />
		void IPartTwoTest.Test()
		{
			var actualResults = SolvePartTwoParts(TestInput);
			actualResults.Select((r, i) => new { Index = i, Result = r })
				.ToList()
				.ForEach(i => Debug.Assert(PartTwoExpectedResults[i.Index] == i.Result));
			var actual = actualResults.Sum();
			Debug.Assert(actual == PartTwoExpectedResult);
		}

		private static long SolvePartTwo(List<string> input)
		{
			return SolvePartTwoParts(input).Sum();
		}

		private static List<long> SolvePartTwoParts(List<string> input)
		{
			//	Find the types of problems to be solved
			var problemTypes = input[^1]
				.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
				.Select(s => s.ToHomeworkProblemType())
				.ToList();
			//	Find the common split points in the numeric part of the input
			var splitPoints = input[..^1].ParseEnumerableOfStringForCommonSplitPositions([' '])
				.ToList();
			//	Convert into a list of separate string elements
			var problemParts = input[..^1]
				//	Split at the common points
				.Select(s =>
				{
					var idx = 0;
					var parts = splitPoints.Select(p =>
						{
							var r = s[idx..p];
							idx = 1 + p;
							return r;
						})
						.ToList();
					parts.Add(s[idx..]);
					return parts;
				})
				//	Flatten to a single list, with row/column numbering
				.SelectMany((s, row) => s.Select((e, col) => new { col, row, e }))
				//	Group by column
				.GroupBy(g => g.col)
				//	Recombine to a list of problems in their column orders
				.Select(g => g.ToList().OrderBy(o => o.col).Select(s => s.e).ToList())
				.ToList();

			//	Convert to a list of homework problems to be solved
			var homeworkProblems = problemParts
				//	First task is to split each string value into characters so we can pivot their values
				.Select(pp => pp.SelectMany((s, row) => s.ToCharArray().Select((c, col) => new { col, row, c }))
					.GroupBy(g => g.col)
					.Select(g =>
						new string(g.OrderBy(o => o.row)
							.Select(o => o.c).ToArray())
					)
					//	characters pivot done, convert to a value for the problem
					.Select(s => long.Parse(s))
					.ToArray())
				//	Create the homework problem
				.Select((s, i) => new HomeworkProblem(problemTypes[i], s))
				.ToList();
			//	Extract results for checking/summing
			return homeworkProblems.Select(p => p.Result).ToList();
		}

		private static readonly List<long> PartTwoExpectedResults = [8544, 625, 3253600, 1058];
		private const long PartTwoExpectedResult = 3263827;
	}
}