using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class SecuritySchemaIndexChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiredDate",
                schema: "Security",
                table: "Tokens",
                type: "datetime",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<bool>(
                name: "IsExpired",
                schema: "Security",
                table: "Tokens",
                type: "bit",
                nullable: false,
                defaultValueSql: "0");

            migrationBuilder.CreateIndex(
                name: "IX_TraditionalUsers_PasswordHash",
                schema: "Security",
                table: "TraditionalUsers",
                column: "PasswordHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_AccessToken",
                schema: "Security",
                table: "Tokens",
                column: "AccessToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_RefreshToken",
                schema: "Security",
                table: "Tokens",
                column: "RefreshToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_Name",
                schema: "Security",
                table: "Devices",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_OSName",
                schema: "Security",
                table: "Devices",
                column: "OSName");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_UUID",
                schema: "Security",
                table: "Devices",
                column: "UUID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TraditionalUsers_PasswordHash",
                schema: "Security",
                table: "TraditionalUsers");

            migrationBuilder.DropIndex(
                name: "IX_Tokens_AccessToken",
                schema: "Security",
                table: "Tokens");

            migrationBuilder.DropIndex(
                name: "IX_Tokens_RefreshToken",
                schema: "Security",
                table: "Tokens");

            migrationBuilder.DropIndex(
                name: "IX_Devices_Name",
                schema: "Security",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_OSName",
                schema: "Security",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_UUID",
                schema: "Security",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "ExpiredDate",
                schema: "Security",
                table: "Tokens");

            migrationBuilder.DropColumn(
                name: "IsExpired",
                schema: "Security",
                table: "Tokens");
        }
    }
}
