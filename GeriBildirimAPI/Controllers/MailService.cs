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
                    Subject = "Yeni Geri Bildirim Al�nd�!",
                    Body = $"Ad Soyad: {adSoyad}\nEmail: {email}\n\nMesaj:\n{mesaj}",
                    IsBodyHtml = false
                };

                mailMessage.To.Add(smtpSettings["SenderEmail"]); // Mail kendine gidecek

                Console.WriteLine("? Mail g�nderilmeye �al���l�yor...");
                await client.SendMailAsync(mailMessage);
                Console.WriteLine("? Mail ba�ar�yla g�nderildi.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("? Mail g�nderme hatas�: " + ex.Message);
            if (ex.InnerException != null)
            {
                Console.WriteLine("?? �� hata: " + ex.InnerException.Message);
            }
            throw; // iste�e ba�l�
        }
    }

}
