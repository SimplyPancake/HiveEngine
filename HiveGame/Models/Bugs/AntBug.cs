using Hive.Core.Attributes;
using Hive.Core.Enums;

namespace Hive.Core.Models.Bugs;

[CanWalk(0)]
public class AntBug : Bug
{
	public override string Name => "Ant";

	public override string Description => "The ant, moves very well";

	public override int BugTypeId => (int)BugType.Ant;

	public override char ShortRepresentation => 'A';

	public override MoveBehavior MoveBehavior => MoveBehavior.MustMove;

	private protected override Func<Move, bool> MoveFilter()
	{
		return move => true;
	}

	private protected override List<Move> PieceMoves(Piece piece, Board board)
	{
		return [];
	}
}
