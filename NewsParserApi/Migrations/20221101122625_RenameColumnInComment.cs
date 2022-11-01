using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsParserApi.Migrations
{
    public partial class RenameColumnInComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_username",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "Comments",
                newName: "Username");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_username",
                table: "Comments",
                newName: "IX_Comments_Username");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_Username",
                table: "Comments",
                column: "Username",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_Username",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Comments",
                newName: "username");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_Username",
                table: "Comments",
                newName: "IX_Comments_username");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_username",
                table: "Comments",
                column: "username",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
