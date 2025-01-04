using System;
using Hive.Core;
using Hive.Core.Models;
using Hive.Core.Models.Players;

namespace Hive.AI.Explorers;
public class AlphaBetaSearch : IExplorer
{
	private int MaxDepth;

	public AlphaBetaSearch(int maxDepth, Match match)
	{
		this.maxDepth = maxDepth;
	}

	public Move FindBestMove()
	{
		double alpha = double.NegativeInfinity;
		double beta = double.PositiveInfinity;
		Move bestMove = null;
		double bestValue = double.NegativeInfinity;

		foreach (var move in match.Board.PossibleMoves(player))
		{
			double value = EvaluateMove(match, move, player, maxDepth - 1, alpha, beta, false);

			if (value > bestValue)
			{
				bestValue = value;
				bestMove = move;
			}

			alpha = Math.Max(alpha, value);
		}

		return bestMove;
	}

	private double EvaluateMove(Match match, Move move, Player player, int depth, double alpha, double beta, bool isMaximizingPlayer)
	{
		Match simulatedMatch = match.Clone();
		simulatedMatch.Board.MakeMove(move, player);
		Player nextPlayer = player.GetOpponent();

		if (depth == 0 || simulatedMatch.Board.HasWinCondition())
		{
			return Evaluate(simulatedMatch, player);
		}

		if (isMaximizingPlayer)
		{
			return MaxValue(simulatedMatch, depth, alpha, beta, nextPlayer);
		}
		else
		{
			return MinValue(simulatedMatch, depth, alpha, beta, nextPlayer);
		}
	}

	private double MaxValue(Match match, int depth, double alpha, double beta, Player player)
	{
		double value = double.NegativeInfinity;

		foreach (var move in match.Board.PossibleMoves(player))
		{
			value = Math.Max(value, EvaluateMove(match, move, player, depth - 1, alpha, beta, false));

			if (value >= beta)
			{
				return value;
			}

			alpha = Math.Max(alpha, value);
		}

		return value;
	}

	private double MinValue(Match match, int depth, double alpha, double beta, Player player)
	{
		double value = double.PositiveInfinity;

		foreach (var move in match.Board.PossibleMoves(player))
		{
			value = Math.Min(value, EvaluateMove(match, move, player, depth - 1, alpha, beta, true));

			if (value <= alpha)
			{
				return value;
			}

			beta = Math.Min(beta, value);
		}

		return value;
	}

	private double Evaluate(Match match, Player player)
	{
		// Implement your evaluation function here
		return 0.0;
	}
}