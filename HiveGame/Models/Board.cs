using Hive.Core.Models.Coordinate;
using Hive.Core.Services;

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
		_Pieces = [];
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
		_Pieces = [];
	}

	public Board Copy() => Copy(this);

	public static Board Copy(Board board)
	{
		List<Piece> copiesPieces = new(board.Pieces);

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

		MakeMoveNoCheck(move);

		player.Pieces.Remove(move.Piece.Bug);
		// TODO; remove piece from player
	}

	/// <summary>
	/// Makes a move on the board without checking if the move is valid.
	/// </summary>
	/// <param name="move"></param>
	private void MakeMoveNoCheck(Move move)
	{
		if (move.MoveType == MoveType.Activate)
		{
			throw new NotImplementedException("Need to implement activating a piece");
		}

		if (move.MoveType == MoveType.Place)
		{
			// MoveType is place
			_Pieces.Add(move.Piece);
			return;
		}

		AttackMove attackMove = (AttackMove)move;

		// Get the original piece and update it's location
		Piece toMove = _Pieces.First(p => p.Equals(attackMove.Piece));
		toMove.Position = attackMove.AttackPosition;
		toMove.Height = attackMove.AttackHeight;
	}

	#region Helpers

	public Board SimulateMove(Move move)
	{
		Board newBoard = Copy();
		newBoard.MakeMoveNoCheck(move);
		return newBoard;
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
			if (!player.Pieces.Contains(move.Piece.Bug))
			{
				throw new IllegalPiecesAmountException($"Player {player.Playername} does not have enough pieces");
			}

			// on placing, is the placed piece next to a different colored piece?
			if (HighestPieces().Where(hp => SurroundingPositions(move.Piece.Position).Contains(hp.Position)).Any(p => p.Color == player.Color.GetOtherColor()))
			{
				throw new IllegalPlacementException("Placed piece must not be placed near a piece of a different color");
			}
		}
		else
		{
			// Problem for later; attacking
			throw new NotImplementedException("Can't attack yet!");
		}


		// Simulate move being made
		Board simulatedMove = SimulateMove(move);

		// All pieces should be connected
		if (!simulatedMove.AllPiecesConnected())
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


	// public static List<Piece> SurroundingPieces(Piece piece, List<Piece> pieces) => SurroundingPieces(piece.Position, pieces);
	public List<Piece> SurroundingPieces(Piece position) => SurroundingPieces(position, _Pieces);

	public static List<Piece> SurroundingPieces(Piece position, List<Piece> pieces) => SurroundingPieces(position, pieces, false);

	public static List<Piece> SurroundingPieces(Piece position, List<Piece> pieces, bool includeHeight)
	{
		List<Cube> surroundingPositions = SurroundingPositions(position.Position);

		// Can be optimised, is now n^2
		pieces = pieces.Where(p => surroundingPositions.Contains(p.Position)).ToList();
		if (includeHeight)
		{
			pieces = pieces.Where(p => p.Height == position.Height).ToList();
		}

		return pieces;
	}

	public static List<Cube> SurroundingCubes(Piece position, List<Piece> pieces)
	{
		return SurroundingPieces(position, pieces).Select(p => p.Position).ToList();
	}

	public int AmountOfSurroundingPieces(Piece piece) => AmountOfSurroundingPieces(piece, _Pieces);

	public static int AmountOfSurroundingPieces(Piece position, List<Piece> pieces)
	{
		return SurroundingPieces(position, pieces).Count;
	}

	public static int AmountOfSurroundingCubes(Piece position, List<Piece> pieces)
	{
		return SurroundingCubes(position, pieces).Count;
	}

	public static bool IsNextToPiece(Piece position, List<Piece> pieces)
	{
		return SurroundingPieces(position, pieces).Count != 0;
	}

	public bool HasHigherPiece(Piece piece) => HasHigherPiece(piece, _Pieces);

	public static bool HasHigherPiece(Piece piece, List<Piece> pieces)
	{
		return pieces.Any(p =>
			piece.Position.Equals(p.Position) &&
			p.Height > piece.Height
		);
	}

	public bool HasLowerPiece(Piece piece) => HasLowerPiece(piece, _Pieces);


	public static bool HasLowerPiece(Piece piece, List<Piece> pieces)
	{
		return pieces.Any(p =>
			piece.Position.Equals(p.Position) &&
			p.Height < piece.Height
		);
	}

	public Piece HighestPiece(Cube position) => HighestPiece(position, _Pieces);

	public static Piece HighestPiece(Cube position, List<Piece> pieces)
	{
		Piece highestPiece = pieces.First(p => p.Height == 0 && p.Position.Equals(position));
		int maxPieceHeight = pieces.Max(p => p.Height);

		for (int i = 1; i <= maxPieceHeight + 1; i++)
		{
			if (HasHigherPiece(highestPiece, pieces))
			{
				highestPiece = pieces.First(p =>
					p.Position.Equals(highestPiece.Position) &&
					p.Height == i
				);
				continue;
			}
			else
			{
				return highestPiece;
			}
		}

		return highestPiece;
	}

	/// <summary>
	/// Gets the top-most pieces, so the highest pieces possible of each position
	/// </summary>
	/// <returns>All the highest pieces</returns>
	public List<Piece> HighestPieces() => HighestPieces(_Pieces);

	/// <summary>
	/// Gets the top-most pieces, so the highest pieces possible of each position
	/// </summary>
	/// <param name="pieces">Pieces of the board</param>
	/// <returns>All the highest pieces</returns>
	public static List<Piece> HighestPieces(List<Piece> pieces)
	{
		List<Piece> highestPieces = [];
		List<Piece> lowestPieces = pieces.Where(p => p.Height == 0).ToList();

		// For each piece that has a piece on top of it
		foreach (Piece piece in lowestPieces)
		{
			highestPieces.Add(HighestPiece(piece.Position, pieces));
		}

		return highestPieces;
	}

	public Board LowestPiecesBoard() => LowestPiecesBoard(this);

	public static Board LowestPiecesBoard(Board board)
	{
		return new Board(board.Pieces.Where(p => p.Height == 0).ToList());
	}

	public static List<Cube> SurroundingPositions(Cube position)
	{
		int q = position.Q;
		int r = position.R;
		int s = position.S;
		return [
			new(q + 1, r - 1, s),   // Move in the +Q direction
        	new(q + 1, r, s - 1),   // Move in the -S direction
        	new(q, r + 1, s - 1),   // Move in the +R direction
        	new(q - 1, r + 1, s),   // Move in the -Q direction
        	new(q - 1, r, s + 1),   // Move in the +S direction
        	new(q, r - 1, s + 1)    // Move in the -R direction
		];
	}

	public bool HasWinCondition()
	{
		// Queen surrounded means wincondition
		return _Pieces.Where(p => p.BugType == (int)BugType.Queen)
			.Any(p => SurroundingPieces(p).Count == 6);
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

	// TODO; check for height
	public bool AllPiecesConnected()
	{
		if (Pieces.Count < 2)
		{
			return true;
		}

		// Use a queue for BFS and a HashSet to track visited pieces
		Queue<Piece> queue = new();
		HashSet<Piece> visited = [];

		// Start BFS from the first piece
		Piece startPiece = _Pieces.Where(p => p.Height == 0).First(); // begin from bottom and travel through top
		queue.Enqueue(startPiece);
		visited.Add(startPiece);

		// BFS Loop
		while (queue.Count > 0)
		{
			Piece current = queue.Dequeue();

			// Explore neighbors (surrounding pieces)
			List<Piece> surroundingPieces = SurroundingPieces(current);

			// Check if piece is higher, and add it
			if (HasHigherPiece(current))
			{
				surroundingPieces.Add(Pieces.First(
					p => p.Position.Equals(current.Position) &&
					p.Height == current.Height + 1));
			}

			foreach (var neighbor in SurroundingPieces(current))
			{
				if (!visited.Any(p => p.Equals(neighbor)))
				{
					visited.Add(neighbor);
					queue.Enqueue(neighbor);
				}
			}
		}

		// If all pieces have been visited, the board is connected
		if (visited.Count != _Pieces.Count)
		{
			return false;
		}

		return true;
	}

	public bool IsPinned(Piece piece)
	{
		if (!_Pieces.Contains(piece))
		{
			throw new Exception("piece must be in board");
		}

		if (HasHigherPiece(piece))
		{
			return true;
		}

		List<Piece> pieceCopiesWithoutPiece = _Pieces.Where(p => !p.Equals(piece))
			.ToList();

		Board possibleDisconnectedBoard = new(pieceCopiesWithoutPiece);

		// If possibleDisconnectedBoard is disconnected, 
		// then that means that the removed piece is pinned
		return !possibleDisconnectedBoard.AllPiecesConnected();
	}

	public override string ToString()
	{
		return ConsoleHexPrinter.BoardStringWithHeight(Copy());
	}

	#endregion
}
