using System.Diagnostics;
using Hive.Core;
using Hive.Core.Enums;
using Hive.Core.Models;
using Hive.Core.Models.Bugs;
using Hive.Core.Models.Coordinate;
using Hive.Core.Services;

namespace Hive.Test.Bugs;

public class QueenTest
{
	[SetUp]
	public void Setup()
	{
	}

	[Test]
	public void PossibleMovesTest()
	{
		Piece whiteQueen = new(Color.White, new QueenBug(), new Cube(-1, 1, 0));

		List<Piece> pieces = [
			whiteQueen,
			new(Color.Black, new QueenBug(), new Cube(0, -1, 1)),
			new(Color.Black, new QueenBug(), new Cube(1, -2, 1)),
			new(Color.Black, new QueenBug(), new Cube(2, -2, 0)),
			new(Color.Black, new QueenBug(), new Cube(2, -1, -1)),
			new(Color.Black, new QueenBug(), new Cube(2, 0, -2)),
			new(Color.White, new QueenBug(), new Cube(0, 0, 0)),
		];

		Board board = new(pieces);

		Debug.WriteLine(board);

		List<Move> moves = new QueenBug().PossibleMoves(whiteQueen, board);

		// TODO; incorporate placing rules
		// Strictly speaking move-wise, a queen could only move 2 places.
		Assert.That(moves, Has.Count.EqualTo(2));

		// Assert move positions
		List<Cube> movePositions = moves.Select(m => ((AttackMove)m).AttackPosition).ToList();
		Assert.Multiple(() =>
		{
			Assert.That(movePositions, Does.Contain(new Cube(CubeVector.Left)));
			Assert.That(movePositions, Does.Contain(new Cube(CubeVector.BottomRight)));

		});
	}
}
