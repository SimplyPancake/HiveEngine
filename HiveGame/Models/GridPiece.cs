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

	public GridPiece(GridPiece p, int pieceNum)
	{
		Color = p.Color;
		Position = p.Position;
		Height = p.Height;
		Bug = p.Bug;
		OriginalPosition = p.OriginalPosition;
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
		return GridPieces(pieces.Select(p => new GridPiece(p, 0)).ToList());
	}

	public static List<GridPiece> GridPieces(List<GridPiece> pieces)
	{
		// Create a dictionary with a composite key of Bug and Color
		Dictionary<(Bug, Color), int> bugColorAmounts = new Dictionary<(Bug, Color), int>();
		List<GridPiece> gridPieces = new List<GridPiece>();

		foreach (GridPiece piece in pieces)
		{
			int bugColorAmount = 1;
			var key = (piece.Bug, piece.Color);

			if (bugColorAmounts.ContainsKey(key))
			{
				// get the amount of pieces with the same Bug and Color
				bugColorAmount = bugColorAmounts[key] + 1;
				bugColorAmounts[key] = bugColorAmount;
			}
			else
			{
				// add it to the dictionary
				bugColorAmounts.Add(key, 1);
			}

			// Create a new GridPiece with the updated bugColorAmount
			gridPieces.Add(new GridPiece(piece, bugColorAmount));
		}

		// If only 1 per bug-color combination, then make PieceNum 0
		List<(Bug, Color)> singleBugColors = bugColorAmounts
			.Where(pair => pair.Value == 1)
			.Select(pair => pair.Key)
			.ToList();

		foreach (var gridPiece in gridPieces)
		{
			var key = (gridPiece.Bug, gridPiece.Color);
			if (singleBugColors.Contains(key))
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

	public string ToBoardString()
	{
		return $"{(Color == Color.Black ? 'b' : 'w')}{Bug.ShortRepresentation}{(PieceNum == 0 ? " " : PieceNum.ToString())}";
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

		return obj.GetHashCode() == GetHashCode();
	}

	// override object.GetHashCode
	public override int GetHashCode()
	{
		return HashCode.Combine(Color, Position, Height, Bug, OriginalPosition);
	}
}
