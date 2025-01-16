using System;
using Hive.Core.Models;

namespace Hive.AI.Scorers;

public class FeatureScorer : IScorer
{
	public static double Score(Match state)
	{
		// We only score the board based on the current's player board piece only
		// We don't care about the opponent's board piece

		List<Feature> features = FeatureExtractor.AllFeatures(state);

		List<Feature> playerFeatures = features
			.Where(f => !f.CalculateOpponent)
			.Where(f => f.FeatureType != FeatureType.NUM_FEATURES) // Exclude the last feature
			.ToList();

		// Then we calculate the score of each feature
		float totalScore = 0;

		foreach (Feature f in playerFeatures)
		{
			float[] featureScore = f.Score(state, state.CurrentPlayerTurn());
			totalScore += featureScore.Sum();
		}


		// Opponent features
		List<Feature> opponentFeatures = features
			.Where(f => f.CalculateOpponent)
			.Where(f => f.FeatureType != FeatureType.NUM_FEATURES) // Exclude the last feature
			.ToList();

		float totalOpponentScore = 0;

		foreach (Feature f in opponentFeatures)
		{
			float[] featureScore = f.Score(state, state.OtherPlayerTurn());
			totalOpponentScore += featureScore.Sum();
		}

		// We subtract the opponent's score from the player's score
		totalScore -= totalOpponentScore;

		// Add the last feature
		totalScore += features.First(f => f.FeatureType == FeatureType.NUM_FEATURES)
			.Score(state, state.CurrentPlayerTurn())
			.Sum();

		return totalScore;
	}
}
