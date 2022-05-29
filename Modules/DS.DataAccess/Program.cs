using DS.DataAccess;
using DS.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

var options = new DbContextOptionsBuilder<MusicDbContext>().UseSqlite(@"Data Source=D:\Do-Svyazi.Music\test.sqlite").Options;
IMusicContext test = new MusicDbContext(options);
