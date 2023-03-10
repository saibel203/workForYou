namespace WorkForYou.Core.DTOModels.ChatDTOs;

public class CreateMessageDto
{
    public string MessageContent { get; set; } = string.Empty;
    public int RoomId { get; set; }
}
