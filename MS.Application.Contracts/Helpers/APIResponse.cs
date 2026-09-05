namespace MS.Application.Contracts.Helpers;

public class APIResponse<T>
{
    public int StatusCode { get; set; }
    public List<string> Errors { get; set; } = new();
    public bool IsSuccess { get; set; }
    public T Data { get; set; }

    // operation success
    public APIResponse(int statusCode, T data)
    {
        StatusCode = statusCode;        
        IsSuccess = true;
        Data = data;
    }
    // operation failed
    public APIResponse(int statusCode, List<string> errors)
    {
        StatusCode = statusCode;
        Errors = errors;
        IsSuccess = false;        
    }
    // operation failed (with one error)
    public APIResponse(int statusCode, string error)
    {
        StatusCode = statusCode;
        Errors.Add(error);
        IsSuccess = false;        
    }
}