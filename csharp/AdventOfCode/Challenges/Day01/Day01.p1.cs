using System.Diagnostics;
using AdventOfCode.Enums;
using AdventOfCode.Interfaces;
using AdventOfCode.Models;

namespace AdventOfCode.Challenges.Day01
{
	public partial class Day01
		: IPartOne, IPartOneTest
	{
		/// <inheritdoc />
		bool IPartOne.Run()
		{
			PartOneResult = $"Password is : {Solve1(50, InputFileLines)}";
			return true;
		}

		/// <inheritdoc />
		void IPartOneTest.Test()
		{
			Logger.Log(LogLevel.Info, "Test for Part One");
			var actual = Solve1(50, partOneTestData);
			Debug.Assert(actual == partOneExpectedResult);
		}

		private int Solve1(int startPosition, List<string> input)
		{
			_ = input ?? throw new ArgumentNullException(nameof(input));

			var instructions = input.Select(s => new DialRotation(s))
				.ToList();
			var zeroPositions = 0;
			var currentPosition = startPosition;
			instructions.ForEach(i =>
			{
				currentPosition = (currentPosition + 100 + (Math.Sign((int)i.Direction) * i.Amount)) % 100;
				if (currentPosition == 0)
					zeroPositions++;
			});
			return zeroPositions;
		}

		private readonly List<string> partOneTestData = ["L68", "L30", "R48", "L5", "R60", "L55", "L1", "L99", "R14", "L82"];
		private readonly int partOneExpectedResult = 3;
	}
}