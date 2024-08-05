namespace Hive.Core.Models.Coordinate;

public class Cube
{
	public int Q;
	public int R;
	public int S;

	public Cube(int q, int r, int s)
	{
		Q = q;
		R = r;
		S = s;
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
}
