// See https://aka.ms/new-console-template for more information

using Hive.Console;
using Hive.Core;
using Hive.Core.Models;
using Hive.Core.Models.Bugs;
using Hive.Core.Models.Coordinate;
using Hive.Core.Services;

// Player p1 = new ConsolePlayer("Hans", Color.Black);
// Player p2 = new ConsolePlayer("Frans", Color.White);

// Match m = new Match(p1, p2);
// m.Start();


Piece whiteQueen = new(Color.White, new QueenBug(), new Cube(0, 0, 0));

List<Piece> pieces = [
	whiteQueen,
			new(Color.Black, new QueenBug(), new Cube(0, -1, 1)),
			new(Color.Black, new QueenBug(), new Cube(1, -2, 1)),
			new(Color.Black, new QueenBug(), new Cube(2, -2, 0)),
			new(Color.Black, new QueenBug(), new Cube(2, -1, -1)),
			new(Color.Black, new QueenBug(), new Cube(2, 0, -2)),
			new(Color.Black, new QueenBug(), new Cube(-1, 1, 0)),
		];

Board board = new(pieces);

Console.WriteLine(ConsoleHexPrinter.BoardString(board));
