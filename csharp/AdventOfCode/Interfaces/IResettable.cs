namespace AdventOfCode.Interfaces
{
	public interface IResettable
	{
		/// <summary>
		/// Implementation of this method should reset the internal state of the daily challenge to a known state, ready to
		/// be run again (if needed)
		/// </summary>
		void Reset();
	}
}