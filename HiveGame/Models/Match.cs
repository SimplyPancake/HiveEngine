namespace Hive.Core;


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
	}

	public void Start()
	{
		bool playingGame = true;
		while (playingGame)
		{
			play();
			if (Board.HasWinCondition())
			{
				return;
			}
		}

		System.Console.WriteLine($"Thanks for playing! The winning player is {CurrentPlayerTurn().Playername}");
	}

	private void play()
	{
		while (!Board.HasWinCondition())
		{
			// assume move is not valid, then check if they CAN make the move.
			bool validMove = false;
			Player toMove = CurrentPlayerTurn();
			Move toMake = toMove.MakeMove();

			while (!validMove)
			{
				try
				{
					if (Board.AllowedToMakeMove(toMake, toMove))
					{
						validMove = true;
						continue;
					}
				}
				catch (Exception e)
				{
					// nothing lol
				}
				finally
				{
					toMove = CurrentPlayerTurn();
					toMake = toMove.MakeMove();
				}
			}

			// Move is valid
			Board.MakeMove(toMake, toMove);

			// Print board for now, can make custom client later
			Board.PrintBoard();

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
