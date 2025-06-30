using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Facturas_simplified.Migrations
{
  /// <inheritdoc />
  public partial class InvoiceModule : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "NcfRanges",
          columns: table => new
          {
            Id = table.Column<int>(type: "INTEGER", nullable: false)
                  .Annotation("Sqlite:Autoincrement", true),
            Description = table.Column<string>(type: "TEXT", nullable: true),
            ValidFrom = table.Column<DateTime>(type: "TEXT", nullable: false),
            ValidTo = table.Column<DateTime>(type: "TEXT", nullable: false),
            NumberMin = table.Column<int>(type: "INTEGER", nullable: false),
            NumberMax = table.Column<int>(type: "INTEGER", nullable: false),
            CurrentNumber = table.Column<int>(type: "INTEGER", nullable: false),
            NcfRangeStatus = table.Column<int>(type: "INTEGER", nullable: false),
            CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
            CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
            LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
            LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_NcfRanges", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Services",
          columns: table => new
          {
            Id = table.Column<int>(type: "INTEGER", nullable: false)
                  .Annotation("Sqlite:Autoincrement", true),
            Name = table.Column<string>(type: "TEXT", nullable: true),
            Description = table.Column<string>(type: "TEXT", nullable: false),
            Amount = table.Column<decimal>(type: "REAL", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Services", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Ncfs",
          columns: table => new
          {
            Id = table.Column<int>(type: "INTEGER", nullable: false)
                  .Annotation("Sqlite:Autoincrement", true),
            Type = table.Column<int>(type: "INTEGER", nullable: false),
            NcfNumber = table.Column<string>(type: "TEXT", nullable: false),
            AssignedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
            NcfStatus = table.Column<int>(type: "INTEGER", nullable: false),
            NcfRangeId = table.Column<int>(type: "INTEGER", nullable: false),
            InvoiceId = table.Column<int>(type: "INTEGER", nullable: false),
            CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
            CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
            LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
            LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Ncfs", x => x.Id);
            table.ForeignKey(
                      name: "FK_Ncfs_NcfRanges_NcfRangeId",
                      column: x => x.NcfRangeId,
                      principalTable: "NcfRanges",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "Invoices",
          columns: table => new
          {
            Id = table.Column<int>(type: "INTEGER", nullable: false)
                  .Annotation("Sqlite:Autoincrement", true),
            InvoiceStatus = table.Column<int>(type: "INTEGER", nullable: false),
            IssuedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
            PaidAt = table.Column<DateTime>(type: "TEXT", nullable: true),
            InvoiceType = table.Column<int>(type: "INTEGER", nullable: false),
            Subtotal = table.Column<decimal>(type: "REAL", nullable: false),
            TaxPercentage = table.Column<decimal>(type: "REAL", nullable: false),
            TaxAmount = table.Column<decimal>(type: "REAL", nullable: false),
            Total = table.Column<decimal>(type: "REAL", nullable: false),
            Note = table.Column<string>(type: "TEXT", nullable: true),
            AttentionTo = table.Column<string>(type: "TEXT", nullable: true),
            Project = table.Column<string>(type: "TEXT", nullable: true),
            ClientId = table.Column<int>(type: "INTEGER", nullable: false),
            NcfId = table.Column<int>(type: "INTEGER", nullable: false),
            CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
            CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
            LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
            LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Invoices", x => x.Id);
            table.ForeignKey(
                      name: "FK_Invoices_Clients_ClientId",
                      column: x => x.ClientId,
                      principalTable: "Clients",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_Invoices_Ncfs_NcfId",
                      column: x => x.NcfId,
                      principalTable: "Ncfs",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "InvoiceDetails",
          columns: table => new
          {
            Id = table.Column<int>(type: "INTEGER", nullable: false)
                  .Annotation("Sqlite:Autoincrement", true),
            Description = table.Column<string>(type: "TEXT", nullable: true),
            Quantity = table.Column<int>(type: "INTEGER", nullable: false),
            UnitPrice = table.Column<decimal>(type: "REAL", nullable: false),
            SubTotal = table.Column<decimal>(type: "REAL", nullable: false),
            SectionTitle = table.Column<string>(type: "TEXT", nullable: true),
            InvoiceId = table.Column<int>(type: "INTEGER", nullable: false),
            ServiceId = table.Column<int>(type: "INTEGER", nullable: false),
            CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
            CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
            LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
            LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_InvoiceDetails", x => x.Id);
            table.ForeignKey(
                      name: "FK_InvoiceDetails_Invoices_InvoiceId",
                      column: x => x.InvoiceId,
                      principalTable: "Invoices",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_InvoiceDetails_Services_ServiceId",
                      column: x => x.ServiceId,
                      principalTable: "Services",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "Payments",
          columns: table => new
          {
            Id = table.Column<int>(type: "INTEGER", nullable: false)
                  .Annotation("Sqlite:Autoincrement", true),
            PaidAt = table.Column<DateTime>(type: "TEXT", nullable: false),
            Amount = table.Column<decimal>(type: "REAL", nullable: false),
            PaymentMethod = table.Column<int>(type: "INTEGER", nullable: false),
            Reference = table.Column<string>(type: "TEXT", nullable: true),
            InvoiceId = table.Column<int>(type: "INTEGER", nullable: false),
            CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
            CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
            LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
            LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Payments", x => x.Id);
            table.ForeignKey(
                      name: "FK_Payments_Invoices_InvoiceId",
                      column: x => x.InvoiceId,
                      principalTable: "Invoices",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "IX_InvoiceDetails_InvoiceId",
          table: "InvoiceDetails",
          column: "InvoiceId");

      migrationBuilder.CreateIndex(
          name: "IX_InvoiceDetails_ServiceId",
          table: "InvoiceDetails",
          column: "ServiceId");

      migrationBuilder.CreateIndex(
          name: "IX_Invoices_ClientId",
          table: "Invoices",
          column: "ClientId");

      migrationBuilder.CreateIndex(
          name: "IX_Invoices_NcfId",
          table: "Invoices",
          column: "NcfId",
          unique: true);

      migrationBuilder.CreateIndex(
          name: "IX_Ncfs_NcfRangeId",
          table: "Ncfs",
          column: "NcfRangeId");

      migrationBuilder.CreateIndex(
          name: "IX_Payments_InvoiceId",
          table: "Payments",
          column: "InvoiceId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "InvoiceDetails");

      migrationBuilder.DropTable(
          name: "Payments");

      migrationBuilder.DropTable(
          name: "Services");

      migrationBuilder.DropTable(
          name: "Invoices");

      migrationBuilder.DropTable(
          name: "Ncfs");

      migrationBuilder.DropTable(
          name: "NcfRanges");
    }
  }
}
