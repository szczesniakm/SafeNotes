using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafeNotes.Infrastructure.Migrations
{
    public partial class AddSaltToNote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Note",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Note");
        }
    }
}
