using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Facturas_simplified.Migrations
{
    /// <inheritdoc />
    public partial class ProvinceIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Provinces_ProvinceId",
                table: "Clients");

            migrationBuilder.AlterColumn<int>(
                name: "ProvinceId",
                table: "Clients",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Provinces_ProvinceId",
                table: "Clients",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Provinces_ProvinceId",
                table: "Clients");

            migrationBuilder.AlterColumn<int>(
                name: "ProvinceId",
                table: "Clients",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Provinces_ProvinceId",
                table: "Clients",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
