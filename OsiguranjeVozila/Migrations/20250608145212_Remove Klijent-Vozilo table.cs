using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OsiguranjeVozila.Migrations
{
    /// <inheritdoc />
    public partial class RemoveKlijentVozilotable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vozila_Klijenti_vlasnikId",
                table: "Vozila");

            migrationBuilder.DropIndex(
                name: "IX_Vozila_vlasnikId",
                table: "Vozila");

            migrationBuilder.DropColumn(
                name: "vlasnikId",
                table: "Vozila");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "vlasnikId",
                table: "Vozila",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vozila_vlasnikId",
                table: "Vozila",
                column: "vlasnikId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vozila_Klijenti_vlasnikId",
                table: "Vozila",
                column: "vlasnikId",
                principalTable: "Klijenti",
                principalColumn: "Id");
        }
    }
}
