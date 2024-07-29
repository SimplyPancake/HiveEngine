using System.Numerics;

namespace HiveGame;

public class Piece2D(Vector2 pos, Piece piece)
{
	public Vector2 Position = pos;

	public Piece piece = piece;
}

public static class HexagonService
{
	public static void PrintBoard(List<Piece> pieces)
	{
		List<Piece2D> piece2Ds = CubesToOddQs(pieces);

		// a - b - c - d - e -
		// -f - g - h - i - j
		// k - l - m - n - o -
		// -p - q - r - s - t
		// u - v - w - x - y -


	}

	public static List<Piece2D> CubesToOddQs(List<Piece> pieces)
	{
		List<Piece2D> output = new List<Piece2D>();
		foreach (Piece piece in pieces)
		{
			output.Add(CubeToOddQ(piece));
		}

		// Now fix the coordinates
		float minX = output.Min(p => p.Position.X);
		float minY = output.Min(p => p.Position.Y);

		float inverseX = minX * -1;
		float inverseY = minY * -1;

		// MinX and minY are almost all the time minimum
		// Shift everything by the inverse of minX and minY
		output.ForEach(p =>
			p.Position.X += inverseX
		);

		output.ForEach(p =>
			p.Position.Y += inverseY
		);

		// Now, we only have the populated positions,

		return output;
	}

	public static Piece2D CubeToOddQ(Piece piece)
	{
		Vector2 newCoords = new Vector2();
		Vector3 position3d = piece.Position;
		newCoords.X = position3d.X; // col = hex.q

		bool isEven = Convert.ToInt32(Math.Floor(position3d.X)) % 2 == 0;
		int offset = isEven ? 1 : 0;
		newCoords.Y = position3d.Y + (position3d.X - offset) / 2; // row = hex.r + (hex.q - (hex.q&1)) / 2

		return new Piece2D(newCoords, piece);
	}

	public static List<List<Piece2D>> Grid2d(List<Piece> pieces)
	{
		// Convert cube to 
		return new List<List<Piece2D>>();
	}
}
