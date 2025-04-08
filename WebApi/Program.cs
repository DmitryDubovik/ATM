using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json.Serialization;
using WebApi.Data;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ATMDbContext>(options =>
    options.UseSqlite("Data Source=atm.db"));

builder.Services.AddScoped<IAccountService, AccountService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "LocalhostOrigin",
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5500") // Frontend URL
              .AllowAnyHeader()
              .AllowAnyMethod();
                      });
});

builder.Services.AddControllers()
       .AddJsonOptions(options =>
       {
           options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
       });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();



using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ATMDbContext>();
    dbContext.Database.Migrate();  // Apply migrations and create database

    DataSeeder.Seed(dbContext);
}

app.UseCors("LocalhostOrigin");
app.MapControllers();

app.Run();
