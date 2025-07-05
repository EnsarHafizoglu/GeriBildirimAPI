using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/geribildirim")]
[ApiController]
public class GeriBildirimController : ControllerBase
{
    private readonly MailService _mailService;

    public GeriBildirimController(MailService mailService)
    {
        _mailService = mailService;
    }

    [HttpPost("gonder")]
    public async Task<IActionResult> GeriBildirimGonder([FromBody] GeriBildirimModel model)
    {
        if (string.IsNullOrWhiteSpace(model.AdSoyad) || string.IsNullOrWhiteSpace(model.Mesaj))
        {
            return BadRequest("Ad Soyad ve Mesaj boþ býrakýlamaz.");
        }

        await _mailService.SendMailAsync(model.AdSoyad,model.Email, model.Mesaj);
        return Ok("Geri bildiriminiz baþarýyla gönderildi.");
    }
}

public class GeriBildirimModel
{
    public string AdSoyad { get; set; }
    public string Email { get; set; }  // yeni
    public string Mesaj { get; set; }
}
