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

            // ? ENV’den alýyoruz (Render > Environment Variables ayarlandý!)
            var senderEmail = Environment.GetEnvironmentVariable("SENDER_EMAIL");
            var senderPassword = Environment.GetEnvironmentVariable("SENDER_PASSWORD");

            using (var client = new SmtpClient(server, port))
            {
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);
                client.EnableSsl = bool.Parse(smtpSettings["EnableSSL"]);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, senderName),
                    Subject = "Yeni Geri Bildirim Alýndý!",
                    Body = $"Ad Soyad: {adSoyad}\nEmail: {email}\n\nMesaj:\n{mesaj}",
                    IsBodyHtml = false
                };

                mailMessage.To.Add(senderEmail);

                Console.WriteLine("?? Mail gönderilmeye çalýþýlýyor...");
                await client.SendMailAsync(mailMessage);
                Console.WriteLine("? Mail baþarýyla gönderildi.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("? Mail gönderme hatasý: " + ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine("? Ýç hata: " + ex.InnerException.Message);
            }
            throw;
        }
    }
}
