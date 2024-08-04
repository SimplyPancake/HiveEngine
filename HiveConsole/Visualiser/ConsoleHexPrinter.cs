using Hive.Console.Visualiser.Printers;
using Hive.Core.Models;

namespace Hive.Console.Visualiser;

/// <summary>
/// The class that creates an ASCII Hex Grid, but it takes in a List of Pieces.
/// </summary>
public static class ConsoleHexPrinter
{
	public static void Print(List<Piece> pieces)
	{
		if (pieces.Count == 0)
		{
			AsciiBoard b = new(0, 2, 0, 2, new SmallFlatAsciiHexPrinter());
			System.Console.WriteLine(b.PrettyPrint(true));
			return;
		}

		// Convert coordinates from cube to axial
		List<GridPiece> gridPieces = pieces.Select(p => new GridPiece(p)).ToList();

		// TODO; support showing that a piece is on top of another!
		// TODO; support for piece jumping on another piece

		// Remove negatives from coordinates
		gridPieces = ShiftedPieces(gridPieces);

		// get min/max Q and R coords, should already be adjusted
		int maxQ = gridPieces.Max(p => p.Position.Q);
		int maxR = gridPieces.Max(p => p.Position.R);

		AsciiBoard board = new(0, maxQ, 0, maxR, new SmallFlatAsciiHexPrinter());

		// Add grid pieces to the board with specified text, filler character, and coordinates
		foreach (GridPiece piece in gridPieces)
		{
			board.AddHex(piece);
		}

		System.Console.WriteLine(board.PrettyPrint(true));
	}

	/// <summary>
	/// Shifts the GridPieces such that the coordinates are not negative
	/// </summary>
	/// <returns></returns>
	private static List<GridPiece> ShiftedPieces(List<GridPiece> pieces)
	{
		int minQ = pieces.Min(p => p.Position.Q);
		int minR = pieces.Min(p => p.Position.R);

		List<GridPiece> shiftedPieces = [];
		foreach (GridPiece piece in pieces)
		{
			piece.Position.Q = piece.Position.Q - minQ;
			piece.Position.R = piece.Position.R - minR;

			shiftedPieces.Add(piece);
		}

		return shiftedPieces;
	}
}
