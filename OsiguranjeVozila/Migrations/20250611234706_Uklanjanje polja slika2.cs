using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OsiguranjeVozila.Migrations
{
    /// <inheritdoc />
    public partial class Uklanjanjepoljaslika2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SlikaPolise",
                table: "Polise");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SlikaPolise",
                table: "Polise",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
