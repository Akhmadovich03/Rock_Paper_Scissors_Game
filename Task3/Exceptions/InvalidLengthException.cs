namespace Task3.Exceptions;

public class InvalidLengthException : Exception
{
	public InvalidLengthException() : base() { }

	public InvalidLengthException(string message) : base(message) { }

	public InvalidLengthException(string message, Exception innerException) : base(message, innerException) { }
}
