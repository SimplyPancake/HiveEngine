using System.Numerics;

namespace HiveGame;

public class Piece
{
	public Color Color { get; }
	public int BugType { get; }
	public Vector3 Position { get; }

	public Piece(Color c, int b)
	{
		BugType = b;
		Color = c;
		Position = new Vector3(0, 0, 0);
	}

	public Piece(Color c, int b, Vector3 position)
	{
		Color = c;
		BugType = b;
		Position = position;
	}

	public static List<Vector3> PossibleMoves(Board board)
	{
		return new List<Vector3> { };
	}
}
