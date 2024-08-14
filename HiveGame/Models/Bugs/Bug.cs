using System.Diagnostics;
using Hive.Core.Attributes;
using Hive.Core.Enums;
using Hive.Core.Services;

namespace Hive.Core.Models.Bugs;

public abstract class Bug
{
	/// <summary>
	/// The name of the bug type
	/// </summary>
	public abstract string Name { get; }

	/// <summary>
	/// The description of the Bug type
	/// </summary>
	public abstract string Description { get; }

	/// <summary>
	/// The ID value that a bug has (now inherited from BugType)
	/// </summary>
	public abstract int BugTypeId { get; }

	/// <summary>
	/// The amount of pieces that a player should get of this class
	/// </summary>
	public abstract int GetAmount { get; }

	/// <summary>
	/// Short representation of a Bug, like 'Q' for the Queen.
	/// </summary>
	public abstract char ShortRepresentation { get; }

	/// <summary>
	/// The type of move behavior of the bug;
	/// On attacking, does it always have to move or not?
	/// </summary>
	public abstract MoveBehavior MoveBehavior { get; }


	/// <summary>
	/// Returns all possible moves that a piece of a board could make.
	/// </summary>
	/// <param name="piece">Piece in the board</param>
	/// <param name="board">The game board</param>
	/// <returns></returns>
	public List<Move> PossibleMoves(Piece piece, Board board)
	{
		if (!board.Pieces.Any(p => p.Equals(piece)))
		{
			throw new ArgumentException("piece must be in board");
		}

		// TODO; test pinned hive
		// Check for pins
		if (MoveBehavior == MoveBehavior.MustMove && board.IsPinned(piece))
		{
			return [];
		}

		// Check for Bug attributes, where we will add moves to the bug based on it's attributes
		List<Move> pieceMoves = ProcessAttributes(GetType().GetCustomAttributes(false), piece, board);

		pieceMoves.AddRange(PieceMoves(piece, board));

		// then filter the moves, if that is necessary
		pieceMoves = pieceMoves.Where(MoveFilter()).ToList();

		List<Move> toReturn = [];

		// Check for pins and cycles (maybe other pieces were moved?)
		foreach (Move move in pieceMoves)
		{
			// Copy board
			// Simulate move being made
			// then check if cyclic
			Board newBoard = board.SimulateMove(move);

#if DEBUG
			Debug.WriteLine($"{GetType().Name}: Showing possible move");
			Debug.WriteLine(newBoard);
#endif

			if (newBoard.AllPiecesConnected())
			{
				toReturn.Add(move);
			}
		}

		return toReturn;
	}

	private static List<Move> ProcessAttributes(object[] attributes, Piece piece, Board board)
	{
		List<Move> possibleMovesToAdd = [];

		foreach (var attr in attributes)
		{
			if (attr.GetType().BaseType == typeof(BugAttribute))
			{
				BugAttribute bugAttribute = (BugAttribute)attr;
				possibleMovesToAdd.AddRange(bugAttribute.Moves(board, piece));
			}
		}

		return possibleMovesToAdd;
	}


	/// <summary>
	/// All extra special moves that a piece has that are not made easily generic
	/// </summary>
	/// <param name="piece"></param>
	/// <param name="board"></param>
	/// <returns></returns>
	private abstract protected List<Move> PieceMoves(Piece piece, Board board);

	private abstract protected Func<Move, bool> MoveFilter();

	// Produces stackoverflowexception. TODO
	public bool Equals(Bug obj)
	{
		if (obj == null) return false;
		if (ReferenceEquals(this, obj)) { return true; }

		return GetHashCode() == obj.GetHashCode();
	}

	public override int GetHashCode()
	{
		return BugTypeId;
	}
}
