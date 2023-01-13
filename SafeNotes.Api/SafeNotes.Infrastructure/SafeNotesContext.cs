using Microsoft.EntityFrameworkCore;

namespace SafeNotes.Infrastructure
{
    public class SafeNotesContext : DbContext
    {
        public SafeNotesContext(DbContextOptions<SafeNotesContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SafeNotesContext).Assembly);
        }
    }
}
