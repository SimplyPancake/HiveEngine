using Hive.Core;
using Hive.Core.Models;
using Hive.Core.Models.Bugs;
using Hive.Core.Models.Coordinate;

namespace Hive.Console.Visualiser;

public class GridPiece
{
	public Color Color { get; }
	public Axial Position { get; set; }
	public int Height { get; set; } = 0;
	public Bug Bug { get; }

	public Cube OriginalPosition { get; set; }

	public GridPiece(Color c, Axial pos, int height, Bug b, Cube cube)
	{
		Color = c;
		Position = pos;
		Height = height;
		Bug = b;
		OriginalPosition = cube;
	}

	public GridPiece(Color c, Axial pos, Bug b, Cube cube)
	{
		Color = c;
		Position = pos;
		Height = 0;
		Bug = b;
		OriginalPosition = cube;
	}

	public GridPiece(Piece p)
	{
		Color = p.Color;
		Position = new Axial(p.Position);
		Height = p.Height;
		Bug = p.Bug;
		OriginalPosition = p.Position;
	}
}
