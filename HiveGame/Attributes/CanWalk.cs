using System.Globalization;
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

	public override List<Move> Moves(Board board, Piece piece)
	{
		List<Cube> walkPositions = [];
		if (WalkAmount == 1)
		{
			walkPositions = WalkSingle(piece.Position, board.Pieces.Select(p => piece.Position).ToList());
		}

		throw new NotImplementedException();
	}

	private List<Cube> WalkSingle(Cube piecePosition, List<Cube> piecePositions)
	{
		// Can walk to a surrounding position if
		// - it is next to (at least one) piece, that is not the original position
		// - does not pass between two pieces
		// This algorithm is very slow; n^2, can be optimised

		List<Cube> surroundingPositions = Board.SurroundingPositions(piecePosition);

		List<Cube> positionsWithoutPiece = new(piecePositions);
		positionsWithoutPiece.Remove(piecePosition);

		// get all surroundingPositions that have at least one piece next to them
		List<Cube> surroundingWithPieceNextTo = surroundingPositions
			.Where(surroundingPosition => Board.AmountOfSurroundingCubes(
				surroundingPosition, positionsWithoutPiece) > 0)
			.ToList();

		// Then we have get all piecePositions that are around the original piece
		List<Cube> piecePositionsAroundPiece = piecePositions
			.Where(sur => Cube.Distance(sur, piecePosition) == 1).ToList();

		// Then we have to check the passing between pieces rule
		List<Cube> canWalkThrough = CanWalkThrough(piecePosition, surroundingWithPieceNextTo, surroundingPositions);

		return [];
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
				case var _ when toWalkToVector.Equals(topLeft):
					if (
						CubeListExtensions.ContainsCube(surroundingPieceVectors, topRight) &&
						CubeListExtensions.ContainsCube(surroundingPieceVectors, left))
					{
						isApprovedWalkingPosition = false;
					}
					break;

				// top right
				case var _ when toWalkToVector.Equals(topRight):
					if (
						CubeListExtensions.ContainsCube(surroundingPieceVectors, topLeft) &&
						CubeListExtensions.ContainsCube(surroundingPieceVectors, right))
					{
						isApprovedWalkingPosition = false;
					}
					break;

				// right
				case var _ when toWalkToVector.Equals(right):
					if (
						CubeListExtensions.ContainsCube(surroundingPieceVectors, topRight) &&
						CubeListExtensions.ContainsCube(surroundingPieceVectors, bottomRight))
					{
						isApprovedWalkingPosition = false;
					}
					break;

				// bottomRight
				case var _ when toWalkToVector.Equals(bottomRight):
					if (
						CubeListExtensions.ContainsCube(surroundingPieceVectors, right) &&
						CubeListExtensions.ContainsCube(surroundingPieceVectors, bottomLeft))
					{
						isApprovedWalkingPosition = false;
					}
					break;

				// bottomLeft
				case var _ when toWalkToVector.Equals(bottomLeft):
					if (
						CubeListExtensions.ContainsCube(surroundingPieceVectors, left) &&
						CubeListExtensions.ContainsCube(surroundingPieceVectors, bottomRight))
					{
						isApprovedWalkingPosition = false;
					}
					break;

				// left
				case var _ when toWalkToVector.Equals(left):
					if (
						CubeListExtensions.ContainsCube(surroundingPieceVectors, bottomLeft) &&
						CubeListExtensions.ContainsCube(surroundingPieceVectors, topLeft))
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
