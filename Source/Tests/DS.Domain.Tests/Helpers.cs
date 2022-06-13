using System.Linq;
using AutoMapper;
using DS.Application.CQRS.Mapping;
using Microsoft.EntityFrameworkCore;

namespace DS.Tests;

public class Helpers
{
    public static bool EntityExistsInDatabase<T>(T entity, DbContext context) where T : class
        => context.Set<T>().Local.Any(p => p.Equals(entity));

    public static IMapper GenerateMapper()
    {
        var mapperConfig = new MapperConfiguration(c =>
        {
            c.AddProfile<DomainToResponse>();
        });
        
        return mapperConfig.CreateMapper();
    }
}