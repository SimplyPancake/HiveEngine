using System.Numerics;

namespace HiveGame;

// Move in the game
public class Move
{
	public Vector3 AttackPosition { get; set; }

	public int AttackHeight { get; set; } = 0;

	public Piece Piece { get; set; }

	public MoveType MoveType { get; }

	public Move(Piece piece, Vector3 attackPosition, MoveType moveType)
	{
		AttackPosition = attackPosition;
		Piece = piece;
		MoveType = moveType;
	}

	public Move(Piece piece, Vector3 attackPosition, int height, MoveType moveType)
	{
		AttackPosition = attackPosition;
		Piece = piece;
		MoveType = moveType;
		AttackHeight = height;
	}

	public Move()
	{
		// Default move
		AttackPosition = Vector3.Zero;
		Piece = new Piece(Color.Black, new QueenBug());
		MoveType = MoveType.Place;
	}

}
