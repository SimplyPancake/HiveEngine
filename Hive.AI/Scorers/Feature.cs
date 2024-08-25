using Hive.Core.Models;
using Hive.Core.Models.Players;

namespace Hive.AI.Scorers;

// Should use delegate?
// public delegate int ScoreCaulculationDelegate(Board board, Player player);

public class Feature(string name, FeatureType featureType, int dims, bool calcOpponent, Func<Board, Player, float[]> scoreSetter)
{
	public string Name { get; } = name;

	public bool CalculateOpponent { get; } = calcOpponent;

	/// <summary>
	/// Represents how many dimensions a feature has in the float[] that it returns
	/// </summary>
	public int FeatureDimensions { get; } = dims;

	public FeatureType FeatureType { get; } = featureType;

	public Func<Board, Player, float[]> Score { get; } = scoreSetter;
}