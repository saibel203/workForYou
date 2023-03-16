namespace WorkForYou.Shared.InputModels;

public class SendMessageInputModel
{
    public string Message { get; set; } = string.Empty;
    public int RoomId { get; set; }
}
