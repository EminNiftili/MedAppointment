using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class ClassifierLocalization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Specialties_Name",
                schema: "Classifier",
                table: "Specialties");

            migrationBuilder.DropIndex(
                name: "IX_PlanPaddingTypes_Name",
                schema: "Classifier",
                table: "PlanPaddingTypes");

            migrationBuilder.DropIndex(
                name: "IX_Periods_Name",
                schema: "Classifier",
                table: "Periods");

            migrationBuilder.DropIndex(
                name: "IX_PaymentTypes_Name",
                schema: "Classifier",
                table: "PaymentTypes");

            migrationBuilder.DropIndex(
                name: "IX_Currencies_Name",
                schema: "Classifier",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Classifier",
                table: "Specialties");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Classifier",
                table: "Specialties");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Classifier",
                table: "PlanPaddingTypes");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Classifier",
                table: "PlanPaddingTypes");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Classifier",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Classifier",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Classifier",
                table: "PaymentTypes");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Classifier",
                table: "PaymentTypes");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Client",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "Client",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Classifier",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Classifier",
                table: "Currencies");

            migrationBuilder.EnsureSchema(
                name: "Localization");

            migrationBuilder.AddColumn<long>(
                name: "DescriptionTextId",
                schema: "Classifier",
                table: "Specialties",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Key",
                schema: "Classifier",
                table: "Specialties",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "NameTextId",
                schema: "Classifier",
                table: "Specialties",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DescriptionTextId",
                schema: "Classifier",
                table: "PlanPaddingTypes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Key",
                schema: "Classifier",
                table: "PlanPaddingTypes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "NameTextId",
                schema: "Classifier",
                table: "PlanPaddingTypes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DescriptionTextId",
                schema: "Classifier",
                table: "Periods",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Key",
                schema: "Classifier",
                table: "Periods",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "NameTextId",
                schema: "Classifier",
                table: "Periods",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DescriptionTextId",
                schema: "Classifier",
                table: "PaymentTypes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Key",
                schema: "Classifier",
                table: "PaymentTypes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "NameTextId",
                schema: "Classifier",
                table: "PaymentTypes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DescriptionTextId",
                schema: "Client",
                table: "Doctors",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TitleTextId",
                schema: "Client",
                table: "Doctors",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsOnSiteService",
                schema: "Doctor",
                table: "DaySchemas",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<bool>(
                name: "IsOnlineService",
                schema: "Doctor",
                table: "DaySchemas",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<byte>(
                name: "PeriodCount",
                schema: "Doctor",
                table: "DaySchemas",
                type: "tinyint",
                nullable: false,
                defaultValueSql: "0",
                comment: "Number of periods (slots) for this day; 0 when closed.");

            migrationBuilder.AddColumn<long>(
                name: "DescriptionTextId",
                schema: "Classifier",
                table: "Currencies",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Key",
                schema: "Classifier",
                table: "Currencies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "NameTextId",
                schema: "Classifier",
                table: "Currencies",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Resources",
                schema: "Localization",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                schema: "Classifier",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameTextId = table.Column<long>(type: "bigint", nullable: false),
                    DescriptionTextId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Languages_Resources_DescriptionTextId",
                        column: x => x.DescriptionTextId,
                        principalSchema: "Localization",
                        principalTable: "Resources",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Languages_Resources_NameTextId",
                        column: x => x.NameTextId,
                        principalSchema: "Localization",
                        principalTable: "Resources",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Translations",
                schema: "Localization",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceId = table.Column<long>(type: "bigint", nullable: false),
                    LanguageId = table.Column<long>(type: "bigint", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Translations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "Classifier",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Translations_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalSchema: "Localization",
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_DescriptionTextId",
                schema: "Classifier",
                table: "Specialties",
                column: "DescriptionTextId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_Key",
                schema: "Classifier",
                table: "Specialties",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_NameTextId",
                schema: "Classifier",
                table: "Specialties",
                column: "NameTextId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanPaddingTypes_DescriptionTextId",
                schema: "Classifier",
                table: "PlanPaddingTypes",
                column: "DescriptionTextId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanPaddingTypes_Key",
                schema: "Classifier",
                table: "PlanPaddingTypes",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanPaddingTypes_NameTextId",
                schema: "Classifier",
                table: "PlanPaddingTypes",
                column: "NameTextId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Periods_DescriptionTextId",
                schema: "Classifier",
                table: "Periods",
                column: "DescriptionTextId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Periods_Key",
                schema: "Classifier",
                table: "Periods",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Periods_NameTextId",
                schema: "Classifier",
                table: "Periods",
                column: "NameTextId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTypes_DescriptionTextId",
                schema: "Classifier",
                table: "PaymentTypes",
                column: "DescriptionTextId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTypes_Key",
                schema: "Classifier",
                table: "PaymentTypes",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTypes_NameTextId",
                schema: "Classifier",
                table: "PaymentTypes",
                column: "NameTextId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_DescriptionTextId",
                schema: "Client",
                table: "Doctors",
                column: "DescriptionTextId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_TitleTextId",
                schema: "Client",
                table: "Doctors",
                column: "TitleTextId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_DescriptionTextId",
                schema: "Classifier",
                table: "Currencies",
                column: "DescriptionTextId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Key",
                schema: "Classifier",
                table: "Currencies",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_NameTextId",
                schema: "Classifier",
                table: "Currencies",
                column: "NameTextId",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Resources_Key",
                schema: "Localization",
                table: "Resources",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Translations_LanguageId_ResourceId",
                schema: "Localization",
                table: "Translations",
                columns: new[] { "LanguageId", "ResourceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Translations_ResourceId",
                schema: "Localization",
                table: "Translations",
                column: "ResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Currencies_Resources_DescriptionTextId",
                schema: "Classifier",
                table: "Currencies",
                column: "DescriptionTextId",
                principalSchema: "Localization",
                principalTable: "Resources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Currencies_Resources_NameTextId",
                schema: "Classifier",
                table: "Currencies",
                column: "NameTextId",
                principalSchema: "Localization",
                principalTable: "Resources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Resources_DescriptionTextId",
                schema: "Client",
                table: "Doctors",
                column: "DescriptionTextId",
                principalSchema: "Localization",
                principalTable: "Resources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Resources_TitleTextId",
                schema: "Client",
                table: "Doctors",
                column: "TitleTextId",
                principalSchema: "Localization",
                principalTable: "Resources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTypes_Resources_DescriptionTextId",
                schema: "Classifier",
                table: "PaymentTypes",
                column: "DescriptionTextId",
                principalSchema: "Localization",
                principalTable: "Resources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTypes_Resources_NameTextId",
                schema: "Classifier",
                table: "PaymentTypes",
                column: "NameTextId",
                principalSchema: "Localization",
                principalTable: "Resources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Periods_Resources_DescriptionTextId",
                schema: "Classifier",
                table: "Periods",
                column: "DescriptionTextId",
                principalSchema: "Localization",
                principalTable: "Resources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Periods_Resources_NameTextId",
                schema: "Classifier",
                table: "Periods",
                column: "NameTextId",
                principalSchema: "Localization",
                principalTable: "Resources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanPaddingTypes_Resources_DescriptionTextId",
                schema: "Classifier",
                table: "PlanPaddingTypes",
                column: "DescriptionTextId",
                principalSchema: "Localization",
                principalTable: "Resources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanPaddingTypes_Resources_NameTextId",
                schema: "Classifier",
                table: "PlanPaddingTypes",
                column: "NameTextId",
                principalSchema: "Localization",
                principalTable: "Resources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Specialties_Resources_DescriptionTextId",
                schema: "Classifier",
                table: "Specialties",
                column: "DescriptionTextId",
                principalSchema: "Localization",
                principalTable: "Resources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Specialties_Resources_NameTextId",
                schema: "Classifier",
                table: "Specialties",
                column: "NameTextId",
                principalSchema: "Localization",
                principalTable: "Resources",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Currencies_Resources_DescriptionTextId",
                schema: "Classifier",
                table: "Currencies");

            migrationBuilder.DropForeignKey(
                name: "FK_Currencies_Resources_NameTextId",
                schema: "Classifier",
                table: "Currencies");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Resources_DescriptionTextId",
                schema: "Client",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Resources_TitleTextId",
                schema: "Client",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTypes_Resources_DescriptionTextId",
                schema: "Classifier",
                table: "PaymentTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTypes_Resources_NameTextId",
                schema: "Classifier",
                table: "PaymentTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Periods_Resources_DescriptionTextId",
                schema: "Classifier",
                table: "Periods");

            migrationBuilder.DropForeignKey(
                name: "FK_Periods_Resources_NameTextId",
                schema: "Classifier",
                table: "Periods");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanPaddingTypes_Resources_DescriptionTextId",
                schema: "Classifier",
                table: "PlanPaddingTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanPaddingTypes_Resources_NameTextId",
                schema: "Classifier",
                table: "PlanPaddingTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Specialties_Resources_DescriptionTextId",
                schema: "Classifier",
                table: "Specialties");

            migrationBuilder.DropForeignKey(
                name: "FK_Specialties_Resources_NameTextId",
                schema: "Classifier",
                table: "Specialties");

            migrationBuilder.DropTable(
                name: "Translations",
                schema: "Localization");

            migrationBuilder.DropTable(
                name: "Languages",
                schema: "Classifier");

            migrationBuilder.DropTable(
                name: "Resources",
                schema: "Localization");

            migrationBuilder.DropIndex(
                name: "IX_Specialties_DescriptionTextId",
                schema: "Classifier",
                table: "Specialties");

            migrationBuilder.DropIndex(
                name: "IX_Specialties_Key",
                schema: "Classifier",
                table: "Specialties");

            migrationBuilder.DropIndex(
                name: "IX_Specialties_NameTextId",
                schema: "Classifier",
                table: "Specialties");

            migrationBuilder.DropIndex(
                name: "IX_PlanPaddingTypes_DescriptionTextId",
                schema: "Classifier",
                table: "PlanPaddingTypes");

            migrationBuilder.DropIndex(
                name: "IX_PlanPaddingTypes_Key",
                schema: "Classifier",
                table: "PlanPaddingTypes");

            migrationBuilder.DropIndex(
                name: "IX_PlanPaddingTypes_NameTextId",
                schema: "Classifier",
                table: "PlanPaddingTypes");

            migrationBuilder.DropIndex(
                name: "IX_Periods_DescriptionTextId",
                schema: "Classifier",
                table: "Periods");

            migrationBuilder.DropIndex(
                name: "IX_Periods_Key",
                schema: "Classifier",
                table: "Periods");

            migrationBuilder.DropIndex(
                name: "IX_Periods_NameTextId",
                schema: "Classifier",
                table: "Periods");

            migrationBuilder.DropIndex(
                name: "IX_PaymentTypes_DescriptionTextId",
                schema: "Classifier",
                table: "PaymentTypes");

            migrationBuilder.DropIndex(
                name: "IX_PaymentTypes_Key",
                schema: "Classifier",
                table: "PaymentTypes");

            migrationBuilder.DropIndex(
                name: "IX_PaymentTypes_NameTextId",
                schema: "Classifier",
                table: "PaymentTypes");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_DescriptionTextId",
                schema: "Client",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_TitleTextId",
                schema: "Client",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Currencies_DescriptionTextId",
                schema: "Classifier",
                table: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_Currencies_Key",
                schema: "Classifier",
                table: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_Currencies_NameTextId",
                schema: "Classifier",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "DescriptionTextId",
                schema: "Classifier",
                table: "Specialties");

            migrationBuilder.DropColumn(
                name: "Key",
                schema: "Classifier",
                table: "Specialties");

            migrationBuilder.DropColumn(
                name: "NameTextId",
                schema: "Classifier",
                table: "Specialties");

            migrationBuilder.DropColumn(
                name: "DescriptionTextId",
                schema: "Classifier",
                table: "PlanPaddingTypes");

            migrationBuilder.DropColumn(
                name: "Key",
                schema: "Classifier",
                table: "PlanPaddingTypes");

            migrationBuilder.DropColumn(
                name: "NameTextId",
                schema: "Classifier",
                table: "PlanPaddingTypes");

            migrationBuilder.DropColumn(
                name: "DescriptionTextId",
                schema: "Classifier",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "Key",
                schema: "Classifier",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "NameTextId",
                schema: "Classifier",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "DescriptionTextId",
                schema: "Classifier",
                table: "PaymentTypes");

            migrationBuilder.DropColumn(
                name: "Key",
                schema: "Classifier",
                table: "PaymentTypes");

            migrationBuilder.DropColumn(
                name: "NameTextId",
                schema: "Classifier",
                table: "PaymentTypes");

            migrationBuilder.DropColumn(
                name: "DescriptionTextId",
                schema: "Client",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "TitleTextId",
                schema: "Client",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "IsOnSiteService",
                schema: "Doctor",
                table: "DaySchemas");

            migrationBuilder.DropColumn(
                name: "IsOnlineService",
                schema: "Doctor",
                table: "DaySchemas");

            migrationBuilder.DropColumn(
                name: "PeriodCount",
                schema: "Doctor",
                table: "DaySchemas");

            migrationBuilder.DropColumn(
                name: "DescriptionTextId",
                schema: "Classifier",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "Key",
                schema: "Classifier",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "NameTextId",
                schema: "Classifier",
                table: "Currencies");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Classifier",
                table: "Specialties",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Classifier",
                table: "Specialties",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Classifier",
                table: "PlanPaddingTypes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Classifier",
                table: "PlanPaddingTypes",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Classifier",
                table: "Periods",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Classifier",
                table: "Periods",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Classifier",
                table: "PaymentTypes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Classifier",
                table: "PaymentTypes",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Client",
                table: "Doctors",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "Client",
                table: "Doctors",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Classifier",
                table: "Currencies",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Classifier",
                table: "Currencies",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_Name",
                schema: "Classifier",
                table: "Specialties",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanPaddingTypes_Name",
                schema: "Classifier",
                table: "PlanPaddingTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Periods_Name",
                schema: "Classifier",
                table: "Periods",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTypes_Name",
                schema: "Classifier",
                table: "PaymentTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Name",
                schema: "Classifier",
                table: "Currencies",
                column: "Name",
                unique: true);
        }
    }
}
