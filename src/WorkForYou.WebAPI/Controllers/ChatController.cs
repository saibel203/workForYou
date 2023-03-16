using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.Responses.Repositories;
using WorkForYou.WebAPI.Attributes;

namespace WorkForYou.WebAPI.Controllers;

public class ChatController : BaseController
{
    private readonly IStringLocalizer<ChatController> _stringLocalization;
    private readonly IUnitOfWork _unitOfWork;

    public ChatController(IUnitOfWork unitOfWork, IStringLocalizer<ChatController> stringLocalization)
    {
        _unitOfWork = unitOfWork;
        _stringLocalization = stringLocalization;
    }

    [HttpDelete("removeChat/{chatId:int}")] // /api/chat/removeChat/{chatId}
    [JwtAuthorize]
    public async Task<IActionResult> RemoveChat(int chatId)
    {
        ChatResponse removeChatResult =  await _unitOfWork.ChatRepository
            .RemoveChatRoomAsync(chatId);

        ApiError error = new ApiError();

        if (!removeChatResult.IsSuccessfully)
        {
            error.ErrorCode = NotFound().StatusCode;
            error.ErrorMessage = removeChatResult.Message;
            return NotFound(error);
        }

        return Ok(new
        {
            Message = _stringLocalization["SuccessRemoveChat"]
        });
    }
}
