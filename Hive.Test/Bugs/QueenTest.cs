using Hive.Core;
using Hive.Core.Models;
using Hive.Core.Models.Coordinate;
using NUnit.Framework.Constraints;

namespace Hive.Test;

public class QueenTest
{
	public Board board;
	public Piece whiteQueen;

	[SetUp]
	public void Setup()
	{
		whiteQueen = new(Color.White, new QueenBug(), new Cube(0, 0, 0));
		Piece blackQueen = new(Color.Black, new QueenBug(), new Cube(0, -1, 1));

		List<Piece> pieces = [whiteQueen, blackQueen];

		board = new(pieces);
	}

	[Test]
	public void SurroundingPositionsTest()
	{
		List<Move> moves = new QueenBug().PossibleMoves(whiteQueen, board);

		// TODO; incorporate placing rules
		// Strictly speaking move-wise, a queen could only move 2 places.
		Assert.That(moves.Count, Is.EqualTo(2));

		// Assert move positions
	}
}
