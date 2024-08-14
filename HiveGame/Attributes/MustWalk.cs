using Hive.Core.Models;

namespace Hive.Core.Attributes;

public class ExactWalk : BugAttribute
{
	public int WalkAmount { get; set; }

	public override List<AttackMove> Moves(Board board, Piece piece)
	{
		// get the CanWalk but filter on moves with a distance of WalkAmount.
		CanWalk canWalk = new(WalkAmount)
		{
			ReturnOnly = true
		};

		return canWalk.Moves(board, piece);
	}
}
