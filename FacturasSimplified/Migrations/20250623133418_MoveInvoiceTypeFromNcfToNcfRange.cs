using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Facturas_simplified.Migrations
{
    /// <inheritdoc />
    public partial class MoveInvoiceTypeFromNcfToNcfRange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Ncfs");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "NcfRanges",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "NcfRanges");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Ncfs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
