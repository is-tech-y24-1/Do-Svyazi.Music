using System.Reflection;
using AutoMapper;
using DS.Application.CQRS.ListeningQueue.Commands;
using DS.Application.CQRS.ListeningQueue.Queries;
using DS.Application.CQRS.Mapping;
using DS.Application.CQRS.MediaLibrary.Commands;
using DS.Application.CQRS.MediaLibrary.Queries;
using DS.Application.CQRS.MusicUser.Commands;
using DS.Application.CQRS.Playlist.Commands;
using DS.Application.CQRS.Playlist.Queries;
using DS.Application.CQRS.Song.Commands;
using DS.Application.CQRS.Song.Queries;
using DS.Application.CQRS.Helpers;
using DS.Application.CQRS.MusicUser.Queries;
using DS.DataAccess;
using DS.DataAccess.ContentStorages;
using DS.DataAccess.Context;
using DS.Music.WebApi.Middlewares;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using Validators.MusicUser;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();

builder.Services.AddAutoMapper(typeof(DomainToResponse).GetTypeInfo().Assembly);

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("debug");

builder.Services.AddDbContext<MusicDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgresLocalhost"));
    // opt.UseLazyLoadingProxies();
});

builder.Services.AddScoped<IMusicContext, MusicDbContext>();

var storage = new FileSystemStorage(builder.Configuration
        .GetSection("StorageDirectories")
        .GetValue<string>("RelativeTestDirectory"));

builder.Services.AddFluentValidation(s => 
{ 
    s.RegisterValidatorsFromAssemblyContaining<AddMusicUserCommandValidator>();
});

builder.Services.AddScoped<IContentStorage>(_ => storage);
builder.Services.AddSingleton(new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new DomainToResponse(storage));
}).CreateMapper());

#region MediatR injectiong
// Queue commands
builder.Services.AddMediatR(typeof(AddLastToQueue.AddLastToQueueCommand).Assembly);
builder.Services.AddMediatR(typeof(AddNextToQueue.AddNextToQueueCommand).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(ChangeQueueSongPosition.ChangeQueueSongPositionCommand).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(ClearQueue.ClearQueueCommand).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(DeleteFromQueue.DeleteFromQueueCommand).GetTypeInfo().Assembly);

// Queue queries
builder.Services.AddMediatR(typeof(GetQueueInfo.GetInfoQuery).GetTypeInfo().Assembly);

// Media library command
builder.Services.AddMediatR(typeof(AddPlaylist.AddPlaylistCommand).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(AddSong.AddSongCommand).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(CreateNewPlaylist.CreateNewPlaylistCommand).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(CreateNewSong.CreateNewSongCommand).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(DeleteAuthoredPlaylist.DeleteAuthoredPlaylistCommand).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(DeleteAuthoredSong.DeleteAuthoredSongCommand).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(DeletePlaylist.DeletePlaylistCommand).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(DeleteSong.DeleteSongCommand).GetTypeInfo().Assembly);

// Media library queries
builder.Services.AddMediatR(typeof(GetAuthoredPlaylists.GetAuthoredPlaylistsQuery).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(GetAuthoredSongs.GetAuthoredSongsQuery).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(GetPlaylistCover.GetPlaylistCoverQuery).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(GetUserInfo.GetInfoQuery).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(GetPlaylists.GetPlaylistsQuery).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(GetSongs.GetSongsQuery).GetTypeInfo().Assembly);

// Music user commands
builder.Services.AddMediatR(typeof(AddMusicUser.AddUserCommand).GetTypeInfo().Assembly);

// Music user queries
builder.Services.AddMediatR(typeof(GetUserForAuthorization.GetUserQuery).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(GetUserInfo.GetInfoQuery).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(GetUserProfilePicture.GetPictureQuery).GetTypeInfo().Assembly);

// Playlist commands
builder.Services.AddMediatR(typeof(AddSongToPlaylist.AddPlaylistSongCommand).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(ChangePlaylistSongPosition.ChangePositionCommand).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(DeleteSongFromPlaylist.DeletePlaylistSongCommand).GetTypeInfo().Assembly);

// Playlist queries
builder.Services.AddMediatR(typeof(GetPlaylistInfo.GetInfoQuery).GetTypeInfo().Assembly);

// Song commands
builder.Services.AddMediatR(typeof(AddFeaturing.AddFeaturingCommand).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(RemoveFeaturing.RemoveFeaturingCommand).GetTypeInfo().Assembly);

// Song queries
builder.Services.AddMediatR(typeof(GetSongContent.GetSongContentQuery).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(GetSongCover.GetSongCoverQuery).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(GetSongInfo.GetInfoQuery).GetTypeInfo().Assembly);

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
#endregion

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