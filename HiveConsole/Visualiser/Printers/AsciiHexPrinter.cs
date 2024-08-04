using System.Globalization;

namespace Hive.Console.Visualiser.Printers;
public abstract class AsciiHexPrinter
{
	/// <summary>
	/// Returns the hex.
	/// </summary>
	public abstract string GetHex(string textLine1, string textLine2, char fillerChar);

	/// <summary>
	/// Viewing the board as a grid of hexes. Each hex has a bounding box. Map top-left of bounding box given by hex
	/// coordinates to same area viewed as char grid.
	///
	/// <returns>A int[2] with (x,y) char coordinates. (top,left) is (0,0)</returns>
	/// </summary>
	public abstract int[] MapHexCoordsToCharCoords(int q, int r);

	/// <summary>
	/// Returns the bounding box in chars for a map of the given size.
	///
	/// <param name="hexWidth">Size of board in hexes horizontally.</param>
	/// <param name="hexHeight">Size of board in hexes vertically.</param>
	/// <returns>A int[2]: int[0] gives the width in chars and int[1] gives the height.</returns>
	/// </summary>
	public abstract int[] GetMapSizeInChars(int hexWidth, int hexHeight);

	/// <summary>
	/// Returns the orientation of hexes from the given HexPrinter.
	/// </summary>
	public abstract HexOrientation HexOrientation { get; }

	/// <summary>
	/// Makes sure that a string has the given length, using " " (whitespace) if input string is shorter.
	/// </summary>
	protected string RestrictToLength(string str, int length)
	{
		string result = "  ";
		if (str != null)
		{
			result = str.Length > length
				? str.ToUpper(CultureInfo.CurrentCulture).Substring(0, length)
				: str.Length < length
					? Pad(str.ToUpper(CultureInfo.CurrentCulture), length - str.Length)
					: str;
		}
		return result;
	}

	/// <summary>
	/// Pads whitespace to both sides, effectively centering the text.
	/// Padding starts on the left side.
	/// </summary>
	/// <param name="str">The string to pad.</param>
	/// <param name="size">Size of the final string.</param>
	/// <returns>Padded string.</returns>
	private string Pad(string str, int size)
	{
		string s = str;
		int n = size;
		while (n > 0)
		{
			s = n % 2 == 0 ? " " + s : s + " ";
			n--;
		}
		return s;
	}
}