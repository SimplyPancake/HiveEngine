using Hive.Core;
using Hive.Core.Models.Coordinate;

namespace Hive.Test;

public class CoordinateTest
{
	[Test]
	public void CubeTest()
	{
		Cube truthCube = new(0, 0, 0);
		Axial truthAxial = new(0, 0);
		Cube testCube = new(truthAxial);

		Assert.That(Equals(truthCube, testCube));

		truthCube = new(3, -2, -1);
		truthAxial = new(3, -2);
		testCube = new(truthAxial);

		Assert.That(Equals(truthCube, testCube));
	}

	private bool Equals(Cube c1, Cube c2)
	{
		return c1.Q == c2.Q && c1.R == c2.R && c1.S == c2.S;
	}
}
