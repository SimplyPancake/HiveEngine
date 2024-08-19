using Hive.Core;
using Hive.Core.Models;
using Hive.Core.Models.Bugs;
using Hive.Core.Models.Players;

namespace Hive.AI.Scorers;

// https://github.dev/janpfeifer/hiveGo (features.go)

public static class FeatureExtractor
{
	public static List<Feature> AllFeatures(Board board)
	{
		int amountOfPieceTypes = PieceCollectionMethods.GetPieceBugs(PieceCollection.All).Count;

		return [
			new("NumOffBoard", FeatureType.NUM_OFFBOARD, amountOfPieceTypes, false, NumOffBoard),
			new("OppNumOffBoard", FeatureType.OPP_NUM_OFFBOARD, amountOfPieceTypes, true, NumOffBoard),
			new("NumSurroundQueen", FeatureType.NUM_SURROUNDING_QUEEN, 1, false, NumSurroundQueen),
			new("OppNumSurroundQueen", FeatureType.OPP_NUM_SURROUNDING_QUEEN, 1, true, NumSurroundQueen),
			new("NumCanMove", FeatureType.NUM_CAN_MOVE, 1, false, NumSurroundQueen),
		];
	}

	private static float[] NumOffBoard(Board board, Player player)
	{
		List<Bug> pieceTypes = PieceCollectionMethods.GetPieceBugs(PieceCollection.All).Distinct().ToList();
		List<float> amounts = [];

		foreach (Bug bugType in pieceTypes)
		{
			int bugAmount = 0;

			if (player.Pieces.Any(p => p.Equals(bugType)))
			{
				bugAmount = player.Pieces.Where(b => b.Equals(bugType)).Count();
			}

			amounts.Add(bugAmount);
		}

		return [.. amounts];
	}

	private static float[] NumSurroundQueen(Board board, Player player)
	{
		int num = 0;
		if (board.Pieces.Any(piece => piece.Color == player.Color && piece.BugType.Equals(BugType.Queen)))
		{
			return [board.AmountOfSurroundingPieces(
				board.Pieces.First(piece =>
					piece.Color == player.Color &&
					piece.BugType.Equals(BugType.Queen))
			)];
		}

		return [num];
	}

	// How many pieces can move. Two numbers per insect: the first is considering any pieces,
	// the second discards the pieces that are surrounding the opponent's queen (and presumably
	// not to be moved)
	private static float[] NumCanMove(Board board, Player player)
	{
		return [];
	}
}

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
