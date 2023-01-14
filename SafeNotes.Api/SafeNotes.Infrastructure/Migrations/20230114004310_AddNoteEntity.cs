using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafeNotes.Infrastructure.Migrations
{
    public partial class AddNoteEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerEmail = table.Column<string>(type: "nvarchar(320)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    IsEncryptedWithUserSpecifiedKey = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Note_User_OwnerEmail",
                        column: x => x.OwnerEmail,
                        principalTable: "User",
                        principalColumn: "Email");
                });

            migrationBuilder.CreateTable(
                name: "UserAccess",
                columns: table => new
                {
                    NoteId = table.Column<int>(type: "int", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CanRead = table.Column<bool>(type: "bit", nullable: false),
                    CanWrite = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccess", x => new { x.NoteId, x.UserEmail });
                    table.ForeignKey(
                        name: "FK_UserAccess_Note_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Note",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Note_OwnerEmail",
                table: "Note",
                column: "OwnerEmail");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccess_NoteId",
                table: "UserAccess",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccess_UserEmail",
                table: "UserAccess",
                column: "UserEmail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAccess");

            migrationBuilder.DropTable(
                name: "Note");
        }
    }
}
