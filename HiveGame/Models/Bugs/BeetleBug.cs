using Hive.Core.Attributes;
using Hive.Core.Enums;

namespace Hive.Core.Models.Bugs;

[CanWalk(1)]
[CanChangeHeight]
public class BeetleBug : Bug
{
	public override string Name => "Beetle";

	public override string Description => "The beetle bug, a shitstorm to implement...";

	public override int BugTypeId => (int)BugType.Beetle;

	public override int GetAmount => 2;

	public override char ShortRepresentation => 'B';

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
