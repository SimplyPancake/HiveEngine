using Hive.Core.Models;

namespace Hive.Console.Visualiser;

/// <summary>
/// The class that creates an ASCII Hex Grid, but it takes in a List of Pieces.
/// </summary>
public static class ConsoleHexPrinter
{
	const int MIN_BOARD_SIZE = 2;

	public static void Print(Board board)
	{
		List<GridPiece> pieces = board.Pieces.Select(p => new GridPiece(p)).ToList();

		int minQ = pieces.Min(p => p.Position.Q);
		int maxQ = pieces.Max(p => p.Position.Q);
		int minR = pieces.Min(p => p.Position.R);
		int maxR = pieces.Max(p => p.Position.R);

		// shift all by 1/2 so they end up in the middle
		int shiftQ = (minQ + maxQ) / 2;
		int shiftR = (minR + maxR) / 2;

		// All pieces shifted
		foreach (var piece in pieces)
		{
			piece.Position.Q -= shiftQ;
			piece.Position.R -= shiftR;
		}

		// determine size of hexagon
		int size = (maxQ - minQ) > (maxR - minR) ? (maxQ - minQ) : (maxR - minR);
		size = size < MIN_BOARD_SIZE ? MIN_BOARD_SIZE : size; // min size is 2
		size = 2 * size + 1; // always uneven number

		// sort pieces on Q and then R
		// pieces = pieces.OrderByDescending(p => p.Position.Q).ThenBy(p => p.Position.R).ToList();
		int maxCharRowSize = size * 3 + size - 1; // each piece has 3 chars & 1 char in between every

		for (int line = 1; line <= size; line++)
		{
			// amount of dots to draw given the line and max size
			int horizontalPositions = size - Math.Abs(((line + 1) / 2) - line);

		}


	}
}
