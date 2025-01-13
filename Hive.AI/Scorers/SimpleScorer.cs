using System;
using Hive.Core.Models;

namespace Hive.AI.Scorers;

public class SimpleScorer : IScorer
{
	public static double Score(Match state)
	{
		// We only score the board based on the current's player board piece only
		// We don't care about the opponent's board piece

		List<Feature> features = FeatureExtractor.AllFeatures(state.Board);

		List<Feature> playerFeatures = features
			.Where(f => !f.CalculateOpponent)
			.Where(f => f.FeatureType != FeatureType.NUM_FEATURES) // Exclude the last feature
			.ToList();

		// Then we calculate the score of each feature
		float totalScore = 0;

		foreach (Feature f in playerFeatures)
		{
			float[] featureScore = f.Score(state.Board, state.CurrentPlayerTurn());
			totalScore += featureScore.Sum();
		}

		return totalScore;
	}
}
