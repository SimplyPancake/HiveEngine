using System.Numerics;

namespace Hive.Core;

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

	public static List<Vector3> PossibleMoves(Board board)
	{
		return new List<Vector3> { };
	}
}
