using DS.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DS.DataAccess;

public interface IDbContextSeeder
{
    void Seed(MusicDbContext context);
}