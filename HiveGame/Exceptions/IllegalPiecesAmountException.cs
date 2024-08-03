namespace Hive.Core;

public class IllegalPiecesAmountException : IllegalMoveException
{
	public IllegalPiecesAmountException()
	{
	}

	public IllegalPiecesAmountException(string? message) : base(message)
	{
	}
}
