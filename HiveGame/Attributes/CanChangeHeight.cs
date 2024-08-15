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
		List<(Cube position, int height)> movePositions = [];

		#region Jump on piece
		// if jump on a piece, check it's max height.
		foreach (Piece neighbor in neighbors)
		{
			// get max height of stack to jump on
			int heighestOnStack = board.HighestPiece(neighbor.Position).Height;

			// jump on top of it
			movePositions.Add((neighbor.Position, heighestOnStack + 1));
		}
		#endregion

		#region Jump off of piece

		#endregion

		throw new NotImplementedException();
	}
}
