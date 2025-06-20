using To_Do_API.Infraestructure.Repositories;
using To_Do_API.Application.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using To_Do_API.Domain.Interfaces.Auth;
using To_Do_API.Domain.Interfaces.TodoTasks;
using To_Do_API.Domain.Interfaces.Users;
using To_Do_API.Insfraestructure.Repositories;
using To_Do_API.Hubs;
using To_Do_API.Application.Settings;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(typeof(ITaskRepository), typeof (TaskRepository)); //Al trabajar con datos en memoria no iniciara una nueva instancia.
builder.Services.AddSingleton(typeof(ITaskService), typeof(TaskService));
builder.Services.AddSingleton<TaskQueueHandler>();
builder.Services.AddSingleton(typeof(IUserRepository), typeof(UserRepository));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSignalR();


var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
     {
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = true,
             ValidateAudience = true,
             ValidateLifetime = true,
             ValidateIssuerSigningKey = true,
             ValidIssuer = jwtSettings.Issuer,
             ValidAudience = jwtSettings.Audience,
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
         };
     }
    );

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.MapHub<TasksHub>("/taskHub");

app.Run();
