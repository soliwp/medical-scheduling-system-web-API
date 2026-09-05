namespace MS.Domain.Exceptions;
public class DateException : Exception
{
    public DateException() : base("تاریخ پایان نباید از تاریخ شروع کوچک تر باشد") {}
}
