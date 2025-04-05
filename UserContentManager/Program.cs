using UserContentManager.Repositories;
using UserContentManager.Service;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IPostRepository, PostRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();

builder.Services.AddHttpClient();
builder.Services.AddScoped<PostService>();

builder.Services.AddScoped<UserService>();



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
