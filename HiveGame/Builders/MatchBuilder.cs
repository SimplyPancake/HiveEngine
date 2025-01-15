using System;
using Hive.Core.Models;
using Hive.Core.Models.Players;

namespace Hive.Core.Builders;

public class MatchBuilder
{
	private Player player1;
	private Player player2;
	private Board? board;

	public MatchBuilder SetPlayer1(Player player1)
	{
		this.player1 = player1;
		return this;
	}

	public MatchBuilder SetPlayer2(Player player2)
	{
		this.player2 = player2;
		return this;
	}

	public MatchBuilder SetBoard(Board board)
	{
		this.board = board;
		return this;
	}

	public Match Build()
	{
		Match toReturn;

		if (player1 == null || player2 == null)
		{
			throw new InvalidOperationException("Both players must be set before building a match.");
		}

		if (board == null)
		{
			toReturn = new Match(player1, player2);
		} else {
			toReturn = new Match(player1, player2, board);
		}

		player1.SetMatch(toReturn);
		player2.SetMatch(toReturn);

		return toReturn;
	}
}
