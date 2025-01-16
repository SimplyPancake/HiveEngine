using System;
using Hive.AI.Enums;
using Hive.Core.Models;

namespace Hive.AI.Explorers;

public static class ExplorerCreator
{
	public static IExplorer CreateExplorer(ExplorerType type, Match match)
	{
		return type switch
		{
			ExplorerType.AlphaBeta => new AlphaBetaSearch(4, match),
			_ => throw new ArgumentException("Invalid explorer type", nameof(type)),
		};
	}
}
