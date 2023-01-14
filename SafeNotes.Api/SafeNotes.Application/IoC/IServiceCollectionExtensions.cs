using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SafeNotes.Application.Authorization;
using SafeNotes.Application.Services;
using SafeNotes.Application.Settings;
using SafeNotes.Application.Utils;

namespace SafeNotes.Application.IoC
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddTransient<JwtTokenGenerator>();
            
            services.AddTransient<IUserIdentityProvider, UserIdentityProvider>();
            services.AddTransient<UserService>();
            services.AddTransient<AuthService>();
            services.AddTransient<NoteService>();
            services.AddValidatorsFromAssemblyContaining<UserService>();


            return services;
        }
    }
}
