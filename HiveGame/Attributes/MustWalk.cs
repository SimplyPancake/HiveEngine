using Hive.Core.Models;
using Hive.Core.Models.Coordinate;

namespace Hive.Core.Attributes;

public class MustWalk : BugAttribute
{
	public int WalkAmount { get; set; }

	public override List<AttackMove> Moves(Board board, Piece piece)
	{
		// get the CanWalk but filter on moves with a distance of WalkAmount.
		CanWalk canWalk = new(WalkAmount);
		List<AttackMove> generatedMoves = canWalk.Moves(board, piece);

		return generatedMoves.Where(move => Cube.Distance(move.AttackPosition, piece.Position) == WalkAmount).ToList();
	}
}
