using AdventOfCode.Interfaces;

namespace AdventOfCode.Challenges.Day03
{
	/// <summary>
	/// Class for executing day 3 challenge
	/// </summary>
	public partial class Day03
		: BaseDailyChallenge, IAutoRegister
	{
		public Day03(ILogger logger)
			: base(logger, 3, "day03-input.txt", "Lobby")
		{
		}
	}
}