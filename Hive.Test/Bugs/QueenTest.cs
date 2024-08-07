using System.Diagnostics;
using Hive.Console.Visualiser;
using Hive.Core;
using Hive.Core.Enums;
using Hive.Core.Models;
using Hive.Core.Models.Coordinate;

namespace Hive.Test;

public class QueenTest
{
	public Board board;
	public Piece whiteQueen;

	[SetUp]
	public void Setup()
	{
	}

	[Test]
	public void PossibleMovesTest()
	{
		// TODO update for use of cycle
		whiteQueen = new(Color.White, new QueenBug(), new Cube(0, 0, 0));

		List<Piece> pieces = [
			whiteQueen,
			new(Color.Black, new QueenBug(), new Cube(0, -1, 1)),
			new(Color.Black, new QueenBug(), new Cube(1, -2, 1)),
			new(Color.Black, new QueenBug(), new Cube(2, -2, 0)),
			new(Color.Black, new QueenBug(), new Cube(2, -1, -1)),
			new(Color.Black, new QueenBug(), new Cube(2, 0, -2)),
			new(Color.Black, new QueenBug(), new Cube(-1, 1, 0)),
		];

		board = new(pieces);

		Debug.WriteLine(ConsoleHexPrinter.HexOutput(board.Pieces));

		List<Move> moves = new QueenBug().PossibleMoves(whiteQueen, board);

		// TODO; incorporate placing rules
		// Strictly speaking move-wise, a queen could only move 2 places.
		Assert.That(moves, Has.Count.EqualTo(3));

		// Assert move positions
		List<Cube> movePositions = moves.Select(m => ((AttackMove)m).AttackPosition).ToList();
		Assert.Multiple(() =>
		{
			Assert.That(CubeListExtensions.ContainsCube(movePositions, new Cube(CubeVector.TopRight)));
			Assert.That(CubeListExtensions.ContainsCube(movePositions, new Cube(CubeVector.Right)));
			Assert.That(CubeListExtensions.ContainsCube(movePositions, new Cube(CubeVector.BottomRight)));
		});
	}
}
