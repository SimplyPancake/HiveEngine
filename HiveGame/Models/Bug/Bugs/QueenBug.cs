
using Hive.Core.Attributes;
using Hive.Core.Models;

namespace Hive.Core;

[CanWalk(WalkAmount = 1)]
public class QueenBug : Bug
{
	public override string Name => "Queen";

	public override string Description => "The Queen Bug";

	public override int BugTypeId => (int)BugType.Queen;

	public override int GetAmount => 1;

	public override char ShortRepresentation => 'Q';

	public override bool MoveRestrictionsApply => true;


	private protected override List<Move> PieceMoves(Piece piece, Board board)
	{
		return new List<Move>();
	}
}
