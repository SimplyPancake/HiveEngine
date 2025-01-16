using Hive.Core;
using Hive.Core.Models;
using Hive.Core.Models.Players;

namespace Hive.AI.Scorers;

public class SimpleScorer : IScorer
{
	private const double InitialAttackingDefendingWeight = 0.75; // Initial weight for attacking
	private const double FinalAttackingDefendingWeight = 0.25; // Final weight for attacking

	public static double Score(Match state) => Score(state.Board, state.CurrentPlayerTurn(), state.Turn);
	public static double Score(Board board, Player currentPlayer, int turns)
	{
		double totalScore = 0;

		double attackingDefendingWeight = CalculateAttackingDefendingWeight(turns);

		// Calculate scores for attacking and defending pieces
		totalScore += attackingDefendingWeight * CalculateAttackingPieceScore(board, currentPlayer);
		totalScore += (1 - attackingDefendingWeight) * CalculateDefendingPieceScore(board, currentPlayer);

		return totalScore;
	}

	private static double CalculateAttackingDefendingWeight(int turns)
	{
		int maxTurns = 30; // Assuming 30 turns as the average game length
						   // Adjust the weight based on the number of turns
		double weightRange = InitialAttackingDefendingWeight - FinalAttackingDefendingWeight;

		// The weight adjustment is linearly decreasing from the initial weight to the final weight
		if (turns >= maxTurns)
		{
			return FinalAttackingDefendingWeight;
		}

		double weightAdjustment = weightRange * (1 - (double)turns / maxTurns); // Assuming 30 turns as the average game length
		return FinalAttackingDefendingWeight + weightAdjustment;
	}

	private static double CalculateAttackingPieceScore(Board board, Player player)
	{
		double score = 0;
		Piece? opponentQueen = board.Pieces.FirstOrDefault(p => p.Bug.BugTypeId == (int)BugType.Queen && p.Color != player.Color);

		if (opponentQueen == null)
		{
			return score;
		}

		foreach (Piece piece in board.PlayerPieces(player))
		{
			if (PieceCollectionMethods.IsAttacking(piece.BugType))
			{
				score += 1;
			}
		}

		return score;
	}

	private static double CalculateDefendingPieceScore(Board board, Player player)
	{
		double score = 0;
		Piece? playerQueen = board.Pieces.FirstOrDefault(p => p.Bug.BugTypeId == (int)BugType.Queen && p.Color == player.Color);

		if (playerQueen == null)
		{
			return score;
		}

		foreach (Piece piece in board.PlayerPieces(player))
		{
			if (PieceCollectionMethods.IsDefending(piece.BugType))
			{
				score += 1;
			}
		}

		return score;
	}
}
