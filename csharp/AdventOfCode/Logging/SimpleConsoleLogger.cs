using AdventOfCode.Enums;
using AdventOfCode.Interfaces;

namespace AdventOfCode.Logging
{
	/// <summary>
	/// Very simple logging class sending information to the system console
	/// </summary>
	public sealed class SimpleConsoleLogger
		: ILogger
	{
		/// <summary>
		/// Holds method information about the function used to format the message
		/// </summary>
		private readonly Func<LogLevel, string, string> _formatter;

		/// <summary>
		/// Holds details of the minimum <see cref="LogLevel"/> that shall be output to the console
		/// </summary>
		private LogLevel LogLevel { get; }

		#region Constructor

		/// <summary>
		/// Internal constructor to set all aspects of the logging
		/// </summary>
		/// <param name="level">The minimum level of logging to be directed to the console</param>
		/// <param name="formatter">A function that will be used to format the output for the console</param>
		/// <exception cref="ArgumentNullException"></exception>
		private SimpleConsoleLogger(LogLevel level, Func<LogLevel, string, string> formatter)
		{
			LogLevel = level;
			_formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
		}

		/// <summary>
		/// Simplified constructor, allowing setting of whether the logging is at "debug" level or "error" level
		/// </summary>
		/// <param name="isDebug">A flag indicating if "debug" is needed</param>
		/// <param name="formatter">The function used to format the message</param>
		public SimpleConsoleLogger(bool isDebug, Func<LogLevel, string, string> formatter)
			: this(isDebug ? LogLevel.Debug : LogLevel.Error, formatter)
		{
		}

		/// <summary>
		/// Simple constructor, creates a logger using "debug" level and the default formatter
		/// </summary>
		public SimpleConsoleLogger()
			: this(LogLevel.Debug, SimpleConsoleMessageFormatter.FormatMessage)
		{
		}

		#endregion

		/// <inheritdoc />
		public void Log(LogLevel level, string message)
		{
			if (!IsEnabled(level))
				return;

			var consoleOutput = _formatter(level, message);
			if (level is LogLevel.Error or LogLevel.Critical)
				Console.Error.Write(consoleOutput);
			else
				Console.Write(consoleOutput);
		}

		/// <inheritdoc />
		public bool IsEnabled(LogLevel level)
		{
			return level >= LogLevel;
		}
	}
}