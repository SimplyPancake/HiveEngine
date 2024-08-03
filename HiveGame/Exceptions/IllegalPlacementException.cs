using System.Runtime.Serialization;

namespace Hive.Core;

public class IllegalPlacementException : Exception
{
	public IllegalPlacementException()
	{
	}

	public IllegalPlacementException(string? message) : base(message)
	{
	}

	public IllegalPlacementException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}
