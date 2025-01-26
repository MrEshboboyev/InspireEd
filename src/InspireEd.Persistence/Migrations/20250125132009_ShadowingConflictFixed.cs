using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InspireEd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ShadowingConflictFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Classes_ClassId1",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Faculties_FacultyId1",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_FacultyId1",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_ClassId_StudentId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "FacultyId1",
                table: "Groups");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClassId1",
                table: "Attendances",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_ClassId",
                table: "Attendances",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Attendances_ClassId",
                table: "Attendances",
                column: "ClassId",
                principalTable: "Attendances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Classes_ClassId1",
                table: "Attendances",
                column: "ClassId1",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Attendances_ClassId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Classes_ClassId1",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_ClassId",
                table: "Attendances");

            migrationBuilder.AddColumn<Guid>(
                name: "FacultyId1",
                table: "Groups",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ClassId1",
                table: "Attendances",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_FacultyId1",
                table: "Groups",
                column: "FacultyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_ClassId_StudentId",
                table: "Attendances",
                columns: new[] { "ClassId", "StudentId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Classes_ClassId1",
                table: "Attendances",
                column: "ClassId1",
                principalTable: "Classes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Faculties_FacultyId1",
                table: "Groups",
                column: "FacultyId1",
                principalTable: "Faculties",
                principalColumn: "Id");
        }
    }
}
