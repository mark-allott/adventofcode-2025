using AdventOfCode.Interfaces;

namespace AdventOfCode.Challenges.Day05
{
	/// <summary>
	/// Base class for executing day 5 challenge
	/// </summary>
	public partial class Day05
		: BaseDailyChallenge, IAutoRegister
	{
		public Day05(ILogger logger)
			: base(logger, 5, "day05-input.txt", "Cafeteria")
		{
		}
	}
}