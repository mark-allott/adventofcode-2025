using AdventOfCode.Enums;
using AdventOfCode.Extensions;
using AdventOfCode.Interfaces;

namespace AdventOfCode.Models
{
	internal class TheatreGrid
		: IGenericGrid<TheatreGridCell, TileColour, TheatreCoordinate, int>
	{
		#region Fields

		/// <summary>
		/// Holds the range for the grid - a max. of 100,000 * 100,000 cells
		/// </summary>
		private readonly TheatreCoordinate _bounds;

		/// <summary>
		/// Holds the cell details
		/// </summary>
		private readonly Dictionary<TheatreCoordinate, TileColour> _gridCells;

		/// <summary>
		/// Holds the range of cells for rows in the grid
		/// </summary>
		private readonly List<TheatreCoordinateRange> _coordinateRanges = [];

		#endregion

		#region IGenericGrid<TheatreCoordinate,TileColour> Members

		/// <inheritdoc/>
		public TheatreGridCell this[int x, int y]
		{
			get
			{
				ValidateCoords(x, y);
				var coord = new TheatreCoordinate(x, y);
				return _gridCells.TryGetValue(coord, out var colour)
					? new TheatreGridCell(coord, colour)
					: null!;
			}
			set
			{
				ValidateCoords(x, y);
				var coord = new TheatreCoordinate(x, y);
				_gridCells.TryAdd(coord, value.State);
			}
		}

		/// <inheritdoc/>
		public TheatreGridCell this[TheatreCoordinate coord]
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

		/// <summary>
		/// Returns all cells in the grid with a red tile
		/// </summary>
		public IEnumerable<TheatreGridCell> RedTileCells
		{
			get
			{
				return _gridCells.Where(q => q.Value == TileColour.Red)
					.OrderBy(o => o.Key.Y)
					.ThenBy(o => o.Key.X)
					.Select(s => new TheatreGridCell(s.Key, s.Value));
			}
		}

		#endregion

		#region Constructors

		public TheatreGrid(int width, int height)
		{
			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(width, 100000);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(height, 100000);
			_bounds = new TheatreCoordinate(width, height);
			_gridCells = new Dictionary<TheatreCoordinate, TileColour>();
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

			//	Convert to a list of coords in the theatre
			var coordList = strings.ParseEnumerableOfStringToListOfListOfInt()
				.Select(m => new TheatreCoordinate(m[0], m[1]))
				.OrderBy(o => o.Y)
				.ThenBy(o => o.X)
				.ToList();

			//	Assign each coord in the grid a red tile
			coordList.ForEach(c => this[c] = new TheatreGridCell(c, TileColour.Red));

			SetGreenTiles(coordList);
			SetFilledArea();
		}

		/// <summary>
		/// Places green tile in the grid, connecting between the red tiles
		/// </summary>
		/// <param name="coordList">The list of locations with red tiles</param>
		private void SetGreenTiles(List<TheatreCoordinate> coordList)
		{
			// Get a queue for the coords needing to be processed
			var coords = GetCoordQueue(coordList);

			//	Get the first location of a tile
			var first = coords.Dequeue();
			//	Re-queue it as the end-point
			coords.Enqueue(first);

			//	First step is to create the enclosing area
			while (coords.Count > 0)
			{
				var second = coords.Dequeue();
				var dx = Math.Abs(first.X - second.X);
				var dy = Math.Abs(first.Y - second.Y);

				var range = (dx > 0
						? Enumerable.Range(Math.Min(first.X, second.X) + 1, dx - 1)
						: Enumerable.Range(Math.Min(first.Y, second.Y) + 1, dy - 1))
					.ToList();
				range.ForEach(v =>
				{
					var greenTileLocation = dx != 0
						? new TheatreCoordinate(v, first.Y)
						: new TheatreCoordinate(first.X, v);
					this[greenTileLocation] = new TheatreGridCell(greenTileLocation, TileColour.Green);
				});
				first = second;
			}
		}

		/// <summary>
		/// Creates a queue of locations from <paramref name="coordList"/> that will describe a route around the grid
		/// </summary>
		/// <param name="coordList">The list of coordinates for the red tiles</param>
		/// <returns>A queue of <see cref="TheatreCoordinate"/> objects which represent the order of tiles to be linked</returns>
		/// <exception cref="ArgumentException"></exception>
		private Queue<TheatreCoordinate> GetCoordQueue(List<TheatreCoordinate> coordList)
		{
			var orderedCoords = coordList.OrderBy(o => o.Y).ThenBy(o => o.X).ToList();
			//	Make a queue for the coords needing to be processed
			var coords = new Queue<TheatreCoordinate>();
			//	Get the first coord in the top-left corner as the starting point
			var current = orderedCoords.First();
			orderedCoords.Remove(current);
			coords.Enqueue(current);

			while (orderedCoords.Count > 0)
			{
				var nextXCoord = orderedCoords.FirstOrDefault(q => q.Y == current.Y && q.X != current.X);
				var nextYCoord = orderedCoords.FirstOrDefault(q => q.X == current.X && q.Y != current.Y);

				var nextCoord = nextXCoord ?? nextYCoord;
				if (nextCoord is null)
					throw new ArgumentException("Cannot determine next coordinate");
				coords.Enqueue(nextCoord);
				orderedCoords.Remove(nextCoord);
				current = nextCoord;
			}

			return coords;
		}

		/// <summary>
		/// Fill the enclosed area with green tiling
		/// </summary>
		private void SetFilledArea()
		{
			//	Second stage is to "fill" the enclosed areas
			var gridRows = _gridCells
				.Select(s => s.Key)
				.GroupBy(g => g.Y)
				.Select(s => new { Row = s.Key, Cells = s.OrderBy(o => o.X).ToList() })
				.OrderBy(o => o.Row)
				.ToList();

			foreach (var row in gridRows)
			{
				var pairs = row.Cells[..^1]
					.Select((l, i) => new { Left = l, Right = row.Cells[i + 1] })
					.ToList();

				TheatreCoordinateRange? range = null;

				for (var i = 0; i < pairs.Count; i++)
				{
					if (range is null)
					{
						range = new TheatreCoordinateRange(pairs[i].Left, pairs[i].Right);
						continue;
					}

					if (pairs[i].Left.X == range.EndX)
					{
						range = new TheatreCoordinateRange(range.X, pairs[i].Right.X, row.Row);
						continue;
					}

					if (pairs.Count < i + 1)
						throw new ArgumentException("oops");

					//	Work out the remaining pairs, if an odd number, then take the next one, adding it to the current range and loop around
					var remaining = pairs[(i + 1)..];
					if (remaining.Count % 2 != 1)
						throw new ArgumentException("oops - not enough pairs");

					range = new TheatreCoordinateRange(pairs[i].Left, remaining.First().Right);
					_coordinateRanges.Add(range);
					range = null;
				}

				if (range is not null)
					_coordinateRanges.Add(range);
			}
		}

		/// <summary>
		/// Checks whether the passed coordinate locations are wholly within the tiled ranges
		/// </summary>
		/// <param name="c1">The first red tile location</param>
		/// <param name="c2">The second red tile location</param>
		/// <returns>True if all tiles between the two coordinates exist</returns>
		public bool RangesContain(TheatreCoordinate c1, TheatreCoordinate c2)
		{
			var minX = Math.Min(c1.X, c2.X);
			var maxX = Math.Max(c1.X, c2.X);
			var minY = Math.Min(c1.Y, c2.Y);
			var maxY = Math.Max(c1.Y, c2.Y);

			var ranges = _coordinateRanges.Where(q => q.Y >= minY && q.Y <= maxY)
				.ToList();
			return ranges.Count == 1 + maxY - minY &&
			       ranges.All(r => r.X <= minX && r.EndX >= maxX);
		}

		#endregion
	}
}