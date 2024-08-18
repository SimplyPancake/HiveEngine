using System.Diagnostics;
using Hive.Console;
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

		Assert.That(b.SurroundingPieces(whiteQueen), Does.Contain(blackQueen));
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

		Debug.WriteLine(board);

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

		Debug.WriteLine(board);

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
		Piece test0 = new(Color.Black, new SpiderBug(), new Cube(2, -2, 0));
		Piece test1 = new(Color.Black, new QueenBug(), new Cube(2, -2, 0), 1);
		Piece test2 = new(Color.Black, new QueenBug(), new Cube(2, -2, 0), 2);

		List<Piece> pieces = [
			new(Color.Black, new QueenBug(), new Cube(3, -2, -1)),
			new(Color.Black, new QueenBug(), new Cube(0, -1, 1)),
			new(Color.Black, new QueenBug(), new Cube(1, -2, 1)),
			test0,
			test1,
			test2,
			new(Color.Black, new SpiderBug(), new Cube(2, -1, -1)),
			new(Color.Black, new QueenBug(), new Cube(2, 0, -2)),
			middle,
			new(Color.Black, new SpiderBug(), new Cube(0, 0, 0), 1)
		];

		Board board = new(pieces);
		Assert.Multiple(() =>
		{
			Assert.That(board.HasHigherPiece(middle), Is.True);
			Assert.That(board.HasHigherPiece(test1), Is.True);
			Assert.That(board.HasHigherPiece(test2), Is.False);
		});

		List<Piece> trueHighestPieces = [
			new(Color.Black, new QueenBug(), new Cube(3, -2, -1)),
			new(Color.Black, new QueenBug(), new Cube(0, -1, 1)),
			new(Color.Black, new QueenBug(), new Cube(1, -2, 1)),
			test2,
			new(Color.Black, new SpiderBug(), new Cube(2, -1, -1)),
			new(Color.Black, new QueenBug(), new Cube(2, 0, -2)),
			new(Color.Black, new SpiderBug(), new Cube(0, 0, 0), 1)
		];

		List<Piece> highestPieces = board.HighestPieces();

		Assert.That(highestPieces, Has.Count.EqualTo(trueHighestPieces.Count));

		foreach (var p in trueHighestPieces)
		{
			Assert.That(highestPieces.Any(piece => piece.Equals(p)));
		}

		Assert.That(highestPieces, Does.Not.Contain(middle));
		Assert.That(highestPieces, Does.Not.Contain(test1));
		Assert.That(highestPieces, Does.Not.Contain(test0));
	}

	[Test]
	public void BoardPrintTest()
	{
		Piece middle = new(Color.White, new QueenBug(), new Cube(0, 0, 0));
		Piece test0 = new(Color.Black, new SpiderBug(), new Cube(2, -2, 0));
		Piece test1 = new(Color.Black, new QueenBug(), new Cube(2, -2, 0), 1);
		Piece test2 = new(Color.Black, new QueenBug(), new Cube(2, -2, 0), 2);

		List<Piece> pieces = [
			new(Color.Black, new QueenBug(), new Cube(3, -2, -1)),
			new(Color.Black, new QueenBug(), new Cube(0, -1, 1)),
			new(Color.Black, new QueenBug(), new Cube(1, -2, 1)),
			test0,
			test1,
			test2,
			new(Color.Black, new SpiderBug(), new Cube(2, -1, -1)),
			new(Color.Black, new QueenBug(), new Cube(2, 0, -2)),
			middle,
			new(Color.Black, new SpiderBug(), new Cube(0, 0, 0), 1)
		];

		Board board = new(pieces);

		Debug.WriteLine(ConsoleHexPrinter.BoardStringWithHeight(board));
	}

	[Test]
	public void PiecePlacePositionsTest()
	{
		List<Piece> pieces = [
			new(Color.White, new QueenBug(), new Cube(0, -1, 1)),
			new(Color.Black, new QueenBug(), new Cube(1, -2, 1)),
			new(Color.White, new QueenBug(), new Cube(2, -2, 0)),
			new(Color.Black, new QueenBug(), new Cube(2, -1, -1)),
			new(Color.Black, new QueenBug(), new Cube(2, 0, -2)),
			new(Color.White, new QueenBug(), new Cube(0, 0, 0)),
		];

		Board board = new(pieces);

		Debug.WriteLine(board);

		List<Cube> canPlace = board.PlacePositions(Color.White);

		Assert.That(canPlace, Has.Count.EqualTo(5));
	}

	[Test]
	public void PossibleMovesTest()
	{
		Player p1 = new ConsolePlayer("Hans", Color.Black);
		Player p2 = new ConsolePlayer("Frans", Color.White);

		Match m = new(p1, p2);

		Board board = m.Board;

		Debug.WriteLine(board);

		List<Move> moves = board.PossibleMoves();

		List<string> moveStrings = moves.Select(m => m.MoveString(pieces)).ToList();
		// Assert.That(canPlace, Has.Count.EqualTo(5));
	}
}
