using Hive.Core;
using Hive.Core.Models;

namespace Hive.Console.Visualiser;

public class GridPiece
{
	public Color Color { get; }
	public Axial Position { get; set; }
	public int Height { get; set; } = 0;
	public Bug Bug { get; }

	public GridPiece(Color c, Axial pos, int height, Bug b)
	{
		Color = c;
		Position = pos;
		Height = height;
		Bug = b;
	}

	public GridPiece(Color c, Axial pos, Bug b)
	{
		Color = c;
		Position = pos;
		Height = 0;
		Bug = b;
	}

	public GridPiece(Piece p)
	{
		Color = p.Color;
		Position = new Axial(p.Position);
		Height = p.Height;
		Bug = p.Bug;
	}

}
