using System;
using Hive.AI.Enums;
using Hive.AI.Players;
using Hive.Core;
using Hive.Core.Builders;
using Hive.Core.Models;

namespace Hive.Test.AI;

public class AIPlayerTest : ITestBase
{
	[SetUp]
	public void SetUp()
	{
		// Nothing
	}

	[Test]
	public void TestBetaVersusBeta()
	{
		Match match = new MatchBuilder()
			.SetPlayer1(new AIPlayer(ExplorerType.AlphaBeta, "Beta1", Color.White))
			.SetPlayer2(new AIPlayer(ExplorerType.AlphaBeta, "Beta2", Color.Black))
			.Build();

		match.Start();
	}
}
