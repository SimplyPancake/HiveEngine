using Hive.Core.Enums;
using Hive.Core.Models;
using Hive.Core.Models.Coordinate;

namespace Hive.Core.Attributes;

public class CanChangeHeight : BugAttribute
{
	// Can change height is also affected by the freedom to move rule
	public override List<AttackMove> Moves(Board board, Piece piece)
	{
		// Only focus on jumping on and off of pieces
		// TODO: slide rule applies
		// List<Piece> neighbors = board.SurroundingPieces(piece).Where(p => p.Height == piece.Height).ToList();
		// List<Cube> piecePositions = neighbors.Select(p => p.Position).ToList();
		List<(Cube position, int height)> movePositions = [];

		#region Jump on/off a piece
		List<Cube> positionsAround = Board.SurroundingPositions(piece.Position);
		// if jump on a piece, check it's max height.
		foreach (Cube position in positionsAround)
		{
			// get max height of stack to jump on/off on different heights only
			if (board.Pieces.Any(p => p.Position.Equals(position)))
			{
				int heighestOnStack = board.HighestPiece(position).Height;
				// jump on top of it
				movePositions.Add((position, heighestOnStack + 1));
			}
			// Can only jump on empty position if we're off the ground
			else if (piece.Height != 0)
			{
				movePositions.Add((position, 0));
			}
		}
		#endregion

		// Then filter out FTM rule (Freedom to move) https://boardgamegeek.com/wiki/page/Hive_FAQ
		// If
		// height(A) < height(C) and
		// height(A) < height(D) and
		// height(B) < height(C) and
		// height(B) < height(D)
		// then
		// moving between A and B(in either direction) is illegal, because the beetle cannot slip through the "gate" formed by C and D, which are both strictly higher than A and B.
		// Otherwise, movement between A and B is legal.
		List<(Cube position, int height)> ftmFilteredMoves = [];
		List<Cube> boardPositions = board.Pieces.Select(p => p.Position).ToList();
		foreach (var (position, height) in movePositions)
		{
			(Cube position, int height) B = (piece.Position, piece.Height);

			// jumping to empty position
			(Cube position, int height) A = (position, height);
			if (height != 0)
			{
				// Jumping to non-empty position
				A.height = board.HighestPiece(position).Height;
			}

			CubeVector toMoveTo = CubeVectorExtensions.CubeToVector(position - piece.Position);

			// Get C position
			Cube cPosition = piece.Position + CubeVectorExtensions.PredecessorCube(toMoveTo);
			if (!boardPositions.Contains(cPosition))
			{
				// Valid move
				ftmFilteredMoves.Add((position, height));
				continue;
			}
			Piece cPiece = board.HighestPiece(cPosition);

			// get D position
			Cube dPosition = piece.Position + CubeVectorExtensions.SuccessorCube(toMoveTo);
			if (!boardPositions.Contains(dPosition))
			{
				// Valid move
				ftmFilteredMoves.Add((position, height));
				continue;
			}
			Piece dPiece = board.HighestPiece(dPosition);

			if (
				A.height < cPiece.Height &&
				A.height < dPiece.Height &&
				B.height < cPiece.Height &&
				B.height < dPiece.Height
			)
			{
				// moving from A to B is illegal
				continue;
			}
			ftmFilteredMoves.Add((position, height));
		}

		return ftmFilteredMoves.Select(pos =>
			new AttackMove(piece, pos.position, pos.height, MoveType.Move))
			.ToList();
	}
}
