using Hive.Core.Models;
using Hive.Core.Models.Coordinate;

namespace Hive.Core.Attributes;

public class MustWalk : BugAttribute
{
	public int WalkAmount { get; set; }

	public override List<AttackMove> Moves(Board board, Piece piece)
	{
		// get the CanWalk but filter on moves with a distance of WalkAmount.
		CanWalk canWalk = new(WalkAmount)
		{
			returnOnly = true
		};

		return canWalk.Moves(board, piece);
	}
}
