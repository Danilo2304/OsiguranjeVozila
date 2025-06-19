using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OsiguranjeVozila.Migrations
{
    /// <inheritdoc />
    public partial class KlijentVoziloRelacija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PolisaUslovOsiguranja");

            migrationBuilder.AddColumn<Guid>(
                name: "vlasnikId",
                table: "Vozila",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PoliseUslovOsiguranja",
                columns: table => new
                {
                    PoliseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsloviOsiguranjaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliseUslovOsiguranja", x => new { x.PoliseId, x.UsloviOsiguranjaId });
                    table.ForeignKey(
                        name: "FK_PoliseUslovOsiguranja_Polise_PoliseId",
                        column: x => x.PoliseId,
                        principalTable: "Polise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PoliseUslovOsiguranja_UslovOsiguranja_UsloviOsiguranjaId",
                        column: x => x.UsloviOsiguranjaId,
                        principalTable: "UslovOsiguranja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vozila_vlasnikId",
                table: "Vozila",
                column: "vlasnikId");

            migrationBuilder.CreateIndex(
                name: "IX_PoliseUslovOsiguranja_UsloviOsiguranjaId",
                table: "PoliseUslovOsiguranja",
                column: "UsloviOsiguranjaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vozila_Klijenti_vlasnikId",
                table: "Vozila",
                column: "vlasnikId",
                principalTable: "Klijenti",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vozila_Klijenti_vlasnikId",
                table: "Vozila");

            migrationBuilder.DropTable(
                name: "PoliseUslovOsiguranja");

            migrationBuilder.DropIndex(
                name: "IX_Vozila_vlasnikId",
                table: "Vozila");

            migrationBuilder.DropColumn(
                name: "vlasnikId",
                table: "Vozila");

            migrationBuilder.CreateTable(
                name: "PolisaUslovOsiguranja",
                columns: table => new
                {
                    PoliseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsloviOsiguranjaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolisaUslovOsiguranja", x => new { x.PoliseId, x.UsloviOsiguranjaId });
                    table.ForeignKey(
                        name: "FK_PolisaUslovOsiguranja_Polise_PoliseId",
                        column: x => x.PoliseId,
                        principalTable: "Polise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PolisaUslovOsiguranja_UslovOsiguranja_UsloviOsiguranjaId",
                        column: x => x.UsloviOsiguranjaId,
                        principalTable: "UslovOsiguranja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PolisaUslovOsiguranja_UsloviOsiguranjaId",
                table: "PolisaUslovOsiguranja",
                column: "UsloviOsiguranjaId");
        }
    }
}
