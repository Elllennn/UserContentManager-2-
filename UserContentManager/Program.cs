using UserContentManager.Repositories;
using UserContentManager.Service;
using Serilog;
using UserContentManager.Contracts;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IPostRepository, PostRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();

builder.Services.AddHttpClient();
builder.Services.AddScoped<IPostService,PostService>();

builder.Services.AddScoped<IUserService,UserService>();


builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.WriteTo.File(
        path: $"logs/log-{DateTime.Now:yyyy-ww}.txt", 
        rollingInterval: RollingInterval.Day);
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
