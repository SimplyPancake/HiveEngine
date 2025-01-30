using System;
using Hive.Core;
using Hive.Core.Models;

namespace Hive.AI.Scorers;

public class FeatureScorer : IScorer
{
	public static double Score(Match state)
	{
		// Make sure that if the board consists of only two queens, do not move the queen immediately
		if (state.Board.Pieces.Count == 2 && state.LastMove.MoveType is MoveType.Move)
		{
			return 0;
		}

		// We only score the board based on the current's player board piece only

		List<Feature> features = FeatureExtractor.AllFeatures(state);

		Dictionary<FeatureType, double> multipliers = new()
		{
			{ FeatureType.AVERAGE_DISTANCE_TO_QUEEN, -0.5 }, // Closer pieces to the queen are better
            { FeatureType.OPP_NUM_OFFBOARD, 1.0 }, // More opponent pieces off the board is better
            { FeatureType.NUM_SURROUNDING_QUEEN, 2.0 }, // More pieces surrounding the opponent's queen is better
            { FeatureType.OPP_NUM_SURROUNDING_QUEEN, 2.0 }, // Fewer pieces surrounding our queen is better
            { FeatureType.NUM_CAN_MOVE, 1.5 }, // More pieces that can move is better
            { FeatureType.OPP_NUM_CAN_MOVE, -1.5 }, // Fewer opponent pieces that can move is better
            { FeatureType.NUM_THREATENING_MOVES, 2.0 }, // More threatening moves is better
            { FeatureType.OPP_NUM_THREATENING_MOVES, -2.0 }, // Fewer opponent threatening moves is better
            { FeatureType.MOVES_TO_DRAW, -1.0 }, // Fewer moves to draw is better
            { FeatureType.NUM_SINGLE, 1.0 }, // More single pieces is better
            { FeatureType.OPP_NUM_SINGLE, 1.0 }, // Fewer opponent single pieces is better
            { FeatureType.QUEEN_COVERED, -3.0 }, // Our queen being covered is very bad
            { FeatureType.OPP_QUEEN_COVERED, 5.0 }, // Opponent's queen being covered is very good
            { FeatureType.OPP_AVERAGE_DISTANCE_TO_QUEEN, 0.5 }, // Opponent's pieces being closer to our queen is worse
            { FeatureType.NUM_TURNS, -0.1 }, // Scores get lower the further we go on
			{ FeatureType.NUM_FEATURES, 0.0 }, // This is a placeholder and should not affect the score
			{ FeatureType.LAST_MOVE_EFFECTIVE, 5.0 } // Effective moves are good!
        };

		List<double> scores = [.. multipliers.Select(kvp => CalculateScore(features, state, kvp.Key) * kvp.Value)];

		return scores.Sum();
	}

	private static double CalculateScore(List<Feature> features, Match state, FeatureType type)
	{
		Feature feature = features.First(f => f.FeatureType == type);
		if (feature.CalculateOpponent)
		{
			return feature.Score(state, state.OtherPlayerTurn()).Sum();
		}

		return feature.Score(state, state.CurrentPlayerTurn()).Sum();
	}
}
