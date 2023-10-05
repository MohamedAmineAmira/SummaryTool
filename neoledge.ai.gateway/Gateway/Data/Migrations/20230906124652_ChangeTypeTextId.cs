using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTypeTextId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Texts_TextId1",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_TextId1",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "TextId1",
                table: "Messages");

            migrationBuilder.AlterColumn<long>(
                name: "TextId",
                table: "Messages",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_TextId",
                table: "Messages",
                column: "TextId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Texts_TextId",
                table: "Messages",
                column: "TextId",
                principalTable: "Texts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Texts_TextId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_TextId",
                table: "Messages");

            migrationBuilder.AlterColumn<int>(
                name: "TextId",
                table: "Messages",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "TextId1",
                table: "Messages",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_TextId1",
                table: "Messages",
                column: "TextId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Texts_TextId1",
                table: "Messages",
                column: "TextId1",
                principalTable: "Texts",
                principalColumn: "Id");
        }
    }
}
