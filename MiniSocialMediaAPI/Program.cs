using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniSocialMediaAPI.Data;
using MiniSocialMediaAPI.Entities;
using MiniSocialMediaAPI.Services;
using MiniSocialMediaAPI.Utils;
using System.Text;

// ==========================================
// CONFIGURACION DE SERVICIOS DE LA APLICACIÓN
// ==========================================

var builder = WebApplication.CreateBuilder(args);

// Configura Swagger para la documentación de la API
builder.Services.AddSwaggerGen();

// Configuración de seguridad y CORS
// AddDataProtection: proporciona protección de datos
builder.Services.AddDataProtection();

// CORS permite que otros dominios accedan a la API
builder.Services.AddCors(o =>
{
    o.AddDefaultPolicy(cors =>
    {
        cors.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// Registra los controladores de la aplicación
builder.Services.AddControllers();

// AutoMapper se utiliza para mapear entidades a DTOs
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Configuración de la base de datos usando SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer("name=DefaultConnection"));

// Identity Core para gestionar usuarios y autenticación
builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Registra UserManager y SignInManager para la gestión de usuarios
builder.Services.AddScoped<UserManager<User>>();
builder.Services.AddScoped<SignInManager<User>>();

// Registra el servicio de usuario
builder.Services.AddTransient<IUserService, UserService>();

// Acceso al contexto HTTP para obtener información de la solicitud actual
builder.Services.AddHttpContextAccessor();

// Configuración de autenticación JWT
builder.Services.AddAuthentication().AddJwtBearer(o =>
{
    o.MapInboundClaims = false;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        // Clave secreta para firmar los tokens JWT
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["keyjwt"]!)),
        ClockSkew = TimeSpan.Zero
    };
});

// Políticas de autorización
builder.Services.AddAuthorization(o =>
{
    // Solo usuarios con el claim "admin" pueden acceder a recursos protegidos por esta política
    o.AddPolicy("admin", p => p.RequireClaim("admin"));
});

// ==========================================
// CONFIGURACION DE MIDDLEWARES
// ==========================================

var app = builder.Build();

// Habilita la documentación interactiva de Swagger
app.UseSwagger();

// Aplica la política CORS configurada
app.UseCors();

// Mapea las rutas de los controladores
app.MapControllers();

// Inicia la aplicación
app.Run();
