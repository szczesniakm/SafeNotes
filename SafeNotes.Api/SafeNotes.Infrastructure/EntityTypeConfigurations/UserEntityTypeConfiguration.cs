using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafeNotes.Domain.Entities;

namespace SafeNotes.Infrastructure.EntityTypeConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Email);

            builder.Property(x => x.Email)
                .HasMaxLength(320)
                .IsRequired();

            builder.Property(x => x.PasswordHash)
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(x => x.SecurityCode)
                .HasMaxLength(64)
                .IsRequired(false);
        }
    }
}
