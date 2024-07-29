namespace HiveGame;

public enum PieceCollection
{
	Classic,

}

public static class PieceCollectionMethods
{

	public static List<IBug> GetPieceBugs(this PieceCollection c)
	{
		return new List<IBug>
		{
			new QueenBugCreator().CreateBug()
		};
	}
}