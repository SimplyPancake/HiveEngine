﻿namespace Hive.Core;


public class HumanPlayer : Player
{
	public override string Playername { get; }

	public override Color Color { get; }

	public override List<Piece> Pieces { get; }

	public override Board Board { get; }

	public HumanPlayer(string playername, Color color, List<Piece> pieces, Board board)
	{
		Playername = playername;
		Color = color;
		Pieces = pieces;
		Board = board;
	}

	public HumanPlayer(string playername, Color color, Board board)
	{
		Playername = playername;
		Color = color;
		Board = board;
		Pieces = new List<Piece>();

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

	public override Move MakeMove()
	{
		// ask the player to make a move, but now we return just this
		return new PlaceMove(new Piece(Color.White, new QueenBug()));
	}
}
