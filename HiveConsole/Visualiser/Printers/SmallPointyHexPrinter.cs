namespace Hive.Console.Visualiser.Printers;

public class SmallPointyAsciiHexPrinter : AsciiHexPrinter
{
	private readonly int _width = 10;
	private readonly int _height = 6;
	private readonly int _sideLength = 4;
	private readonly int _sideHeight = 2;
	private readonly int _bordersLength = 2;

	public override string GetHex(string textLine1, string textLine2, char fillerChar)
	{
		string line1 = RestrictToLength(textLine1, 3);
		string line2 = RestrictToLength(textLine2, 3);
		string hex = TEMPLATE;

		hex = hex.Replace("XXX", line1);
		hex = hex.Replace("YYY", line2);

		return hex.Replace('#', fillerChar);
	}

	public override int[] MapHexCoordsToCharCoords(int q, int r)
	{
		int[] result = [(_width - _bordersLength) * q + r % 2 * (_height - _sideHeight), (_height - _sideHeight) * r];
		return result;
	}

	public override int[] GetMapSizeInChars(int hexWidth, int hexHeight)
	{
		int widthInChars = hexWidth * _width + _sideLength;
		int heightInChars = hexHeight * (_height - 2) + 2;
		return [widthInChars, heightInChars];
	}

	public override HexOrientation HexOrientation
	{
		get { return HexOrientation.Pointy; }
	}

	private static readonly string TEMPLATE =
		  "   /#\\   \n" // 0 - 10
		+ " /# # #\\ \n" // 10 - 20
		+ "|# XXX #|\n" // 20 - 30
		+ "|# YYY #|\n" // 30 - 40
		+ " \\# # #/ \n" // 40 - 50
		+ "   \\#/   \n"; // 50 - 60
}
