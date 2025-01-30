using System;
using Hive.AI.Explorers;
using Hive.Core;
using Hive.Core.Models;
using Hive.Core.Models.Bugs;
using Hive.Core.Models.Coordinate;
using Hive.Core.Models.Players;

namespace Hive.Test.AI;

public class AlphaBetaTest : ITestBase
{
	private Match match;
	private Player player1;
	private Player player2;

	[SetUp]
	public void SetUp()
	{
		player1 = new ConsolePlayer("Hans", Color.White, [
				new AntBug(),
				new AntBug(),
				new AntBug(),
				new BeetleBug(),
				new BeetleBug(),
				new GrasshopperBug(),
				new GrasshopperBug(),
				new GrasshopperBug(),
				new SpiderBug(),
				new SpiderBug()]);

		player2 = new ConsolePlayer("Frans", Color.Black, [
				new AntBug(),
				new AntBug(),
				new AntBug(),
				new BeetleBug(),
				new BeetleBug(),
				new GrasshopperBug(),
				new GrasshopperBug(),
				new GrasshopperBug(),
				new SpiderBug(),
				new SpiderBug()
		]);

		List<Piece> pieces =
		[
			new Piece(Color.White, new QueenBug(), new Cube(0, 0, 0)),
			new Piece(Color.Black, new QueenBug(), new Cube(1, -1, 0)),
			new Piece(Color.White, new SpiderBug(), new Cube(2, -2, 0)),
			new Piece(Color.Black, new SpiderBug(), new Cube(3, -3, 0))
		];

		Board board = new(pieces);
		match = new(player1, player2, board);
	}

	[Test]
	public void TestAlphaBetaSearch()
	{
		AlphaBetaSearch search = new(3, match);
		Move bestMove = search.FindBestMove();

		Assert.That(bestMove, Is.Not.Null);
		Console.WriteLine($"Best move found:\n {bestMove}");
	}

	[Test]
	public void TestNogWat()
	{
		int[] code = [-1, -1, -1, -1, -1, 8];

		// Generate all possibilities of the code, ranging from 0 to 9.
		// No int in the code is the same as the other
		var possibilities = new List<int[]>();
	}
}
