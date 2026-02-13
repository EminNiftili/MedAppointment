using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class LanguageFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Languages_Resources_DescriptionTextId",
                schema: "Classifier",
                table: "Languages");

            migrationBuilder.DropForeignKey(
                name: "FK_Languages_Resources_NameTextId",
                schema: "Classifier",
                table: "Languages");

            migrationBuilder.DropIndex(
                name: "IX_Languages_DescriptionTextId",
                schema: "Classifier",
                table: "Languages");

            migrationBuilder.DropIndex(
                name: "IX_Languages_Key",
                schema: "Classifier",
                table: "Languages");

            migrationBuilder.DropIndex(
                name: "IX_Languages_NameTextId",
                schema: "Classifier",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "DescriptionTextId",
                schema: "Classifier",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "Key",
                schema: "Classifier",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "NameTextId",
                schema: "Classifier",
                table: "Languages");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Classifier",
                table: "Languages",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Name",
                schema: "Classifier",
                table: "Languages",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Languages_Name",
                schema: "Classifier",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Classifier",
                table: "Languages");

            migrationBuilder.AddColumn<long>(
                name: "DescriptionTextId",
                schema: "Classifier",
                table: "Languages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Key",
                schema: "Classifier",
                table: "Languages",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "NameTextId",
                schema: "Classifier",
                table: "Languages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_DescriptionTextId",
                schema: "Classifier",
                table: "Languages",
                column: "DescriptionTextId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Key",
                schema: "Classifier",
                table: "Languages",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_NameTextId",
                schema: "Classifier",
                table: "Languages",
                column: "NameTextId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Languages_Resources_DescriptionTextId",
                schema: "Classifier",
                table: "Languages",
                column: "DescriptionTextId",
                principalSchema: "Localization",
                principalTable: "Resources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Languages_Resources_NameTextId",
                schema: "Classifier",
                table: "Languages",
                column: "NameTextId",
                principalSchema: "Localization",
                principalTable: "Resources",
                principalColumn: "Id");
        }
    }
}
