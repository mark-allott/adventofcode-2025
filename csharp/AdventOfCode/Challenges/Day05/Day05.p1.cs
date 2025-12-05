using System.Diagnostics;
using AdventOfCode.Extensions;
using AdventOfCode.Interfaces;
using AdventOfCode.Models;

namespace AdventOfCode.Challenges.Day05
{
	public partial class Day05
		: IPartOne, IPartOneTest
	{
		/// <inheritdoc />
		bool IPartOne.Run()
		{
			var result = SolvePartOne(InputFileLines);
			PartOneResult = $"Number of fresh = {result}";
			return true;
		}

		/// <inheritdoc />
		void IPartOneTest.Test()
		{
			var actual = SolvePartOne(_partOneTestInput);
			Debug.Assert(actual == PartOneExpected);
		}

		private static long SolvePartOne(List<string> input)
		{
			var sections = input.ParseEnumerableOfStringToListOfListOfString();
			Debug.Assert(sections.Count == 2);
			var ranges = sections[0].Select(s => new NumberRange(s))
				.ToList();
			var ingredients = sections[1].ParseEnumerableOfStringToListOfLong(ranges.Count + 1);

			return ingredients.Count(i => ranges.Any(r => r.Contains(i)));
		}

		private readonly List<string> _partOneTestInput = ["3-5","10-14","16-20","12-18","","1","5","8","11","17","32"];

		private const int PartOneExpected = 3;
	}
}