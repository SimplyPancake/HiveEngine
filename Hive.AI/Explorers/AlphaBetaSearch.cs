using Hive.AI.Scorers;
using Hive.Core;
using Hive.Core.Models;

namespace Hive.AI.Explorers;
public class AlphaBetaSearch(int maxDepth, Match match) : IExplorer
{
	private readonly int MaxDepth = maxDepth;
	private readonly Match InitialMatch = match;

	public Move FindBestMove()
	{
		Match match = InitialMatch;
		double alpha = double.NegativeInfinity;
		double beta = double.PositiveInfinity;
		Move? bestMove = null;
		double bestValue = double.NegativeInfinity;

		foreach (var move in match.Board.PossibleMoves(match.CurrentPlayerTurn()))
		{
			double value = MinValue(match.Result(move), MaxDepth - 1, alpha, beta);

			if (value > bestValue)
			{
				bestValue = value;
				bestMove = move;
			}

			alpha = Math.Max(alpha, value);
		}

		if (bestMove == null)
		{
			throw new Exception("No move found");
		}

		return bestMove;
	}

	private double MaxValue(Match state, int depth, double a, double b)
	{
		if (IsTerminal(state) || depth == 0) return Utility(state);

		double v = double.NegativeInfinity;

		foreach (Move action in state.Board.PossibleMoves(state.CurrentPlayerTurn()))
		{
			v = Math.Max(v, MinValue(state.Result(action), depth - 1, a, b));

			if (v >= b) return v;

			a = Math.Max(a, v);
		}

		return v;
	}

	private double MinValue(Match state, int depth, double a, double b)
	{
		if (IsTerminal(state) || depth == 0) return Utility(state);

		double v = double.PositiveInfinity;

		foreach (Move action in state.Board.PossibleMoves(state.CurrentPlayerTurn()))
		{
			v = Math.Min(v, MaxValue(state.Result(action), depth - 1, a, b));

			if (v <= a) return v;

			b = Math.Min(b, v);
		}

		return v;
	}

	private static bool IsTerminal(Match state)
	{
		return state.Board.HasWinCondition();
	}

	private static double Utility(Match state)
	{
		return SimpleScorer.Score(state);
	}
}