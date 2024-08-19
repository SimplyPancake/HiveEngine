using System.Text.RegularExpressions;
using Hive.Core.Enums;
using Hive.Core.Exceptions;
using Hive.Core.Models;
using Hive.Core.Models.Bugs;
using Hive.Core.Models.Coordinate;

namespace Hive.Core;

// Move in the game
public abstract class Move
{
	public Piece Piece { get; set; }

	public abstract MoveType MoveType { get; }

	/// <summary>
	///	Move constructor takes the piece including position to place
	/// </summary>
	public Move(Piece piece)
	{
		Piece = piece;
	}

	public Move()
	{
		// Default move
		Piece = new Piece(Color.Black, new QueenBug());
	}

	public abstract override string ToString();

	public abstract string MoveString(List<GridPiece> pieces, bool returnPlaceMoves);

	public string MoveString(List<Piece> pieces, bool returnPlaceMoves)
	{
		return MoveString(GridPiece.GridPieces(pieces), returnPlaceMoves);
	}

	private protected static string MovedPieceString(GridPiece gotMoved, GridPiece movedNextTo)
	{
		// If the moving piece is placed left of the target piece, / - or \ preceeds 
		// the piece name indicating the point of attachment.   
		// If the moving piece is placed right of the target piece, / - or \ follows the piece name.

		string movePieceString = "";

		// write 
		movePieceString += gotMoved + " ";

		// get vector
		CubeVector movedFromNextTo = CubeVectorExtensions.CubeToVector(gotMoved.OriginalPosition - movedNextTo.OriginalPosition);

		if (movedFromNextTo.Equals(CubeVector.BottomLeft)) movePieceString += "/";
		if (movedFromNextTo.Equals(CubeVector.Left)) movePieceString += "-";
		if (movedFromNextTo.Equals(CubeVector.TopLeft)) movePieceString += "\\";

		movePieceString += movedNextTo;

		if (movedFromNextTo.Equals(CubeVector.TopRight)) movePieceString += "/";
		if (movedFromNextTo.Equals(CubeVector.Right)) movePieceString += "-";
		if (movedFromNextTo.Equals(CubeVector.BottomRight)) movePieceString += "\\";

		return movePieceString;
	}

	public static Move MoveFromAttackString(string attackString, Board board, Player player)
	{
		List<GridPiece> gridPieces = GridPiece.GridPieces(board.Pieces);

		// easiest is to first generate all the possible attackStrings and choose on of them
		// won't deal with placeMove, because there are too many
		List<Move> possibleMoves = board.PossibleMoves(player); // TODO fix under this

		// if (possibleMoves.Any(pm => pm.MoveString(gridPieces) == attackString))
		// {
		// 	return possibleMoves.First(pm => pm.MoveString(gridPieces) == attackString);
		// }

		// first piece played
		if (board.Pieces.Count == 0)
		{
			// PlaceMove
			Color toPlace = player.Color;
			// Could throw error when player is trying to place a piece thats not their color?

			// Get all bugs
			List<Bug> playerBugs = player.Pieces;
			Bug placedBug = playerBugs.FirstOrDefault(b => b.ShortRepresentation == attackString[1]) ??
				throw new MoveStringProcessingException($"Bug type {attackString[1]} could not be found");

			Piece toPlacePiece = new(toPlace, placedBug, new Cube(0, 0, 0), 0);
			PlaceMove placeMove = new(toPlacePiece);
			return placeMove;
		}

		Regex r = new(@"(b|w)([A-Z])(\d)* ([\\\-\/])?(b|w)([A-Z])(\d)*([\\\-\/])?");

		System.Text.RegularExpressions.Match m = r.Match(attackString);
		Dictionary<int, string> matchResults = [];

		while (m.Success)
		{
			for (int i = 1; i <= 8; i++)
			{
				Group g = m.Groups[i];
				matchResults.Add(i, g.Value);
			}
			m = m.NextMatch();
		}

		// First we find the related piece
		if (!matchResults.ContainsKey(5) || !matchResults.ContainsKey(6) || !matchResults.ContainsKey(7))
		{
			throw new MoveStringProcessingException("Must give piece to put next to");
		}
		string nextToPieceString = matchResults[5] + matchResults[6] + matchResults[7];

		if (!gridPieces.Any(gp => gp.ToString() == nextToPieceString))
		{
			throw new MoveStringProcessingException($"{nextToPieceString} is not in board.");
		}

		// we found a piece!
		GridPiece nextToPiece = gridPieces.First(gp => gp.ToString() == nextToPieceString);

		CubeVector toPlacePieceNext = CubeVector.Zero;

		// left of nextToPiece
		toPlacePieceNext = matchResults[4] switch
		{
			@"\" => CubeVector.TopLeft,
			@"-" => CubeVector.Left,
			@"/" => CubeVector.BottomLeft,
			_ => CubeVector.Zero,
		};

		if (toPlacePieceNext.Equals(CubeVector.Zero))
		{
			toPlacePieceNext = matchResults[8] switch
			{
				@"\" => CubeVector.BottomRight,
				@"-" => CubeVector.Right,
				@"/" => CubeVector.TopRight,
				_ => CubeVector.Zero,
			};
		}

		Cube placedOn = nextToPiece.OriginalPosition + toPlacePieceNext;
		int placedOnHeight = nextToPiece.Height;

		if (toPlacePieceNext.Equals(CubeVector.Zero))
		{
			// original was placed on top
			placedOnHeight = nextToPiece.Height + 1;
		}

		// Determine if AttackMove or PlaceMove
		// Does the piece to be placed already exist?
		string placedPieceString = matchResults[1] + matchResults[2] + matchResults[3];
		Move madeMove;

		// if (board.Pieces.Any(p => p.Position.Equals(placedOn) && p.Height == placedOnHeight))
		if (gridPieces.Any(gp => gp.ToString() == placedPieceString))
		{
			// AttackMove
			// now get the piece that is attacking
			GridPiece attackingPiece = gridPieces.First(gp => gp.ToString() == placedPieceString);
			Piece boardPiece = board.Pieces.First(p =>
				p.Position.Equals(attackingPiece.OriginalPosition) &&
				p.Height == attackingPiece.Height);

			AttackMove move = new(boardPiece, placedOn, placedOnHeight, MoveType.Activate);
			madeMove = move;
		}
		else
		{
			// PlaceMove
			Color toPlace = player.Color;
			// Could throw error when player is trying to place a piece thats not their color?

			// Get all bugs
			// Maybe rework how bugs are gotten when adding mod support?
			List<Bug> playerBugs = player.Pieces;
			Bug placedBug = playerBugs.FirstOrDefault(b => b.ShortRepresentation == matchResults[2][0]) ??
				throw new MoveStringProcessingException($"Bug type {matchResults[2][0]} could not be found");

			Piece toPlacePiece = new(toPlace, placedBug, placedOn, placedOnHeight);
			PlaceMove placeMove = new(toPlacePiece);
			madeMove = placeMove;
		}

		return madeMove;
	}

	// override object.Equals
	public override bool Equals(object? obj)
	{
		//
		// See the full list of guidelines at
		//   http://go.microsoft.com/fwlink/?LinkID=85237
		// and also the guidance for operator== at
		//   http://go.microsoft.com/fwlink/?LinkId=85238
		//

		if (obj == null || obj.GetType().BaseType != typeof(Move))
		{
			return false;
		}

		Move move = (Move)obj;

		if (move.MoveType == MoveType.Activate)
		{
			return ((AttackMove)move).GetHashCode() == GetHashCode();
		}

		return move.GetHashCode() == GetHashCode();
	}

	// override object.GetHashCode
	public abstract new int GetHashCode();
}
