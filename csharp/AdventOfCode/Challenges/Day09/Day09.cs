using AdventOfCode.Interfaces;

namespace AdventOfCode.Challenges.Day09
{
	/// <summary>
	/// Skeleton class for executing a daily challenge
	/// </summary>
	/// <remarks>
	/// Code to perform the required steps to find the solution should be placed in the individual nested class files for:
	/// <ul>
	/// <li>part one (Day<i>nn</i>.p1.cs)</li>
	/// <li>part two (Day<i>nn</i>.p2.cs)</li>
	/// </ul>
	/// </remarks>
	public partial class Day09
		: BaseDailyChallenge, IAutoRegister
	{
		public Day09(ILogger logger)
			: base(logger, 9, "day09-input.txt", "Movie Theater")
		{
		}

		private static readonly List<string> TestInput =
		[
			"7,1",
			"11,1",
			"11,7",
			"9,7",
			"9,5",
			"2,5",
			"2,3",
			"7,3"
		];

		private const int PartOneExpected = 50;
	}
}