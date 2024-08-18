using System;

namespace Hive.Core.Exceptions;

public class MoveStringProcessingException : Exception
{
	public MoveStringProcessingException()
	{
	}

	public MoveStringProcessingException(string? message) : base(message)
	{
	}
}
