using System.Diagnostics;
using Hive.Core;
using Hive.Core.Models;
using Hive.Core.Models.Bugs;
using Hive.Core.Models.Coordinate;
using Hive.Core.Services;

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
		Piece blackQueen = new(Color.Black, new QueenBug(), new Cube(1, -1, 0));

		List<Piece> pieces = [whiteQueen, blackQueen];

		Board b = new(pieces);

		Assert.That(b.SurroundingPieces(whiteQueen.Position), Does.Contain(blackQueen));
	}

	[Test]
	public void AllPiecesConnectedTest()
	{
		List<Piece> pieces = [
			new(Color.White, new QueenBug(), new Cube(-1, 1, 0)),
			new(Color.Black, new QueenBug(), new Cube(0, -1, 1)),
			new(Color.Black, new QueenBug(), new Cube(1, -2, 1)),
			new(Color.Black, new QueenBug(), new Cube(2, -2, 0)),
			new(Color.Black, new QueenBug(), new Cube(2, -1, -1)),
			new(Color.Black, new QueenBug(), new Cube(2, 0, -2)),
			new(Color.Black, new QueenBug(), new Cube(0, 0, 0)),
		];

		Board board = new(pieces);

		Debug.WriteLine(ConsoleHexPrinter.BoardString(board));

		Assert.That(board.AllPiecesConnected());

		// Now disconnect a piece
		pieces = [
			new(Color.White, new QueenBug(), new Cube(-1, 1, 0)),
			new(Color.Black, new QueenBug(), new Cube(0, -1, 1)),
			new(Color.Black, new QueenBug(), new Cube(1, -2, 1)),
			new(Color.Black, new QueenBug(), new Cube(2, -2, 0)),
			// new(Color.Black, new QueenBug(), new Cube(2, -1, -1)),
			new(Color.Black, new QueenBug(), new Cube(2, 0, -2)),
			new(Color.Black, new QueenBug(), new Cube(0, 0, 0)),
		];

		board = new(pieces);

		Debug.WriteLine(board);

		Assert.That(!board.AllPiecesConnected());

		// Now we create a cycle, can the board handle cycles?
		pieces = [
			new(Color.White, new QueenBug(), new Cube(1, 0, -1)),
			new(Color.Black, new QueenBug(), new Cube(0, -1, 1)),
			new(Color.Black, new QueenBug(), new Cube(1, -2, 1)),
			new(Color.Black, new QueenBug(), new Cube(2, -2, 0)),
			new(Color.Black, new QueenBug(), new Cube(2, -1, -1)),
			new(Color.Black, new QueenBug(), new Cube(2, 0, -2)),
			new(Color.Black, new QueenBug(), new Cube(0, 0, 0)),
		];

		board = new(pieces);
		Debug.WriteLine(board);
		Assert.That(board.AllPiecesConnected());
	}

	[Test]
	public void SimulateMoveTest()
	{
		// Tests if the baord is correctly simulating various sorts of moves
		Piece attackingPiece = new(Color.White, new QueenBug(), new Cube(-1, 1, 0));

		List<Piece> pieces = [
			attackingPiece,
			new(Color.Black, new QueenBug(), new Cube(0, -1, 1)),
			new(Color.Black, new QueenBug(), new Cube(1, -2, 1)),
			new(Color.Black, new QueenBug(), new Cube(2, -2, 0)),
			new(Color.Black, new QueenBug(), new Cube(2, -1, -1)),
			new(Color.Black, new QueenBug(), new Cube(2, 0, -2)),
			new(Color.Black, new QueenBug(), new Cube(0, 0, 0)),
		];

		Board board = new(pieces);

		Debug.WriteLine(ConsoleHexPrinter.BoardString(board));

		Cube attackPosition = new(0, 1, -1);
		AttackMove toMake = new(attackingPiece, attackPosition, 0, MoveType.Move);
		Board simulatedBoard = board.SimulateMove(toMake);
		Assert.Multiple(() =>
		{
			Assert.That(simulatedBoard.Pieces.Any(p => p.Position.Equals(attackPosition)));
			Assert.That(simulatedBoard.Pieces.Select(p => p.Position).Contains(attackPosition));
			Assert.That(!simulatedBoard.Pieces.Any(p => p.Position.Equals(new Cube(-1, 1, 0))));
		});
	}

	[Test]
	public void HigherPiecesTest()
	{
		Piece middle = new(Color.White, new QueenBug(), new Cube(0, 0, 0));
		List<Piece> pieces = [
			new(Color.Black, new QueenBug(), new Cube(3, -2, -1)),
			new(Color.Black, new QueenBug(), new Cube(0, -1, 1)),
			new(Color.Black, new QueenBug(), new Cube(1, -2, 1)),
			new(Color.Black, new QueenBug(), new Cube(2, -2, 0)),
			new(Color.Black, new QueenBug(), new Cube(2, -1, -1)),
			new(Color.Black, new QueenBug(), new Cube(2, 0, -2)),
			middle,
			new(Color.White, new QueenBug(), new Cube(0, 0, 0), 1)
		];

		Board board = new(pieces);

		Assert.That(board.HasHigherPiece(middle), Is.True);
	}
}
