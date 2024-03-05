using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkVisits_Users_UserId",
                table: "WorkVisits");

            migrationBuilder.DropIndex(
                name: "IX_WorkVisits_UserId",
                table: "WorkVisits");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WorkVisits");

            migrationBuilder.AddColumn<Guid>(
                name: "UserModelId",
                table: "WorkVisits",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkVisits_UserModelId",
                table: "WorkVisits",
                column: "UserModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkVisits_Users_UserModelId",
                table: "WorkVisits",
                column: "UserModelId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkVisits_Users_UserModelId",
                table: "WorkVisits");

            migrationBuilder.DropIndex(
                name: "IX_WorkVisits_UserModelId",
                table: "WorkVisits");

            migrationBuilder.DropColumn(
                name: "UserModelId",
                table: "WorkVisits");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "WorkVisits",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WorkVisits_UserId",
                table: "WorkVisits",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkVisits_Users_UserId",
                table: "WorkVisits",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
