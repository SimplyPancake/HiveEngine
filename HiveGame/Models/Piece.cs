using System.Numerics;
using Hive.Core.Models.Coordinate;
using Hive.Core.Models.Bugs;

namespace Hive.Core.Models;

public class Piece : IEquatable<Piece>
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

	// override object.Equals
	public override bool Equals(object? obj)
	{
		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}

		return GetHashCode() == obj.GetHashCode();
	}

	public bool Equals(Piece? obj)
	{
		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}

		return GetHashCode() == obj.GetHashCode();
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Color, Bug, Position, Height);
	}

	public override string ToString()
	{
		return $"{(Color == Color.Black ? 'b' : 'w')}{Bug.ShortRepresentation}{Height}";
	}
}
