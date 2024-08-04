using System.Numerics;
using Hive.Core.Models.Coordinate;

namespace Hive.Core.Models;

public class Piece
{
	public Color Color { get; }
	public int BugType
	{
		get
		{
			return Bug.BugTypeId;
		}
	}
	public Cube Position { get; set; }
	public int Height { get; set; } = 0;
	public Bug Bug { get; }

	public Piece(Color c, Bug bug)
	{
		Bug = bug;
		Color = c;
		Position = new Cube(0, 0, 0);
	}

	public Piece(Color c, Bug bug, Cube position)
	{
		Color = c;
		Bug = bug;
		Position = position;
		// height is default 0.
	}

	public Piece(Color c, Bug bug, Cube position, int height)
	{
		Color = c;
		Bug = bug;
		Position = position;
		Height = height;
	}

	public List<Move> PossibleMoves(Board board)
	{
		return Bug.PossibleMoves(this, board);
	}

	public virtual bool Equals(Piece obj)
	{
		if (obj == null) return false;
		if (ReferenceEquals(this, obj)) { return true; }

		return GetHashCode() == obj.GetHashCode();
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Color, Bug, Position, Height);
	}
}
