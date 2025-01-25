using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InspireEd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PendingChangesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Attendances_ClassId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Classes_ClassId1",
                table: "Attendances");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClassId1",
                table: "Attendances",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Classes_ClassId1",
                table: "Attendances",
                column: "ClassId1",
                principalTable: "Classes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Classes_ClassId1",
                table: "Attendances");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClassId1",
                table: "Attendances",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

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
    }
}
