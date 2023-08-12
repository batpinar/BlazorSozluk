using BlazorSozluk.Infrastructure.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorSozluk.Infrastructure.Persistance.Extensions;

public static class Registration
{
    public static IServiceCollection AddInfrastructureRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BlazorSozlukDbContext>(conf =>
        {
            var connStr = configuration["BlazorSozlukDbConnectionString"].ToString();
            conf.UseSqlServer(connStr, opt =>
            {
                opt.EnableRetryOnFailure();
            });
        });

        //var seedData = new SeedData();
        //seedData.SeedAsync(configuration).GetAwaiter().GetResult();

        return services;
    }
}
