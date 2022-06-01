using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DS.Application.CQRS;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMediatR(typeof(Startup));
    }
}