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
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseRouting();

// 🔥 CORS middleware'i burada kullan
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// 🌟 PORT ayarı: Render gibi ortamlar için gerekli
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://*:{port}");

app.Run();
