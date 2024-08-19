using Hive.Core;
using Hive.Core.Exceptions;
using Hive.Core.Models;
using Hive.Core.Models.Bugs;

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
		System.Console.WriteLine(Board);

		List<Move> possibleMoves = Board.PossibleMoves(this);
		List<string> possibleMoveStrings = possibleMoves.Select(move => move.MoveString(Board.Pieces, true)).ToList();
		System.Console.WriteLine($"Possible moves:\n {string.Join(", ", possibleMoveStrings)}");

		while (true)
		{
			PrintPlayer("Please make a move...");

			string? madeMove = System.Console.ReadLine();

			if (madeMove == null || madeMove == "")
			{
				PrintPlayer("You must input a move");
			}

			try
			{
				return Move.MoveFromAttackString(madeMove, Board, this);
			}
			catch (MoveStringProcessingException e)
			{
				System.Console.WriteLine($"Move error: {e.Message}");
			}
		}
	}

	public override Move MakeMove(IllegalMoveException illegalMoveException)
	{
		throw new NotImplementedException();
	}
}
