using System.Diagnostics;
using Hive.Core;
using Hive.Core.Models;
using Hive.Core.Models.Bugs;
using Hive.Core.Models.Coordinate;

namespace Hive.Test.Bugs;

public class BeetleTest
{
	[Test]
	public void JumpGroundMoveTest()
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
	}

	[Test]
	public void JumpFtmBLockedTest()
	{
		Piece beetle = new(Color.White, new BeetleBug(), new Cube(0, 1, -1)); // B

		List<Piece> pieces = [
			new(Color.White, new SpiderBug(), new Cube(0, 0, 0)), // D
			new(Color.White, new SpiderBug(), new Cube(0, 0, 0), 1), // D
			new(Color.White, new QueenBug(), new Cube(-1, 1, 0)), // A
			new(Color.White, new QueenBug(), new Cube(-1, 2, -1)), // C
			new(Color.White, new QueenBug(), new Cube(-1, 2, -1), 1), // C
			beetle, // B
		];

		Board board = new(pieces);

		Debug.WriteLine(board);

		List<Move> moves = beetle.PossibleMoves(board);

		Assert.That(moves, Has.Count.EqualTo(4));

		List<AttackMove> attackMoves = moves.Select(move => (AttackMove)move).ToList();
		Assert.That(!attackMoves.Any(m =>
			m.AttackPosition.Equals(new Axial(-1, 1))
		)); // The beetle may NOT jump on top of A due to the "fence" by C and D
	}

	[Test]
	public void JumpFtmAllowedTest()
	{
		Piece beetle = new(Color.White, new BeetleBug(), new Cube(0, 1, -1)); // B

		List<Piece> pieces = [
			new(Color.White, new SpiderBug(), new Cube(0, 0, 0)), // D
			new(Color.White, new SpiderBug(), new Cube(0, 0, 0), 1), // D
			new(Color.White, new QueenBug(), new Cube(-1, 1, 0)), // A
			new(Color.White, new QueenBug(), new Cube(-1, 1, 0), 1), // A
			new(Color.White, new QueenBug(), new Cube(-1, 1, 0), 2), // A
			new(Color.White, new QueenBug(), new Cube(-1, 2, -1)), // C
			new(Color.White, new QueenBug(), new Cube(-1, 2, -1), 1), // C
			beetle, // B
		];

		Board board = new(pieces);

		Debug.WriteLine(board);

		List<Move> moves = beetle.PossibleMoves(board);

		Assert.That(moves, Has.Count.EqualTo(5));

		List<AttackMove> attackMoves = moves.Select(move => (AttackMove)move).ToList();
		Assert.That(attackMoves.Any(m =>
			m.AttackPosition.Equals(new Axial(-1, 1))
		)); // The beetle may jump on top of A due to A being higher than the "fence" by C and D
	}

	[Test]
	public void JumpOffAndSlideTest()
	{
		// test on-top movement
		Piece beetle = new(Color.White, new BeetleBug(), new Cube(0, 0, 0), 1);

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

		Debug.WriteLine("Testing on-top movement\n" + board);

		List<Move> moves = beetle.PossibleMoves(board);

		foreach (var move in moves)
		{
			Debug.WriteLine(move.MoveString(pieces));
		}

		Assert.That(moves, Has.Count.EqualTo(6));
	}
}
