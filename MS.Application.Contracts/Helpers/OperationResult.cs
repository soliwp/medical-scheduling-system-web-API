namespace MS.Application.Contracts.Helpers;
public class OperationResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public OperationResult()
    {
        IsSuccess = false;
    }
    public OperationResult Succesed(string massage = "عملیات با موفقیت انجام شد")
    {
        IsSuccess = true;
        Message = massage;
        return this;
    }
    public OperationResult Failed(string massage = "عملیات ناموفق بود")
    {
        IsSuccess = false;
        Message = massage;
        return this;
    }
}
