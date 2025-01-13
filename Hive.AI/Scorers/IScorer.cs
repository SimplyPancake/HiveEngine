using Hive.Core.Models;

namespace Hive.AI.Scorers;

public interface IScorer
{
	public abstract static double Score(Match state);
}
