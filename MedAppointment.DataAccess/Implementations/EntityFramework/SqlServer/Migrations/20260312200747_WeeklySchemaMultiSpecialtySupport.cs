using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class WeeklySchemaMultiSpecialtySupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DayPlans_Specialties_SpecialtyId",
                schema: "Service",
                table: "DayPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_DaySchemas_Specialties_SpecialtyId",
                schema: "Doctor",
                table: "DaySchemas");

            migrationBuilder.DropIndex(
                name: "IX_DaySchemas_SpecialtyId",
                schema: "Doctor",
                table: "DaySchemas");

            migrationBuilder.DropIndex(
                name: "IX_DayPlans_SpecialtyId",
                schema: "Service",
                table: "DayPlans");

            migrationBuilder.DropColumn(
                name: "SpecialtyId",
                schema: "Doctor",
                table: "DaySchemas");

            migrationBuilder.DropColumn(
                name: "SpecialtyId",
                schema: "Service",
                table: "DayPlans");

            migrationBuilder.AddColumn<long>(
                name: "GenderId",
                schema: "Client",
                table: "People",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "PresentationVideoUrl",
                schema: "Client",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProfessionId",
                schema: "Client",
                table: "Doctors",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "DayPlanSpecialties",
                schema: "Compositions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayPlanId = table.Column<long>(type: "bigint", nullable: false),
                    SpecialtyId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayPlanSpecialties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DayPlanSpecialties_DayPlans_DayPlanId",
                        column: x => x.DayPlanId,
                        principalSchema: "Service",
                        principalTable: "DayPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DayPlanSpecialties_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalSchema: "Classifier",
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoctorServiceLanguages",
                schema: "Compositions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<long>(type: "bigint", nullable: false),
                    LanguageId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorServiceLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorServiceLanguages_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalSchema: "Client",
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorServiceLanguages_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "Classifier",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                schema: "Classifier",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameTextId = table.Column<long>(type: "bigint", nullable: false),
                    DescriptionTextId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Genders_Resources_DescriptionTextId",
                        column: x => x.DescriptionTextId,
                        principalSchema: "Localization",
                        principalTable: "Resources",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Genders_Resources_NameTextId",
                        column: x => x.NameTextId,
                        principalSchema: "Localization",
                        principalTable: "Resources",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Professions",
                schema: "Classifier",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameTextId = table.Column<long>(type: "bigint", nullable: false),
                    DescriptionTextId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Professions_Resources_DescriptionTextId",
                        column: x => x.DescriptionTextId,
                        principalSchema: "Localization",
                        principalTable: "Resources",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Professions_Resources_NameTextId",
                        column: x => x.NameTextId,
                        principalSchema: "Localization",
                        principalTable: "Resources",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WeeklySchemaSpecialties",
                schema: "Compositions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeeklySchemaId = table.Column<long>(type: "bigint", nullable: false),
                    SpecialtyId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklySchemaSpecialties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeeklySchemaSpecialties_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalSchema: "Classifier",
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeeklySchemaSpecialties_WeeklySchemas_WeeklySchemaId",
                        column: x => x.WeeklySchemaId,
                        principalSchema: "Doctor",
                        principalTable: "WeeklySchemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoctorServiceGenderTypes",
                schema: "Compositions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<long>(type: "bigint", nullable: false),
                    GenderId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorServiceGenderTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorServiceGenderTypes_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalSchema: "Client",
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorServiceGenderTypes_Genders_GenderId",
                        column: x => x.GenderId,
                        principalSchema: "Classifier",
                        principalTable: "Genders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_People_GenderId",
                schema: "Client",
                table: "People",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_ProfessionId",
                schema: "Client",
                table: "Doctors",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_DayPlanSpecialties_DayPlanId_SpecialtyId",
                schema: "Compositions",
                table: "DayPlanSpecialties",
                columns: new[] { "DayPlanId", "SpecialtyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DayPlanSpecialties_SpecialtyId",
                schema: "Compositions",
                table: "DayPlanSpecialties",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorServiceGenderTypes_DoctorId",
                schema: "Compositions",
                table: "DoctorServiceGenderTypes",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorServiceGenderTypes_GenderId",
                schema: "Compositions",
                table: "DoctorServiceGenderTypes",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorServiceLanguages_DoctorId",
                schema: "Compositions",
                table: "DoctorServiceLanguages",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorServiceLanguages_LanguageId",
                schema: "Compositions",
                table: "DoctorServiceLanguages",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Genders_DescriptionTextId",
                schema: "Classifier",
                table: "Genders",
                column: "DescriptionTextId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genders_Key",
                schema: "Classifier",
                table: "Genders",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genders_NameTextId",
                schema: "Classifier",
                table: "Genders",
                column: "NameTextId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Professions_DescriptionTextId",
                schema: "Classifier",
                table: "Professions",
                column: "DescriptionTextId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Professions_Key",
                schema: "Classifier",
                table: "Professions",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Professions_NameTextId",
                schema: "Classifier",
                table: "Professions",
                column: "NameTextId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeeklySchemaSpecialties_SpecialtyId",
                schema: "Compositions",
                table: "WeeklySchemaSpecialties",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklySchemaSpecialties_WeeklySchemaId_SpecialtyId",
                schema: "Compositions",
                table: "WeeklySchemaSpecialties",
                columns: new[] { "WeeklySchemaId", "SpecialtyId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Professions_ProfessionId",
                schema: "Client",
                table: "Doctors",
                column: "ProfessionId",
                principalSchema: "Classifier",
                principalTable: "Professions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_People_Genders_GenderId",
                schema: "Client",
                table: "People",
                column: "GenderId",
                principalSchema: "Classifier",
                principalTable: "Genders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Professions_ProfessionId",
                schema: "Client",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_People_Genders_GenderId",
                schema: "Client",
                table: "People");

            migrationBuilder.DropTable(
                name: "DayPlanSpecialties",
                schema: "Compositions");

            migrationBuilder.DropTable(
                name: "DoctorServiceGenderTypes",
                schema: "Compositions");

            migrationBuilder.DropTable(
                name: "DoctorServiceLanguages",
                schema: "Compositions");

            migrationBuilder.DropTable(
                name: "Professions",
                schema: "Classifier");

            migrationBuilder.DropTable(
                name: "WeeklySchemaSpecialties",
                schema: "Compositions");

            migrationBuilder.DropTable(
                name: "Genders",
                schema: "Classifier");

            migrationBuilder.DropIndex(
                name: "IX_People_GenderId",
                schema: "Client",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_ProfessionId",
                schema: "Client",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "GenderId",
                schema: "Client",
                table: "People");

            migrationBuilder.DropColumn(
                name: "PresentationVideoUrl",
                schema: "Client",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "ProfessionId",
                schema: "Client",
                table: "Doctors");

            migrationBuilder.AddColumn<long>(
                name: "SpecialtyId",
                schema: "Doctor",
                table: "DaySchemas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SpecialtyId",
                schema: "Service",
                table: "DayPlans",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_DaySchemas_SpecialtyId",
                schema: "Doctor",
                table: "DaySchemas",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_DayPlans_SpecialtyId",
                schema: "Service",
                table: "DayPlans",
                column: "SpecialtyId");

            migrationBuilder.AddForeignKey(
                name: "FK_DayPlans_Specialties_SpecialtyId",
                schema: "Service",
                table: "DayPlans",
                column: "SpecialtyId",
                principalSchema: "Classifier",
                principalTable: "Specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DaySchemas_Specialties_SpecialtyId",
                schema: "Doctor",
                table: "DaySchemas",
                column: "SpecialtyId",
                principalSchema: "Classifier",
                principalTable: "Specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
