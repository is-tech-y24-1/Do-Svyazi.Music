using System.Reflection;
using DS.Application.CQRS.MusicUser.Queries;
using DS.DataAccess;
using DS.DataAccess.ContentStorages;
using DS.DataAccess.Context;
using DS.Music.WebApi.Middlewares;
using MediatR;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(GetUserInfo.GetInfoQuery).GetTypeInfo().Assembly);
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<MusicDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgresLocalhost"));
});

builder.Services.AddScoped<IMusicContext, MusicDbContext>();

var storage =
    new FileSystemStorage(builder.Configuration
        .GetSection("StorageDirectories")
        .GetValue<string>("RelativeTestDirectory"));

builder.Services.AddScoped<IContentStorage>(_ => storage);

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