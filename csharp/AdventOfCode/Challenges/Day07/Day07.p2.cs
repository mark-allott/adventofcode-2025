using System.Diagnostics;
using AdventOfCode.Extensions;
using AdventOfCode.Interfaces;
using AdventOfCode.Models;

namespace AdventOfCode.Challenges.Day07
{
	public partial class Day07
		: IPartTwo, IPartTwoTest
	{
		/// <inheritdoc />
		bool IPartTwo.Run()
		{
			var grid = new TachyonSplitterGrid(InputFileLines[0].Length, InputFileLines.Count, EnumExtensions.ToChar);
			grid.LoadGrid(InputFileLines);
			var result = grid.CalculateTimelines();
			PartTwoResult = $"Number of timelines = {result}";
			return true;
		}

		/// <inheritdoc />
		void IPartTwoTest.Test()
		{
			var grid = new TachyonSplitterGrid(TestInput[0].Length, TestInput.Count, EnumExtensions.ToChar);
			grid.LoadGrid(TestInput);
			var actual = grid.CalculateTimelines();
			Debug.Assert(actual == PartTwoExpected);
		}
	}
}