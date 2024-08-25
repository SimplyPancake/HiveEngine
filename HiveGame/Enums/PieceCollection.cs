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
}