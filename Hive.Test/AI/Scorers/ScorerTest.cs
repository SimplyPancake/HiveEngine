using System.Diagnostics;
using Hive.AI.Scorers;
using Hive.Core;
using Hive.Core.Builders;
using Hive.Core.Models;
using Hive.Core.Models.Bugs;
using Hive.Core.Models.Coordinate;
using Hive.Core.Models.Players;

namespace Hive.Test.AI.Scorers;

public class ScorerTest : ITestBase
{
	private Match match;
	// private FeatureScorer featureScorer;

	[SetUp]
	public void SetUp()
	{
		Player player1 = new ConsolePlayer("Hans", Color.White, [
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

		Player player2 = new ConsolePlayer("Frans", Color.Black, [
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
	public void TestSimpleScorer()
	{
		double initialScore = SimpleScorer.Score(match);

		// Add beetle
		Move placeBeetle = new PlaceMove(new Piece(Color.White, new BeetleBug(), new Cube(0, 1, -1)));
		Move placeGrasshopper = new PlaceMove(new Piece(Color.White, new GrasshopperBug(), new Cube(0, 1, -1)));

		Match matchAfterBeetle = match.Result(placeBeetle, false);
		Match matchAfterGrasshopper = match.Result(placeGrasshopper, false);

		double scoreAfterBeetle = SimpleScorer.Score(matchAfterBeetle);
		double scoreAfterGrasshopper = SimpleScorer.Score(matchAfterGrasshopper);

		Debug.WriteLine($"Initial score: {initialScore}");
		Debug.WriteLine($"Score after beetle: {scoreAfterBeetle}");
		Debug.WriteLine($"Score after grasshopper: {scoreAfterGrasshopper}");
	}
}
