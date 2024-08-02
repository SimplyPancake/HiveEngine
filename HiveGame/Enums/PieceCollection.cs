namespace HiveGame;

public enum PieceCollection
{
	Classic,

}

public static class PieceCollectionMethods
{

	public static List<Bug> GetPieceBugs(this PieceCollection c)
	{
		return new List<Bug>
		{
			new QueenBug()
		};
	}
}