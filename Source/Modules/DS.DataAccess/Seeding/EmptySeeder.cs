using Microsoft.EntityFrameworkCore;

namespace DS.DataAccess.Seeding;

public class EmptySeeder : IDbContextSeeder
{
    public void Seed(ModelBuilder modelBuilder) { }
}