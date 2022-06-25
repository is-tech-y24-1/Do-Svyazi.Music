using System.Reflection;
using DS.Application.CQRS.Helpers;
using DS.Application.CQRS.MusicUser.Commands;
using DS.Application.CQRS.MusicUser.Queries;
using DS.DataAccess;
using DS.DataAccess.ContentStorages;
using DS.DataAccess.Context;
using DS.Music.WebApi.Middlewares;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using Validators.Song;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("debug");

builder.Services.AddDbContext<MusicDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgresLocalhost"));
});

builder.Services.AddScoped<IMusicContext, MusicDbContext>();

var storage = new FileSystemStorage(builder.Configuration
        .GetSection("StorageDirectories")
        .GetValue<string>("RelativeTestDirectory"));

builder.Services.AddScoped<IContentStorage>(_ => storage);
builder.Services.AddValidatorsFromAssembly(typeof(CreateNewSongCommandValidator).Assembly);

builder.Services.AddMediatR(typeof(GetUserInfo).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(AddMusicUser).GetTypeInfo().Assembly);
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

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