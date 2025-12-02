using AdventOfCode.Enums;
using AdventOfCode.Interfaces;

namespace AdventOfCode.Challenges.Day00
{
	public partial class Day00
		: IPartOne, IPartOneTest
	{
		/// <inheritdoc />
		bool IPartOne.Run()
		{
			Logger.Log(LogLevel.Info, "Running Part One");
			return true;
		}

		/// <inheritdoc />
		void IPartOneTest.Test()
		{
			Logger.Log(LogLevel.Info, "Test for Part One");
		}
	}
}