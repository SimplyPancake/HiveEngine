using Hive.Core.Models;
using Hive.Core.Models.Coordinate;

namespace Hive.Core.Attributes;

public class CanChangeHeight : BugAttribute
{
	public override List<AttackMove> Moves(Board board, Piece piece)
	{
		// Only focus on jumping on and off of pieces
		// slide rule applies
		List<Piece> neighbors = board.SurroundingPieces(piece.Position).Where(p => p.Height == piece.Height).ToList();
		List<Cube> piecePositions = neighbors.Select(p => p.Position).ToList();
		List<(Cube position, int height)> movePositions = [];

		#region Jump on/off a piece
		// if jump on a piece, check it's max height.
		foreach (Cube position in Board.SurroundingCubes(piece.Position, piecePositions))
		{
			// get max height of stack to jump on/off on different heights only
			if (board.Pieces.Any(p => p.Position == position))
			{
				int heighestOnStack = board.HighestPiece(position).Height;
				// jump on top of it
				movePositions.Add((position, heighestOnStack + 1));
			}
			// Can only jump on empty position if we're off the ground
			else if (board.HighestPiece(position).Height != 0)
			{
				movePositions.Add((position, 0));
			}
		}
		#endregion

		throw new NotImplementedException();
	}
}
