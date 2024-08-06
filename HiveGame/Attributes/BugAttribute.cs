using Hive.Core.Models;

namespace Hive.Core.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public abstract class BugAttribute : Attribute
{
	public abstract List<Move> Moves(Board board, Piece piece);
}
