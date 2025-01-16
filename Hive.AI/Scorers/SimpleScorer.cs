using System;
using Hive.Core;
using Hive.Core.Models;
using Hive.Core.Models.Coordinate;
using Hive.Core.Models.Players;

namespace Hive.AI.Scorers;

public class SimpleScorer : IScorer
{
	private const double AttackingDefendingWeight = 5; //0 for balanced, high for attack, low for defense
	public static double Score(Match state)
	{
		Player currentPlayer = state.CurrentPlayerTurn();
		Board board = state.Board;

		double totalScore = 0;

		// Calculate scores for attacking and defending pieces
		totalScore += Sigmoid(AttackingDefendingWeight) * CalculateAttackingPieceScore(board, currentPlayer);
		totalScore += -1 * Sigmoid(AttackingDefendingWeight) * CalculateDefendingPieceScore(board, currentPlayer);

		return totalScore;
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
				double distance = Cube.Distance(piece.Position, opponentQueen.Position);
				score += distance; // Higher score for pieces farther away from the opponent's queen
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
				double distance = Cube.Distance(piece.Position, playerQueen.Position);
				score += 1 / (distance + 1); // Higher score for pieces closer to the player's queen
			}
		}

		return score;
	}

	private static double Sigmoid(double x)
	{
		return 1 / (1 + Math.Exp(-x));
	}
}
