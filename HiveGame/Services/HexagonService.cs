using System.Numerics;

namespace Hive.Core;

public class Piece2D
{
	public Piece2D(int x, int y, Piece piece)
	{
		this.x = x;
		this.y = y;
		this.Piece = piece;
	}

	public Piece2D(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public int x;

	public int y;

	public int height = 0;

	public Piece? Piece;
}

// TODO; add support for pieces on top of each-other
public static class HexagonService
{
	public static void PrintBoard(List<Piece> pieces)
	{
		List<List<Piece2D>> piece2Ds = Grid2d(pieces);

		for (int i = 0; i < piece2Ds.Count(); i++)
		{
			// write the row
			string row = i % 2 == 0 ? "" : " ";

			foreach (Piece2D piece in piece2Ds[i])
			{
				row += " ";
				char pieceRepresentation = ' ';

				if (piece.Piece != null)
				{
					pieceRepresentation = piece.Piece.Bug.ShortRepresentation;
				}
				row += pieceRepresentation;
			}

			Console.WriteLine(row);
		}
	}

	public static List<Piece2D> CubesToOddQs(List<Piece> pieces)
	{
		// https://www.redblobgames.com/grids/hexagons/#conversions
		List<Piece2D> output = new List<Piece2D>();

		if (pieces.Count == 0)
		{
			return output;
		}

		foreach (Piece piece in pieces)
		{
			output.Add(CubeToOddQ(piece));
		}

		// Now fix the coordinates (by a stupid way, but it wors hahah)
		float minX = output.Min(p => p.x);
		float minY = output.Min(p => p.y);

		float inverseX = minX * -1;
		float inverseY = minY * -1;

		// MinX and minY are almost all the time minimum
		// Shift everything by the inverse of minX and minY
		output.ForEach(p =>
			p.x += Convert.ToInt32(inverseX)
		);

		output.ForEach(p =>
			p.y += Convert.ToInt32(inverseY)
		);

		// Now, we only have the populated positions,

		return output;
	}

	public static Piece2D CubeToOddQ(Piece piece)
	{
		Vector3 position3d = piece.Position;
		int x = Convert.ToInt32(position3d.X); // col = hex.q

		bool isEven = Convert.ToInt32(Math.Floor(position3d.X)) % 2 == 0;
		int offset = isEven ? 1 : 0;
		int y = Convert.ToInt32(position3d.Y + (position3d.X - offset) / 2); // row = hex.r + (hex.q - (hex.q&1)) / 2

		return new Piece2D(x, y, piece);
	}

	public static List<List<Piece2D>> Grid2d(List<Piece> pieces)
	{
		// get cubes to oddqs converted
		List<List<Piece2D>> grid = new();

		if (pieces.Count == 0)
		{
			return grid;
		}

		List<Piece2D> pieces2d = CubesToOddQs(pieces);

		// TODO: make sure the visual alignment is also centered

		// then build the field with empty pieces (or not)
		int maxx = Convert.ToInt32(pieces2d.Min(p => p.x));
		int maxy = Convert.ToInt32(pieces2d.Max(p => p.y));

		for (int y = 0; y < maxy; y++)
		{
			List<Piece2D> row = new();

			for (int x = 0; x < maxx; x++)
			{
				Piece2D piece = new(x, y);

				// is there already a piece with these coordinates?
				if (pieces2d.Exists(p => p.x == x && p.y == y))
				{
					piece = pieces2d.First(p => p.x == x && p.y == y);
				}

				row.Add(piece);
			}

			grid.Add(row);
		}

		return grid;
	}
}