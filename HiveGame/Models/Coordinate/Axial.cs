using Hive.Core.Models.Coordinate;

namespace Hive.Core;

public class Axial
{
	public int Q;
	public int R;

	public Axial(int q, int r)
	{
		Q = q;
		R = r;
	}

	public Axial(Cube c)
	{
		Q = c.Q;
		R = c.R;
	}
}
