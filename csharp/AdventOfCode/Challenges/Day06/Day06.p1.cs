using System.Diagnostics;
using AdventOfCode.Extensions;
using AdventOfCode.Interfaces;
using AdventOfCode.Models;

namespace AdventOfCode.Challenges.Day06
{
	public partial class Day06
		: IPartOne, IPartOneTest
	{
		/// <inheritdoc />
		bool IPartOne.Run()
		{
			var result = SolvePartOne(InputFileLines);
			PartOneResult = $"Homework total = {result}";
			return true;
		}

		/// <inheritdoc />
		void IPartOneTest.Test()
		{
			var actualResults = SolvePartOneParts(TestInput);
			actualResults.Select((r, i) => new { Index = i, Result = r })
				.ToList()
				.ForEach(i => Debug.Assert(PartOneExpectedResults[i.Index] == i.Result));
			var actual = actualResults.Sum();
			Debug.Assert(actual == PartOneExpectedResult);
		}

		private static long SolvePartOne(List<string> input)
		{
			return SolvePartOneParts(input).Sum();
		}

		private static List<long> SolvePartOneParts(List<string> input)
		{
			//	Get the list of values from all but the last row of input
			var problemLists = input[..^1].ParseEnumerableOfStringToListOfListOfLong(1);
			//	Get the list of problem types from the last row
			var problemTypes = input[^1]
				.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
				.Select(s => s.ToHomeworkProblemType())
				.ToList();
			//	Make sure the lists are all equal lengths
			Debug.Assert(problemLists.All(p => p.Count == problemTypes.Count));

			//	The inputs are currently in horizontal lists and we need columns, so the values have the equivalent of a PIVOT operation performed on them to convert them into the correct format
			var homeworkInputs = problemLists
				//	Flatten the lists into a single long list, adding row/column monikers
				.SelectMany((p, row) => p.Select((t, col) => new { col, row, t }))
				//	Arrange the list by column values
				.GroupBy(g => g.col)
				//	Convert to problem classes, adding the appropriate entries as input
				.Select(g => new HomeworkProblem(problemTypes[g.Key], g.OrderBy(o => o.row).Select(s => s.t).ToArray()))
				.ToList();
			//	Grab the results from the homework problem classes
			return homeworkInputs.Select(i => i.Result).ToList();
		}

		private static readonly List<string> TestInput =
		[
			"123 328  51 64 ",
			" 45 64  387 23 ",
			"  6 98  215 314",
			"*   +   *   +  "
		];

		private static readonly List<long> PartOneExpectedResults = [33210, 490, 4243455, 401];
		private const long PartOneExpectedResult = 4277556;
	}
}