using System.Text;
using bookStore.BusinessLogic.Configuration;
using bookStore.BusinessLogic.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

bookStore.DataAccess.DbSession.ConnectionStrings =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? bookStore.DataAccess.DbSession.ConnectionStrings;

var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtOptions = new JwtOptions
{
    Key = jwtSection["Key"] ?? string.Empty,
    Issuer = jwtSection["Issuer"] ?? "bookStore",
    Audience = jwtSection["Audience"] ?? "bookStoreClients",
    ExpiresMinutes = int.TryParse(jwtSection["ExpiresMinutes"], out var expiresMinutes)
        ? expiresMinutes
        : 120
};

if (string.IsNullOrWhiteSpace(jwtOptions.Key) || jwtOptions.Key.Length < 32)
{
    throw new InvalidOperationException("Jwt:Key must be at least 32 characters long.");
}

JwtOptions.Configure(jwtOptions);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
        ClockSkew = TimeSpan.FromMinutes(1)
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var corsOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
if (corsOrigins == null || corsOrigins.Length == 0)
{
    corsOrigins = new[]
    {
        "http://localhost:5173",
        "http://localhost:3000",
        "http://localhost:4200"
    };
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("SpaCors", policy =>
        policy.WithOrigins(corsOrigins).AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "bookStore API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Введите JWT-токен в формате: Bearer {token}"
    });
    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", document, null),
            new List<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

AuthSeed.EnsureAdmin(builder.Configuration);

app.UseHttpsRedirection();
app.UseCors("SpaCors");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
