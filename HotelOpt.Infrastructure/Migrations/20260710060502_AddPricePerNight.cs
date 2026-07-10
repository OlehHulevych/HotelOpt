using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelOpt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPricePerNight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PricePerNight",
                table: "Rooms",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerNight",
                table: "Rooms");
        }
    }
}
