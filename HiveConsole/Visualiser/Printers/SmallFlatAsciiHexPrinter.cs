
namespace Hive.Console.Visualiser.Printers;

public class SmallFlatAsciiHexPrinter : AsciiHexPrinter
{
	private readonly int _width = 9;
	private readonly int _height = 5;
	private readonly int _sideLength = 2;

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
		int[] result =
		[
			7 * q, // q * (width - side)
			2 * q + 4 * r, // height/2 * q + (height - 1) * r
		];
		return result;
	}

	public override int[] GetMapSizeInChars(int hexWidth, int hexHeight)
	{
		int widthInChars = hexWidth * (_width - _sideLength) + _sideLength;
		int heightInChars = (hexWidth - 1) * _height / 2 + hexHeight * _height;
		return [widthInChars, heightInChars];
	}

	public override HexOrientation HexOrientation => HexOrientation.Flat;

	private const string TEMPLATE =
		"   _ _   \n" + // 0 - 9
		" /# # #\\ \n" + // 9 - 18
		"/# XXX #\\\n" + // 18 - 27
		"\\# YYY #/\n" + // 27 - 36
		" \\#_#_#/ ";   // 36 - 45
}
