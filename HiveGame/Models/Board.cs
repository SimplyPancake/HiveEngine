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
		List<Piece> copiesPieces = new List<Piece>(board.Pieces);

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
			// Problem for later; attacking
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

	public List<Piece> SurroundingPieces(Cube position) => SurroundingPieces(position, _Pieces);

	public static List<Piece> SurroundingPieces(Piece piece, List<Piece> pieces) => SurroundingPieces(piece.Position, pieces);

	public static List<Piece> SurroundingPieces(Cube position, List<Piece> pieces)
	{
		List<Cube> surroundingPositions = SurroundingPositions(position);

		// Can be optimised, is now n^2
		return pieces
			.Where(p => CubeListExtensions.ContainsCube(surroundingPositions, p.Position))
			.ToList();
	}

	public static List<Cube> SurroundingCubes(Cube position, List<Cube> pieces)
	{
		List<Cube> surroundingPositions = SurroundingPositions(position);

		// Can be optimised, is now n^2
		return pieces
			.Where(p => CubeListExtensions.ContainsCube(surroundingPositions, p))
			.ToList();
	}

	public int AmountOfSurroundingPieces(Piece piece) => AmountOfSurroundingPieces(piece.Position, _Pieces);

	public int AmountOfSurroundingPieces(Cube position) => AmountOfSurroundingPieces(position, _Pieces);

	public static int AmountOfSurroundingPieces(Cube position, List<Piece> pieces)
	{
		return SurroundingPieces(position, pieces).Count;
	}

	public static int AmountOfSurroundingCubes(Cube position, List<Cube> pieces)
	{
		return SurroundingCubes(position, pieces).Count;
	}

	public static bool IsNextToPiece(Cube position, List<Piece> pieces)
	{
		return SurroundingPieces(position, pieces).Any();
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
		if (Pieces.Count < 2)
		{
			return true;
		}
		else if (Pieces.Count == 2 && Cube.Distance(_Pieces[0].Position, _Pieces[1].Position) == 1)
		{
			return true;
		}

		// Check if all pieces are connected using something similar to Dijkstra (without the distance part)

		List<Cube> visited = [];
		List<Cube> unvisited = _Pieces.Select(p => p.Position).ToList();
		List<Cube> lastVisited = [];

		// Initialize visited and lastVisited to the first piece that we're exploring from
		Cube startPiece = _Pieces.First().Position;
		visited.Add(startPiece);
		lastVisited.Add(startPiece);

		// Remove startPiece from unvisited
		unvisited.Remove(startPiece);

		while (lastVisited.Count > 0)
		{
			List<Cube> closest = [];

			// Explore each cube in lastVisited
			foreach (Cube lv in lastVisited)
			{
				// Get surrounding pieces' positions that are unvisited
				List<Cube> surrounding = SurroundingPieces(lv)
										  .Select(p => p.Position)
										  .Where(unvisited.Contains)
										  .ToList();

				// Add these surrounding positions to closest
				closest.AddRange(surrounding);
			}

			if (closest.Count == 0)
			{
				// If no new positions are found, break the loop
				break;
			}

			// Visit the closest cubes
			visited.AddRange(closest);

			// Set lastVisited to closest for the next iteration
			lastVisited = closest;

			// Remove visited cubes from unvisited
			unvisited = unvisited.Except(closest).ToList();
		}

		// If all pieces have been visited, the board is connected
		return visited.Count == _Pieces.Count;
	}

	public bool IsPinned(Piece piece)
	{
		if (!_Pieces.Contains(piece))
		{
			throw new Exception("piece must be in board");
		}

		List<Piece> pieceCopiesWithoutPiece = _Pieces.Where(p => p.Equals(piece))
			.ToList();

		Board possibleDisconnectedBoard = new(pieceCopiesWithoutPiece);

		// If possibleDisconnectedBoard is disconnected, 
		// then that means that the removed piece is pinned
		return !possibleDisconnectedBoard.AllPiecesConnected();
	}

	public override string ToString()
	{
		return ConsoleHexPrinter.BoardString(Copy());
	}

	#endregion
}
