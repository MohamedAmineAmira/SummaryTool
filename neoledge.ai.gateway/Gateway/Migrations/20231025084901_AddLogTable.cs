using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.Migrations
{
    /// <inheritdoc />
    public partial class AddLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentCreationTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentCleaningDuration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentProcessingDuration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalDuration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Log_Texts_TextId",
                        column: x => x.TextId,
                        principalTable: "Texts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Log_TextId",
                table: "Log",
                column: "TextId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Log");
        }
    }
}
