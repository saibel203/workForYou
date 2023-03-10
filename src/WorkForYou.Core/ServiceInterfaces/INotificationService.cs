namespace WorkForYou.Core.ServiceInterfaces;

public interface INotificationService
{
    void CustomErrorMessage(string? message, int duration = 2);
    void CustomSuccessMessage(string? message, int duration = 2);
}
