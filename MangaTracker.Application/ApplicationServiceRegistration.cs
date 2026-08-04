using MangaTracker.Application.Features.Users.LoginUser;
using MangaTracker.Application.Features.Users.RegisterUser;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<RegisterUserCommandHandler>();
            services.AddScoped<LoginUserCommandHandler>();

            return services;
        }
    }
}
