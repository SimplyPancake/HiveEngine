using Hive.Core.Models.Bugs;
using Hive.Core.Models.Players;

namespace Hive.Core.Models;


public class Match
{
	public Board Board { get; }

	public Player Player1 { get; }

	public Player Player2 { get; }

	public int Turn { get; private set; } = 0;

	public Move? LastMove { get; private set; } = null;
	public Color LastMoveColor { get; private set; } = Color.White;

	public Color CurrentTurn
	{
		get
		{
			return _CurrentTurn;
		}
	}

	private Color _CurrentTurn;

	public Match()
	{
		Player1 = new ConsolePlayer("Hans", Color.White);
		Player2 = new ConsolePlayer("Frans", Color.Black);
		_CurrentTurn = Color.White;
		Board = new Board();
	}

	public Match(Player player1, Player player2)
	{
		Player1 = player1;
		Player2 = player2;
		_CurrentTurn = Color.White;
		Board = new Board();
	}

	public Match(Player player1, Player player2, Board board)
	{
		Player1 = player1;
		Player2 = player2;
		_CurrentTurn = player1.Color;
		Board = board;
	}

	public void Start()
	{
		Play();

		Console.WriteLine($"Thanks for playing! The winning player is {CurrentPlayerTurn().Playername}");
	}

	private void Play()
	{
		while (!Board.HasWinCondition())
		{
			// assume move is not valid, then check if they CAN make the move.
			bool validMove = false;
			Player toMove = CurrentPlayerTurn();

			if (Board.PossibleMoves(toMove).Count == 0)
			{
				Console.WriteLine(Board);
				Console.WriteLine("No pieces to move or play. Skipping turn...");

				// Next player's turn
				SwitchTurns();
				continue;
			}

			Move toMake = toMove.MakeMove();

			while (!validMove)
			{
				try
				{
					// AlloweToMakeMove throws exception if false, so no if statement is needed
					Board.AllowedToMakeMove(toMake, toMove);
					validMove = true;
					continue;
				}
				catch (IllegalMoveException e)
				{
					// Not allowed to make move
					Console.WriteLine($"Illegal move: {e.Message}");
					toMake = toMove.MakeMove();
				}
			}

			// Move is valid
			Board.MakeMove(toMake, toMove);
			LastMove = toMake;
			LastMoveColor = CurrentPlayerTurn().Color;

			// Next player's turn
			SwitchTurns();
		}
	}

	public Player CurrentPlayerTurn()
	{
		return Player1.Color == CurrentTurn ? Player1 : Player2;
	}

	public Player OtherPlayerTurn()
	{
		return Player1.Color == CurrentTurn ? Player2 : Player1;
	}
	public void SwitchTurns()
	{
		_CurrentTurn = CurrentTurn == Color.Black ? Color.White : Color.Black;
		Turn++;
	}

	public void SetLastMove(Move? move, Color color)
	{
		LastMove = move;
		LastMoveColor = color;
	}

	public Match Clone()
	{
		// currentplayer of match is;
		Match toReturn = new(CurrentPlayerTurn(), OtherPlayerTurn(), Board.Copy());
		toReturn.SetLastMove(LastMove, LastMoveColor);
		return toReturn;
	}

	/// <summary>
	/// Returns a copy of the match with the given action applied.
	/// Switches turns.
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	public Match Result(Move action, bool switchTurns = true)
	{
		Board newBoard = Board.SimulateMove(action);
		Match toReturn = new(CurrentPlayerTurn(), OtherPlayerTurn(), newBoard);
		toReturn.SetLastMove(action, LastMoveColor);
		if (switchTurns)
		{
			toReturn.SwitchTurns();
		}

		return toReturn;
	}
}
