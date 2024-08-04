using System.Numerics;
using Hive.Console.Visualiser;
using Hive.Console.Visualiser.Printers;
using Hive.Core.Models;

namespace Hive.Console;

/// <summary>
/// The class that creates an ASCII Hex Grid, but it takes in a List of Pieces.
/// </summary>
public static class ConsoleHexPrinter
{
	public static void Print(List<Piece> pieces)
	{
		AsciiBoard board = new AsciiBoard(0, 2, 0, 1, new SmallFlatAsciiHexPrinter());

		// Convert coordinates from cube to axial

		// Add hexagons to the board with specified text, filler character, and coordinates
		board.AddHex("HX1", "-B-", '#', 0, 0);
		board.AddHex("HX2", "-W-", '+', 1, 0);
		board.AddHex("HX3", "-W-", 'x', 2, 0);
		board.AddHex("HX3", "-B-", '•', 2, 1);

		System.Console.WriteLine(board.PrettyPrint(true));
	}

	public static Tuple<int, int> CubeToAxial(Vector3 vector3)
	{
		return new Tuple<int, int>(Convert.ToInt32(vector3.X), Convert.ToInt32(vector3.Y));
	}
}
