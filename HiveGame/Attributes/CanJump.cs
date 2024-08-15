using Hive.Core.Models;
using Hive.Core.Models.Coordinate;

namespace Hive.Core.Attributes;

public class CanJump : BugAttribute
{
	public override List<AttackMove> Moves(Board board, Piece piece)
	{
		List<Cube> boardCoordinates = board.Pieces.Where(p => !p.Equals(piece)).Select(p => p.Position).ToList();

		List<Cube> neighborPositions = board.SurroundingPieces(piece.Position).Select(p => p.Position).ToList();

		List<Cube> possibleMoves = [];

		foreach (Cube neighbor in neighborPositions)
		{
			possibleMoves.Add(JumpArrivePosition(boardCoordinates, piece.Position, neighbor));
		}

		// Now make moves out of them
		return possibleMoves.Select(walkPos =>
			new AttackMove(piece, walkPos, 0, MoveType.Move))
			.ToList();
	}

	private Cube JumpArrivePosition(List<Cube> pieces, Cube from, Cube neighborToJumpOver)
	{
		Cube jumpDirectionVector = neighborToJumpOver - from;

		// we increase the jump length until we do not reach a piece anymore
		for (int i = 1; i < pieces.Count; i++)
		{
			// to jump, multiply the jump vector by jumpLength
			Cube toCheck = from + (i * jumpDirectionVector);
			if (!pieces.Any(p => p.Equals(toCheck)))
			{
				// i is the jump length 
				return toCheck;
			}
		}

		throw new Exception("Jumping over a neighbor did not reach an endpoint. Your board is cursed.");
	}
}
