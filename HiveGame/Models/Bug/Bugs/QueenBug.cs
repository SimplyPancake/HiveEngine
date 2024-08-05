
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
		List<Cube> positions = Board.SurroundingPositions(piece.Position);

		// We get all pieces that are not the queen itself
		List<Piece> piecesWithoutQueen = new(board.Pieces);
		piecesWithoutQueen.Remove(piece); // will this work?


		// TODO!

		// now we select all the pieces from the board that are in the
		// surrounding positions AND that only have exactly 1 neighbour

		var temp = piecesWithoutQueen.Where(p1 => positions.Any(p2 =>
			p1.Position.Q == p2.Q &&
			p1.Position.R == p2.R &&
			p1.Position.R == p2.S
		)).ToList(); // select intersect from all pieces from board without queen AND surrounding of queen

		// Then select all the surrounding positions of the queen where the distance to one of temp
		temp = positions.Where(p => board.AmountOfSurroundingPieces(p) == 1).ToList();

		positions = piecesWithoutQueen
			.Where(p => positions.Contains(p.Position))
			.Where(p => board.AmountOfSurroundingPieces(p) == 1)
			.Select(p => p.Position)
			.ToList();

		// Then assemble moves from these positions
		List<Move> moves = [];
		foreach (Cube pos in positions)
		{
			moves.Add(new AttackMove(piece, pos, 0));
		}

		return moves;
	}
}
