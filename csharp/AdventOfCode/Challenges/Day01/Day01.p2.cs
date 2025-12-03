using System.Diagnostics;
using AdventOfCode.Enums;
using AdventOfCode.Interfaces;
using AdventOfCode.Models;

namespace AdventOfCode.Challenges.Day01
{
	public partial class Day01
		: IPartTwo, IPartTwoTest
	{
		/// <inheritdoc />
		bool IPartTwo.Run()
		{
			PartTwoResult = $"Password is : {Solve2(50, InputFileLines)}";
			return true;
		}

		/// <inheritdoc />
		void IPartTwoTest.Test()
		{
			Logger.Log(LogLevel.Info, "Test for Part Two");
			var actual = Solve2(50, partOneTestData);
			Debug.Assert(actual == partTwoExpectedResult);
		}

		private int Solve2(int startPosition, List<string> input)
		{
			_ = input ?? throw new ArgumentNullException(nameof(input));

			var instructions = input.Select(s => new DialRotation(s))
				.ToList();
			var zeroPositions = 0;
			var currentPosition = startPosition;

			instructions.ForEach(i =>
			{
				(currentPosition, zeroPositions) = Click(i.Direction, currentPosition, i.Amount, zeroPositions);
			});
			return zeroPositions;
		}

		private static (int position, int zeroCount) Click(RotationDirection direction, int position, int moveCount,
			int zeroCount)
		{
			while (moveCount > 0)
			{
				position = (100 + position + Math.Sign((int)direction)) % 100;
				if (position == 0)
					zeroCount++;
				moveCount--;
			}

			return (position, zeroCount);
		}

		private readonly int partTwoExpectedResult = 6;
	}
}