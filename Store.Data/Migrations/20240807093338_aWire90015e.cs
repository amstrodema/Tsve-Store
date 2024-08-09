using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Data.Migrations
{
    /// <inheritdoc />
    public partial class aWire90015e : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "Tsve_Trackings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "Tsve_Trackings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderID",
                table: "Tsve_Trackings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "Tsve_Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVideo",
                table: "Tsve_Files",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Resource",
                table: "Tsve_Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "Tsve_Trackings");

            migrationBuilder.DropColumn(
                name: "Details",
                table: "Tsve_Trackings");

            migrationBuilder.DropColumn(
                name: "OrderID",
                table: "Tsve_Trackings");

            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "Tsve_Orders");

            migrationBuilder.DropColumn(
                name: "IsVideo",
                table: "Tsve_Files");

            migrationBuilder.DropColumn(
                name: "Resource",
                table: "Tsve_Files");
        }
    }
}
