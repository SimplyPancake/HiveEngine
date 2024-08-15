using System.Diagnostics.CodeAnalysis;
using Hive.Core.Enums;

namespace Hive.Core.Models.Coordinate;

public class Cube : IEquatable<Cube>, IEquatable<Axial>, IEquatable<CubeVector>
{
	public int Q;
	public int R;
	public int S;

	public Cube(int q, int r, int s)
	{
		if (q + r + s != 0)
		{
			throw new Exception($"q+r+s must be 0, instead it is {q + r + s}");
		}

		Q = q;
		R = r;
		S = s;
	}

	public Cube(CubeVector cubeVector)
	{
		Cube c = CubeVectorExtensions.VectorToCube(cubeVector);
		Q = c.Q;
		R = c.R;
		S = c.S;
	}

	public Cube(int q, int r) : this(new Axial(q, r)) { }

	public Cube(Axial a)
	{
		Q = a.Q;
		R = a.R;
		S = -a.Q - a.R;
	}

	public override string ToString()
	{
		return $"{Q},{R},{S}";
	}

	public bool IsVector()
	{
		return
			(Q == 1 || Q == -1 || Q == 0) &&
			(R == 1 || R == -1 || R == 0) &&
			(S == 1 || S == -1 || S == 0) &&
			Q + R + S == 0;
	}

	public static int Distance(Cube a, Cube b)
	{
		// (abs(a.q - b.q) + abs(a.r - b.r) + abs(a.s - b.s)) / 2
		return (Math.Abs(a.Q - b.Q) + Math.Abs(a.R - b.R) + Math.Abs(a.S - b.S)) / 2;
	}

	#region Overrides
	public override bool Equals(object? obj)
	{

		if (obj == null || !(obj.GetType() == typeof(Cube) || obj.GetType() == typeof(CubeVector) || obj.GetType() == typeof(Axial)))
		{
			return false;
		}

		Cube other;

		if (obj.GetType() == typeof(CubeVector))
		{
			other = CubeVectorExtensions.VectorToCube((CubeVector)obj);
		}
		else if (obj.GetType() == typeof(Axial))
		{
			other = new Cube((Axial)obj);
		}
		else
		{
			other = (Cube)obj;
		}

		return Q == other.Q && R == other.R && S == other.S;
	}

	// Override GetHashCode method
	public override int GetHashCode()
	{
		return HashCode.Combine(Q, R, S);
	}

	public bool Equals(Cube? other)
	{
		if (other == null)
		{
			return false;
		}

		return other.Q == Q && other.R == R && S == other.S;
	}

	public bool Equals(Axial? other)
	{
		if (other == null)
		{
			return false;
		}

		Cube cube = new(other);

		return cube.Q == Q && cube.R == R && S == cube.S;
	}

	public bool Equals(CubeVector other)
	{
		Cube cube = CubeVectorExtensions.VectorToCube(other);

		return cube.Q == Q && cube.R == R && S == cube.S;
	}

	public static Cube operator +(Cube a) => a;

	public static Cube operator +(Cube a, Cube b)
		=> new(a.Q + b.Q, a.R + b.R, a.S + b.S);

	public static Cube operator -(Cube a) => new(-a.Q, -a.R, -a.S);

	public static Cube operator -(Cube a, Cube b) => a + (-b);

	public static Cube operator *(Cube a, int b) => new(b * a.Q, b * a.R, b * a.S);

	public static Cube operator *(int b, Cube a) => new(b * a.Q, b * a.R, b * a.S);

	#endregion
}

public class CubeComparer : IEqualityComparer<Cube>
{
	public bool Equals(Cube? x, Cube? y)
	{
		if (x == null && y == null)
		{
			return true;
		}

		if (x == null)
		{
			return false;
		}

		return x.Equals(y);
	}

	public int GetHashCode([DisallowNull] Cube obj)
	{
		return obj.GetHashCode();
	}
}