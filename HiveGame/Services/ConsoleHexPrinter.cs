using Hive.Core;
using Hive.Core.Models;

namespace Hive.Core.Services;

/// <summary>
/// The class that creates an ASCII Hex Grid, but it takes in a List of Pieces.
/// </summary>
public static class ConsoleHexPrinter
{
	const int MIN_BOARD_SIZE = 2;

	[Obsolete("Use BoardStringWithHeight() instead")]
	public static string BoardString(Board board)
	{
		List<GridPiece> pieces = GridPiece.GridPieces(board.Pieces);

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
		int size = maxQ - minQ > maxR - minR ? maxQ - minQ : maxR - minR;
		size = size < MIN_BOARD_SIZE ? MIN_BOARD_SIZE : size; // min size is 2
		size = 2 * size + 1; // always uneven number

		// sort pieces on Q and then R
		// pieces = pieces.OrderByDescending(p => p.Position.Q).ThenBy(p => p.Position.R).ToList();
		int maxCharRowSize = 4 * size - 1; // each piece has 3 chars & 1 char in between every
										   // size * 3 + size - 1

		string outPut = string.Empty;

		for (int line = 1; line <= size; line++)
		{
			// amount of dots to draw given the line and max size
			int horizontalPositions = size - Math.Abs((size + 1) / 2 - line);

			// fist build the horizontal row
			// then, add the padding
			int skippedPositions = size - horizontalPositions;
			string rowString = new(' ', skippedPositions * 2);

			int differenceWithMiddle = line - (size + 1) / 2; // -2, -1, 0 (on middle), 1, 2

			for (int i = 0; i < horizontalPositions; i++)
			{
				// Get the coordinates
				int q = -((size - 1) / 2) + i;
				if (differenceWithMiddle < 0) q -= differenceWithMiddle; // -2 - -2 = 0
				int r = differenceWithMiddle;

				if (i != 0) rowString += ' ';

				// does this position have a piece?
				if (pieces.Any(p => p.Position.Equals(new Axial(q, r))))
				{
					GridPiece toDraw = pieces.First(p => p.Position.Equals(new Axial(q, r)));
					rowString += toDraw;
				}
				else
				{
					rowString += " . ";
				}
			}
			outPut += rowString + "\n";
		}

		return outPut;
	}

	public static string BoardStringWithHeight(Board board)
	{
		if (board.Pieces.Count == 0) return "<empty board>";
		List<GridPiece> pieces = GridPiece.GridPieces(board.Pieces);

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
		int size = maxQ - minQ > maxR - minR ? maxQ - minQ : maxR - minR;
		size = size < MIN_BOARD_SIZE ? MIN_BOARD_SIZE : size; // min size is 2
		size = 2 * size + 1; // always uneven number

		// sort pieces on Q and then R
		// pieces = pieces.OrderByDescending(p => p.Position.Q).ThenBy(p => p.Position.R).ToList();
		int maxCharRowSize = 4 * size - 1; // each piece has 3 chars & 1 char in between every
										   // size * 3 + size - 1

		string outPut = string.Empty;

		for (int line = 1; line <= size; line++)
		{
			// amount of dots to draw given the line and max size
			int horizontalPositions = size - Math.Abs((size + 1) / 2 - line);

			// fist build the horizontal row
			// then, add the padding
			int skippedPositions = size - horizontalPositions;
			string rowString = "";

			int differenceWithMiddle = line - (size + 1) / 2; // -2, -1, 0 (on middle), 1, 2

			// get all the pieces we want to draw on this line
			List<GridPiece> piecesToDrawOnLine = pieces.Where(p => p.Position.R == differenceWithMiddle).ToList();

			// check if there is any piece with height > 0
			if (piecesToDrawOnLine.Any(p => p.Height > 0))
			{
				// loop from highest to lowest and do not draw dots
				int highestHeightOnLine = piecesToDrawOnLine.Max(p => p.Height);

				// loop from max height to height = 1, height = 0 will be drawn after
				for (int i = highestHeightOnLine; i >= 1; i--)
				{
					// get all pieces with that height
					List<GridPiece> piecesOnThatHeight = piecesToDrawOnLine.Where(p => p.Height == i).ToList();

					// then draw that line
					rowString += new string(' ', skippedPositions * 2) + DrawLine(piecesOnThatHeight, size, differenceWithMiddle, horizontalPositions, false);

					piecesToDrawOnLine.RemoveAll(piecesOnThatHeight.Contains);
				}
			}
			// draw normal line with dots
			rowString += new string(' ', skippedPositions * 2) + DrawLine(piecesToDrawOnLine, size, differenceWithMiddle, horizontalPositions, true);

			outPut += rowString;
		}

		return outPut;
	}

	private static string DrawLine(List<GridPiece> toDraw, int size, int differenceWithMiddle, int horizontalPositions, bool withDots)
	{
		string toReturn = "";

		for (int i = 0; i < horizontalPositions; i++)
		{
			// Get the coordinates
			int q = -((size - 1) / 2) + i;
			if (differenceWithMiddle < 0) q -= differenceWithMiddle; // -2 - -2 = 0
			int r = differenceWithMiddle;

			if (i != 0) toReturn += ' ';

			// does this position have a piece?
			if (toDraw.Any(p => p.Position.Equals(new Axial(q, r))))
			{
				GridPiece pieceToDraw = toDraw.First(p => p.Position.Equals(new Axial(q, r)));
				toReturn += pieceToDraw.ToBoardString();
			}
			else
			{
				toReturn += $" {(withDots ? '.' : ' ')} ";
			}
		}

		return toReturn + "\n";
	}
}
