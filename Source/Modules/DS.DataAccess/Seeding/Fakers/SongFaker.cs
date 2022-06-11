using AutoBogus;
using DS.Common.Extensions;
using DS.Domain;

namespace DS.DataAccess.Seeding.Fakers;

public sealed class SongFaker : AutoFaker<Song>
{
    public SongFaker(MusicUser author, SongGenre genre)
    {
        author.ThrowIfNull();
        genre.ThrowIfNull();
        
        RuleFor(e => e.Id, Guid.NewGuid());
        RuleFor(e => e.Name, f => f.Lorem.Word());
        RuleFor(e => e.ContentUri, f => f.Internet.Url());
        RuleFor(e => e.CoverUri, f => f.Image.PicsumUrl());
        RuleFor(e => e.Author, author);
        RuleFor(e => e.Genre, genre);
        RuleFor(e => e.SharedForCommunity, true);
    }
}