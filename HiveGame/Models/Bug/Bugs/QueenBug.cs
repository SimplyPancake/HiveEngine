
using Hive.Core.Models;

namespace Hive.Core;

public class QueenBug : Bug
{
	public override string Name => "Queen";

	public override string Description => "The Queen Bug";

	public override int BugTypeId => (int)BugType.Queen;

	public override int GetAmount => 1;

	public override char ShortRepresentation => 'Q';

	public override bool MoveRestrictionsApply => true;

	private protected override List<Move> pieceMoves(Piece piece, Board board)
	{
		// Piece may only move one spot.
		throw new NotImplementedException();
	}
}
