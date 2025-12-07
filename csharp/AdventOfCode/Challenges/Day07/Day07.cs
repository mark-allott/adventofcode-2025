using AdventOfCode.Interfaces;

namespace AdventOfCode.Challenges.Day07
{
	/// <summary>
	/// Base class for daily challenge
	/// </summary>
	public partial class Day07
		: BaseDailyChallenge, IAutoRegister
	{
		public Day07(ILogger logger)
			: base(logger, 7, "day07-input.txt", "Laboratories")
		{
		}

		private static readonly List<string> TestInput =
		[
			".......S.......",
			"...............",
			".......^.......",
			"...............",
			"......^.^......",
			"...............",
			".....^.^.^.....",
			"...............",
			"....^.^...^....",
			"...............",
			"...^.^...^.^...",
			"...............",
			"..^...^.....^..",
			"...............",
			".^.^.^.^.^...^.",
			"..............."
		];

		private static readonly List<string> PartOneExpectedState =
		[
			".......S.......",
			".......|.......",
			"......|^|......",
			"......|.|......",
			".....|^|^|.....",
			".....|.|.|.....",
			"....|^|^|^|....",
			"....|.|.|.|....",
			"...|^|^|||^|...",
			"...|.|.|||.|...",
			"..|^|^|||^|^|..",
			"..|.|.|||.|.|..",
			".|^|||^||.||^|.",
			".|.|||.||.||.|.",
			"|^|^|^|^|^|||^|",
			"|.|.|.|.|.|||.|"
		];

		private const int PartOneExpected = 21;
	}
}