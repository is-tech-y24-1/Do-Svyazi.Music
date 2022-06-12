using Bogus;
using DS.DataAccess.Seeding.Fakers;
using DS.Domain;

MusicUser user = (new MusicUserFaker()).Generate();
Console.WriteLine(user.Id);
Console.WriteLine(user.Name);
Console.WriteLine(user.ProfilePictureUri);
Console.WriteLine(user.MediaLibrary.OwnerId);
Console.WriteLine(user.ListeningQueue.OwnerId);
var faker = new Faker();
Console.WriteLine(faker.Hacker.Noun());
