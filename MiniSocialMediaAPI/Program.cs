using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniSocialMediaAPI.Data;
using MiniSocialMediaAPI.Entities;
using MiniSocialMediaAPI.Services;
using MiniSocialMediaAPI.Utils;
using System.Text;

/**
 * ==============================================================
 * ==============================================================
 * ==============================================================
 *                          SERVICES AREA
 * ==============================================================
 * ==============================================================
 * ==============================================================
 */
var builder = WebApplication.CreateBuilder(args);

// SWAGGER SERVICE
builder.Services.AddSwaggerGen();

// SECURITY CONFIG
builder.Services.AddDataProtection();
builder.Services.AddCors(o =>
{
    o.AddDefaultPolicy(cors =>
    {
        cors.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer("name=DefaultConnection"));

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<User>>();
builder.Services.AddScoped<SignInManager<User>>();

builder.Services.AddTransient<IUserService, UserService>();


builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication().AddJwtBearer(o =>
{
    o.MapInboundClaims = false;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["keyjwt"]!)),
        ClockSkew = TimeSpan.Zero
    };
});

/**
 * Politicas de autorizacion:
 * - Admin
 */
builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("admin", p => p.RequireClaim("admin"));
});

/**
 * ==============================================================
 * ==============================================================
 * ==============================================================
 *                          MIDDLEWARES AREA
 * ==============================================================
 * ==============================================================
 * ==============================================================
 */
var app = builder.Build();

app.UseSwagger();
app.UseCors();
app.MapControllers();
app.Run();
