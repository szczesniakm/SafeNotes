using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafeNotes.Domain.Entities;

namespace SafeNotes.Infrastructure.EntityTypeConfigurations
{
    internal class NoteEntityTypeConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .HasMaxLength(1024)
                .IsRequired();

            builder.Property(x => x.Content)
                .IsRequired();

            builder.Property(x => x.LastModifiedAt)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.LastModifiedAt)
                .IsRequired();

            builder.Property(x => x.LastModifiedBy)
                .IsRequired();

            builder.HasOne(x => x.Owner)
                .WithMany()
                .HasForeignKey(x => x.OwnerEmail)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => x.OwnerEmail);
        }
    }
}
