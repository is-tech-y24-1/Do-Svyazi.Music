using System.Reflection;
using DS.DataAccess;
using DS.DataAccess.Context;
using DS.Music.WebApi.Middlewares;
using MediatR;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<MusicDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("SqliteTest"));
});

builder.Services.AddScoped<IMusicContext, MusicDbContext>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();