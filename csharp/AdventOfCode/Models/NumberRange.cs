namespace AdventOfCode.Models
{
	public record NumberRange
	{
		public IEnumerable<long> Numbers
		{
			get
			{
				for (var i = Start; i <= End; i++)
					yield return i;
			}
		}

		public long Start { get; init; }
		public long End { get; init; }

		public NumberRange(long start, long end)
		{
			Start = start;
			End = end;
		}

		public NumberRange(string range)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(range);
			var parts = range.Split('-');
			ArgumentOutOfRangeException.ThrowIfNotEqual(parts.Length, 2, nameof(range));
			Start = long.Parse(parts[0]);
			End = long.Parse(parts[1]);
		}

		public bool Contains(long number)
		{
			return number >= Start && number <= End;
		}

#if DEBUG
		public override string ToString()
		{
			return $"Start: {Start}, End: {End}, Count={1 + End - Start}";
		}
#endif
	}
}