using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// 🌟 ENVIRONMENT VARIABLE'ları da oku
builder.Configuration.AddEnvironmentVariables();

// MailService'i ekleyelim
builder.Services.AddSingleton<MailService>();

// Controller'ları ekleyelim
builder.Services.AddControllers();

// 🔥 CORS politikası tanımla
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "https://inquisitive-brioche-f5d22b.netlify.app" 
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// 🚨 ÖNEMLİ: CORS middleware sırası
app.UseRouting();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

// 🌟 PORT ayarı: Render gibi ortamlar için gerekli
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://*:{port}");

app.Run();
