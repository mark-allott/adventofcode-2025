using System.ComponentModel;
using AdventOfCode.Enums;

namespace AdventOfCode.Extensions
{
	internal static class EnumExtensions
	{
		/// <summary>
		/// Extracts the <see cref="DescriptionAttribute"/> from the enum value, if present
		/// </summary>
		/// <param name="value">The enum value to extract</param>
		/// <returns>The value of the <see cref="DescriptionAttribute"/>, or the ToString equivalent</returns>
		public static string Description(Enum value)
		{
			//	Get the FieldInfo details for the enum value
			var fi = value.GetType().GetField($"{value}");

			//	Extract [Description("")] values. If found, return first description, or ToString of value
			return fi?.GetCustomAttributes(typeof(DescriptionAttribute), false) is not DescriptionAttribute[] descriptions || descriptions.Length == 0
				? $"{value}"
				: descriptions[0].Description;
		}

		/// <summary>
		/// Converts the <paramref name="input"/> to the equivalent <see cref="TachyonSplitterState"/> value
		/// </summary>
		/// <param name="input">The character to convert</param>
		/// <returns>The <see cref="TachyonSplitterState"/> value</returns>
		public static TachyonSplitterState ToState(this char input)
		{
			return input switch
			{
				'S' => TachyonSplitterState.Start,
				'.' => TachyonSplitterState.Empty,
				'^' => TachyonSplitterState.Splitter,
				'|' => TachyonSplitterState.Beam,
				_ => TachyonSplitterState.Unknown
			};
		}

		/// <summary>
		/// Converts the <paramref name="state"/> into the default characters
		/// </summary>
		/// <param name="state">The cell state</param>
		/// <returns>The character for the state</returns>
		public static char ToChar(this TachyonSplitterState state)
		{
			return state switch
			{
				TachyonSplitterState.Start => 'S',
				TachyonSplitterState.Empty => '.',
				TachyonSplitterState.Splitter => '^',
				TachyonSplitterState.Beam => '|',
				_ => ' '
			};
		}
	}
}