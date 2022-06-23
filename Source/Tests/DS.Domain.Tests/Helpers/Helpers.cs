using System.Linq;
using AutoMapper;
using DS.Application.CQRS.Mapping;
using Microsoft.EntityFrameworkCore;

namespace DS.Tests.Helpers;

public static class Helpers
{
    public struct Constants
    {
        public static int SingleEntity = 1;
    }

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