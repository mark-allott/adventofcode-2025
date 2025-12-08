using System.Diagnostics;
using AdventOfCode.Enums;
using AdventOfCode.Extensions;
using AdventOfCode.Interfaces;
using AdventOfCode.Models;

namespace AdventOfCode.Challenges.Day07
{
	public partial class Day07
		: IPartOne, IPartOneTest
	{
		/// <inheritdoc />
		bool IPartOne.Run()
		{
			var grid = new TachyonSplitterGrid(InputFileLines[0].Length, InputFileLines.Count, EnumExtensions.ToChar);
			grid.LoadGrid(InputFileLines);
			var result = grid.CalculateSplitBeams();
			PartOneResult = $"Number of split beams = {result}";
			return true;
		}

		/// <inheritdoc />
		void IPartOneTest.Test()
		{
			var grid = new TachyonSplitterGrid(TestInput[0].Length, TestInput.Count, EnumExtensions.ToChar);
			grid.LoadGrid(TestInput);
			Logger.Log(LogLevel.Info, "Loaded Grid with:");
			var gridAsString = grid.ToString();
			Logger.Log(LogLevel.Info, gridAsString);
			var lines = gridAsString.Split(Environment.NewLine).ToList();
			if (AssertStringsEqual(lines, TestInput))
				Logger.Log(LogLevel.Info, "Initial grid check completed");

			var actual = grid.CalculateSplitBeams();
			gridAsString = grid.ToString();
			lines = gridAsString.Split(Environment.NewLine).ToList();
			if (AssertStringsEqual(lines, PartOneExpectedState))
				Logger.Log(LogLevel.Info, "Part One grid check completed");
			Debug.Assert(actual == PartOneExpected);
		}

		private bool AssertStringsEqual(List<string> actual, List<string> expected)
		{
			var result = true;
			try
			{
				Debug.Assert(actual.Count == expected.Count);
				for (var r = 0; r < expected.Count; r++)
					try
					{
						Debug.Assert(actual[r] == expected[r]);
					}
					catch (Exception e)
					{
						result = false;
						string[] errorText =
						[
							$"Line check after load failed on line {r}:",
							$"Actual='{actual[r]}'",
							$"Expected='{expected[r]}'",
							$"Error='{e.Message}'"
						];
						Logger.Log(LogLevel.Error, string.Join(Environment.NewLine, errorText));
					}
			}
			catch (Exception e)
			{
				Logger.Log(LogLevel.Error, e.Message);
				result = false;
			}

			return result;
		}
	}
}