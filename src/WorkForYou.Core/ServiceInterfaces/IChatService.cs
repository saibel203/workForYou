﻿using WorkForYou.Core.Responses.Services;

namespace WorkForYou.Core.ServiceInterfaces;

public interface IChatService
{
    Task<ChatResponse> IsChatExistsAsync(string chatName);
}