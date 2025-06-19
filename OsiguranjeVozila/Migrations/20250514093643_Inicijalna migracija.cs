using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OsiguranjeVozila.Migrations
{
    /// <inheritdoc />
    public partial class Inicijalnamigracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Klijenti",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrojTelefona = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumRodjenja = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klijenti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Polise",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Trajanje = table.Column<int>(type: "int", nullable: false),
                    UslovOsiguranja = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NominalniIznos = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Polise", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vozila",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Marka = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistarskaOznaka = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GodinaProizvodnje = table.Column<int>(type: "int", nullable: false),
                    Kubikaza = table.Column<float>(type: "real", nullable: false),
                    SnagaMotora = table.Column<int>(type: "int", nullable: false),
                    BrojSasije = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumRegistracije = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumPrveRegistracije = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vozila", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prodaje",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DatumKupovine = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumIsteka = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cijena = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UcesceUSteti = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    KlijentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoziloId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PolisaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prodaje", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prodaje_Klijenti_KlijentId",
                        column: x => x.KlijentId,
                        principalTable: "Klijenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prodaje_Polise_PolisaId",
                        column: x => x.PolisaId,
                        principalTable: "Polise",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Prodaje_Vozila_VoziloId",
                        column: x => x.VoziloId,
                        principalTable: "Vozila",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prodaje_KlijentId",
                table: "Prodaje",
                column: "KlijentId");

            migrationBuilder.CreateIndex(
                name: "IX_Prodaje_PolisaId",
                table: "Prodaje",
                column: "PolisaId");

            migrationBuilder.CreateIndex(
                name: "IX_Prodaje_VoziloId",
                table: "Prodaje",
                column: "VoziloId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prodaje");

            migrationBuilder.DropTable(
                name: "Klijenti");

            migrationBuilder.DropTable(
                name: "Polise");

            migrationBuilder.DropTable(
                name: "Vozila");
        }
    }
}
