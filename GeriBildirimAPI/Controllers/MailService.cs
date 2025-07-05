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

            using (var client = new SmtpClient(smtpSettings["Server"], int.Parse(smtpSettings["Port"])))
            {
                client.Credentials = new NetworkCredential(smtpSettings["SenderEmail"], smtpSettings["Password"]);
                client.EnableSsl = bool.Parse(smtpSettings["EnableSSL"]);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpSettings["SenderEmail"], smtpSettings["SenderName"]),
                    Subject = "Yeni Geri Bildirim Alýndý!",
                    Body = $"Ad Soyad: {adSoyad}\nEmail: {email}\n\nMesaj:\n{mesaj}",
                    IsBodyHtml = false
                };

                mailMessage.To.Add(smtpSettings["SenderEmail"]); // Mail kendine gidecek

                Console.WriteLine("? Mail gönderilmeye çalýþýlýyor...");
                await client.SendMailAsync(mailMessage);
                Console.WriteLine("? Mail baþarýyla gönderildi.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("? Mail gönderme hatasý: " + ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine("?? Ýç hata: " + ex.InnerException.Message);
            }
            throw; // isteðe baðlý
        }
    }

}
