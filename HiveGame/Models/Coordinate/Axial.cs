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

	public override bool Equals(object? obj)
	{
		//
		// See the full list of guidelines at
		//   http://go.microsoft.com/fwlink/?LinkID=85237
		// and also the guidance for operator== at
		//   http://go.microsoft.com/fwlink/?LinkId=85238
		//

		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}

		return GetHashCode() == obj.GetHashCode();
	}

	// override object.GetHashCode
	public override int GetHashCode()
	{
		return HashCode.Combine(Q, R);
	}
}
