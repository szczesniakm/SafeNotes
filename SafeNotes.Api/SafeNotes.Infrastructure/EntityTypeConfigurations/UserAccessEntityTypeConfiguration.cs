using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafeNotes.Domain.Entities;

namespace SafeNotes.Infrastructure.EntityTypeConfigurations
{
    internal class UserAccessEntityTypeConfiguration : IEntityTypeConfiguration<UserAccess>
    {
        public void Configure(EntityTypeBuilder<UserAccess> builder)
        {
            builder.HasKey(x => new { x.NoteId, x.UserEmail });

            builder.HasOne(x => x.Note)
                .WithMany(x => x.AllowedUsers)
                .HasForeignKey(x => x.NoteId);

            builder.HasIndex(x => x.UserEmail);

            builder.HasIndex(x => x.NoteId);
        }
    }
}
