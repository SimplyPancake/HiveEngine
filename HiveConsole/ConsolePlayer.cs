using System.Numerics;
using Hive.Core;
using HiveGame;
namespace Hive.Console;

public class ConsolePlayer : Player
{
	public override string Playername { get; }

	public override Color Color { get; }

	public override List<Piece> Pieces { get; }

	public override Board Board { get; }

	public ConsolePlayer(string playername, Color color, List<Piece> pieces)
	{
		Playername = playername;
		Color = color;
		Pieces = pieces;
		Board = new Board();
	}

	public ConsolePlayer(string playername, Color color)
	{
		Playername = playername;
		Color = color;
		Pieces = new List<Piece>();
		Board = new Board();

		// Initialise pieces collection
		List<Bug> bugs = PieceCollectionMethods.GetPieceBugs(PieceCollection.Classic);
		foreach (Bug bug in bugs)
		{
			Piece p = new Piece(Color, bug);
			for (int i = 0; i < bug.GetAmount; i++)
			{
				Pieces.Add(p);
			}
		}
	}

	private void printPlayer(string message)
	{
		System.Console.WriteLine($"{Playername}: {message}");
	}

	public override Move MakeMove()
	{
		// ask the player to make a move, but now we return just this
		Board.PrintBoard();
		printPlayer("Please make a move.");

		// we just place a queen!
		Bug queen = new QueenBug();

		return new Move(new Piece(Color.White, queen), new Vector3(0, 0, 0), MoveType.Place);
	}
}
