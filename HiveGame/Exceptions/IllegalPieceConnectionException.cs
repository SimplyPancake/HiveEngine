namespace Hive.Core;

public class IllegalPieceConnectionException : IllegalMoveException
{
	public IllegalPieceConnectionException()
	{
	}

	public IllegalPieceConnectionException(string? message) : base(message)
	{
	}
}
