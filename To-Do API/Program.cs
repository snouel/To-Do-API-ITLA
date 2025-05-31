using To_Do_API.Infraestructure.Repositories;
using To_Do_API.Application.Services;
using To_Do_API.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(typeof(ITaskRepository), typeof (TaskRepository)); //Al trabajar con datos en memoria no iniciara una nueva instancia.
builder.Services.AddScoped(typeof(ITaskService), typeof(TaskService));
builder.Services.AddSingleton<TaskQueueHandler>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
