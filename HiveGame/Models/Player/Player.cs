namespace HiveGame;

public abstract class Player
{
	public abstract string Playername { get; }

	public abstract Color Color { get; }

	public abstract List<Piece> Pieces { get; }

	public abstract Move MakeMove();

	public abstract Board? Board { get; }
}
