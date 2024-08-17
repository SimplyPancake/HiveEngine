using Hive.Core.Models.Coordinate;

namespace Hive.Core.Enums;

public enum CubeVector
{
	TopLeft = 0,
	TopRight = 1,
	Right = 2,
	BottomRight = 3,
	BottomLeft = 4,
	Left = 5,
	Zero = -1
}

public static class CubeVectorExtensions
{
	public static CubeVector Successor(CubeVector v)
	{
		if (v == CubeVector.Zero)
		{
			return CubeVector.Zero;
		}

		int vectorInt = (int)v;

		int nextVector = (vectorInt + 1) % 6;
		return (CubeVector)nextVector;
	}

	public static Cube SuccessorCube(CubeVector v)
	{
		return VectorToCube(Successor(v));
	}

	public static CubeVector Predecessor(CubeVector v)
	{
		if (v == CubeVector.Zero)
		{
			return CubeVector.Zero;
		}

		int vectorInt = (int)v;

		int nextVector = (vectorInt - 1) % 6;
		return (CubeVector)nextVector;
	}

	public static Cube PredecessorCube(CubeVector v)
	{
		return VectorToCube(Predecessor(v));
	}

	public static CubeVector CubeToVector(Cube vector)
	{
		Cube topLeft = new(0, -1, 1);
		Cube topRight = new(1, -1, 0);
		Cube right = new(1, 0, -1);
		Cube bottomRight = -topLeft;
		Cube bottomLeft = -topRight;
		Cube left = -right;

		if (vector.Equals(topLeft))
		{
			return CubeVector.TopLeft;
		}
		else if (vector.Equals(topRight))
		{
			return CubeVector.TopRight;
		}
		else if (vector.Equals(right))
		{
			return CubeVector.Right;
		}
		else if (vector.Equals(bottomRight))
		{
			return CubeVector.BottomRight;
		}
		else if (vector.Equals(bottomLeft))
		{
			return CubeVector.BottomLeft;
		}
		else if (vector.Equals(left))
		{
			return CubeVector.Left;
		}
		return CubeVector.Zero;
	}

	public static Cube VectorToCube(CubeVector vector)
	{
		Cube topLeft = new(0, -1, 1);
		Cube topRight = new(1, -1, 0);
		Cube right = new(1, 0, -1);

		return vector switch
		{
			CubeVector.TopLeft => topLeft,
			CubeVector.TopRight => topRight,
			CubeVector.Right => right,
			CubeVector.BottomRight => -topLeft,
			CubeVector.BottomLeft => -topRight,
			CubeVector.Left => -right,
			_ => new(0, 0, 0),
		};
	}

	public static CubeVector Add(CubeVector a) => a;

	public static CubeVector Add(CubeVector a, CubeVector b)
		=> CubeToVector(VectorToCube(a) + VectorToCube(b));

	public static CubeVector Subtract(CubeVector a)
		=> CubeToVector(-VectorToCube(a));

	public static CubeVector Subtract(CubeVector a, CubeVector b)
		=> CubeToVector(VectorToCube(a) - VectorToCube(b));
}