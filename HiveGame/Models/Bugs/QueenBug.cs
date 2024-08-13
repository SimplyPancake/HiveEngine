using Hive.Core.Attributes;
using Hive.Core.Enums;

namespace Hive.Core.Models.Bugs;

[CanWalk(1)]
public class QueenBug : Bug
{
	public override string Name => "Queen";

	public override string Description => "The Queen Bug";

	public override int BugTypeId => (int)BugType.Queen;

	public override int GetAmount => 1;

	public override char ShortRepresentation => 'Q';

	public override bool MoveRestrictionsApply => true;

	public override MoveBehavior MoveBehavior => MoveBehavior.MustMove;

	private protected override List<Move> FilterMoves(Piece piece, Board board, List<Move> generatedMoves)
	{
		return generatedMoves;
	}

	private protected override List<Move> PieceMoves(Piece piece, Board board)
	{
		return new List<Move>();
	}
}
