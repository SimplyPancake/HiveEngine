namespace Hive.Core;

public class IllegalPlacementException : IllegalMoveException
{
	public IllegalPlacementException()
	{
	}

	public IllegalPlacementException(string? message) : base(message)
	{
	}
}
