namespace Hive.Core;


public class HumanPlayer : Player
{
	public override string Playername { get; }

	public override Color Color { get; }

	public override List<Bug> Pieces { get; }

	public override Board Board { get; }

	public HumanPlayer(string playername, Color color, List<Bug> pieces, Board board)
	{
		Playername = playername;
		Color = color;
		Pieces = pieces;
		Board = board;
	}

	public HumanPlayer(string playername, Color color, Board board)
	{
		Playername = playername;
		Color = color;
		Board = board;
		Pieces = new List<Bug>();

		// Initialise pieces collection
		Pieces = PieceCollectionMethods.GetPieceBugs(PieceCollection.Classic);
	}

	public override Move MakeMove()
	{
		// ask the player to make a move, but now we return just this
		return new PlaceMove(new Piece(Color.White, new QueenBug()));
	}
}
