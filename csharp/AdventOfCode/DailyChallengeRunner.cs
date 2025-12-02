using AdventOfCode.Enums;
using AdventOfCode.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdventOfCode
{
	public class DailyChallengeRunner
		: IHostedService
	{
		/// <summary>
		/// Holds configuration information
		/// </summary>
		private IConfiguration Configuration { get; }

		/// <summary>
		/// Holds the service provider instance
		/// </summary>
		private IServiceProvider ServiceProvider { get; }

		/// <summary>
		/// Holds an instance of the logger to be used for directing messages to the appropriate location
		/// </summary>
		private ILogger Logger { get; }

		/// <summary>
		/// A pre-defined string that can be used to separate the output from multiple days running
		/// </summary>
		private readonly string _challengeSeparator = string.Format("{0}{1}{2}{1}{0}", new string('=', 5), new string(' ', 5), new string('-', 60));

		#region IHostedService implementation

		public Task StartAsync(CancellationToken cancellationToken)
		{
			//	Try to find out what needs to be run
			var x = Configuration["run"];
			var challenges = GetDailyChallenges();

			//	Check: are there any challenges to be run?
			if (challenges.Count == 0)
			{
				Logger.Log(LogLevel.Error, "No challenges to run!");
				return Task.CompletedTask;
			}

			//	run using the different options
			if (string.IsNullOrWhiteSpace(x))
			{
				//	Nothing specific is supplied, so execute the challenge with the greatest day number value
				RunChallenge(challenges.Last());
				return Task.CompletedTask;
			}

			List<IDailyChallenge>? challengesToRun = [];
			if (x.Equals("all", StringComparison.OrdinalIgnoreCase))
			{
				challengesToRun.AddRange(challenges);
			}
			else
			{
				//	If here then we assume a list of days is present, so we need to
				//	convert to integers to find out what these days are
				var days = x.Split([',', ' '], StringSplitOptions.RemoveEmptyEntries)
					.Select(s => int.TryParse(s, out var result) ? result : int.MinValue)
					.Where(q => q != int.MinValue)
					.OrderBy(o => o)
					.ToList();

				//	Filter the challenges to be run that match any specified days
				challengesToRun.AddRange(challenges.Where(c => days.Contains(c.DayNumber)));
			}

			//	Run the requested daily challenges
			challengesToRun.ForEach(c => RunChallenge(c));

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		#endregion

		#region ctor

		public DailyChallengeRunner(ILogger logger, IConfiguration configuration, IServiceProvider serviceProvider)
		{
			//	Verify the inputs to make certain they are valid
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
			Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		#endregion

		/// <summary>
		/// Provide a list of the challenges that the application contains in ascending <see cref="IDailyChallenge.DayNumber"/> order
		/// </summary>
		/// <returns>All challenges registered in <see cref="ServiceProvider"/></returns>
		private List<IDailyChallenge> GetDailyChallenges()
		{
			//	Assuming the services provider is valid, obtain a list of daily challenges ordered by day number
			return ServiceProvider.GetServices<IDailyChallenge>()
				.OrderBy(o => o.DayNumber)
				.ToList();
		}

		/// <summary>
		/// Executes the specified challenge, sending output to the console
		/// </summary>
		/// <param name="challenge">The challenge being executed</param>
		/// <exception cref="ArgumentNullException"></exception>
		private void RunChallenge(IDailyChallenge challenge)
		{
			//	No challenge?? Throw an exception
			ArgumentNullException.ThrowIfNull(challenge);

			//	Write some output to the console stating which challenge is being run
			var title = !string.IsNullOrWhiteSpace(challenge.ChallengeTitle)
				? $": {challenge.ChallengeTitle}"
				: string.Empty;
			Logger.Log(LogLevel.Info, $"Running challenge for day {challenge.DayNumber}{title}");

			//	Run the challenge, state whether it was completed successfully
			//	(both part one and part two must be completed successfully to be
			//	stated as completely successful) and display the results
			var result = challenge.Execute();
			Logger.Log(LogLevel.Info, $"Challenge completed {(result ? "" : "un")}successfully");
			Logger.Log(LogLevel.Info, $"Part One solution: {challenge.PartOneResult}");
			Logger.Log(LogLevel.Info, $"Part Two solution: {challenge.PartTwoResult}");

			//	Print a separator line to distinguish between the end of this challenge and any challenges following it
			Logger.Log(LogLevel.Info, _challengeSeparator);
		}
	}
}