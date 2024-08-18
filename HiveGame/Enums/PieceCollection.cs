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
			_ => [
				new QueenBug(),
				new AntBug(),
				new BeetleBug(),
				new GrasshopperBug(),
				new SpiderBug()
			],
		};
	}
}