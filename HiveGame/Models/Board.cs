using System.Numerics;

namespace Hive.Core;

public class Board
{
	// Board structure in Cube form
	public List<Piece> Pieces
	{
		get
		{
			return _Pieces;
		}

	}
	private List<Piece> _Pieces;

	public Board()
	{
		// Always work from the middle	
		_Pieces = new List<Piece>();
	}

	public Board(List<Piece> pieces)
	{
		_Pieces = pieces;
	}

	public List<Piece> ColorPieces(Color color)
	{
		return _Pieces.Where(p => p.Color.Equals(color)).ToList();
	}

	public void Reset()
	{
		_Pieces = new List<Piece>();
	}

	public Board Copy() => Copy(this);

	public static Board Copy(Board board)
	{
		List<Piece> copiesPieces = new List<Piece>();

		foreach (Piece piece in board.Pieces)
		{
			Piece newPiece = new Piece(piece.Color, piece.Bug);
			copiesPieces.Add(newPiece);
		}

		return new Board(copiesPieces);
	}

	/// <summary>
	///	MakeMove makes a move on the board
	/// </summary>
	public void MakeMove(Move move)
	{
		// A new piece will be put in AttackPosition
		// TODO; make this method not public, or verify that the move to be made may be made.
		// (using AllowedToMakeMove)

		// if space (including height) is already occupied, throw error

		// otherwise check height
	}

	#region Helpers

	public List<Piece> SurroundingPieces(Vector3 position)
	{
		return _Pieces.Where(p =>
			Math.Abs(p.Position.X - position.X) == 1 ||
			Math.Abs(p.Position.Y - position.Y) == 1 ||
			Math.Abs(p.Position.Z - position.Z) == 1)
		.ToList();
	}

	public static List<Vector3> SurroundingPositions(Vector3 position)
	{
		float x = position.X;
		float y = position.Y;
		float z = position.Z;
		return new List<Vector3> {
			new (x - 1, y, z),
			new (x + 1, y, z),
			new (x, y - 1, z),
			new (x, y + 1, z),
			new (x, y, z - 1),
			new (x, y, z + 1)
		};
	}

	public bool HasWinCondition()
	{
		// Queen surrounded means wincondition
		return _Pieces.Where(p => p.BugType == (int)BugType.Queen)
			.Any(p => SurroundingPieces(p.Position).Count() == 6);
	}

	/// <summary>
	/// Checks if a submitted move is allowed to be played.
	/// Will be used to see if a Player inputs a valid move,
	/// even though the already possible moves are already generated
	/// </summary>
	/// <param name="move">The move that the player made</param>
	/// <param name="player">The player</param>
	/// <returns></returns>
	public bool AllowedToMakeMove(Move move, Player player)
	{
		// Easy check
		if (move.Piece.Color != player.Color)
		{
			return false;
		}

		// Simulate move being made
		Board newBoard = Copy();
		newBoard.MakeMove(move);

		// All pieces should be connected
		if (!newBoard.AllPiecesConnected())
		{
			return false;
		}

		return true;
	}

	/// <summary>
	/// Returns a list of positions that are illegal to place given a certain color
	/// </summary>
	/// <param name="c">The color</param>
	/// <returns></returns>
	public List<Vector3> IllegalPlacePositions(Color c)
	{
		List<Vector3> positions = new List<Vector3>();

		// Go along every piece of the other color, and create a list of pieces next to it
		foreach (var piece in _Pieces.Where(p => p.Color != c))
		{
			positions.AddRange(SurroundingPositions(piece.Position));
		}

		// Then filter out the pieces that are already on a piece
		List<Vector3> piecePositions = _Pieces.Select(p => p.Position).ToList();

		// Then remove all piecePositions from positions
		return positions.Where(p => !piecePositions.Contains(p)).ToList();
	}

	// TODO: This logic needs to be put into the client
	public void PrintBoard()
	{
		HexagonService.PrintBoard(Pieces);
	}

	public bool AllPiecesConnected()
	{
		if (Pieces.Count == 0)
		{
			return true;
		}

		// Check if all pieces are connected using something similar to Dijkstra (without the distance part)

		List<Vector3> visited = new List<Vector3>();
		List<Vector3> unvisited = _Pieces.Select(p => p.Position).ToList();
		List<Vector3> lastVisited = new List<Vector3>();

		// Initialise visted and lastvisited to the first piece that we're exploring from
		Vector3 startPiece = _Pieces.First().Position;
		visited.Add(startPiece);
		lastVisited.Add(startPiece);

		while (true)
		{
			if (lastVisited.Count == 0)
			{
				return visited.Count == _Pieces.Count;
			}

			// Get surrounding pieces closest to last visited
			List<Vector3> closest = new List<Vector3>();
			foreach (Vector3 lv in lastVisited)
			{
				// Get the locations of closest pieces
				closest.AddRange(SurroundingPieces(lv).Select(p => p.Position));
			}

			// that are unvisited
			closest = closest.Where(unvisited.Contains).ToList();

			// Then visit them
			visited.AddRange(closest);

			// set lastVisited
			lastVisited = closest;

			// remove from unvisited
			unvisited = unvisited.Where(u => !closest.Contains(u)).ToList();
		}
	}

	#endregion
}
