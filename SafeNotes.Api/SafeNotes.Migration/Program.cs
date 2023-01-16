using Microsoft.EntityFrameworkCore;
using SafeNotes.Infrastructure;

namespace SafeNotes.Migration
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Applying migrations");
            var webHost = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();
            using (var context = (SafeNotesContext)webHost.Services.GetService(typeof(SafeNotesContext)))
            {
                context.Database.Migrate();
            }
            Console.WriteLine("Done");
        }
    }
}