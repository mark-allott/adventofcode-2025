using AdventOfCode.Interfaces;

namespace AdventOfCode.Challenges.Day06
{
	/// <summary>
	/// Base class for daily challenge
	/// </summary>
	public partial class Day06
		: BaseDailyChallenge, IAutoRegister
	{
		public Day06(ILogger logger)
			: base(logger, 6, "day06-input.txt", "Trash Compactor")
		{
		}
	}
}