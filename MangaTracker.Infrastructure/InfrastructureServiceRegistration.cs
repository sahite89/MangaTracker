using Microsoft.Extensions.Configuration;
using MangaTracker.Application.Contracts.Infrastructure;
using MangaTracker.Application.Contracts.Persistence;
using MangaTracker.Infrastructure.Persistence;
using MangaTracker.Infrastructure.Persistence.Repositories;
using MangaTracker.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MangaTrackerDbContext>(
                options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString(
                            "MangaTrackerConnectionString")));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IPasswordHasher,PasswordHasher>();

            return services;
        }
    }
}
