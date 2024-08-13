using System;
using System.Diagnostics;
using Hive.Core;
using Hive.Core.Enums;
using Hive.Core.Models;
using Hive.Core.Models.Bugs;
using Hive.Core.Models.Coordinate;

namespace Hive.Test.Bugs;

public class SpiderTest
{
	[Test]
	public void PossibleMovesTest()
	{
		Piece spider = new(Color.White, new SpiderBug(), new Cube(-1, 1, 0));

		List<Piece> pieces = [
			spider,
			new(Color.Black, new QueenBug(), new Cube(0, -1, 1)),
			new(Color.Black, new QueenBug(), new Cube(1, -2, 1)),
			new(Color.Black, new QueenBug(), new Cube(2, -2, 0)),
			new(Color.Black, new QueenBug(), new Cube(2, -1, -1)),
			new(Color.Black, new QueenBug(), new Cube(2, 0, -2)),
			new(Color.White, new QueenBug(), new Cube(0, 0, 0)),
		];

		Board board = new(pieces);

		Debug.WriteLine(board);

		List<Move> moves = new SpiderBug().PossibleMoves(spider, board);

		// TODO; incorporate placing rules
		// Strictly speaking move-wise, a queen could only move 2 places.
		Assert.That(moves, Has.Count.EqualTo(2));

		// Assert move positions
		List<Cube> movePositions = moves.Select(m => ((AttackMove)m).AttackPosition).ToList();
		// Assert.That(CubeListExtensions.ContainsCube(movePositions, new Cube(CubeVector.BottomRight)));
		// Assert.That(CubeListExtensions.ContainsCube(movePositions, new Cube(CubeVector.Left)));
		Debug.WriteLine("yesy");
	}
}
