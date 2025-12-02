using System;
using System.Diagnostics;
using System.Reflection;
using AdventOfCode.Interfaces;
using ILogger = AdventOfCode.Interfaces.ILogger;
using LogLevel = AdventOfCode.Enums.LogLevel;

namespace AdventOfCode.Challenges
{
	public class BaseDailyChallenge
		: IDailyChallenge
	{
		#region ctor

		/// <summary>
		/// Base constructor for the daily challenges
		/// </summary>
		/// <param name="logger">An instance of the </param>
		/// <param name="dayNumber"></param>
		/// <param name="filename"></param>
		/// <param name="title"></param>
		/// <exception cref="ArgumentNullException"></exception>
		protected BaseDailyChallenge(ILogger logger, int dayNumber, string filename = "", string title = "")
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
			ArgumentOutOfRangeException.ThrowIfLessThan<int>(dayNumber, 1, nameof(dayNumber));
			ArgumentOutOfRangeException.ThrowIfGreaterThan<int>(dayNumber, 25, nameof(dayNumber));
			ArgumentException.ThrowIfNullOrWhiteSpace(filename, nameof(filename));

			DayNumber = dayNumber;
			Filename = filename;
			ChallengeTitle = title;
			//	Blank out the result strings
			PartOneResult = PartTwoResult = string.Empty;

			//	Extract testing and reset details
			PartOneTest = this is IPartOneTest p1
				? p1.Test
				: null;
			PartTwoTest = this is IPartTwoTest p2
				? p2.Test
				: null;
			ResetTest = this is IResettable r
				? r.Reset
				: null;

			//	Extract the challenge execution methods
			PartOneRunner = this is IPartOne r1
				? r1.Run
				: null;
			PartTwoRunner = this is IPartTwo r2
				? r2.Run
				: null;
		}

		/// <summary>
		/// The test to be executed for part one
		/// </summary>
		private Action? PartOneTest { get; }

		/// <summary>
		/// The test to be executed for part two
		/// </summary>
		private Action? PartTwoTest { get; }

		/// <summary>
		/// The method called after a test completes to perform a reset of the internal state
		/// </summary>
		private Action? ResetTest { get; }

		/// <summary>
		/// The method to be called to execute part one of the challenge
		/// </summary>
		private Func<bool>? PartOneRunner { get; }

		/// <summary>
		/// The method to be called to execute part two of the challenge
		/// </summary>
		private Func<bool>? PartTwoRunner { get; }

		/// <summary>
		/// The logger implementation to handle message output
		/// </summary>
		protected ILogger Logger { get; }

		/// <summary>
		/// Holds details of the "data" folder, in which the individual days
		/// </summary>
		private static string? _dataFolder;

		#endregion

		#region IDailyChallenge implementation

		public int DayNumber { get; protected set; }

		public string Filename { get; protected set; }

		public string PartOneResult { get; protected set; }

		public string PartTwoResult { get; protected set; }

		public string ChallengeTitle { get; private set; }

		/// <inheritdoc/>
		public bool Execute()
		{
			try
			{
				ExecuteTests();
				LoadAndReadFile();
				var result1 = ExecutePartOne();
				var result2 = ExecutePartTwo();
				return result1 && result2;
			}
			catch (Exception e)
			{
				Logger.Log(LogLevel.Critical, e.Message);
				return false;
			}
		}

		/// <summary>
		/// Executes the required steps to perform part one of the daily challenge
		/// </summary>
		/// <returns>True if executed successfully, false if an error occurs</returns>

		private bool ExecutePartOne()
		{
			//	There is always a part one, so a missing runner is a failed execution attempt
			if (PartOneRunner is null)
			{
				Logger.Log(LogLevel.Info, "No runner found for part one");
				return false;
			}

			try
			{
				var sw = Stopwatch.StartNew();
				try
				{
					return PartOneRunner.Invoke();
				}
				finally
				{
					sw.Stop();
					PartOneResult = $"{PartOneResult} ({sw.ElapsedMilliseconds}ms / {sw.Elapsed.Ticks} Ticks)";
				}
			}
			catch (Exception e)
			{
				PartOneResult = e.Message;
				return false;
			}
		}

		/// <summary>
		/// Executes the required steps to perform part two of the daily challenge
		/// </summary>
		/// <returns>True if executed successfully, false if an error occurs</returns>
		private bool ExecutePartTwo()
		{
			//	Part two can be optional - either because there isn't one, or it hasn't been attempted yet, so default to a "pass"
			if (PartTwoRunner is null)
			{
				Logger.Log(LogLevel.Info, "No runner found for part two");
				return true;
			}

			try
			{
				var sw = Stopwatch.StartNew();
				try
				{
					return PartTwoRunner.Invoke();
				}
				finally
				{
					sw.Stop();
					PartTwoResult = $"{PartTwoResult} ({sw.ElapsedMilliseconds}ms / {sw.Elapsed.Ticks} Ticks)";
				}
			}
			catch (Exception e)
			{
				PartTwoResult = e.Message;
				return false;
			}
		}

		/// <summary>
		/// If the class is testable (by decorating with IPartOneTestable, IPartTwoTestable or ITestable), run those tests
		/// </summary>
		private void ExecuteTests()
		{
			var sw = new Stopwatch();
			if (PartOneTest is not null)
			{
				Logger.Log(LogLevel.Info, "Executing tests for Part One");
				sw.Start();
				PartOneTest();
				sw.Stop();
				ResetTest?.Invoke();
				Logger.Log(LogLevel.Info, $"Part One tests completed ({sw.Elapsed.Ticks} Ticks)");
			}

			if (PartTwoTest is null)
				return;

			sw.Reset();
			Logger.Log(LogLevel.Info, "Executing tests for Part Two");
			sw.Start();
			PartTwoTest();
			sw.Stop();
			ResetTest?.Invoke();
			Logger.Log(LogLevel.Info, $"Part Two tests completed ({sw.Elapsed.Ticks} Ticks)");
		}

		#endregion

		#region Utility methods

		/// <summary>
		/// Holds the "raw" contents of the input file
		/// </summary>
		protected List<string> InputFileLines = null!;

		/// <summary>
		/// Loads the file specified in the constructor from the data folder
		/// </summary>
		/// <param name="forceReload">Indicates whether a reload of the data is required</param>
		private void LoadAndReadFile(bool forceReload = false)
		{
			//	Don't re-read data if already present, or forced
			if (!(InputFileLines is null || InputFileLines.Count == 0 || forceReload))
				return;

			if (string.IsNullOrWhiteSpace(_dataFolder))
				_dataFolder = GetDataFolder();

			//	Create the path to the filename by combining the data path
			var inputFilePath = Path.Combine(_dataFolder, Filename);
			InputFileLines = new List<string>(File.ReadAllLines(inputFilePath));
		}

		/// <summary>
		/// Determines the location of the "data" folder within the filesystem by walking backwards through the filesystem
		/// to locate the first "data" folder in a parent folder of the programs execution location
		/// </summary>
		/// <returns>The path to the data folder</returns>
		/// <remarks>
		/// Locates the normal location for the data folder, following this pattern:
		/// <code>
		/// path
		///  \
		///   + to
		///    \
		///     + AdventOfCode
		///      \
		///       + year (e.g. 2025)
		///	      |\
		///       | + {program source folder}
		///       |  \
		///       |   + bin/Debug/netxx.x/
		///       | data
		/// </code>
		/// </remarks>
		private static string GetDataFolder()
		{
			//	Get the execution location path
			var execFullPath = Assembly.GetExecutingAssembly().Location;
			//	Just get the folder names
			var testPath = Path.GetDirectoryName(execFullPath);

			while (!string.IsNullOrWhiteSpace(testPath))
			{
				var dataFolders = Directory.EnumerateDirectories(testPath, "data", SearchOption.TopDirectoryOnly)
					.ToList();
				if (dataFolders.Count != 0)
					return dataFolders[0];
				testPath = Path.GetDirectoryName(testPath);
			}

			throw new DirectoryNotFoundException("data folder not found");
		}

		#endregion
	}
}