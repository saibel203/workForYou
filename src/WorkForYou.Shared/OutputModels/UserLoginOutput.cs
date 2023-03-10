namespace WorkForYou.Shared.OutputModels;

public class UserLoginOutput
{
    public string Message { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
}