using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Tesisatci.Migrations
{
    /// <inheritdoc />
    public partial class yeni : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "DeliveredWorks");

            migrationBuilder.CreateTable(
                name: "DeliveredWorkImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Url = table.Column<string>(type: "text", nullable: false),
                    DeliveredWorkId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveredWorkImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveredWorkImages_DeliveredWorks_DeliveredWorkId",
                        column: x => x.DeliveredWorkId,
                        principalTable: "DeliveredWorks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveredWorkImages_DeliveredWorkId",
                table: "DeliveredWorkImages",
                column: "DeliveredWorkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveredWorkImages");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "DeliveredWorks",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
