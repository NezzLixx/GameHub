using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace GameHub.Infrastructure.Services;

// Сервіс для роботи з мережевими протоколами SMTP (відправка Email через MailKit)
public class EmailService
{
    // Дані авторизації поштового сервера
    private const string SenderEmail = "n.nezzlixx@gmail.com";
    private const string AppPassword = "phwjmzxyoltuqgqc"; // Секретний пароль додатків Google
    
    // Асинхронний метод генерації та надсилання привітального HTML-листа
    public async Task SendWelcomeEmailAsync(string targetEmail, string username)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("GameHub Team", SenderEmail));
        message.To.Add(new MailboxAddress(username, targetEmail));
        message.Subject = "Ласкаво просимо до GameHub! 🎮";

        // Формування HTML структури листа із CSS стилями
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $@"
                <div style='font-family: Arial, sans-serif; background-color: #1b2838; color: #ffffff; padding: 30px; border-radius: 8px; max-width: 500px; margin: 0 auto;'>
                    <h1 style='color: #66c0f4; text-align: center; border-bottom: 2px solid #2a475e; padding-bottom: 10px;'>Вітаємо в клубі, {username}!</h1>
                    <p style='font-size: 16px; line-height: 1.5; color: #c7d5e0;'>
                        Ваш акаунт у <strong>GameHub</strong> успішно створено. Тепер перед вами відкритий весь наш каталог ігор та можливість збирати власну цифрову бібліотеку!
                    </p>
                    <div style='background-color: #171a21; padding: 15px; border-radius: 4px; margin: 20px 0; text-align: center;'>
                        <span style='color: #a3cf06; font-weight: bold; font-size: 18px;'>Реєстрацію підтверджено ✅</span>
                    </div>
                    <p style='font-size: 12px; color: #626366; text-align: center; margin-top: 30px;'>
                        Цей лист згенеровано автоматично десктопним додатком GameHub.
                    </p>
                </div>"
        };
        message.Body = bodyBuilder.ToMessageBody();

        // Мережеве підключення до сервера Google та безпосередня відправка
        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);            
            await client.AuthenticateAsync(SenderEmail, AppPassword);
            
            await client.SendAsync(message);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка MailKit: {ex.Message}");
        }
        finally
        {
            // Обов'язкове закриття SMTP сесії
            await client.DisconnectAsync(true);
        }
    }
}