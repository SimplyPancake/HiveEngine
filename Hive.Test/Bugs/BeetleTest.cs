using System.Diagnostics;
using Hive.Core;
using Hive.Core.Models;
using Hive.Core.Models.Bugs;
using Hive.Core.Models.Coordinate;

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

		Debug.WriteLine(board);

		List<Move> moves = beetle.PossibleMoves(board);

		Assert.That(moves, Has.Count.EqualTo(3));

		List<AttackMove> attackMoves = moves.Select(move => (AttackMove)move).ToList();
		List<(Cube position, int height)> rightMovePositions = new([
			new (new Cube(0, 0, 0), 1),
			new (new Cube(-1, 0, 1), 0),
			new (new Cube(0, 1, -1), 0),
		]);

		foreach (var (position, height) in rightMovePositions)
		{
			Assert.That(attackMoves.Any(m => m.AttackPosition.Equals(position) && m.AttackHeight == height));
		}

		// test on-top movement
		beetle = new(Color.White, new BeetleBug(), new Cube(0, 0, 0), 1);

		pieces = [
			beetle,
			new(Color.Black, new QueenBug(), new Cube(0, -1, 1)),
			new(Color.Black, new QueenBug(), new Cube(1, -2, 1)),
			new(Color.Black, new QueenBug(), new Cube(2, -2, 0)),
			new(Color.Black, new QueenBug(), new Cube(2, -1, -1)),
			new(Color.Black, new QueenBug(), new Cube(2, 0, -2)),
			new(Color.White, new QueenBug(), new Cube(0, 0, 0)),
		];

		Debug.WriteLine(board);

		moves = beetle.PossibleMoves(board);

		Assert.That(moves, Has.Count.EqualTo(6));

		// test sliding rule
	}
}
