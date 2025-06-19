using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OsiguranjeVozila.Migrations
{
    /// <inheritdoc />
    public partial class prepravkavozilanazad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Prodaje_VoziloId",
                table: "Prodaje");

            migrationBuilder.CreateIndex(
                name: "IX_Prodaje_VoziloId",
                table: "Prodaje",
                column: "VoziloId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Prodaje_VoziloId",
                table: "Prodaje");

            migrationBuilder.CreateIndex(
                name: "IX_Prodaje_VoziloId",
                table: "Prodaje",
                column: "VoziloId",
                unique: true);
        }
    }
}
