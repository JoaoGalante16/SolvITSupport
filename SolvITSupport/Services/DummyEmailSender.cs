using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace SolvITSupport.Services
{
    // Esta é a nossa implementação "falsa" do IEmailSender.
    // Ela não faz nada, apenas cumpre o contrato da interface para que a aplicação não dê erro.
    public class DummyEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Em produção, este seria o código para se ligar a um serviço como SendGrid ou um servidor SMTP.
            // Para o desenvolvimento, simplesmente retornamos uma tarefa completa.
            return Task.CompletedTask;
        }
    }
}