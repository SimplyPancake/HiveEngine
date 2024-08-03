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

		return pieceMoves(piece, board);
	}


	private abstract protected List<Move> pieceMoves(Piece piece, Board board);
}
