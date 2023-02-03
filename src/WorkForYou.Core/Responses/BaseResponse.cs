namespace WorkForYou.Core.Responses;

public class BaseResponse
{
    public string Message { get; set; } = string.Empty;
    public bool IsSuccessfully { get; set; }
}
