using Hive.Core.Exceptions;
using Hive.Core.Models.Bugs;

namespace Hive.Core.Models.Players;

public class ConsolePlayer : Player
{
	public ConsolePlayer(string playername, Color color, List<Bug> pieces) : base(playername, color, pieces) {}

	public ConsolePlayer(string playername, Color color) : base(playername, color) {}

	private void PrintPlayer(string message)
	{
		Console.WriteLine($"{Playername}: {message}");
	}

	public override Move MakeMove()
	{
		// ask the player to make a move, but now we return just this
		Console.WriteLine(Board);

		List<Move> possibleMoves = Board.PossibleMoves(this);
		List<string> possibleMoveStrings = [];
		foreach (Move possibleMove in possibleMoves)
		{
			string moveString = possibleMove.MoveString(Board.Pieces, true);
			possibleMoveStrings.Add(moveString);
		}

		// List<string> possibleMoveStrings = possibleMoves.Select(move => move.MoveString(Board.Pieces, true)).ToList();
		Console.WriteLine($"Possible moves:\n {string.Join(", ", possibleMoveStrings)}");

		while (true)
		{
			PrintPlayer("Please make a move...");

			string? madeMove = Console.ReadLine();

			if (madeMove == null || madeMove == "")
			{
				PrintPlayer("You must input a move");
				continue;
			}

			try
			{
				return Move.MoveFromAttackString(madeMove, Board, this);
			}
			catch (MoveStringProcessingException e)
			{
				Console.WriteLine($"Move error: {e.Message}");
			}
		}
	}

	public override Move MakeMove(IllegalMoveException illegalMoveException) => MakeMove();
}
