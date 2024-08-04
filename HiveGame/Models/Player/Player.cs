namespace Hive.Core.Models;

public abstract class Player
{
	public abstract string Playername { get; }

	public abstract Color Color { get; }

	public abstract List<Bug> Pieces { get; }

	public abstract Board? Board { get; set; }

	public void SetBoard(Board b)
	{
		Board = b;
	}

	/// <summary>
	/// Asks the client to make a move.
	/// </summary>
	/// <returns></returns>
	public abstract Move MakeMove();

	/// <summary>
	/// Asks the player to make a move, taking input from the board that the player's
	/// last move was illegal.
	/// </summary>
	/// <param name="illegalMoveException"></param>
	/// <returns></returns>
	public abstract Move MakeMove(IllegalMoveException illegalMoveException);

}
