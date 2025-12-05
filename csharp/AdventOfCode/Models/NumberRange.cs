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

		/// <summary>
		/// Determines whether the specified value is within the bounds of this <see cref="NumberRange"/>
		/// </summary>
		/// <param name="number">The value to check</param>
		/// <returns>True if the value is contained by this object</returns>
		public bool Contains(long number)
		{
			return number >= Start && number <= End;
		}

		/// <summary>
		/// Returns the total number of values between the <see cref="Start"/> and <see cref="End"/> inclusive
		/// </summary>
		public long Count
		{
			get
			{
				return End - Start + 1;
			}
		}

		/// <summary>
		/// Determines whether the two <see cref="NumberRange"/> objects can be merged to form a single contiguous one
		/// </summary>
		/// <param name="other">The other <see cref="NumberRange"/> to be compared against</param>
		/// <returns>True if the 2 <see cref="NumberRange"/> objects can form a single contiguous one</returns>
		public bool CanMergeWith(NumberRange other)
		{
			var startsAfterOtherEnd = this.Start > other.End;
			var endsBeforeOtherStarts = this.End < other.Start;
			return !(startsAfterOtherEnd || endsBeforeOtherStarts);
		}

#if DEBUG
		public override string ToString()
		{
			return $"Start: {Start}, End: {End}, Count={Count}";
		}
#endif
	}
}