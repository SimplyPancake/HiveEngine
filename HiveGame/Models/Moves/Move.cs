namespace Hive.Core;

// Move in the game
public abstract class Move
{
	public Piece Piece { get; set; }

	public abstract MoveType MoveType { get; }

	/// <summary>
	///	Move constructor takes the piece including position to place
	/// </summary>
	public Move(Piece piece)
	{
		Piece = piece;
	}

	public Move()
	{
		// Default move
		Piece = new Piece(Color.Black, new QueenBug());
	}

}
