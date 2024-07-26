using Microsoft.EntityFrameworkCore;
using NzWalks;
using NzWalks.Data;
using NzWalkspace.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<NzWalksDbContext>(options=>
options.UseSqlServer(builder.Configuration.GetConnectionString("NzWalksConnectionString")));

builder.Services.AddScoped<IRegionRepository,SQLRegionRepository>();
builder.Services.AddScoped<IWalkRepository,SQLWalkRepository>();

//when someone call the iregion repository they will get the implementation of the InMemoryRegionRepository

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// Add controllers
builder.Services.AddControllers();

var app = builder.Build();

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
