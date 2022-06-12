using DS.DataAccess.Context;

namespace DS.DataAccess;

public interface IDbContextSeeder
{
    void Seed(MusicDbContext context);
}