
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

		// We get all pieces that are not the queen itself
		List<Piece> piecesWithoutSelected = new(board.Pieces);
		piecesWithoutSelected.Remove(piece); // will this work?

		// Queen cannot move on top of another bug, so eliminate those bugs from it's moveset
		foreach (Piece p in Board.SurroundingPieces(piece.Position, piecesWithoutSelected))
		{
			positions.Remove(p.Position);
		}

		// for all probable positions, only choose the ones that are next
		// to one of the bugs in piecesWithoutSelected
		positions = positions.Where(p => Board.IsNextToPiece(p, piecesWithoutSelected)).ToList();

		// Then assemble moves from these positions
		List<Move> moves = [];
		foreach (Cube pos in positions)
		{
			moves.Add(new AttackMove(piece, pos, 0));
		}

		return moves;
	}
}
