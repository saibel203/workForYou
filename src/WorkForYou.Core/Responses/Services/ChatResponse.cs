namespace WorkForYou.Core.Responses.Services;

public class ChatResponse : BaseResponse
{
    public bool IsChatExists { get; set; }
    public string OpponentUserId { get; set; } = string.Empty;
}