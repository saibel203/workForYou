namespace WorkForYou.Core.IServices;

public interface INotificationService
{
    void CustomErrorMessage(string? message, int duration = 2);
    void CustomSuccessMessage(string? message, int duration = 2);
}
