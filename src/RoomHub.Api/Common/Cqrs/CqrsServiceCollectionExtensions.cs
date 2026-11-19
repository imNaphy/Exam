using System.Reflection;

namespace RoomHub.Api.Common.Cqrs;

public static class CqrsServiceCollectionExtensions
{
    public static IServiceCollection AddHandlers(this IServiceCollection services, Assembly assembly)
    {
        var handlerInterfaceType = typeof(ICommandHandler<,>);

        var handlerTypes = assembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false });

        foreach (var implementationType in handlerTypes)
        {
            var serviceInterfaces = implementationType
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType);

            foreach (var serviceInterface in serviceInterfaces)
            {
                services.AddScoped(serviceInterface, implementationType);
            }
        }

        return services;
    }
}
