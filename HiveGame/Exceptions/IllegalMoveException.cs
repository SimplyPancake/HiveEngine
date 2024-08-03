namespace Hive.Core;

public class IllegalMoveException : Exception
{
	public IllegalMoveException()
	{
	}

	public IllegalMoveException(string? message) : base(message)
	{
	}
}
