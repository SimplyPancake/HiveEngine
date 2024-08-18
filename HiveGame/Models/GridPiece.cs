using Hive.Core;
using Hive.Core.Models.Bugs;
using Hive.Core.Models.Coordinate;

namespace Hive.Core.Models;

public class GridPiece
{
	public Color Color { get; }
	public Axial Position { get; set; }
	public int Height { get; set; } = 0;
	public Bug Bug { get; set; }
	public Cube OriginalPosition { get; set; }

	/// <summary>
	/// Used for the n-th piece Hive notation 
	/// </summary>
	public int PieceNum { get; set; }

	public GridPiece(Piece p, int pieceNum)
	{
		Color = p.Color;
		Position = new Axial(p.Position);
		Height = p.Height;
		Bug = p.Bug;
		OriginalPosition = p.Position;
		PieceNum = pieceNum;
	}

	public GridPiece(GridPiece p)
	{
		Color = p.Color;
		Position = p.Position;
		Height = p.Height;
		Bug = p.Bug;
		OriginalPosition = p.OriginalPosition;
		PieceNum = p.PieceNum;
	}

	public static List<GridPiece> GridPieces(List<Piece> pieces)
	{
		Dictionary<Bug, int> bugAmounts = [];
		List<GridPiece> gridPieces = [];

		foreach (Piece piece in pieces)
		{
			int bugAmount = 1;

			if (bugAmounts.Keys.Any(b => b.BugTypeId == piece.Bug.BugTypeId))
			{
				// get the amount of bugs
				bugAmount = bugAmounts[piece.Bug] + 1;
				bugAmounts[piece.Bug] = bugAmount;
			}
			else
			{
				// add it to the list
				bugAmounts.Add(piece.Bug, 1);
			}

			gridPieces.Add(new GridPiece(piece, bugAmount));
		}

		// if only 1 per bug, then make piecenum 0
		List<Bug> singleBugs = bugAmounts.Keys.Where(key => bugAmounts[key] == 1).ToList();
		foreach (var gridPiece in gridPieces)
		{
			if (singleBugs.Any(b => b.Equals(gridPiece.Bug)))
			{
				// modify GridPiece
				gridPiece.PieceNum = 0;
			}
		}

		return gridPieces;
	}

	public override string ToString()
	{
		return $"{(Color == Color.Black ? 'b' : 'w')}{Bug.ShortRepresentation}{(PieceNum == 0 ? "" : PieceNum.ToString())}";
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

		// TODO: write your implementation of Equals() here
		return obj.GetHashCode() == GetHashCode();
	}

	// override object.GetHashCode
	public override int GetHashCode()
	{
		// TODO: write your implementation of GetHashCode() here
		return HashCode.Combine(Color, Position, Height, Bug, OriginalPosition);
	}
}
