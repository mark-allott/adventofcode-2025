using AdventOfCode.Enums;

namespace AdventOfCode.Extensions
{
	public static class HomeworkProblemTypeExtensions
	{
		/// <summary>
		/// Converts the <paramref name="input"/> into the corresponding <see cref="HomeworkProblemType"/> value
		/// </summary>
		/// <param name="input">The value to convert</param>
		/// <returns>The <paramref name="input"/> value expressed as a <see cref="HomeworkProblemType"/> value</returns>
		public static HomeworkProblemType ToHomeworkProblemType(this string input)
		{
			return input switch
			{
				"+" => HomeworkProblemType.Addition,
				"*" => HomeworkProblemType.Multiplication,
				_ => HomeworkProblemType.Unknown
			};
		}
	}
}