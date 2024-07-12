namespace Task3.Exceptions;

public class ArgumentRepeatedException : Exception
{
	public ArgumentRepeatedException() : base() { }

	public ArgumentRepeatedException(string message) : base(message) { }

	public ArgumentRepeatedException(string message, Exception innerException) : base(message, innerException) { }
}
