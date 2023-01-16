using Microsoft.EntityFrameworkCore;
using SafeNotes.Infrastructure;

namespace SafeNotes.Migration
{
    public class Startup
    {
        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SafeNotesContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SafeNotesDatabase"));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }
    }
}
