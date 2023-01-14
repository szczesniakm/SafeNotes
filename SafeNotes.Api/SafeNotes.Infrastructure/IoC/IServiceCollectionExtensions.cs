using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SafeNotes.Domain.Repositories;
using SafeNotes.Infrastructure.Emails;
using SafeNotes.Infrastructure.Repositories;

namespace SafeNotes.Infrastructure.IoC
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SafeNotesContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SafeNotesDatabase")));

            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<INotesRepository, NotesRepository>();
            services.AddTransient<IEmailSender, LogEmailSender>();

            return services;
        }
    }
}
