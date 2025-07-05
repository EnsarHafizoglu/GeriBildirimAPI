using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

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

app.Run();
