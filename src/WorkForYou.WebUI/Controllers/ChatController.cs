using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.Responses.Repositories;
using WorkForYou.WebUI.ViewModels;

namespace WorkForYou.WebUI.Controllers;

public class ChatController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ChatController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    

    [HttpGet]
    [Authorize(Roles = "employer")]
    public async Task<IActionResult> CreateChat(string candidateUsername)
    {
        var currentUserUsername = User.Identity?.Name!;
        var currentUserDto = new UsernameDto {Username = currentUserUsername};
        var candidateUserDto = new UsernameDto {Username = candidateUsername};

        var createResult = await _unitOfWork.ChatRepository
            .CreateChatAsync(candidateUserDto, currentUserDto);

        if (!createResult.IsSuccessfully)
        {
            return RedirectToAction("AllCandidates", "EmployerAccount");
        }

        return RedirectToAction(nameof(AllEmployerChats));
    }

    [HttpGet]
    [Authorize(Roles = "employer")]
    public async Task<IActionResult> AllEmployerChats()
    {
        var username = User.Identity?.Name!;
        var usernameDto = new UsernameDto {Username = username};

        var employerChats = await _unitOfWork.ChatRepository.GetAllEmployerChatsAsync(usernameDto);

        if (!employerChats.IsSuccessfully)
            return RedirectToAction("Index", "Main");

        return View(employerChats.EmployerChats);
    }

    [HttpGet]
    [Authorize(Roles = "candidate")]
    public async Task<IActionResult> AllCandidateChats()
    {
        var username = User.Identity?.Name!;
        var usernameDto = new UsernameDto {Username = username};

        var candidateChats = await _unitOfWork.ChatRepository.GetAllCandidateChatsAsync(usernameDto);

        if (!candidateChats.IsSuccessfully)
            return RedirectToAction("Index", "Main");

        return View(candidateChats.CandidateChats);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ChatDetails(string candidateUsername, string employerUsername)
    {
        var senderUsername = User.Identity?.Name!;
        var senderUsernameDto = new UsernameDto {Username = senderUsername};
        var senderData = await _unitOfWork.UserRepository.GetUserDataAsync(senderUsernameDto);

        string recipientUsername;
        UsernameDto recipientUsernameDto;
        UserResponse recipientData;

        int senderId, recipientId;

        ChatResponse chatData;
        ChatDetailsViewModel chatDetailsViewModel;
        
        if (User.IsInRole("employer"))
        {
            recipientUsername = candidateUsername;
            recipientUsernameDto = new UsernameDto {Username = recipientUsername};
            recipientData = await _unitOfWork.UserRepository.GetUserDataAsync(recipientUsernameDto);

            if (!recipientData.IsSuccessfully || recipientData.User.CandidateUser is null)
                return RedirectToAction(nameof(AllEmployerChats));
            
            if (!senderData.IsSuccessfully || senderData.User.EmployerUser is null)
                return RedirectToAction(nameof(AllEmployerChats));

            senderId = senderData.User.EmployerUser.EmployerUserId;
            recipientId = recipientData.User.CandidateUser.CandidateUserId;

            chatData = await _unitOfWork.ChatRepository.GetChatDataAsync(recipientId, senderId);

            chatDetailsViewModel = new()
            {
                ChatDetails = chatData.ChatRoom,
                SenderUsername = senderUsername,
                RecipientUsername = recipientUsername
            };
        }
        else
        {
            recipientUsername = employerUsername;
            recipientUsernameDto = new UsernameDto {Username = recipientUsername};
            recipientData = await _unitOfWork.UserRepository.GetUserDataAsync(recipientUsernameDto);
            
            if (!recipientData.IsSuccessfully || recipientData.User.EmployerUser is null)
                return RedirectToAction(nameof(AllCandidateChats));
            
            if (!senderData.IsSuccessfully || senderData.User.CandidateUser is null)
                return RedirectToAction(nameof(AllCandidateChats));

            senderId = senderData.User.CandidateUser.CandidateUserId;
            recipientId = recipientData.User.EmployerUser.EmployerUserId;
            
            chatData = await _unitOfWork.ChatRepository.GetChatDataAsync(senderId, recipientId);
            
            chatDetailsViewModel = new()
            {
                ChatDetails = chatData.ChatRoom,
                SenderUsername = senderUsername,
                RecipientUsername = recipientUsername
            };
        }

        return View(chatDetailsViewModel);
    }

    // [HttpPost]
    // [Authorize]
    // public async Task<IActionResult> CreateMessage(string content, int chatId)
    // {
    //     var candidateUsername = "candidate";
    //
    //     var candidate = new UsernameDto {Username = candidateUsername};
    //     var candidateId = await _unitOfWork.UserRepository.GetUserDataAsync(candidate);
    //     
    //     _unitOfWork.ChatRepository.CreateMessageAsync(content, chatId, candidateId.User.Id);
    //
    //     return RedirectToAction("ChatDetails", new { candidateUsername = "candidate", employerUsername = "employer" });
    // }

    // [HttpPost]
    // [Authorize]
    // public async Task<IActionResult> SendMessage(string content, string candidateUsername,
    //     [FromServices] IHubContext<ChatHub> chat)
    // {
    //     var senderUsername = User.Identity?.Name!;
    //     var senderUsernameDto = new UsernameDto {Username = senderUsername};
    //     var userData = await _unitOfWork.UserRepository.GetUserDataAsync(senderUsernameDto);
    //     var senderId = userData.User.EmployerUser.EmployerUserId;
    //
    //     candidateUsername = "candidate";
    //
    //     var candidate = new UsernameDto {Username = candidateUsername};
    //     var candidateId = await _unitOfWork.UserRepository.GetUserDataAsync(candidate);
    //     var t = candidateId.User.CandidateUser.CandidateUserId;
    //     
    //     var chatId = await _unitOfWork.ChatRepository.GetChatDataAsync(t, senderId);
    //     
    //     var messageResult = await _unitOfWork.ChatRepository
    //         .CreateMessageAsync(content, chatId.ChatRoom.ChatRoomId, userData.User.Id);
    //
    //     await chat.Clients.Group(senderUsername).SendAsync("ReceiveMessage", new
    //     {
    //         Content = messageResult.ChatMessage.Content
    //     });
    //
    //     return Ok();
    // }
}
