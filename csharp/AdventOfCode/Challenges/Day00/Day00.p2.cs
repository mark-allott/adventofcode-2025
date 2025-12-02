using AdventOfCode.Enums;
using AdventOfCode.Interfaces;

namespace AdventOfCode.Challenges.Day00
{
	public partial class Day00
		: IPartTwo, IPartTwoTest
	{
		/// <inheritdoc />
		bool IPartTwo.Run()
		{
			Logger.Log(LogLevel.Info, "Running Part Two");
			return true;
		}

		/// <inheritdoc />
		void IPartTwoTest.Test()
		{
			Logger.Log(LogLevel.Info, "Test for Part Two");
		}
	}
}