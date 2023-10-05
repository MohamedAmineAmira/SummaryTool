using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTextTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Etat",
                table: "Texts");

            migrationBuilder.RenameColumn(
                name: "TexteTraiter",
                table: "Texts",
                newName: "ProcessText");

            migrationBuilder.RenameColumn(
                name: "TextePreparer",
                table: "Texts",
                newName: "PrepareText");

            migrationBuilder.RenameColumn(
                name: "TexteBrute",
                table: "Texts",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "Priorite",
                table: "Texts",
                newName: "Priority");

            migrationBuilder.AddColumn<string>(
                name: "PlainText",
                table: "Texts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlainText",
                table: "Texts");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Texts",
                newName: "TexteBrute");

            migrationBuilder.RenameColumn(
                name: "ProcessText",
                table: "Texts",
                newName: "TexteTraiter");

            migrationBuilder.RenameColumn(
                name: "Priority",
                table: "Texts",
                newName: "Priorite");

            migrationBuilder.RenameColumn(
                name: "PrepareText",
                table: "Texts",
                newName: "TextePreparer");

            migrationBuilder.AddColumn<int>(
                name: "Etat",
                table: "Texts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
