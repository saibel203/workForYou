using AspNetCoreHero.ToastNotification.Abstractions;
using WorkForYou.Core.ServiceInterfaces;

namespace WorkForYou.Services;

public class NotificationService : INotificationService
{
    private readonly INotyfService _notService;

    public NotificationService(INotyfService notService)
    {
        _notService = notService;
    }

    public void CustomErrorMessage(string? message, int duration = 3)
    {
        _notService.Custom(message, duration, "#ff1c00ad", "fa-solid fa-xmark");
    }
    
    public void CustomSuccessMessage(string? message, int duration = 3)
    {
        _notService.Custom(message, duration, "#11E164E4", "fa-solid fa-check");
    }
}
