using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTableMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Treatment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TextId = table.Column<int>(type: "int", nullable: false),
                    TextId1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Texts_TextId1",
                        column: x => x.TextId1,
                        principalTable: "Texts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_TextId1",
                table: "Messages",
                column: "TextId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}
