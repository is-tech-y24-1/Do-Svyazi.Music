using Bogus;
using DS.DataAccess;
using DS.DataAccess.Context;
using DS.DataAccess.Seeding;
using DS.DataAccess.Seeding.Fakers;
using DS.Domain;
using Microsoft.EntityFrameworkCore;

var options = new DbContextOptionsBuilder<MusicDbContext>().UseSqlite(@"Data Source=D:\Do-Svyazi.Music\test.sqlite").Options;
IMusicContext test = new MusicDbContext(options);

