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
        
        RuleFor(song => song.Id, Guid.NewGuid());
        RuleFor(song => song.Name, faker => faker.Lorem.Word());
        RuleFor(song => song.ContentUri, faker => faker.Internet.Url());
        RuleFor(song => song.CoverUri, faker => faker.Image.PicsumUrl());
        RuleFor(song => song.Author, author);
        RuleFor(song => song.Genre, genre);
        RuleFor(song => song.SharedForCommunity, true);
    }
}