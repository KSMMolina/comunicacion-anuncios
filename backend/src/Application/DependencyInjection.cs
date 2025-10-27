using Communication.Announcements.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Communication.Announcements.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        // Reemplazo de AddValidatorsFromAssembly por registro manual al no estar disponible el método de extensión.
        foreach (var validatorType in Assembly.GetExecutingAssembly().DefinedTypes
                     .Where(t => !t.IsAbstract && !t.IsInterface))
        {
            var interfaces = validatorType.ImplementedInterfaces
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>));

            foreach (var @interface in interfaces)
            {
                services.AddTransient(@interface, validatorType.AsType());
            }
        }

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
