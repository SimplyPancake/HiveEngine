using System.Numerics;

namespace Hive.Core;

public class AttackMove(Piece attackingPiece, Vector3 attackPosition, int attackHeight) : Move(attackingPiece, MoveType.Activate)
{
	public Vector3 AttackPosition { get; set; } = attackPosition;

	public int AttackHeight { get; set; } = attackHeight;
}
