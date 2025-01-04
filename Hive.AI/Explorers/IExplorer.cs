using System;
using Hive.Core;
using Hive.Core.Models;
using Hive.Core.Models.Players;

namespace Hive.AI.Explorers;

public interface IExplorer
{
	Move FindBestMove();
}
