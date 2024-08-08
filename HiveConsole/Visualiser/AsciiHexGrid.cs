using System.Text;
using Hive.Console.Visualiser.Printers;
using Hive.Core.Models.Coordinate;

namespace Hive.Console.Visualiser;

/// <summary>
/// Constructs the virtual hex board.
/// </summary>
/// <param name="minQ">Minimum Q coordinate.</param>
/// <param name="maxQ">Maximum Q coordinate.</param>
/// <param name="minR">Minimum R coordinate.</param>
/// <param name="maxR">Maximum R coordinate.</param>
/// <param name="printer">Reference to the hex printer used.</param>
public class AsciiBoard
{
	private readonly int _width;
	private readonly int _height;
	private readonly AsciiHexPrinter _printer;
	private readonly CharGrid _grid;

	public AsciiBoard(int minQ, int maxQ, int minR, int maxR, AsciiHexPrinter printer)
	{
		_width = maxQ - minQ + 1;
		_height = maxR - minR + 1;
		_printer = printer;
		_grid = CreateGrid();
	}

	private CharGrid CreateGrid()
	{
		// This potentially creates the grid ½ a hexagon too high or wide, as we do not know given the max coordinates
		// (0,0,1,1) if both (0,1) or (1,1) is filled. This is OK, as we can fix it when outputting the grid.
		int[] gridSize = _printer.GetMapSizeInChars(_width, _height);
		return new CharGrid(gridSize[0], gridSize[1]);
	}

	/// <summary>
	/// Adds a hex to the board.
	/// </summary>
	/// <param name="textLine1">First line of text.</param>
	/// <param name="textLine2">Second line of text.</param>
	/// <param name="fillerChar">Character used as filler, may be ' '.</param>
	/// <param name="hexQ">Q coordinate for the hex in the hex grid.</param>
	/// <param name="hexR">R coordinate for the hex in the hex grid.</param>
	public void AddHex(string textLine1, string textLine2, char fillerChar, int hexQ, int hexR)
	{
		string hex = _printer.GetHex(textLine1, textLine2, fillerChar);
		int[] charCoordinates = _printer.MapHexCoordsToCharCoords(hexQ, hexR);
		string[] lines = hex.Split('\n');

		for (int i = 0; i < lines.Length; i++)
		{
			string content = lines[i];
			for (int j = 0; j < content.Length; j++)
			{
				int x = charCoordinates[0] + j;
				int y = charCoordinates[1] + i;

				// Only override empty spaces
				if (_grid.GetChar(x, y) == ' ')
				{
					_grid.AddChar(x, y, content[j]);
				}
			}
		}
	}

	public void AddHex(GridPiece piece)
	{
		// board.AddHex("HX3", "-W-", 'x', 2, 0);
		// Cube cubeCoords = piece.OriginalPosition;
		// string textLine1 = $"{Math.Abs(cubeCoords.Q)}{Math.Abs(cubeCoords.R)}{Math.Abs(cubeCoords.S)}";
		// string textLine2 = $"-{piece.Bug.ShortRepresentation}-";

		string textLine1 = $"{piece.Bug.ShortRepresentation}";
		string textLine2 = $"";

		char fillerChar = piece.Color == Core.Color.Black ? '█' : ' ';

		AddHex(textLine1, textLine2, fillerChar, piece.Position.Q, piece.Position.R);
	}

	/// <summary>
	/// Prints the Hexagonal map as a string.
	/// </summary>
	/// <param name="wrapInBox">If true, output is wrapped in an ASCII-drawn box.</param>
	public string PrettyPrint(bool wrapInBox)
	{
		return PrintBoard(wrapInBox);
	}

	/// <summary>
	/// Returns the Hexagonal map as a string. Any extra empty lines at the end are trimmed away,
	/// but the map still starts at (0,0), so e.g., having a hex at (0,1) will produce whitespace at the top.
	/// </summary>
	/// <param name="wrapInBox">If true, the hex map is wrapped in an ASCII bounding box.</param>
	private string PrintBoard(bool wrapInBox)
	{
		if (wrapInBox)
		{
			StringBuilder sb = new StringBuilder();

			// Get content
			string[] lines = _grid.Print(true).Split('\n');
			int contentLength = lines.Length > 0 ? lines[0].Length : 0;
			string verticalLine = GetVerticalLine('=', contentLength);
			string spacerLine = GetVerticalLine(' ', contentLength);

			// Build output
			sb.Append(verticalLine);
			foreach (string line in lines)
			{
				sb.Append("| ");
				sb.Append(line);
				sb.Append(" |");
				sb.Append('\n');
			}

			// Flat hexes have too little bottom space as they use the _ char, so add an extra filler line.
			if (_printer.HexOrientation == HexOrientation.Flat)
			{
				sb.Append(spacerLine);
			}
			sb.Append(verticalLine);
			return sb.ToString();
		}
		else
		{
			return _grid.Print(true);
		}
	}

	private string GetVerticalLine(char filler, int contentLength)
	{
		StringBuilder verticalLine = new StringBuilder("| ");
		for (int i = 0; i < contentLength; i++)
		{
			verticalLine.Append(i % 2 == 0 ? filler : ' ');
		}
		return verticalLine.Append(" |\n").ToString();
	}
}
