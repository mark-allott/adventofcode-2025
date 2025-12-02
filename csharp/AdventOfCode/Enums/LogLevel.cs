using System.ComponentModel;

namespace AdventOfCode.Enums
{
	public enum LogLevel
	{
		[Description("Unkn")]
		Unknown,
		[Description("Trce")]
		Trace,
		[Description("Dbug")]
		Debug,
		Info,
		[Description("Warn")]
		Warning,
		[Description("Err ")]
		Error,
		[Description("Crit")]
		Critical,
		None
	}
}