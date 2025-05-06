using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Constraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReservationDate",
                table: "Reservations",
                newName: "ReservationDateFrom");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReservationDateTo",
                table: "Reservations",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservationDateTo",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "ReservationDateFrom",
                table: "Reservations",
                newName: "ReservationDate");
        }
    }
}
