
using System.Diagnostics;
using Hive.Core;
using Hive.Core.Models;
using Hive.Core.Models.Bugs;
using Hive.Core.Models.Coordinate;

namespace Hive.Test.Bugs;

public class AntTest
{
	[Test]
	public void PossibleMovesTest()
	{
		Piece ant = new(Color.White, new AntBug(), new Cube(-1, 1, 0));

		List<Piece> pieces = [
			ant,
			new(Color.Black, new QueenBug(), new Cube(0, -1, 1)),
			new(Color.Black, new QueenBug(), new Cube(1, -2, 1)),
			new(Color.Black, new QueenBug(), new Cube(2, -2, 0)),
			new(Color.Black, new QueenBug(), new Cube(2, -1, -1)),
			new(Color.Black, new QueenBug(), new Cube(2, 0, -2)),
			new(Color.White, new QueenBug(), new Cube(0, 0, 0)),
		];

		Board board = new(pieces);

		Debug.WriteLine(board);

		List<Move> moves = ant.PossibleMoves(board);

		Assert.That(moves, Has.Count.EqualTo(13));

		// Assert move positions
		List<Cube> movePositions = moves.Select(m => ((AttackMove)m).AttackPosition).ToList();
		List<Axial> rightMovePositions = new([
			new Axial(0, 1),
			new Axial(1, 0),
			new Axial(1, 1),
			new Axial(2, 1),
			new Axial(3, 0),
			new Axial(3, -1),
			new Axial(3, -2),
			new Axial(3, -3),
			new Axial(2, -3),
			new Axial(1, -3),
			new Axial(0, -2),
			new Axial(-1, -1),
			new Axial(-1, 0),
		]);

		foreach (var pos in rightMovePositions)
		{
			Assert.That(movePositions, Does.Contain(new Cube(pos)));
		}
	}
}