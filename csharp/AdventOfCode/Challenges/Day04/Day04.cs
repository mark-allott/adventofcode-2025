using AdventOfCode.Interfaces;

namespace AdventOfCode.Challenges.Day04
{
	/// <summary>
	/// Base class for executing day 4 challenge
	/// </summary>
	public partial class Day04
		: BaseDailyChallenge, IAutoRegister
	{
		public Day04(ILogger logger)
			: base(logger, 4, "day04-input.txt", "Printing Department")
		{
		}
	}
}