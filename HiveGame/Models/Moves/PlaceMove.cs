namespace Hive.Core;

public class PlaceMove(Piece piece) : Move(piece)
{
	public override MoveType MoveType => MoveType.Place;
}
