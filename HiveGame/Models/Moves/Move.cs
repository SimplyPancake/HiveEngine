using Hive.Core.Enums;
using Hive.Core.Models;
using Hive.Core.Models.Bugs;

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
}
