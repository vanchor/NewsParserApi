using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsParserApi.Migrations
{
    public partial class AddedContentToNews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "News",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "News");
        }
    }
}
