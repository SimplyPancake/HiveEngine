using Hive.Core;
using Hive.Core.Models;
using Hive.Core.Models.Coordinate;

namespace Hive.Test;

public class BoardTest
{
	[SetUp]
	public void Setup()
	{
	}

	[Test]
	public void SurroundingPositionsTest()
	{
		Piece whiteQueen = new(Color.White, new QueenBug(), new Cube(0, 0, 0));
		Piece blackQueen = new(Color.Black, new QueenBug(), new Cube(1, 0, 0));

		List<Piece> pieces = [whiteQueen, blackQueen];

		Board b = new(pieces);

		Assert.That(b.SurroundingPieces(whiteQueen.Position), Does.Contain(blackQueen));
	}
}
