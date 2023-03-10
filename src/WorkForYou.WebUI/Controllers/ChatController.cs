using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.DTOModels.ChatDTOs;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.Responses.Repositories;
using WorkForYou.Core.ServiceInterfaces;
using WorkForYou.WebUI.Hubs;

namespace WorkForYou.WebUI.Controllers;

public class ChatController : BaseController
{
    private readonly IStringLocalizer<ChatController> _stringLocalization;
    private readonly INotificationService _notificationService;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IUnitOfWork _unitOfWork;

    public ChatController(IUnitOfWork unitOfWork, IHubContext<ChatHub> hubContext,
        INotificationService notificationService, IStringLocalizer<ChatController> stringLocalization)
    {
        _unitOfWork = unitOfWork;
        _hubContext = hubContext;
        _notificationService = notificationService;
        _stringLocalization = stringLocalization;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ChatDetails(int id)
    {
        ChatResponse chatDetailsResult;

        if (User.IsInRole(ApplicationRoles.EmployerRole))
            chatDetailsResult =
                await _unitOfWork.ChatRepository.GetChatDetailsAsync(id, ApplicationRoles.EmployerRole, GetUserId());
        else
            chatDetailsResult =
                await _unitOfWork.ChatRepository.GetChatDetailsAsync(id, ApplicationRoles.CandidateRole, GetUserId());

        if (!chatDetailsResult.IsSuccessfully)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["GetChatError"]);
            return RedirectToAction(nameof(AllChats));
        }

        return View(chatDetailsResult.ChatRoom);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ChatDetailsByName(string chatName)
    {
        var chatDetailsResult = await _unitOfWork.ChatRepository
            .GetChatDetailsByChatNameAsync(chatName);

        if (!chatDetailsResult.IsSuccessfully)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["GetChatError"]);
            return RedirectToAction(nameof(AllChats));
        }

        return RedirectToAction(nameof(ChatDetails), new {id = chatDetailsResult.ChatRoom?.ChatRoomId});
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "employer")]
    public async Task<IActionResult> CreateChatRoom(string chatName, string userId)
    {
        var currentUserId = GetUserId();
        var createChatResult = await _unitOfWork.ChatRepository.CreateChatRoomAsync(chatName, currentUserId, userId);

        if (!createChatResult.IsSuccessfully)
            return RedirectToAction("AllCandidates", "EmployerAccount");

        return RedirectToAction(nameof(ChatDetails), new {id = createChatResult.ChatRoom?.ChatRoomId});
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "employer")]
    public async Task<IActionResult> RemoveChatRoom(int chatId)
    {
        var removeChatResult = await _unitOfWork.ChatRepository
            .RemoveChatRoomAsync(chatId);

        if (!removeChatResult.IsSuccessfully)
            return RedirectToAction(nameof(AllChats));

        return RedirectToAction(nameof(AllChats));
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> AllChats()
    {
        var currentUserId = GetUserId();
        ChatResponse allChatsResult;

        if (User.IsInRole(ApplicationRoles.EmployerRole))
            allChatsResult =
                await _unitOfWork.ChatRepository.GetAllUserChatsAsync(currentUserId, ApplicationRoles.EmployerRole);
        else
            allChatsResult =
                await _unitOfWork.ChatRepository.GetAllUserChatsAsync(currentUserId, ApplicationRoles.CandidateRole);

        if (!allChatsResult.IsSuccessfully)
            return RedirectToAction("Index", "Main");

        return View(allChatsResult.ChatRooms);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SendMessage(string message, int roomId)
    {
        var currentUsername = GetUsername();
        var createMessageDto = new CreateMessageDto
        {
            MessageContent = message,
            RoomId = roomId
        };
        var createMessageResult = await _unitOfWork.ChatRepository
            .CreateChatMessageAsync(createMessageDto, currentUsername);

        await _hubContext.Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", new
        {
            createMessageResult.ChatMessage!.Content,
            createMessageResult.ChatMessage.Name,
            Timestamp = createMessageResult.ChatMessage.SendTime.ToString("MM/dd/yyyy hh:mm:ss"),
        });

        return Ok();
    }
}