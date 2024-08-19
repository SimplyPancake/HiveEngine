using Hive.Core.Models.Players;

namespace Hive.Core.Models;


public class Match
{
	public Board Board { get; }

	public Player Player1 { get; }

	public Player Player2 { get; }

	public Color CurrentTurn
	{
		get
		{
			return _CurrentTurn;
		}
	}
	private Color _CurrentTurn;

	public Match(Player player1, Player player2)
	{
		Player1 = player1;
		Player2 = player2;
		_CurrentTurn = Color.White;
		Board = new Board();

		Player1.Board = Board;
		Player2.Board = Board;
	}

	public Match(Player player1, Player player2, List<Piece> pieces)
	{
		Player1 = player1;
		Player2 = player2;
		_CurrentTurn = Color.White;
		Board = new Board(pieces);

		Player1.Board = Board;
		Player2.Board = Board;
	}

	public Match(Player player1, Player player2, Board board)
	{
		Player1 = player1;
		Player2 = player2;
		_CurrentTurn = Color.White;
		Board = board;

		Player1.Board = Board;
		Player2.Board = Board;
	}

	public void Start()
	{
		bool playingGame = true;
		while (playingGame)
		{
			Play();
			if (Board.HasWinCondition())
			{
				return;
			}
		}

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
				switchTurns();
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

			// Next player's turn
			switchTurns();
		}
	}

	public Player CurrentPlayerTurn()
	{
		return Player1.Color == CurrentTurn ? Player1 : Player2;
	}

	private void switchTurns()
	{
		_CurrentTurn = CurrentTurn == Color.Black ? Color.White : Color.Black;
	}

}
