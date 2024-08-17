using Hive.Core.Models;
using Hive.Core.Models.Coordinate;

namespace Hive.Core;

public class PlaceMove(Piece piece) : Move(piece)
{
	public override MoveType MoveType => MoveType.Place;

	public override string MoveString(List<GridPiece> pieces)
	{
		// Only refer to top pieces
		List<GridPiece> highestPieces = Board.HighestGridPieces(pieces);

		GridPiece gotMovedTo = highestPieces.First(p =>
			p.OriginalPosition.Equals(Piece.Position)
		);
		gotMovedTo.Height = 0;

		// pick piece next to endUpPiece
		GridPiece nextToEndupPiece = highestPieces.First(p => Cube.Distance(p.OriginalPosition, gotMovedTo.OriginalPosition) == 1);

		return MovedPieceString(gotMovedTo, nextToEndupPiece);
	}

	public override string ToString()
	{
		return Piece.ToString();
	}
}
