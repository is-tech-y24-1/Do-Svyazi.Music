using AutoBogus;
using DS.Common.Extensions;
using DS.Domain;

namespace DS.DataAccess.Seeding.Fakers;

public sealed class PlaylistFaker : AutoFaker<Playlist>
{
    public PlaylistFaker(MusicUser author)
    {
        author.ThrowIfNull();
        StrictMode(true);
        
        RuleFor(playlist => playlist.Id, Guid.NewGuid);
        RuleFor(playlist => playlist.Name, faker => faker.Lorem.Word());
        RuleFor(playlist => playlist.Author, author);
        RuleFor(playlist => playlist.SharedForCommunity, true);
        RuleFor(playlist => playlist.CoverUri, faker => faker.Image.PicsumUrl());
        RuleFor(playlist => playlist.Description, faker => faker.Lorem.Paragraph());
    }
}