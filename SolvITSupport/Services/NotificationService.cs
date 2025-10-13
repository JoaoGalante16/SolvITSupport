namespace SolvITSupport.Services
{
    public class NotificationService : INotificationService
    {
        public Task NotifyAttendantsAsync(int ticketId, string title, string message)
        {
            // A lógica para enviar notificações virá mais tarde
            return Task.CompletedTask;
        }

        public Task NotifyUserAsync(string userId, int? ticketId, string title, string message)
        {
            // A lógica para enviar notificações virá mais tarde
            return Task.CompletedTask;
        }
    }
}