using Hive.Core.Attributes;
using Hive.Core.Enums;

namespace Hive.Core.Models.Bugs;

[MustWalk(WalkAmount = 3)]
public class SpiderBug : Bug
{
	public override string Name => "Spider";

	public override string Description => "A spider that can walk 3 spaces";

	public override int BugTypeId => (int)BugType.Spider;

	public override int GetAmount => 3;

	public override char ShortRepresentation => 'S';

	public override bool MoveRestrictionsApply => true;

	public override MoveBehavior MoveBehavior => MoveBehavior.MustMove;

	private protected override List<Move> FilterMoves(Piece piece, Board board, List<Move> generatedMoves)
	{
		return generatedMoves;
	}

	private protected override List<Move> PieceMoves(Piece piece, Board board)
	{
		return [];
	}
}
