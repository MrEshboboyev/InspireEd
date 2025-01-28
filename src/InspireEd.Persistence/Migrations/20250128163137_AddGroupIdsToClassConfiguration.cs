using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InspireEd.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupIdsToClassConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Classes_ClassId1",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_ClassId1",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "ClassId1",
                table: "Attendances");

            migrationBuilder.AddColumn<string>(
                name: "GroupIds",
                table: "Classes",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupIds",
                table: "Classes");

            migrationBuilder.AddColumn<Guid>(
                name: "ClassId1",
                table: "Attendances",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_ClassId1",
                table: "Attendances",
                column: "ClassId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Classes_ClassId1",
                table: "Attendances",
                column: "ClassId1",
                principalTable: "Classes",
                principalColumn: "Id");
        }
    }
}
