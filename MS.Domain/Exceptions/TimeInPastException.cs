namespace MS.Domain.Exceptions;

public class TimeInPastException : Exception
{
    public TimeInPastException() : base("ساعت شروع نباید در گذشته باشد") {}
}
