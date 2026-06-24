using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelOpt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedPropertyIdToHouseKeepingTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PropertyId",
                table: "HouseKeepingTasks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_HouseKeepingTasks_PropertyId",
                table: "HouseKeepingTasks",
                column: "PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_HouseKeepingTasks_Properties_PropertyId",
                table: "HouseKeepingTasks",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HouseKeepingTasks_Properties_PropertyId",
                table: "HouseKeepingTasks");

            migrationBuilder.DropIndex(
                name: "IX_HouseKeepingTasks_PropertyId",
                table: "HouseKeepingTasks");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "HouseKeepingTasks");
        }
    }
}
