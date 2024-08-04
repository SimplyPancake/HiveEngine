using Hive.Core.Models;
using Hive.Core.Models.Coordinate;

namespace Hive.Core;

public class AttackMove(Piece attackingPiece, Cube attackPosition, int attackHeight) : Move(attackingPiece)
{
	public Cube AttackPosition { get; set; } = attackPosition;

	public int AttackHeight { get; set; } = attackHeight;

	public override MoveType MoveType => MoveType.Attack;
}
