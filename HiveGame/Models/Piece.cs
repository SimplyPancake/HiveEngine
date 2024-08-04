using System.Numerics;

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
	public Vector3 Position { get; set; }
	public int Height { get; set; } = 0;
	public Bug Bug { get; }

	public Piece(Color c, Bug bug)
	{
		Bug = bug;
		Color = c;
		Position = new Vector3(0, 0, 0);
	}

	public Piece(Color c, Bug bug, Vector3 position)
	{
		Color = c;
		Bug = bug;
		Position = position;
		// height is default 0.
	}

	public Piece(Color c, Bug bug, Vector3 position, int height)
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
