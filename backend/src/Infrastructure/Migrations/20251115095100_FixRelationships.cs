using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrderLines_Items_ItemId",
                table: "SalesOrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrderLines_Items_ItemId1",
                table: "SalesOrderLines");

            migrationBuilder.DropIndex(
                name: "IX_SalesOrderLines_ItemId1",
                table: "SalesOrderLines");

            migrationBuilder.DropColumn(
                name: "ItemId1",
                table: "SalesOrderLines");

            migrationBuilder.AlterColumn<decimal>(
                name: "TaxRate",
                table: "SalesOrderLines",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrderLines_Items_ItemId",
                table: "SalesOrderLines",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrderLines_Items_ItemId",
                table: "SalesOrderLines");

            migrationBuilder.AlterColumn<decimal>(
                name: "TaxRate",
                table: "SalesOrderLines",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2);

            migrationBuilder.AddColumn<int>(
                name: "ItemId1",
                table: "SalesOrderLines",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderLines_ItemId1",
                table: "SalesOrderLines",
                column: "ItemId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrderLines_Items_ItemId",
                table: "SalesOrderLines",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrderLines_Items_ItemId1",
                table: "SalesOrderLines",
                column: "ItemId1",
                principalTable: "Items",
                principalColumn: "Id");
        }
    }
}
