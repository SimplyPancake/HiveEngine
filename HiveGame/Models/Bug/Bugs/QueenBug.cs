
using Hive.Core.Models;
using Hive.Core.Models.Coordinate;

namespace Hive.Core;

public class QueenBug : Bug
{
	public override string Name => "Queen";

	public override string Description => "The Queen Bug";

	public override int BugTypeId => (int)BugType.Queen;

	public override int GetAmount => 1;

	public override char ShortRepresentation => 'Q';

	public override bool MoveRestrictionsApply => true;

	private protected override List<Move> PieceMoves(Piece piece, Board board)
	{
		// Piece may only move one spot.
		// Or; every spot that is next to another bug, which is not itself
		List<Cube> positions = Board.SurroundingPositions(piece.Position);

		// We get all pieces
		List<Piece> piecesWithoutSelected = new(board.Pieces);
		piecesWithoutSelected.Remove(piece);

		// for all probable positions, only choose the ones that 
	}
}
