// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SafeNotes.Infrastructure;

#nullable disable

namespace SafeNotes.Infrastructure.Migrations
{
    [DbContext(typeof(SafeNotesContext))]
    [Migration("20230114004310_AddNoteEntity")]
    partial class AddNoteEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SafeNotes.Domain.Entities.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsEncryptedWithUserSpecifiedKey")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(320)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerEmail");

                    b.ToTable("Note");
                });

            modelBuilder.Entity("SafeNotes.Domain.Entities.User", b =>
                {
                    b.Property<string>("Email")
                        .HasMaxLength(320)
                        .HasColumnType("nvarchar(320)");

                    b.Property<bool>("IsEmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("SecurityCode")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<DateTime>("SecurityCodeExpirationDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Email");

                    b.ToTable("User");
                });

            modelBuilder.Entity("SafeNotes.Domain.Entities.UserAccess", b =>
                {
                    b.Property<int>("NoteId")
                        .HasColumnType("int");

                    b.Property<string>("UserEmail")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("CanRead")
                        .HasColumnType("bit");

                    b.Property<bool>("CanWrite")
                        .HasColumnType("bit");

                    b.HasKey("NoteId", "UserEmail");

                    b.HasIndex("NoteId");

                    b.HasIndex("UserEmail");

                    b.ToTable("UserAccess");
                });

            modelBuilder.Entity("SafeNotes.Domain.Entities.Note", b =>
                {
                    b.HasOne("SafeNotes.Domain.Entities.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerEmail")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("SafeNotes.Domain.Entities.UserAccess", b =>
                {
                    b.HasOne("SafeNotes.Domain.Entities.Note", "Note")
                        .WithMany("AllowedUsers")
                        .HasForeignKey("NoteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Note");
                });

            modelBuilder.Entity("SafeNotes.Domain.Entities.Note", b =>
                {
                    b.Navigation("AllowedUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
