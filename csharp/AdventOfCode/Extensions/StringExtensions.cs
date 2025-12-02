namespace AdventOfCode.Extensions
{
	public static class StringExtensions
	{
		/// <summary>
		/// Parse the input into a list of integer values. Values can be separated by spaces or commas
		/// </summary>
		/// <param name="line">The string to parse</param>
		/// <param name="rowNumber">(Optional) specifies the row number in a file</param>
		/// <returns>The list of integer parts</returns>
		/// <exception cref="ArgumentException"></exception>
		public static List<int> ParseStringToListOfInt(this string line, int rowNumber = 0)
		{
			var parts = line.Split([' ', ','], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
			return parts.ParseEnumerableOfStringToListOfInt(rowNumber);
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
				var counter = 1;
				var rowValues = new List<int>();
				var strings = input as string[] ?? [];

				foreach (var part in strings)
				{
					if (!int.TryParse(part, out var v))
					{
						var message = $"Data error with line {(rowNumber != 0 ? $"{rowNumber}" : $"{string.Join(',', strings)}")} and part #{counter} => '{part}'";
						throw new ArgumentException(message);
					}

					counter++;
					rowValues.Add(v);
				}

				return rowValues;
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
						if (section.Count > 0)
						{
							result.Add(section);
							section = new List<string>();
						}
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
				var strings = input as string[] ?? [];

				return strings.Select(line => line.ParseStringToListOfInt(++rowNumber))
					.ToList();
			}
		}
	}
}