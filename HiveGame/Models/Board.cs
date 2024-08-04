using Hive.Core.Models.Coordinate;

namespace Hive.Core.Models;

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
	///	MakeMove makes a move on the board and checks if the move to be made is valid.
	/// </summary>
	/// <exception cref="IllegalMoveException"></exception>
	public void MakeMove(Move move, Player player)
	{
		// Ensures that it is checked if move is allowed to be made
		// Method will throw Exception if not allowed to make move
		AllowedToMakeMove(move, player);

		MakeMoveNoCheck(move, player);
	}

	/// <summary>
	/// Makes a move on the board without checking if the move is valid.
	/// </summary>
	/// <param name="move"></param>
	/// <param name="player"></param>
	/// <exception cref="NotImplementedException"></exception>
	private void MakeMoveNoCheck(Move move, Player player)
	{
		// TODO: support attacking
		if (move.MoveType.Equals(MoveType.Attack))
		{
			throw new NotImplementedException();
		}

		// MoveType is place
		_Pieces.Add(move.Piece);

		// TODO; remove piece from player
	}

	#region Helpers

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
		// Since this function should normally return true, as it's expected behavior,
		// an exception will be thrown.

		// Easy check
		if (move.Piece.Color != player.Color)
		{
			throw new IllegalColorException($"Move color {move.Piece.Color} is not the same as the player's color {player.Color}");
		}

		// if space (including height) is already occupied, throw error
		if (PositionOccupied(move.Piece.Position, move.Piece.Height))
		{
			throw new IllegalPlacementException("There already exists a piece at that location");
		}

		// Place moves
		if (move.MoveType.Equals(MoveType.Place))
		{
			// does the player have enough pieces?
			if (!player.Pieces.Any(p => p.Equals(move.Piece.Bug)))
			{
				throw new IllegalPiecesAmountException($"Player {player.Playername} does not have enough pieces");
			}

			// on placing, is the placed piece next to a different colored piece?
			if (SurroundingPieces(move.Piece.Position).Any(p => p.Color == player.Color.GetOtherColor()))
			{
				throw new IllegalPlacementException("Placed piece must not be placed near a piece of a different color");
			}
		}
		else
		{
			// atack moves
			// TODO

			// Check if piece can travel that way
		}


		// Simulate move being made
		Board newBoard = Copy();
		newBoard.MakeMoveNoCheck(move, player);

		// All pieces should be connected
		if (!newBoard.AllPiecesConnected())
		{
			throw new IllegalPieceConnectionException("All pieces must be connected");
		}

		return true;
	}

	public bool PositionOccupied(Cube position, int height)
	{
		return _Pieces.Exists(p => position.Q == p.Position.Q
		&& position.R == p.Position.R
		&& position.S == p.Position.S
		&& p.Height == height);
	}

	public List<Piece> SurroundingPieces(Cube position)
	{
		return _Pieces.Where(p =>
			Math.Abs(p.Position.Q - position.Q) == 1 ||
			Math.Abs(p.Position.R - position.R) == 1 ||
			Math.Abs(p.Position.S - position.S) == 1)
		.ToList();
	}

	public static List<Cube> SurroundingPositions(Cube position)
	{
		int q = position.Q;
		int r = position.R;
		int s = position.S;
		return [
			new(q - 1, r, s),
			new(q + 1, r, s),
			new(q, r - 1, s),
			new(q, r + 1, s),
			new(q, r, s - 1),
			new(q, r, s + 1)
		];
	}

	public bool HasWinCondition()
	{
		// Queen surrounded means wincondition
		return _Pieces.Where(p => p.BugType == (int)BugType.Queen)
			.Any(p => SurroundingPieces(p.Position).Count == 6);
	}

	/// <summary>
	/// Returns a list of positions that are illegal to place given a certain color
	/// </summary>
	/// <param name="c">The color</param>
	/// <returns></returns>
	public List<Cube> IllegalPlacePositions(Color c)
	{
		List<Cube> positions = [];

		// Go along every piece of the other color, and create a list of pieces next to it
		foreach (var piece in _Pieces.Where(p => p.Color != c))
		{
			positions.AddRange(SurroundingPositions(piece.Position));
		}

		// Then filter out the pieces that are already on a piece
		List<Cube> piecePositions = _Pieces.Select(p => p.Position).ToList();

		// Then remove all piecePositions from positions
		return positions.Where(p => !piecePositions.Contains(p)).ToList();
	}

	public bool AllPiecesConnected()
	{
		if (Pieces.Count == 0)
		{
			return true;
		}

		// Check if all pieces are connected using something similar to Dijkstra (without the distance part)

		List<Cube> visited = [];
		List<Cube> unvisited = _Pieces.Select(p => p.Position).ToList();
		List<Cube> lastVisited = [];

		// Initialise visted and lastvisited to the first piece that we're exploring from
		Cube startPiece = _Pieces.First().Position;
		visited.Add(startPiece);
		lastVisited.Add(startPiece);

		while (true)
		{
			if (lastVisited.Count == 0)
			{
				return visited.Count == _Pieces.Count;
			}

			// Get surrounding pieces closest to last visited
			List<Cube> closest = [];
			foreach (Cube lv in lastVisited)
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
