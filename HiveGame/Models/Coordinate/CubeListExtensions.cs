using Hive.Core.Enums;

namespace Hive.Core.Models.Coordinate;

public static class CubeListExtensions
{
	public static bool ContainsCube<T>(List<Cube> list, T cube)
	{
		return list.Any(x => x.Equals(cube));
	}
}
