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

            // ? ENV�den al�yoruz (Render > Environment Variables ayarland�!)
            var senderEmail = Environment.GetEnvironmentVariable("SENDER_EMAIL");
            var senderPassword = Environment.GetEnvironmentVariable("SENDER_PASSWORD");

            using (var client = new SmtpClient(server, port))
            {
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);
                client.EnableSsl = bool.Parse(smtpSettings["EnableSSL"]);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, senderName),
                    Subject = "Yeni Geri Bildirim Al�nd�!",
                    Body = $"Ad Soyad: {adSoyad}\nEmail: {email}\n\nMesaj:\n{mesaj}",
                    IsBodyHtml = false
                };

                mailMessage.To.Add(senderEmail);

                Console.WriteLine("?? Mail g�nderilmeye �al���l�yor...");
                await client.SendMailAsync(mailMessage);
                Console.WriteLine("? Mail ba�ar�yla g�nderildi.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("? Mail g�nderme hatas�: " + ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine("? �� hata: " + ex.InnerException.Message);
            }
            throw;
        }
    }
}
