using System;
using System.Diagnostics;
using Hive.Core;
using Hive.Core.Enums;
using Hive.Core.Models;
using Hive.Core.Models.Bugs;
using Hive.Core.Models.Coordinate;
using Hive.Core.Services;

namespace Hive.Test.Bugs;

public class BeetleTest
{
	[Test]
	public void PossibleMovesTest()
	{
		Piece beetle = new(Color.White, new BeetleBug(), new Cube(-1, 1, 0));

		List<Piece> pieces = [
			beetle,
			new(Color.Black, new QueenBug(), new Cube(0, -1, 1)),
			new(Color.Black, new QueenBug(), new Cube(1, -2, 1)),
			new(Color.Black, new QueenBug(), new Cube(2, -2, 0)),
			new(Color.Black, new QueenBug(), new Cube(2, -1, -1)),
			new(Color.Black, new QueenBug(), new Cube(2, 0, -2)),
			new(Color.White, new QueenBug(), new Cube(0, 0, 0)),
		];

		Board board = new(pieces);

		Debug.WriteLine(ConsoleHexPrinter.BoardString(board));

		List<Move> moves = beetle.PossibleMoves(board);

		// TODO; incorporate placing rules
		// Strictly speaking move-wise, a queen could only move 2 places.
		Assert.That(moves, Has.Count.EqualTo(2));
	}
}
