using BlazorSozluk.Api.Applicaition.Interfaces.Repositories;
using BlazorSozluk.Infrastructure.Persistance.Context;
using BlazorSozluk.Infrastructure.Persistance.Repositories;
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

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEntryRepository, EntryRepository>();
        services.AddScoped<IEntryCommentRepository, EntryCommentRepository>();
        services.AddScoped<IEmailConfirmationRepository, EmailConfirmationRepository>();

        return services;
    }
}
