namespace Hive.Console.Visualiser.Printers;

public class TinyFlatAsciiHexPrinter : AsciiHexPrinter
{
	private readonly int _width = 3;
	private readonly int _height = 2;
	private readonly int _sideLength = 1;

	public override HexOrientation HexOrientation => HexOrientation.Flat;

	public override string GetHex(string textLine1, string textLine2, char fillerChar)
	{
		string line1 = RestrictToLength(textLine1, 1);
		string hex = TEMPLATE;

		hex = hex.Replace("X", line1);
		hex = hex.Replace("Y", fillerChar.ToString());

		return hex;
	}

	public override int[] GetMapSizeInChars(int hexWidth, int hexHeight)
	{
		int widthInChars = hexWidth * (_width - _sideLength) + _sideLength;
		int heightInChars = (hexWidth - 1) * _height / 2 + hexHeight * _height;
		return [widthInChars, heightInChars];
	}

	public override int[] MapHexCoordsToCharCoords(int q, int r)
	{
		int[] result =
		[
			(_width - _sideLength) * q, // q * (width - side)
			_height / 2 * q + (_height - 1) * r, // height/2 * q + (height - 1) * r
		];
		return result;
	}

	private static readonly string TEMPLATE =
		  "/X\\\n" // 0 - 2 
		+ "\\Y/\n"; // 3 - 5
}
