using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.Responses.Repositories;
using WorkForYou.WebUI.Hubs;

namespace WorkForYou.WebUI.Controllers;

public class ChatController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<ChatHub> _hubContext;

    public ChatController(IUnitOfWork unitOfWork, IHubContext<ChatHub> hubContext)
    {
        _unitOfWork = unitOfWork;
        _hubContext = hubContext;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ChatDetails(int id)
    {
        ChatResponse chatDetailsResult;
        
        if (User.IsInRole(EmployerRole))
            chatDetailsResult = await _unitOfWork.ChatRepository.GetChatDetailsAsync(id, EmployerRole, GetUserId());
        else 
            chatDetailsResult = await _unitOfWork.ChatRepository.GetChatDetailsAsync(id, CandidateRole, GetUserId());
        
        if (!chatDetailsResult.IsSuccessfully)
            return RedirectToAction(nameof(AllChats));

        return View(chatDetailsResult.ChatRoom);
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
        
        if (User.IsInRole(EmployerRole))
            allChatsResult = await _unitOfWork.ChatRepository.GetAllUserChatsAsync(currentUserId, EmployerRole);
        else 
            allChatsResult = await _unitOfWork.ChatRepository.GetAllUserChatsAsync(currentUserId, CandidateRole);

        if (!allChatsResult.IsSuccessfully)
            return RedirectToAction("Index", "Main");

        return View(allChatsResult.ChatRooms);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SendMessage(string message, int roomId)
    {
        var currentUsername = GetUsername();
        var createMessageResult = await _unitOfWork.ChatRepository
            .CreateChatMessageAsync(message, currentUsername, roomId);

        await _hubContext.Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", new
        {
            createMessageResult.ChatMessage!.Content,
            createMessageResult.ChatMessage.Name,
            Timestamp = createMessageResult.ChatMessage.SendTime.ToString("MM/dd/yyyy hh:mm:ss"),
        });
        
        return Ok();
    }
}
