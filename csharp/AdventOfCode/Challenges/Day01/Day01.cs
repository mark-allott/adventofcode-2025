using AdventOfCode.Interfaces;

namespace AdventOfCode.Challenges.Day01
{
	/// <summary>
	/// Challenge solution for "Secret Entrance" problem
	/// </summary>
	public partial class Day01
		: BaseDailyChallenge, IAutoRegister
	{
		public Day01(ILogger logger)
			: base(logger, 1, "day01-input.txt", "Secret Entrance")
		{
		}
	}
}