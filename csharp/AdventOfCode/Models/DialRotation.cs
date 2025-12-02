using AdventOfCode.Enums;

namespace AdventOfCode.Models
{
	public record DialRotation
	{
		public int Amount { get; init; }
		public RotationDirection Direction { get; init; }

		public DialRotation(string input)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(input, nameof(input));
			Direction = input[0] switch
			{
				'L' => RotationDirection.Left,
				'R' => RotationDirection.Right,
				_ => throw new ArgumentException(nameof(Direction))
			};
			if (!int.TryParse(input[1..], out var value))
				throw new ArgumentException("Invalid conversion of input to value", nameof(input));
			Amount = value;
		}
	}
}