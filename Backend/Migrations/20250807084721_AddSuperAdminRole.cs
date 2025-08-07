using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddSuperAdminRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update existing roles to have consistent IDs and names with a more robust approach
            migrationBuilder.Sql(@"
                -- Create temporary roles with new IDs if they don't exist
                INSERT INTO ""AspNetRoles"" (""Id"", ""Name"", ""NormalizedName"")
                SELECT '1', 'superadmin', 'SUPERADMIN'
                WHERE NOT EXISTS (SELECT 1 FROM ""AspNetRoles"" WHERE ""Id"" = '1');
                
                INSERT INTO ""AspNetRoles"" (""Id"", ""Name"", ""NormalizedName"")
                SELECT '2', 'admin', 'ADMIN'
                WHERE NOT EXISTS (SELECT 1 FROM ""AspNetRoles"" WHERE ""Id"" = '2');
                
                INSERT INTO ""AspNetRoles"" (""Id"", ""Name"", ""NormalizedName"")
                SELECT '3', 'user', 'USER'
                WHERE NOT EXISTS (SELECT 1 FROM ""AspNetRoles"" WHERE ""Id"" = '3');
                
                -- Update user role assignments to point to the new admin role
                UPDATE ""AspNetUserRoles"" 
                SET ""RoleId"" = '2'
                WHERE ""RoleId"" IN (
                    SELECT ""Id"" FROM ""AspNetRoles"" 
                    WHERE (""Name"" = 'admin' OR ""Name"" = 'Admin') AND ""Id"" != '2'
                );
                
                -- Update user role assignments to point to the new user role
                UPDATE ""AspNetUserRoles"" 
                SET ""RoleId"" = '3'
                WHERE ""RoleId"" IN (
                    SELECT ""Id"" FROM ""AspNetRoles"" 
                    WHERE (""Name"" = 'user' OR ""Name"" = 'User') AND ""Id"" != '3'
                );
                
                -- Remove duplicate/old roles, but keep the ones with the correct IDs
                DELETE FROM ""AspNetRoles"" 
                WHERE ""Id"" NOT IN ('1', '2', '3');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "50f75716-438c-495c-98c7-4f68438288b9", null, "user", "USER" },
                    { "7e2fd730-f437-4fb4-bb10-d1e81a722291", null, "admin", "ADMIN" }
                });
        }
    }
}
