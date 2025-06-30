using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Facturas_simplified.Migrations
{
    /// <inheritdoc />
    public partial class ServiceChangedToProductAndDoubleToDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetails_Services_ServiceId",
                table: "InvoiceDetails");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "InvoiceDetails",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceDetails_ServiceId",
                table: "InvoiceDetails",
                newName: "IX_InvoiceDetails_ProductId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Payments",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "REAL");

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "Invoices",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "REAL");

            migrationBuilder.AlterColumn<decimal>(
                name: "TaxPercentage",
                table: "Invoices",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "REAL");

            migrationBuilder.AlterColumn<decimal>(
                name: "TaxAmount",
                table: "Invoices",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "REAL");

            migrationBuilder.AlterColumn<decimal>(
                name: "Subtotal",
                table: "Invoices",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "REAL");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "InvoiceDetails",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "REAL");

            migrationBuilder.AlterColumn<decimal>(
                name: "SubTotal",
                table: "InvoiceDetails",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "REAL");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetails_Products_ProductId",
                table: "InvoiceDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetails_Products_ProductId",
                table: "InvoiceDetails");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "InvoiceDetails",
                newName: "ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceDetails_ProductId",
                table: "InvoiceDetails",
                newName: "IX_InvoiceDetails_ServiceId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Payments",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "Invoices",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<decimal>(
                name: "TaxPercentage",
                table: "Invoices",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<decimal>(
                name: "TaxAmount",
                table: "Invoices",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<decimal>(
                name: "Subtotal",
                table: "Invoices",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "InvoiceDetails",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<decimal>(
                name: "SubTotal",
                table: "InvoiceDetails",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Amount = table.Column<decimal>(type: "REAL", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetails_Services_ServiceId",
                table: "InvoiceDetails",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
