using System.Diagnostics;
using Hive.AI.Enums;
using Hive.AI.Explorers;
using Hive.Core;
using Hive.Core.Models;
using Hive.Core.Models.Bugs;
using Hive.Core.Models.Players;

namespace Hive.AI.Players;

public class AIPlayer : Player
{
	private IExplorer explorer;

	private ExplorerType explorerType;

	public AIPlayer(ExplorerType type, string playername, Color color) : base(playername, color)
	{
		// Explorer set later
		explorerType = type;
	}

	public AIPlayer(ExplorerType type, string playername, Color color, List<Bug> pieces) : base(playername, color, pieces)
	{
		// Explorer set later
		explorerType = type;
	}

	public override Move MakeMove()
	{
		Console.WriteLine($"AIPlayer {Playername} is making a move...");
		Debug.WriteLine($"AIPlayer {Playername} is making a move...");
		Console.WriteLine(Board);
		Debug.WriteLine(Board);
		return explorer.FindBestMove();
	}

	protected override void OnMatchSet(Match match)
	{
		explorer = ExplorerCreator.CreateExplorer(explorerType, match);
	}

	public override Move MakeMove(IllegalMoveException illegalMoveException) => MakeMove();
}
