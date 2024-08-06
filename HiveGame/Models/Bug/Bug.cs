using System.Reflection.Metadata;
using Hive.Core.Attributes;
using Hive.Core.Models;

namespace Hive.Core;

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
	/// If the normal move restrictions apply
	/// TODO; convert this system into attributes and put attributes on PossibleMoves
	/// </summary>
	public abstract bool MoveRestrictionsApply { get; }

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

		// Check for Bug attributes, where we will add moves to the bug based on it's attributes
		List<Move> pieceMoves = ProcessAttributes(GetType().GetCustomAttributes(false), piece, board);

		List<Move> extraMoves = PieceMoves(piece, board);

		return pieceMoves;
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


	private abstract protected List<Move> PieceMoves(Piece piece, Board board);

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
