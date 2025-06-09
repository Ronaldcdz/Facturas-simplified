using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Facturas_simplified.Migrations
{
    /// <inheritdoc />
    public partial class SectorAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sector",
                table: "Clients",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sector",
                table: "Clients");
        }
    }
}
