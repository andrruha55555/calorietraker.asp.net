using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using calorietraker.Context;

var builder = WebApplication.CreateBuilder(args);

// Подключение контроллеров и Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Загрузка конфигурации JWT
var jwtSection = builder.Configuration.GetSection("Jwt");
var issuer = jwtSection["Issuer"] ?? "CalorieTrackerIssuer";
var audience = jwtSection["Audience"] ?? "CalorieTrackerAudience";
var key = jwtSection["Key"];

// Проверка ключа на null
if (string.IsNullOrWhiteSpace(key))
{
    throw new InvalidOperationException("⚠ Ошибка: В appsettings.json отсутствует ключ Jwt:Key");
}

// Добавление аутентификации
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

// Авторизация
builder.Services.AddAuthorization();

var app = builder.Build();

// Включение Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

// Подключение JWT
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();