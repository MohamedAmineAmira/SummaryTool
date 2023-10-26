using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.Migrations
{
    /// <inheritdoc />
    public partial class UpdateData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdOperation",
                table: "Logs");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TextProcessors",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "OperationName",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "DataPreprocessors",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_TextProcessors_Name",
                table: "TextProcessors",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataPreprocessors_Name",
                table: "DataPreprocessors",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TextProcessors_Name",
                table: "TextProcessors");

            migrationBuilder.DropIndex(
                name: "IX_DataPreprocessors_Name",
                table: "DataPreprocessors");

            migrationBuilder.DropColumn(
                name: "OperationName",
                table: "Logs");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TextProcessors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "IdOperation",
                table: "Logs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "DataPreprocessors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
