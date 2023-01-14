using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafeNotes.Infrastructure.Migrations
{
    public partial class UpdateNoteEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Note",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Note");
        }
    }
}
