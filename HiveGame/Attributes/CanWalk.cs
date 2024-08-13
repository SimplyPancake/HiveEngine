﻿using Hive.Core.Enums;
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

	public CanWalk(int WalkAmount)
	{
		this.WalkAmount = WalkAmount;
	}

	public override List<AttackMove> Moves(Board board, Piece piece)
	{
		List<Cube> walkPositions = [];
		List<Cube> boardCoordinates = board.Pieces.Select(p => p.Position).ToList();
		// if (WalkAmount == 1)
		// {
		// 	walkPositions = WalkSingle(piece.Position, boardCoordinates);
		// }

		// start with the first piece, and walk from there
		List<Cube> visited = [piece.Position];
		List<Cube> toVisit = WalkSingle(piece.Position, boardCoordinates);
		int walked = 0;

		// List<Cube> visited = [];
		// List<Cube> toVisit = [piece.Position];
		// int walked = 0;

		bool isDone = false;

		while (!isDone)
		{
			List<Cube> walkNextTime = [];

			// Explore all current positions in 'toVisit'
			foreach (Cube toExplore in toVisit)
			{
				// Add the current position to the visited list
				if (!visited.Any(x => x.Equals(toExplore)))
				{
					// we've explored it.
					visited.Add(toExplore);
				}

				// Walk one step from the current position and get new positions to explore
				List<Cube> newPositions = WalkSingle(toExplore, boardCoordinates);

				// Add only non-visited positions to the list of positions to explore next
				foreach (var pos in newPositions)
				{
					if (!visited.Any(x => x.Equals(pos)) && !toVisit.Any(x => x.Equals(pos)))
					{
						walkNextTime.Add(pos);
					}
				}
			}

			// Prepare for the next iteration: update 'toVisit' and increment 'walked'
			toVisit = walkNextTime;
			walked++;

			// Stop the loop when the walk amount is reached or there are no more positions to explore
			isDone = WalkAmount > 0 ? walked == WalkAmount : toVisit.Count == 0 && visited.Count != 0;
		}

		// then add visited to walkPositions
		walkPositions.AddRange(visited);

		// we do not care about the original position
		walkPositions = walkPositions.Where(pos => !pos.Equals(piece.Position)).ToList();

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

		List<Cube> surroundingPieces = piecePositions.Where(piece => Cube.Distance(piece, piecePosition) == 1).ToList();

		// Remove the pieces positions that are already there
		surroundingWithPieceNextTo.RemoveAll(surroundingPieces.Contains);

		// Then we have to check the passing between pieces rule
		List<Cube> canWalkThrough = CanWalkThrough(piecePosition, surroundingWithPieceNextTo, surroundingPieces);

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
						CubeListExtensions.ContainsCube(surroundingPieceVectors, CubeVector.TopRight) &&
						CubeListExtensions.ContainsCube(surroundingPieceVectors, CubeVector.TopLeft))
					{
						isApprovedWalkingPosition = false;
					}
					break;

				// top right
				case var _ when toWalkToVector.Equals(CubeVector.TopRight):
					if (
						CubeListExtensions.ContainsCube(surroundingPieceVectors, CubeVector.TopLeft) &&
						CubeListExtensions.ContainsCube(surroundingPieceVectors, CubeVector.Right))
					{
						isApprovedWalkingPosition = false;
					}
					break;

				// right
				case var _ when toWalkToVector.Equals(CubeVector.Right):
					if (
						CubeListExtensions.ContainsCube(surroundingPieceVectors, CubeVector.TopRight) &&
						CubeListExtensions.ContainsCube(surroundingPieceVectors, CubeVector.BottomRight))
					{
						isApprovedWalkingPosition = false;
					}
					break;

				// bottomRight
				case var _ when toWalkToVector.Equals(CubeVector.BottomRight):
					if (
						CubeListExtensions.ContainsCube(surroundingPieceVectors, CubeVector.Right) &&
						CubeListExtensions.ContainsCube(surroundingPieceVectors, CubeVector.BottomLeft))
					{
						isApprovedWalkingPosition = false;
					}
					break;

				// bottomLeft
				case var _ when toWalkToVector.Equals(CubeVector.BottomLeft):
					if (
						CubeListExtensions.ContainsCube(surroundingPieceVectors, CubeVector.Left) &&
						CubeListExtensions.ContainsCube(surroundingPieceVectors, CubeVector.BottomRight))
					{
						isApprovedWalkingPosition = false;
					}
					break;

				// left
				case var _ when toWalkToVector.Equals(CubeVector.Left):
					if (
						CubeListExtensions.ContainsCube(surroundingPieceVectors, CubeVector.TopLeft) &&
						CubeListExtensions.ContainsCube(surroundingPieceVectors, CubeVector.BottomLeft))
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
