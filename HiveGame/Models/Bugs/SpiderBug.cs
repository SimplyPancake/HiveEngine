using Hive.Core.Attributes;
using Hive.Core.Enums;

namespace Hive.Core.Models.Bugs;

[ExactWalk(WalkAmount = 3)]
public class SpiderBug : Bug
{
	public override string Name => "Spider";

	public override string Description => "A spider that can walk 3 spaces";

	public override int BugTypeId => (int)BugType.Spider;

	public override char ShortRepresentation => 'S';

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
