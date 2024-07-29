namespace HiveGame;

public class QueenBug : IBug
{
	public string Name => "Queen";

	public string Description => "The queen";

	public int BugTypeId => (int)BugType.Queen;

	public int GetAmount => 1;

	public char ShortRepresentation => 'Q';

	public bool MoveRestrictionsApply => true;
}

public class QueenBugCreator : BugCreator
{
	public override IBug CreateBug()
	{
		return new QueenBug();
	}
}