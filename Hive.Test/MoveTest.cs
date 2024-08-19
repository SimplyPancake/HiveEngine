using System;
using System.Diagnostics;
using Hive.Core;
using Hive.Core.Models;
using Hive.Core.Models.Bugs;
using Hive.Core.Models.Coordinate;

namespace Hive.Test;

public class MoveTest
{
	[Test]
	public void MoveStringTest()
	{
		Piece hopper = new(Color.White, new GrasshopperBug(), new Cube(3, -3));

		List<Piece> pieces = [
			hopper,
			new(Color.Black, new QueenBug(), new Cube(3, -2, -1)),
			new(Color.Black, new QueenBug(), new Cube(0, -1, 1)),
			new(Color.Black, new QueenBug(), new Cube(1, -2, 1)),
			new(Color.Black, new QueenBug(), new Cube(2, -2, 0)),
			new(Color.Black, new QueenBug(), new Cube(2, -1, -1)),
			new(Color.Black, new QueenBug(), new Cube(2, 0, -2)),
			new(Color.White, new AntBug(), new Cube(0, 0, 0)),
		];

		Board board = new(pieces);

		// TODO
		// Debug.WriteLine(Move.MoveFromAttackString("wA1 bQ/", board));
	}

}
