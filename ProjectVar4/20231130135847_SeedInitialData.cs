using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Football.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Seeding pentru AspNetRoles
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[,]
                {
            { Guid.NewGuid().ToString(), "Admin", "ADMIN", Guid.NewGuid().ToString() },
            { Guid.NewGuid().ToString(), "User", "USER", Guid.NewGuid().ToString() }
                });

            // Seeding pentru AspNetUsers
            var adminUserId = Guid.NewGuid().ToString();
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnabled", "AccessFailedCount", "FullName", "CreatedAt", "UpdatedAt" },
                values: new object[,]
                {
            { adminUserId, "admin@example.com", "ADMIN@EXAMPLE.COM", "admin@example.com", "ADMIN@EXAMPLE.COM", true, "AQAAAAEAACcQAAAAE...hashedpassword...", Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "1234567890", true, false, false, 0, "Admin User", DateTime.UtcNow, DateTime.UtcNow }
                });

            // După ce ai inserat utilizatorii, poți continua cu tabelele 'Fields', 'Bookings', etc.
            // Aici ar trebui să continui cu restul seeding-ului respectând dependențele dintre tabele.
            migrationBuilder.InsertData(
        table: "Fields",
        columns: new[] { "Id", "Name", "Location", "IsIndoor", "Capacity", "SurfaceType", "HasLighting", "AdditionalFacilities" },
        values: new object[,]
        {
            // Adaugă date pentru fiecare teren; asigură-te că fiecare rând are un ID unic
            { 1, "Teren 1", "Locație 1", false, 100, "Iarbă", true, "Vestiare, Dușuri" },
            { 2, "Teren 2", "Locație 2", true, 50, "Synthetic", false, "Vestiare" }
            // Adaugă alte terenuri după necesități
        });
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
