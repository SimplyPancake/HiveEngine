using Hive.Core.Attributes;
using Hive.Core.Enums;

namespace Hive.Core.Models.Bugs;

[CanJump]
public class GrasshopperBug : Bug
{
	public override string Name => "Grasshopper";

	public override string Description => "Hops over pieces";

	public override int BugTypeId => (int)BugType.Grasshopper;

	public override int GetAmount => 3;

	public override char ShortRepresentation => 'G';

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
