using Supabase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using EliteAI.Infrastructure.Data;
using EliteAI.Application.Interfaces;
using EliteAI.Infrastructure.Repositories;
using System.Reflection;
using Microsoft.OpenApi.Models;
using System.Text;
using EliteAI.Application.Services;
using EliteAI.API.Services;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpClient();

// Configure CORS for Swagger UI
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EliteAI API",
        Version = "v1",
        Description = "API for EliteAI basketball training application"
    });

    // Add JWT Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Bearer",
                Name = "Authorization",
                In = ParameterLocation.Header
            },
            Array.Empty<string>()
        }
    });
});

// Add Supabase client
var supabaseUrl = builder.Configuration["Supabase:Url"] ?? 
    throw new InvalidOperationException("Supabase URL is not configured. Please add 'Supabase:Url' to your configuration.");
var supabaseAnonKey = builder.Configuration["Supabase:AnonKey"] ?? 
    throw new InvalidOperationException("Supabase Anon Key is not configured. Please add 'Supabase:AnonKey' to your configuration.");

var supabaseJwtSecret = builder.Configuration["Supabase:JwtSecret"] ?? throw new InvalidOperationException("Supabase JWT Secret is not configured. Please add 'Supabase:JwtScret' to your configuration.");

builder.Services.AddScoped<Client>(_ => new Client(supabaseUrl, supabaseAnonKey));

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {       
        options.TokenValidationParameters = new TokenValidationParameters
        { 
    
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Qofhpe8/4N1l8q5U1eu28U/qkwPLNyOgYgL0QdLO/ttdWz+49SrYjm7Z9nu2+TD85TmAqPzmbFbheFlkpkOAmw==")),
            ValidIssuer = "https://sddwxswbsezboorhimuf.supabase.co/auth/v1",            
            ValidAudience = "authenticated",        
   
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("JWT Error: " + context.Exception.Message);
                return Task.CompletedTask;
            }
        };
    });

// Configure Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// RabbitMQ Configuration
builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory
    {
        HostName = builder.Configuration["RabbitMQ:HostName"],
        UserName = builder.Configuration["RabbitMQ:UserName"],
        Password = builder.Configuration["RabbitMQ:Password"],
        Port = int.Parse(builder.Configuration["RabbitMQ:Port"] ?? "5672"),
        VirtualHost = builder.Configuration["RabbitMQ:VirtualHost"] ?? "/"
    };
    return factory.CreateConnectionAsync().Result;
});

// Register services
builder.Services.AddScoped<EliteAI.Application.Interfaces.IMessagePublisher, RabbitMQMessagePublisher>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<ISportsRepository, SportsRepository>();

builder.Services.AddScoped<OnboardingService>();
builder.Services.AddScoped<UserService>();

// Add logging
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EliteAI API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

// Enable CORS
app.UseCors();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
