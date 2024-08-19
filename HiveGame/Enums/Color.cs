namespace Hive.Core;

public enum Color
{
	Black,
	White
}

public static class ColorMethods
{

	public static Color GetOtherColor(this Color c)
	{
		if (c == Color.White)
		{
			return Color.Black;
		}

		return Color.White;
	}
}