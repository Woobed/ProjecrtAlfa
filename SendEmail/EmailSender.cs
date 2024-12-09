
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ProjectAlfa.SendEmail
{
	public class EmailSender : IEmailSender
	{
		public Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			// Здесь вы можете использовать любой SMTP-клиент для отправки электронной почты
			// Например, использование SmtpClient или любого другого сервиса
			return Task.CompletedTask; // Замените это на вашу логику отправки
		}
	}
}