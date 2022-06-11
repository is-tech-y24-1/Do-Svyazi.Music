using Microsoft.EntityFrameworkCore;

namespace DS.DataAccess;

public interface IDbContextSeeder
{
    void Seed(ModelBuilder modelBuilder);
}