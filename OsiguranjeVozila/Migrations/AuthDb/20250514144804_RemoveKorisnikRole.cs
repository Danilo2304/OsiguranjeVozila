using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OsiguranjeVozila.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class RemoveKorisnikRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "48f99218-ba20-40ea-a88d-af23161d939c", "5af46be6-2284-4d41-96c3-a914aa147ff0" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "48f99218-ba20-40ea-a88d-af23161d939c");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5af46be6-2284-4d41-96c3-a914aa147ff0",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "58b88ed8-6df4-4e66-a454-011715451869", "AQAAAAIAAYagAAAAEJ2NMH2eiHHSMu1U9dE8un47zOSUs4311gv3teHEft4FPGKIbrnhuCX7y5iqseI0ow==", "6eacd2e4-517d-426a-99c1-b2b028fa74b9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "48f99218-ba20-40ea-a88d-af23161d939c", "48f99218-ba20-40ea-a88d-af23161d939c", "Korisnik", "Korisnik" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5af46be6-2284-4d41-96c3-a914aa147ff0",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "462f4d78-9ca2-4910-85a1-cb12fa72fdaa", "AQAAAAIAAYagAAAAEPGCa6VZ1E8KI5we12WKC3m5kL734UXfMppu+jo2mVszwwdy9hCaJ4Mu5q1dHnJgrg==", "3a1d6a19-2373-4f15-9958-aee7c087c505" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "48f99218-ba20-40ea-a88d-af23161d939c", "5af46be6-2284-4d41-96c3-a914aa147ff0" });
        }
    }
}
