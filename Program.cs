using Microsoft.EntityFrameworkCore;
using NzWalks.Data;
using NzWalkspace.Mapping;
using AutoMapper;
using NzWalks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<NzWalksDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NzWalksConnectionString")));

builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();
builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// Add controllers
builder.Services.AddControllers();

var app = builder.Build();
// Validate AutoMapper configuration
// Validate AutoMapper configuration
var mapper = app.Services.GetRequiredService<IMapper>();
mapper.ConfigurationProvider.AssertConfigurationIsValid();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();



// Map controllers
app.MapControllers();

app.Run();
