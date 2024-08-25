using Hive.Core;
using Hive.Core.Models;
using Hive.Core.Models.Bugs;
using Hive.Core.Models.Coordinate;
using Hive.Core.Models.Players;

namespace Hive.AI.Scorers;

// https://github.dev/janpfeifer/hiveGo (features.go)

public static class FeatureExtractor
{
	public static List<Feature> AllFeatures(Board board)
	{
		// TODO; get board piece set
		int amountOfPieceTypes = PieceCollectionMethods.GetPieceBugs(PieceCollection.All).Count;

		return [
			new("NumOffBoard", FeatureType.NUM_OFFBOARD, amountOfPieceTypes, false, NumOffBoard),
			new("OppNumOffBoard", FeatureType.OPP_NUM_OFFBOARD, amountOfPieceTypes, true, NumOffBoard),

			new("NumSurroundQueen", FeatureType.NUM_SURROUNDING_QUEEN, 1, false, NumSurroundQueen),
			new("OppNumSurroundQueen", FeatureType.OPP_NUM_SURROUNDING_QUEEN, 1, true, NumSurroundQueen),

			new("NumCanMove", FeatureType.NUM_CAN_MOVE, 2 * amountOfPieceTypes, false, NumCanMove),
			new("OppNumCanMove", FeatureType.OPP_NUM_CAN_MOVE, 2 * amountOfPieceTypes, true, NumCanMove),

			new("NumThreateningMoves", FeatureType.NUM_THREATENING_MOVES, 2, false, NumThreateningMove),
			new("OppNumThreateningMoves", FeatureType.OPP_NUM_THREATENING_MOVES, 2, true, NumThreateningMove),

			// TODO:
			// new("NumMovesToDraw", FeatureType.MOVES_TO_DRAW, 2, false, NumMovesToDraw),

			new("NumSinglePieces", FeatureType.NUM_SINGLE, 1, false, NumSinglePieces),
			new("OppNumSinglePieces", FeatureType.OPP_NUM_SINGLE, 1, true, NumSinglePieces),

			new("QueenCovered", FeatureType.QUEEN_COVERED, 2, false, QueenCovered),
			new("OppQueenCovered", FeatureType.OPP_QUEEN_COVERED, 2, true, QueenCovered),

			new("AverageQueenDistance", FeatureType.AVERAGE_DISTANCE_TO_QUEEN, amountOfPieceTypes, false, AverageQueenDistance),
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
		List<Bug> pieceTypes = PieceCollectionMethods.GetPieceBugs(PieceCollection.All).Distinct().ToList();
		List<float> amounts = [];

		foreach (Bug bugType in pieceTypes)
		{
			int bugAmount = 0;

			if (player.Pieces.Any(p => p.BugTypeId.Equals(bugType)))
			{
				bugAmount = player.Pieces.Where(b => b.Equals(bugType)).Count();
			}

			amounts.Add(bugAmount);
		}

		List<float> queenAmounts = [];

		Color otherColor = player.Color.GetOtherColor();
		if (!board.Pieces.Any(p => p.Bug.BugTypeId.Equals(BugType.Queen) && p.Color.Equals(otherColor)))
		{
			queenAmounts = Enumerable.Repeat(0f, pieceTypes.Count).ToList();
		}

		Piece opponentQueen = board.Pieces.First(p => p.Bug.Equals(BugType.Queen) && p.Color.Equals(otherColor));

		List<Piece> piecesAroundOpponentQueen = board.Pieces.Where(p =>
			p.Color.Equals(player.Color) &&
			Cube.Distance(p.Position, opponentQueen.Position) == 1)
			.ToList();

		foreach (Bug bugType in pieceTypes)
		{
			float queenAmount = 0;

			if (piecesAroundOpponentQueen.Any(p => p.BugType.Equals(bugType)))
			{
				queenAmount = player.Pieces.Where(b => b.Equals(bugType)).Count();
			}

			queenAmounts.Add(queenAmount);
		}

		amounts.AddRange(queenAmounts);

		return [.. amounts];
	}

	private static float[] NumThreateningMove(Board board, Player player)
	{
		int amountThreatening = 0;
		int freeQueenPositions = 0;

		Color otherColor = player.Color.GetOtherColor();
		if (board.Pieces.Any(p => p.Bug.BugTypeId.Equals(BugType.Queen) && p.Color.Equals(otherColor)))
		{
			Piece opponentQueen = board.Pieces.First(p => p.Bug.Equals(BugType.Queen) && p.Color.Equals(otherColor));
			freeQueenPositions = 6 - board.HighestPieces()
				.Where(p => Cube.Distance(p.Position, opponentQueen.Position) == 1).Count();

			List<Move> movesToMake = board.PossibleMoves(player);
			foreach (Move move in movesToMake)
			{
				// place
				if (move.GetType() == typeof(PlaceMove))
				{
					PlaceMove m = (PlaceMove)move;
					if (Cube.Distance(m.Piece.Position, opponentQueen.Position) == 1)
					{
						// move is threatening Queen
						amountThreatening += 1;
					}
				}
				else
				{
					// attackmove
					AttackMove m = (AttackMove)move;
					if (Cube.Distance(m.AttackPosition, opponentQueen.Position) == 1)
					{
						amountThreatening += 1;
					}
				}
			}
		}

		return [amountThreatening, freeQueenPositions];
	}

	private static float[] NumSinglePieces(Board board, Player player)
	{
		return [board.Pieces.Where(p =>
			p.Color.Equals(player.Color) &&
			board.AmountOfSurroundingPieces(p) == 1)
		.Count()];
	}

	private static float[] QueenCovered(Board board, Player player)
	{

		// we have a queen?
		if (!board.Pieces.Any(p => p.Bug.BugTypeId.Equals(BugType.Queen) && p.Color.Equals(player.Color)))
		{
			return [0, 0];
		}


		// we have a queen!
		Piece ourQueen = board.Pieces.First(p => p.Bug.BugTypeId.Equals(BugType.Queen) && p.Color.Equals(player.Color));
		if (board.HasHigherPiece(ourQueen))
		{
			Color otherColor = player.Color.GetOtherColor();
			bool opponentCovering = board.HighestPiece(ourQueen.Position).Color.Equals(otherColor);
			return [1, opponentCovering ? 1 : 0];
		}

		return [0, 0];
	}

	private static float[] AverageQueenDistance(Board board, Player player)
	{
		List<int> allPieceTypes = board.Pieces.Where(p => p.Color.Equals(player.Color)).Select(p => p.BugType).ToList();
		allPieceTypes.AddRange(player.Pieces.Select(p => p.BugTypeId));

		List<int> distinctPieceTypes = allPieceTypes.Distinct().ToList();

		// opponent has a queen?
		if (!board.Pieces.Any(p => p.Bug.BugTypeId.Equals(BugType.Queen) && p.Color.Equals(player.Color)))
		{
			// return arbitrary high number
			return Enumerable.Repeat(15f, allPieceTypes.Count).ToArray();
		}

		Piece opponentQueen = board.Pieces.First(p => p.Bug.BugTypeId.Equals(BugType.Queen) && p.Color.Equals(player.Color));

		float maxDistance = 0;
		Dictionary<BugType, float> distancePairs = [];

		foreach (int bugTypeId in distinctPieceTypes)
		{
			int totalBugsOfThisKind = allPieceTypes.Where(p => p == bugTypeId).Count();

			// get bugs in board
			if (board.Pieces.Any(p => p.BugType == bugTypeId && p.Color.Equals(player.Color)))
			{
				List<Piece> bugsInBoard = board.Pieces
					.Where(
						p => p.BugType == bugTypeId &&
						p.Color.Equals(player.Color)
					).ToList();

				foreach (Piece pieceInBoard in bugsInBoard)
				{
					float distance = Cube.Distance(pieceInBoard.Position, opponentQueen.Position);

					maxDistance = distance > maxDistance ? distance : maxDistance;
				}
			}

		}

		// get bugs in player hand

		return [];
	}
}