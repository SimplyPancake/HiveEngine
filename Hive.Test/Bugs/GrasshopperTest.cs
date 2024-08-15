using System;
using System.Diagnostics;
using Hive.Core;
using Hive.Core.Models;
using Hive.Core.Models.Bugs;
using Hive.Core.Models.Coordinate;

namespace Hive.Test.Bugs;

public class GrasshopperTest
{
	[Test]
	public void PossibleMovesTest()
	{
		Piece hopper = new(Color.White, new GrasshopperBug(), new Cube(3, -3));

		List<Piece> pieces = [
			hopper,
			new(Color.Black, new QueenBug(), new Cube(3, -2, -1)),
			new(Color.Black, new QueenBug(), new Cube(0, -1, 1)),
			new(Color.Black, new QueenBug(), new Cube(1, -2, 1)),
			new(Color.Black, new QueenBug(), new Cube(2, -2, 0)),
			new(Color.Black, new QueenBug(), new Cube(2, -1, -1)),
			new(Color.Black, new QueenBug(), new Cube(2, 0, -2)),
			new(Color.White, new QueenBug(), new Cube(0, 0, 0)),
		];

		Board board = new(pieces);

		Debug.WriteLine(board);

		List<Move> moves = hopper.PossibleMoves(board);

		Assert.That(moves, Has.Count.EqualTo(2));

		// Assert move positions
		List<Cube> movePositions = moves.Select(m => ((AttackMove)m).AttackPosition).ToList();
		Assert.Multiple(() =>
		{
			Assert.That(CubeListExtensions.ContainsCube(movePositions, new Axial(3, -1)));
			Assert.That(CubeListExtensions.ContainsCube(movePositions, new Axial(1, -1)));
		});
	}
}
