using Hive.Core.Models.Bugs;

namespace Hive.Core.Models.Players;

public abstract class Player
{
	public string Playername { get; }

	public Color Color { get; }

	public List<Bug> Pieces { get; }

	public List<Bug> OriginalPieceSet { get; }

	private Match _match { get; set; }

	public Match Match => _match;

	public Board Board => Match.Board;

	public Player(string playername, Color color, List<Bug> pieces)
	{
		Playername = playername;
		Color = color;
		Pieces = pieces;
		OriginalPieceSet = [.. pieces];
	}

	public Player(string playername, Color color)
	{
		Playername = playername;
		Color = color;

		// Initialise pieces collection
		Pieces = PieceCollectionMethods.GetPieceBugs(PieceCollection.Classic);
		OriginalPieceSet = [.. Pieces];
	}

	public void SetMatch(Match match)
	{
		_match = match;
		OnMatchSet(match);
	}

	/// <summary>
	/// Hook method that is called when the match is set.
	/// </summary>
	protected virtual void OnMatchSet(Match match)
	{
		// Defualt implementation does nothing
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
