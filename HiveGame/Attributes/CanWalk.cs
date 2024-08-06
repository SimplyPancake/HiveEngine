using Hive.Core.Models;

namespace Hive.Core.Attributes;

/// <summary>
/// Describes if a Bug can walk, and how many times.
/// Walk amount 0 for infinite walking.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class CanWalk : BugAttribute
{
	public int WalkAmount { get; set; }

	public override List<Move> Moves(Board board, Piece piece)
	{
		throw new NotImplementedException();
	}
}
