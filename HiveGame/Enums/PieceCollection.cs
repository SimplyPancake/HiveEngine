using Hive.Core.Models.Bugs;

namespace Hive.Core;

public enum PieceCollection
{
	Classic,
	All

}

public static class PieceCollectionMethods
{
	public static List<Bug> GetPieceBugs(this PieceCollection c)
	{
		return c switch
		{
			PieceCollection.Classic => [
				new QueenBug(),
				new AntBug(),
				new AntBug(),
				new AntBug(),
				new BeetleBug(),
				new BeetleBug(),
				new GrasshopperBug(),
				new GrasshopperBug(),
				new GrasshopperBug(),
				new SpiderBug(),
				new SpiderBug(),
				new SpiderBug()
			],
			_ => [ // defined according to int in BugType
				new QueenBug(),
				new SpiderBug(),
				new AntBug(),
				new GrasshopperBug(),
				new BeetleBug(),
			],
		};
	}

	// Is defending?
	public static bool IsDefending(int bugType)
	{
		return bugType switch
		{
			(int)BugType.Ant => true,
			(int)BugType.Beetle => true,
			(int)BugType.Queen => true,
			_ => false
		};
	}

	// Is attacking?
	public static bool IsAttacking(int bugType)
	{
		return bugType switch
		{
			(int)BugType.Grasshopper => true,
			(int)BugType.Spider => true,
			_ => false
		};
	}
}