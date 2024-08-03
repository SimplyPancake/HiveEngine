using System.Numerics;

namespace Hive.Core;

public class AttackMove(Piece attackingPiece, Vector3 attackPosition, int attackHeight) : Move(attackingPiece)
{
	public Vector3 AttackPosition { get; set; } = attackPosition;

	public int AttackHeight { get; set; } = attackHeight;

	public override MoveType MoveType => MoveType.Attack;
}
