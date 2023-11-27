using EFPoC.DAL.Models;
using Microsoft.Extensions.DependencyInjection;

namespace EFPoC.DAL;
public static class DIExtensions
{
    public static IServiceCollection AddDAL(this IServiceCollection services) {
        services.AddDbContext<MyDbContext>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<UserRepository>();
        services.AddScoped<CommentRepository>();

        return services;
    }
}
