using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InspireEd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSubjectPermissionsToDepartmentHead : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 60,
                column: "Name",
                value: "ManageGroups");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 90,
                column: "Name",
                value: "ViewSubjects");

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 53, "ViewStudents" },
                    { 61, "AssignGroups" },
                    { 95, "ManageSubjects" }
                });

            migrationBuilder.InsertData(
                table: "RolePermission",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 51, 2 },
                    { 52, 2 },
                    { 90, 2 },
                    { 53, 2 },
                    { 61, 2 },
                    { 95, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 51, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 52, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 53, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 61, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 90, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 95, 2 });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 60,
                column: "Name",
                value: "AssignGroups");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 90,
                column: "Name",
                value: "ManageGroups");
        }
    }
}
