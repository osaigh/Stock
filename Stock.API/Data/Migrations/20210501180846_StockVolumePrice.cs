using Microsoft.EntityFrameworkCore.Migrations;

namespace Stock.API.Data.Migrations
{
    public partial class StockVolumePrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Stock",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Volume",
                table: "Stock",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Stock");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "Stock");
        }
    }
}
