using System.Numerics;

namespace Hive.Core;

public class AttackMove : Move
{
	public Piece AttackingPiece;

	public Vector3 AttackPosition { get; set; }

	public int AttackHeight { get; set; } = 0;

	public override MoveType MoveType => MoveType.Activate;

	public AttackMove(Piece attackingPiece, Vector3 attackPosition, int attackHeight)
	{
		AttackingPiece = attackingPiece;
		AttackPosition = attackPosition;
		AttackHeight = attackHeight;
	}

}
