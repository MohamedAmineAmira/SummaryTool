using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.Migrations
{
    /// <inheritdoc />
    public partial class EditRelationBetweenTextLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Logs_TextId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "DocumentCleaningDuration",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "DocumentCreationTime",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "DocumentProcessingDuration",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "ErrorTime",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "TotalDuration",
                table: "Logs");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Logs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Value",
                table: "Logs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Logs_TextId",
                table: "Logs",
                column: "TextId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Logs_TextId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Logs");

            migrationBuilder.AddColumn<string>(
                name: "DocumentCleaningDuration",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentCreationTime",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentProcessingDuration",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErrorTime",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TotalDuration",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Logs_TextId",
                table: "Logs",
                column: "TextId",
                unique: true);
        }
    }
}
