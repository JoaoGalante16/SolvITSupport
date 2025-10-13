namespace SolvITSupport.Services
{
    public interface INotificationService
    {
        Task NotifyAttendantsAsync(int ticketId, string title, string message);
        Task NotifyUserAsync(string userId, int? ticketId, string title, string message);
    }
}