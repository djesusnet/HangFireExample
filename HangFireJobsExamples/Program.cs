using System.Data;
using Hangfire;
using HangFireJobsExamples.Models;
using HangFireJobsExamples.Repositories;
using HangFireJobsExamples.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration["ConnectionStrings:SqlServer"];
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHangfire(config => config
    .UseSqlServerStorage(connectionString));

builder.Services.AddHangfireServer();
builder.Services.AddSingleton<IDbConnection>(sp => new SqlConnection(connectionString));
builder.Services.AddSingleton<IUserRepository, UserRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHangfireDashboard();

app.MapPost("usuarios/agendar", (IUserRepository userRepository, [FromBody] User user) =>
{
    BackgroundJob.Schedule(() => userRepository.UserInsert(user.Name, user.Email), TimeSpan.FromMinutes(1));
    return Results.Ok("Usuário agendado para inserção daqui a 1 minuto!");
});

app.MapPost("usuarios/enfileirar", (IUserRepository userRepository, [FromBody] User user) =>
{
    BackgroundJob.Enqueue(() => userRepository.UserInsert(user.Name, user.Email));
    return Results.Ok("Usuário enfileirado para inserção imediata!");
});

app.Run();

