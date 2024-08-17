using Hive.Core;
using Hive.Core.Models;
using Hive.Core.Models.Bugs;
using Hive.Core.Services;

namespace Hive.Console;

public class ConsolePlayer : Player
{
	public override string Playername { get; }

	public override Color Color { get; }

	public override List<Bug> Pieces { get; }
	public override Board Board { get; set; }

	public ConsolePlayer(string playername, Color color, List<Bug> pieces)
	{
		Playername = playername;
		Color = color;
		Pieces = pieces;
		Board = new();
	}

	public ConsolePlayer(string playername, Color color)
	{
		Playername = playername;
		Color = color;
		Pieces = [];
		Board = new();

		// Initialise pieces collection
		Pieces = PieceCollectionMethods.GetPieceBugs(PieceCollection.Classic);
	}

	private void PrintPlayer(string message)
	{
		System.Console.WriteLine($"{Playername}: {message}");
	}

	public override Move MakeMove()
	{
		// ask the player to make a move, but now we return just this
		System.Console.WriteLine("Printing board before making move...");
		System.Console.WriteLine(Board);
		PrintPlayer("Please make a move...");

		// we just place a queen
		Bug queen = new QueenBug();

		return new PlaceMove(new Piece(Color, queen));
	}

	public override Move MakeMove(IllegalMoveException illegalMoveException)
	{
		throw new NotImplementedException();
	}
}
