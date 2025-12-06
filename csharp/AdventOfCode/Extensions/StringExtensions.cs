namespace AdventOfCode.Extensions
{
	public static class StringExtensions
	{
		/// <param name="line">The string to parse</param>
		extension(string line)
		{
			/// <summary>
			/// Parse the input into a list of integer values. Values can be separated by spaces or commas
			/// </summary>
			/// <param name="rowNumber">(Optional) specifies the row number in a file</param>
			/// <returns>The list of integer parts</returns>
			/// <exception cref="ArgumentException"></exception>
			public List<int> ParseStringToListOfInt(int rowNumber = 0)
			{
				var parts = line.Split([' ', ','],
					StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
				return parts.ParseEnumerableOfStringToListOfInt(rowNumber);
			}

			/// <summary>
			/// Parse the input into a list of long integer values. Values can be separated by spaces or commas
			/// </summary>
			/// <param name="rowNumber">(Optional) specifies the row number in a file</param>
			/// <returns>The list of long integer parts</returns>
			public List<long> ParseStringToListOfLong(int rowNumber = 0)
			{
				var parts = line.Split([' ', ','],
					StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
				return parts.ParseEnumerableOfStringToListOfLong(rowNumber);
			}
		}

		/// <param name="input">The list of strings to parse</param>
		extension(IEnumerable<string> input)
		{
			/// <summary>
			/// Parse the input into a list of integer values. Values can be separated by spaces or commas
			/// </summary>
			/// <param name="rowNumber">(Optional) specifies the row number in a file</param>
			/// <returns>The list of integer parts</returns>
			/// <exception cref="ArgumentException"></exception>
			public List<int> ParseEnumerableOfStringToListOfInt(int rowNumber = 0)
			{
				var strings = (input ?? Enumerable.Empty<string>()).ToList();
				var counter = rowNumber;

				var conversions = strings
					.Select(s => new { Good = int.TryParse(s, out var v), Value = v, Counter = counter++, Input = s })
					.ToList();

				//	If all good, return the list of converted values
				if (conversions.All(c => c.Good))
					return conversions.OrderBy(o => o.Counter).Select(s => s.Value).ToList();

				//	Report the errors
				var errors = conversions
					.Where(c => !c.Good)
					.Select(c => new ArgumentException($"{(rowNumber == 0 ? "Part" : "Row")}# {1 + c.Counter} contains '{c.Input}' which is not a valid int"))
					.ToList();
				throw new AggregateException($"{nameof(ParseEnumerableOfStringToListOfInt)} reports conversion errors", errors);
			}

			/// <summary>
			/// Iterate over the <paramref name="input"/>, splitting into sections identified by empty lines
			/// </summary>
			/// <returns>The <paramref name="input"/>, split into sections</returns>
			public List<List<string>> ParseEnumerableOfStringToListOfListOfString()
			{
				var result = new List<List<string>>();
				var section = new List<string>();

				foreach (var line in input)
				{
					if (string.IsNullOrWhiteSpace(line))
					{
						if (section.Count == 0)
							continue;
						result.Add(section);
						section = [];
					}
					else
						section.Add(line);
				}

				if (section.Count > 0)
					result.Add(section);

				return result;
			}

			/// <summary>
			/// Iterate over <paramref name="input"/>, splitting each line into a list of integers
			/// </summary>
			/// <returns>A list of lists of integer parts</returns>
			public List<List<int>> ParseEnumerableOfStringToListOfListOfInt(int rowNumber = 0)
			{
				var strings = (input ?? Enumerable.Empty<string>()).ToList();

				return strings.Select(line => line.ParseStringToListOfInt(++rowNumber))
					.ToList();
			}

			/// <summary>
			/// Parse the input into a list of long integer values. One value per row
			/// </summary>
			/// <param name="rowNumber">(Optional) specifies the row number in a file</param>
			/// <returns>The list of long integer parts</returns>
			/// <exception cref="ArgumentException"></exception>
			public List<long> ParseEnumerableOfStringToListOfLong(int rowNumber = 0)
			{
				var strings = (input ?? Enumerable.Empty<string>()).ToList();
				var counter = 1;

				var conversions = strings
					.Select(s => new { Good = long.TryParse(s, out var v), Value = v, Counter = counter++, Input = s })
					.ToList();

				//	If all good, return the list of converted values
				if (conversions.All(c => c.Good))
					return conversions.OrderBy(o => o.Counter).Select(s => s.Value).ToList();

				//	Report the errors
				var errors = conversions
					.Where(c => !c.Good)
					.Select(c => new ArgumentException($"{(rowNumber == 0 ? "" : $"Row# {rowNumber}, ")}Part# {c.Counter} contains '{c.Input}' which is not a valid long"))
					.ToList();
				throw new AggregateException($"{nameof(ParseEnumerableOfStringToListOfLong)} reports conversion errors", errors);
			}

			/// <summary>
			/// Iterate over <paramref name="input"/>, splitting each line into a list of long values
			/// </summary>
			/// <returns>A list of lists of <see cref="long"/> parts</returns>
			public List<List<long>> ParseEnumerableOfStringToListOfListOfLong(int rowNumber = 0)
			{
				var strings = (input ?? Enumerable.Empty<string>()).ToList();

				return strings
					.Select(line => line.ParseStringToListOfLong(rowNumber == 0 ? 0 : rowNumber++))
					.ToList();
			}
		}
	}
}