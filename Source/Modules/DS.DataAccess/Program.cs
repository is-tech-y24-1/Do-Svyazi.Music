using DS.DataAccess.Context;
using DS.DataAccess.Seeding;
using Microsoft.EntityFrameworkCore;

DbContextOptions<MusicDbContext> options = new DbContextOptionsBuilder<MusicDbContext>().UseSqlite(@"Data Source=D:\Do-Svyazi.Music\test.sqlite").Options;
var test = new MusicDbContext(options);
var seeder = new AutoBogusSeeder();
seeder.Seed(test);
