using Hive.Core.Enums;
using Hive.Core.Models;
using Hive.Core.Models.Coordinate;

namespace Hive.Core.Attributes;

/// <summary>
/// Describes if a Bug can walk, and how many times.
/// Walk amount 0 for infinite walking.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class CanWalk : BugAttribute
{
	public int WalkAmount { get; set; }

	/// <summary>
	/// Flag can be set to true when the attribute should only return
	/// moves with a precise walk amount of WalkAmount
	/// </summary>
	public bool ReturnOnly { get; set; } = false;

	public CanWalk(int WalkAmount)
	{
		this.WalkAmount = WalkAmount;
	}

	/// <summary>
	/// TODO
	/// CanWalk only walks for height = 0
	/// </summary>
	/// <param name="board"></param>
	/// <param name="piece"></param>
	/// <returns></returns>
	public override List<AttackMove> Moves(Board board, Piece piece)
	{
		board = board.LowestPiecesBoard(); // Only handle pieces with height = 0.
		List<Cube> walkPositions = [];
		List<Cube> boardCoordinates = board.Pieces.Where(p => !p.Equals(piece)).Select(p => p.Position).ToList();

		// Track visited positions with their step count
		List<(Cube position, int steps)> visited = [(piece.Position, 0)];
		// Track positions to visit with their step count
		List<(Cube position, int steps)> toVisit = WalkSingle(piece.Position, boardCoordinates)
			.Select(pos => (pos, 1)).ToList();

		int walked = 0;

		bool isDone = false;

		while (!isDone)
		{
			List<(Cube position, int steps)> walkNextTime = [];

			// Explore all current positions in 'toVisit'
			foreach (var (toExplore, steps) in toVisit)
			{
				// Add the current position to the visited list if not already visited
				if (!visited.Select(p => p.position).Contains(toExplore))
				{
					visited.Add((toExplore, steps));
				}

				// Walk one step from the current position and get new positions to explore
				List<Cube> newPositions = WalkSingle(toExplore, boardCoordinates);

				// Add only non-visited positions to the list of positions to explore next
				foreach (var pos in newPositions)
				{
					// piece was removed, we will not go there
					if (pos.Equals(piece.Position))
					{
						continue;
					}
					// Should this not be OR?
					if (!visited.Select(p => p.position).Contains(pos) && !toVisit.Select(p => p.position).Contains(pos))
					{
						walkNextTime.Add((pos, steps + 1));
					}
				}
			}

			// Prepare for the next iteration: update 'toVisit' and increment 'walked'
			toVisit = walkNextTime;
			walked++;

			// Stop the loop when the walk amount is reached or there are no more positions to explore
			isDone = WalkAmount > 0 ? walked == WalkAmount : toVisit.Count == 0 && visited.Count != 0;
		}

		// Filter the visited positions based on the 'returnOnly' flag
		if (ReturnOnly)
		{
			// Only include positions that took exactly WalkAmount steps to reach
			walkPositions = visited.Where(v => v.steps == WalkAmount).Select(v => v.position).ToList();
		}
		else
		{
			// Include all visited positions except the starting position
			walkPositions = visited.Select(v => v.position).Where(pos => !pos.Equals(piece.Position)).ToList();
		}

		// From walkPositions to move
		if (walkPositions.Count == 0)
		{
			return [];
		}

		return walkPositions.Select(walkPos =>
			new AttackMove(piece, walkPos, 0, MoveType.Move))
			.ToList();
	}

	private List<Cube> WalkSingle(Cube piecePosition, List<Cube> piecePositions)
	{
		List<Cube> surroundingPositions = Board.SurroundingPositions(piecePosition);

		List<Cube> positionsWithoutPiece = new(piecePositions);
		positionsWithoutPiece.Remove(piecePosition);

		List<Cube> neighborPiecePositions = piecePositions.Where(pos => Cube.Distance(pos, piecePosition) == 1).ToList();

		// Can walk is a piece in surrounding positions,
		// which one of those pieces is not already a piece,
		// which borders a neihbor of piece
		List<Cube> canMoveTo = surroundingPositions
		.Where(pos =>
			!neighborPiecePositions.Contains(pos) // surrounding which is not a neighbor
		)
		.Where(pos =>
			neighborPiecePositions.Any(neighbor => Cube.Distance(neighbor, pos) == 1)) // which also borders a neighbor
		.ToList();

		// Then we have to check the passing between pieces rule
		List<Cube> canWalkThrough = CanWalkThrough(piecePosition, canMoveTo, neighborPiecePositions);

		return canWalkThrough;
	}

	/// <summary>
	/// Returns all positions that we can go through (given the Hive barrier rule)
	/// It filters out the positions to walk that would pass through a 'barrier' (aka 2 pieces)
	/// </summary>
	/// <param name="position">The position of the piece (not vector)</param>
	/// <param name="positionsToWalk">The positions around the vector that are considered
	/// to be walkable to</param>
	/// <param name="surroundingPiecePositions">All the pieces surrounding the original piece,
	/// such that we can filter out</param>
	/// <returns></returns>
	private List<Cube> CanWalkThrough(Cube position, List<Cube> positionsToWalk, List<Cube> surroundingPiecePositions)
	{
		List<Cube> surroundingPieceVectors = surroundingPiecePositions.Select(c => c - position).ToList();
		List<Cube> approvedWalkPositions = [];

		foreach (Cube positionToWalk in positionsToWalk)
		{
			// get a vector of which side we're walking to
			Cube toWalkToVector = positionToWalk - position;
			if (!toWalkToVector.IsVector())
			{
				throw new Exception("Positions to walk have a distance of more than 1.");
			}

			// Then simply check if we can pass there
			bool isApprovedWalkingPosition = true;

			switch (toWalkToVector)
			{
				// top left
				case var _ when toWalkToVector.Equals(CubeVector.TopLeft):
					if (
						surroundingPieceVectors.Any(c => c.Equals(CubeVector.TopRight)) &&
						surroundingPieceVectors.Any(c => c.Equals(CubeVector.Left)))
					{
						isApprovedWalkingPosition = false;
					}
					break;

				// top right
				case var _ when toWalkToVector.Equals(CubeVector.TopRight):
					if (
						surroundingPieceVectors.Any(c => c.Equals(CubeVector.TopLeft)) &&
						surroundingPieceVectors.Any(c => c.Equals(CubeVector.Right)))
					{
						isApprovedWalkingPosition = false;
					}
					break;

				// right
				case var _ when toWalkToVector.Equals(CubeVector.Right):
					if (
						surroundingPieceVectors.Any(c => c.Equals(CubeVector.TopRight)) &&
						surroundingPieceVectors.Any(c => c.Equals(CubeVector.BottomRight)))
					{
						isApprovedWalkingPosition = false;
					}
					break;

				// bottomRight
				case var _ when toWalkToVector.Equals(CubeVector.BottomRight):
					if (
						surroundingPieceVectors.Any(c => c.Equals(CubeVector.Right)) &&
						surroundingPieceVectors.Any(c => c.Equals(CubeVector.BottomLeft)))
					{
						isApprovedWalkingPosition = false;
					}
					break;

				// bottomLeft
				case var _ when toWalkToVector.Equals(CubeVector.BottomLeft):
					if (
						surroundingPieceVectors.Any(c => c.Equals(CubeVector.Left)) &&
						surroundingPieceVectors.Any(c => c.Equals(CubeVector.BottomRight)))
					{
						isApprovedWalkingPosition = false;
					}
					break;

				// left
				case var _ when toWalkToVector.Equals(CubeVector.Left):
					if (
						surroundingPieceVectors.Any(c => c.Equals(CubeVector.TopLeft)) &&
						surroundingPieceVectors.Any(c => c.Equals(CubeVector.BottomLeft)))
					{
						isApprovedWalkingPosition = false;
					}
					break;
			}

			if (isApprovedWalkingPosition)
			{
				approvedWalkPositions.Add(positionToWalk);
			}
		}

		return approvedWalkPositions;
	}
}
