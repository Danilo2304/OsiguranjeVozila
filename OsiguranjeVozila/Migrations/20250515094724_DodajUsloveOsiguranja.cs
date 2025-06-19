using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OsiguranjeVozila.Migrations
{
    /// <inheritdoc />
    public partial class DodajUsloveOsiguranja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UslovOsiguranja",
                table: "Polise");

            migrationBuilder.CreateTable(
                name: "UslovOsiguranja",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UslovOsiguranja", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PolisaUslovOsiguranja");

            migrationBuilder.DropTable(
                name: "UslovOsiguranja");

            migrationBuilder.AddColumn<string>(
                name: "UslovOsiguranja",
                table: "Polise",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
