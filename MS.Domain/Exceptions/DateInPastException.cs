namespace MS.Domain.Exceptions;

public class DateInPastException : Exception
{
    public DateInPastException() : base("تاریخ شروع نباید در گذشته باشد") {}
}
