using System.Text;
using AdventOfCode.Enums;
using AdventOfCode.Interfaces;

namespace AdventOfCode.Models
{
	internal class TachyonSplitterGrid
		: IGenericGrid<TachyonSplitterGridCell, TachyonSplitterState, GridCoordinate, int>
	{
		#region Fields

		/// <summary>
		/// Holds the upper range for the grid bounds (lower bounds are always zero)
		/// </summary>
		private readonly GridCoordinate _bounds;

		/// <summary>
		/// Holds the grid of cells
		/// </summary>
		private readonly TachyonSplitterGridCell[,] _grid;

		/// <summary>
		/// Holds details of a function that can be used to convert the state into a character
		/// </summary>
		private readonly Func<TachyonSplitterState, char>? _cellRenderer;

		/// <summary>
		/// Create a memo store for calculating timelines
		/// </summary>
		private readonly Dictionary<GridCoordinate, long> _timelineMemo = [];

		#endregion

		#region Properties

		/// <summary>
		/// Property to expose all cells in the grid as an IEnumerable
		/// </summary>
		public IEnumerable<TachyonSplitterGridCell> Cells
		{
			get
			{
				return Enumerable.Range(0, _bounds.Y)
					.SelectMany(y => Enumerable.Range(0, _bounds.X).Select(x => this[x, y]));
			}
		}

		/// <summary>
		/// Locates the single cell that is the start location
		/// </summary>
		public TachyonSplitterGridCell StartGridCell
		{
			get
			{
				return Cells.Single(c => c.State == TachyonSplitterState.Start);
			}
		}

		/// <summary>
		/// Locates all cells where the splitters are located in the grid
		/// </summary>
		public IEnumerable<TachyonSplitterGridCell> SplitterCells
		{
			get
			{
				return Cells.Where(c => c.State == TachyonSplitterState.Splitter);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new grid for the Tachyon Beam problem
		/// </summary>
		/// <param name="width">The total width of the beam chamber</param>
		/// <param name="height">The height of the beam chamber</param>
		/// <param name="cellRenderer">The function to convert a cell's state into a character</param>
		public TachyonSplitterGrid(int width, int height, Func<TachyonSplitterState, char> cellRenderer = null!)
		{
			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(width, 1000);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(height, 1000);
			_bounds = new GridCoordinate(width, height);
			_grid = new TachyonSplitterGridCell[height, width];
			_cellRenderer = cellRenderer;
		}

		#endregion

		#region IGenericGrid implementation

		/// <inheritdoc/>
		public TachyonSplitterGridCell this[int x, int y]
		{
			get
			{
				ValidateCoords(x, y);
				return _grid[y, x];
			}
			set
			{
				ValidateCoords(x, y);
				_grid[y, x] = value;
			}
		}

		/// <inheritdoc/>
		public TachyonSplitterGridCell this[GridCoordinate coord]
		{
			get
			{
				return this[coord.X, coord.Y];
			}
			set
			{
				this[coord.X, coord.Y] = value;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Validates that the coordinate values are within the bounds of the grid
		/// </summary>
		/// <param name="x">The x-coordinate</param>
		/// <param name="y">The y-coordinate</param>
		private void ValidateCoords(int x, int y)
		{
			ArgumentOutOfRangeException.ThrowIfNegative(x);
			ArgumentOutOfRangeException.ThrowIfNegative(y);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(x, _bounds.X);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(y, _bounds.Y);
		}

		/// <summary>
		/// Loads the grid with cell details from <paramref name="lines"/>
		/// </summary>
		/// <param name="lines">The cell details</param>
		/// <exception cref="ArgumentException"></exception>
		public void LoadGrid(IEnumerable<string> lines)
		{
			var strings = (lines ?? Enumerable.Empty<string>()).ToList();
			if (strings.Count == 0 ||
			    strings.Count != _bounds.Y ||
			    strings.Any(s => s.Length != _bounds.X))
				throw new ArgumentException("Invalid input for grid", nameof(lines));

			strings
				.SelectMany((s, y) => s.ToCharArray().Select((c, x) => new TachyonSplitterGridCell(x, y, c)))
				.ToList()
				.ForEach(gc => this[gc.Location.X, gc.Location.Y] = gc);
		}

		/// <summary>
		/// Override to provide a "pretty print" of the grid internals
		/// </summary>
		/// <returns>The grid's current state in a string form</returns>
		public override string ToString()
		{
			var lines = Enumerable.Range(0, _bounds.Y)
				.Select(y => new string(Enumerable.Range(0, _bounds.X)
					.Select(x => _cellRenderer?.Invoke(this[x, y].State) ?? this[x, y].ToString()[0]).ToArray()))
				.ToArray();
			var sb = new StringBuilder();
			sb.AppendJoin(Environment.NewLine, lines);
			return sb.ToString();
		}

		/// <summary>
		/// Detects whether a splitter has indeed been used to split beams
		/// </summary>
		/// <returns>The number of splitters where a beam has been split through it</returns>
		public int CalculateSplitBeams()
		{
			RunBeam();
			return SplitterCells.Count(c => HasSplit(c));
		}

		/// <summary>
		/// Runs the beam through the steps needed to reach the end of the grid for part one of the challenge
		/// </summary>
		private void RunBeam()
		{
			//	Beams come in at the start cell, so that's the initial point where beams will originate
			var beamCells = new Queue<TachyonSplitterGridCell>();
			beamCells.Enqueue(StartGridCell);

			while (beamCells.Count > 0)
			{
				//	Grab the cell to work with
				var currentCell = beamCells.Dequeue();
				//	Test the row location is not out of bounds
				if (_bounds.Y == currentCell.Location.Y + 1)
					continue;

				var nextCell = this[currentCell.Location.X, currentCell.Location.Y + 1];

				if (nextCell.State == TachyonSplitterState.Splitter)
				{
					var beamL = this[nextCell.Location.X - 1, nextCell.Location.Y];
					var beamR = this[nextCell.Location.X + 1, nextCell.Location.Y];
					if (beamL.State != TachyonSplitterState.Beam)
					{
						beamL.State = TachyonSplitterState.Beam;
						beamCells.Enqueue(beamL);
					}

					if (beamR.State != TachyonSplitterState.Beam)
					{
						beamR.State = TachyonSplitterState.Beam;
						beamCells.Enqueue(beamR);
					}
				}
				else
				{
					nextCell.State = TachyonSplitterState.Beam;
					beamCells.Enqueue(nextCell);
				}
			}
		}

		/// <summary>
		/// Determines whether a splitter cell has actually performed a split on the beam
		/// </summary>
		/// <param name="gridCell">The cell to be tested</param>
		/// <returns>True if the splitter did split an incoming beam</returns>
		private bool HasSplit(TachyonSplitterGridCell gridCell)
		{
			//	Can't split if cell isn't a splitter and it doesn't have a beam entering it from above
			return gridCell.State.Equals(TachyonSplitterState.Splitter) &&
			       this[gridCell.Location.X, gridCell.Location.Y - 1].State == TachyonSplitterState.Beam &&
			       this[gridCell.Location.X - 1, gridCell.Location.Y].State == TachyonSplitterState.Beam &&
			       this[gridCell.Location.X + 1, gridCell.Location.Y].State == TachyonSplitterState.Beam;
		}

		#endregion

		public long CalculateTimelines()
		{
			RunBeam();
			return FindTimelines();
		}

		private long FindTimelines()
		{
			//	Find all rows with splitters that have split beams
			var splitterRows = SplitterCells.GroupBy(g => g.Location.Y)
				.Select(g => new { Row = g.Key, Splitters = g.Where(c => HasSplit(c)).ToList() })
				.OrderByDescending(o => o.Row)
				.ToList();

			_ = splitterRows.SelectMany(m => m.Splitters.Select(s => FindTimelinesForCell(s)));
			return FindTimelinesForCell(StartGridCell);
		}

		/// <summary>
		/// Find the timeline count for the given cell
		/// </summary>
		/// <param name="cell">The cell being checked</param>
		/// <returns>The number of timelines discovered</returns>
		private long FindTimelinesForCell(TachyonSplitterGridCell cell)
		{
			while(true)
			{
				//	Shortcut: if already known, return the stored value
				if (_timelineMemo.TryGetValue(cell.Location, out var timelines))
					return timelines;

				//	if the cell is a splitter, follow each of the new beams
				if (cell.State.Equals(TachyonSplitterState.Splitter))
				{
					var beamL = this[cell.Location.X - 1, cell.Location.Y];
					var beamR = this[cell.Location.X + 1, cell.Location.Y];
					timelines = FindTimelinesForCell(beamL) + FindTimelinesForCell(beamR);
					return _timelineMemo[cell.Location] = timelines;
				}

				//	If we are at the bottom of the grid, the answer is always 1 for a beam, or zero for empty
				if (cell.Location.Y + 1 >= _bounds.Y)
					return cell.State.Equals(TachyonSplitterState.Beam)
						? 1
						: 0;

				//	Get the next cell down and loop
				cell = this[cell.Location.X, cell.Location.Y + 1];
			}
		}
	}
}