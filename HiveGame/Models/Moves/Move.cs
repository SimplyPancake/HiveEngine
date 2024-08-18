using System.Diagnostics;
using System.Text.RegularExpressions;
using Hive.Core.Enums;
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

	public abstract string MoveString(List<GridPiece> pieces);

	public string MoveString(List<Piece> pieces)
	{
		return MoveString(GridPiece.GridPieces(pieces));
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

	public static Move MoveFromAttackString(string attackString, Board board)
	{
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
		List<GridPiece> gridPieces = GridPiece.GridPieces(board.Pieces);
		string nextToPieceString = matchResults[5] + matchResults[6] + matchResults[7];

		if (!gridPieces.Any(gp => gp.ToString() == nextToPieceString))
		{
			throw new Exception($"{nextToPieceString} is not in board.");
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

		toPlacePieceNext = matchResults[8] switch
		{
			@"\" => CubeVector.TopLeft,
			@"-" => CubeVector.Left,
			@"/" => CubeVector.BottomLeft,
			_ => CubeVector.Zero,
		};

		Cube placedOn = nextToPiece.OriginalPosition + toPlacePieceNext;
		int placedOnHeight = nextToPiece.Height;

		if (toPlacePieceNext.Equals(CubeVector.Zero))
		{
			// original was placed on top

			placedOnHeight = nextToPiece.Height + 1;
		}

		// Determine if AttackMove or PlaceMove
		if (board.Pieces.Any(p => p.Position.Equals(placedOn) && p.Height == placedOnHeight))
		{
			// AttackMove

		}
		else
		{
			// PlaceMove

		}

	}
}
