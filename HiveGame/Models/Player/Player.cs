namespace Hive.Core.Models;

public abstract class Player
{
	public abstract string Playername { get; }

	public abstract Color Color { get; }

	public abstract List<Bug> Pieces { get; }

	public abstract Move MakeMove();

	public abstract Board? Board { get; }
}
