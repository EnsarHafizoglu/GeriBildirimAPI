using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

public class MailService
{
    private readonly IConfiguration _configuration;

    public MailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendMailAsync(string adSoyad, string email, string mesaj)
    {
        try
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");

            var server = smtpSettings["Server"];
            var port = int.Parse(smtpSettings["Port"]);
            var senderName = smtpSettings["SenderName"];
            var senderEmail = smtpSettings["SenderEmail"];
            var senderPassword = smtpSettings["Password"];
            var enableSsl = bool.Parse(smtpSettings["EnableSSL"]);

            using (var client = new SmtpClient(server, port))
            {
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);
                client.EnableSsl = enableSsl;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, senderName),
                    Subject = "Yeni Geri Bildirim Alındı!",
                    Body = $"Ad Soyad: {adSoyad}\nEmail: {email}\n\nMesaj:\n{mesaj}",
                    IsBodyHtml = false
                };

                mailMessage.To.Add(senderEmail);

                Console.WriteLine("📧 Mail gönderilmeye çalışılıyor...");
                await client.SendMailAsync(mailMessage);
                Console.WriteLine("✅ Mail başarıyla gönderildi.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Mail gönderme hatası: " + ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine("⚠ İç hata: " + ex.InnerException.Message);
            }
            throw;
        }
    }
}
