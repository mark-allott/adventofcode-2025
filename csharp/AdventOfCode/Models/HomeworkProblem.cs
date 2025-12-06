using AdventOfCode.Enums;

namespace AdventOfCode.Models
{
	public record HomeworkProblem
	{
		private HomeworkProblemType ProblemType { get; init; }
		private long[] Inputs { get; init; }

		public HomeworkProblem(HomeworkProblemType problemType, params long[] values)
		{
			ProblemType = problemType;
			Inputs = values;
		}

		private long Add
		{
			get
			{
				return Inputs.Sum();
			}
		}

		private long Multiply
		{
			get
			{
				return Inputs.Aggregate(1L, (a, v) => a * v);
			}
		}

		public long Result
		{
			get
			{
				return ProblemType switch
				{
					HomeworkProblemType.Addition => Add,
					HomeworkProblemType.Multiplication => Multiply,
					_ => throw new NotImplementedException()
				};
			}
		}
	}
}