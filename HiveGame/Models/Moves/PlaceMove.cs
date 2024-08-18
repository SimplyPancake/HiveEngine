using Hive.Core.Models;
using Hive.Core.Models.Coordinate;

namespace Hive.Core;

public class PlaceMove(Piece piece) : Move(piece)
{
	public override MoveType MoveType => MoveType.Place;

	public override string MoveString(List<GridPiece> pieces)
	{
		if (pieces.Count == 0)
		{
			return $"{(Piece.Color == Color.Black ? "b" : "w")}{Piece.Bug.ShortRepresentation}";
		}

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

	// override object.Equals
	public override bool Equals(object? obj)
	{
		//
		// See the full list of guidelines at
		//   http://go.microsoft.com/fwlink/?LinkID=85237
		// and also the guidance for operator== at
		//   http://go.microsoft.com/fwlink/?LinkId=85238
		//

		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}

		PlaceMove pm = (PlaceMove)obj;

		return pm.Piece.Equals(Piece) && MoveType.Equals(pm.MoveType);
	}

	// override object.GetHashCode
	public override int GetHashCode()
	{
		return HashCode.Combine(Piece, MoveType);
	}
}
