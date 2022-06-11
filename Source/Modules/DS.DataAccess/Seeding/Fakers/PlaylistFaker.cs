using AutoBogus;
using DS.Common.Extensions;
using DS.Domain;

namespace DS.DataAccess.Seeding.Fakers;

public sealed class PlaylistFaker : AutoFaker<Playlist>
{
    public PlaylistFaker(MusicUser author, PlaylistSongs songs)
    {
        author.ThrowIfNull();
        songs.ThrowIfNull();
        
        RuleFor(e => e.Id, Guid.NewGuid);
        RuleFor(e => e.Name, f => f.Lorem.Word());
        RuleFor(e => e.Author, author);
        RuleFor(e => e.Songs, songs);
        RuleFor(e => e.SharedForCommunity, true);
        RuleFor(e => e.CoverUri, f => f.Image.PicsumUrl());
        RuleFor(e => e.Description, f => f.Lorem.Paragraph());
    }
}