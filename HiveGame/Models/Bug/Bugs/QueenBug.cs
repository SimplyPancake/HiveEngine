
using System.Linq;
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

		// Get all POSITIONS surrounding the queen
		List<Cube> surroundingQueen = Board.SurroundingPositions(piece.Position);
		List<Cube> boardPiecePositions = board.Pieces.Select(p => p.Position).ToList();

		// Queen cannot move on a piece that is already there
		List<Cube> surroundingQueenButNotPiece =
			surroundingQueen.Where(position => !boardPiecePositions.Contains(position)).ToList();

		// We then compare with all pieces that are not the queen itself
		List<Piece> allPiecesWithoutQueen = new(board.Pieces.Where(p => p != piece));

		// Check for possible points that are next to exactly 1 other piece
		surroundingQueenButNotPiece = surroundingQueenButNotPiece
			.Where(pos => board.AmountOfSurroundingPieces(pos) == 1)
			.ToList();

		// Then assemble moves from these positions
		List<Move> moves = [];
		foreach (Cube pos in surroundingQueen)
		{
			moves.Add(new AttackMove(piece, pos, 0));
		}

		return moves;
	}
}
