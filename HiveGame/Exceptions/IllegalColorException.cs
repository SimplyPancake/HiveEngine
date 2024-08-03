namespace Hive.Core;

public class IllegalColorException : IllegalMoveException
{
	public IllegalColorException()
	{
	}

	public IllegalColorException(string? message) : base(message)
	{
	}
}
