using System.Numerics;

namespace HiveGame;

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
	public Vector3 Position { get; }
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
	}

	public static List<Vector3> PossibleMoves(Board board)
	{
		return new List<Vector3> { };
	}
}
