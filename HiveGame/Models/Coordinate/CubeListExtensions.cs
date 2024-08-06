namespace Hive.Core.Models.Coordinate;

public static class CubeListExtensions
{
	public static bool ContainsCube(this List<Cube> list, Cube cube)
	{
		foreach (var item in list)
		{
			if (item.Equals(cube))
			{
				return true;
			}
		}
		return false;
	}
}
