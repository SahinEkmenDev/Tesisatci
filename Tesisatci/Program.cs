using Tesisatci.Data;
using Microsoft.EntityFrameworkCore;
using Tesisatci.Services;
using Tesisatci.Models;
using System.Text.Json.Serialization; // ⭐️ cycle hatası için

var builder = WebApplication.CreateBuilder(args);

// ➡️ Veritabanı bağlantısı
builder.Services.AddDbContext<TesisatciDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ➡️ Cloudinary ayarları ve PhotoService
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<PhotoService>();

// ➡️ CORS ayarları (AllowAnyOrigin, AllowAnyHeader, AllowAnyMethod)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ➡️ Controllers + JSON cycle çözümü
builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

// ➡️ Swagger ve endpoints
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ➡️ CORS middleware (en üstte)
app.UseCors("AllowAll");

// ➡️ Development için Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
