using Microsoft.EntityFrameworkCore;
using NzWalks.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using NzWalks;
using Microsoft.OpenApi.Models;
using NzWalks.Services.Registration;
using NzWalks.Services.Verification;
using NzWalks.Services.Authentication;
using NzWalks.Service.Attract;
using NzWalks.Services.Attractions;
using NzWalks.Services.ContactService;
using NzWalks.Models.Domain;
using NzWalks.Service.Blob;
using NzWalks.Services.Comments;
// using NzWalks.Service.Attract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "NzWalks API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// Configure DbContexts
builder.Services.AddDbContext<NzWalksDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NzWalksConnectionString")));


// Register repositories

builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IVerificationService, VerificationService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAttractionService, AttractionService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IBlobService, BlobService>();
builder.Services.AddScoped<ICommentService, CommentService>();



// builder.Services.AddIdentityCore<IdentityUser>()
//     .AddRoles<IdentityRole>()
//     .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NzWalks")
//     .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});


builder.Services.AddControllers();

// Configure authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Add CORS support
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "https://frontiend-mauve.vercel.app",
                "https://frontiend-rdunec4d7-himanshus-projects-4401a1c4.vercel.app",
                "http://127.0.0.1:5500",
                "http://127.0.0.1:5501"
            )
            .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE", "OPTIONS")
            .AllowAnyHeader();
            // add this only if you need cookies/Authorization cross-site:
            // .AllowCredentials();
    });
});
var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowFrontend");
app.UseHttpsRedirection(); // Re-enable HTTPS redirection
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
