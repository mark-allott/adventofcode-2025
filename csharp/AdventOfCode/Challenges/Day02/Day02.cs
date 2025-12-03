using AdventOfCode.Interfaces;

namespace AdventOfCode.Challenges.Day02
{
	/// <summary>
	/// Class for executing the challenge for day 2
	/// </summary>
	public partial class Day02
		: BaseDailyChallenge, IAutoRegister
	{
		public Day02(ILogger logger)
			: base(logger, 2, "day02-input.txt", "Gift Shop")
		{
		}
	}
}