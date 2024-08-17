using Hive.Core.Enums;
using Hive.Core.Models;
using Hive.Core.Models.Bugs;
using Hive.Core.Models.Coordinate;

namespace Hive.Core;

public class AttackMove(Piece attackingPiece, Cube attackPosition, int attackHeight, MoveType moveType) : Move(attackingPiece)
{
	public Cube AttackPosition { get; set; } = attackPosition;

	public int AttackHeight { get; set; } = attackHeight;

	public override MoveType MoveType { get; } = moveType;

	public override string MoveString(List<GridPiece> pieces)
	{
		// simulate the piece that got move not being in the board
		// Only refer to top pieces
		GridPiece gotMovedPiece = pieces.First(p =>
			p.OriginalPosition.Equals(Piece.Position) &&
			p.Height == Piece.Height
		);

		List<GridPiece> highestPieces = Board.HighestGridPieces(
			pieces.Where(p => !p.Equals(gotMovedPiece))
			.ToList());

		GridPiece gotMovedTo = new(gotMovedPiece)
		{
			OriginalPosition = AttackPosition,
			Position = new Axial(AttackPosition),
			Height = AttackHeight
		};

		// pick piece next to endUpPiece
		GridPiece nextToEndupPiece = highestPieces.First(p => Cube.Distance(p.OriginalPosition, AttackPosition) == 1);

		// If a beetle climbs/slides atop another bug, the reference bug is needed, but no direction is needed.
		if (AttackHeight != 0) // jump
		{
			// jumping on piece, if jump on ground, then normal rules
			nextToEndupPiece = highestPieces.First(p => p.OriginalPosition.Equals(AttackPosition) &&
				Math.Abs(AttackHeight - p.Height) == 1);
		}

		return MovedPieceString(gotMovedTo, nextToEndupPiece);
	}

	// ToString for easier debugging
	public override string ToString()
	{
		return AttackPosition.ToString();
	}
}
