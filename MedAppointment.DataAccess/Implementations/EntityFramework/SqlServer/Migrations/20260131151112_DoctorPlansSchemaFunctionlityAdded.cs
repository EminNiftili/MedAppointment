using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class DoctorPlansSchemaFunctionlityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Doctor");

            migrationBuilder.AddColumn<DateTime>(
                name: "BelongDate",
                schema: "Service",
                table: "DayPlans",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "PlanPaddingTypes",
                schema: "Classifier",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaddingPosition = table.Column<byte>(type: "tinyint", nullable: false, comment: "Padding position define where is Padding added. (in minutes)\n1 -> Start of Period\n2 -> End of Period\n3 -> Linear Between of Period\n4 -> Center Between of Period"),
                    PaddingTime = table.Column<byte>(type: "tinyint", nullable: false, comment: "Padding of between all Periods. (in minutes)"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanPaddingTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeeklySchemas",
                schema: "Doctor",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<long>(type: "bigint", nullable: false),
                    ColorHex = table.Column<string>(type: "char(9)", nullable: false, comment: "Color format is RGBA. (#00000000)"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklySchemas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeeklySchemas_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalSchema: "Client",
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DaySchemas",
                schema: "Doctor",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeeklySchemaId = table.Column<long>(type: "bigint", nullable: false),
                    SpecialtyId = table.Column<long>(type: "bigint", nullable: false),
                    PeriodId = table.Column<long>(type: "bigint", nullable: false),
                    PlanPaddingTypeId = table.Column<long>(type: "bigint", nullable: true),
                    DayOfWeek = table.Column<byte>(type: "tinyint", nullable: false, comment: "1=Monday ... 7=Sunday"),
                    OpenTime = table.Column<TimeSpan>(type: "time(7)", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaySchemas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DaySchemas_Periods_PeriodId",
                        column: x => x.PeriodId,
                        principalSchema: "Classifier",
                        principalTable: "Periods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DaySchemas_PlanPaddingTypes_PlanPaddingTypeId",
                        column: x => x.PlanPaddingTypeId,
                        principalSchema: "Classifier",
                        principalTable: "PlanPaddingTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DaySchemas_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalSchema: "Classifier",
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DaySchemas_WeeklySchemas_WeeklySchemaId",
                        column: x => x.WeeklySchemaId,
                        principalSchema: "Doctor",
                        principalTable: "WeeklySchemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DayBreaks",
                schema: "Doctor",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    DaySchemaId = table.Column<long>(type: "bigint", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time(7)", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time(7)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayBreaks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DayBreaks_DaySchemas_DaySchemaId",
                        column: x => x.DaySchemaId,
                        principalSchema: "Doctor",
                        principalTable: "DaySchemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DayBreaks_DaySchemaId",
                schema: "Doctor",
                table: "DayBreaks",
                column: "DaySchemaId");

            migrationBuilder.CreateIndex(
                name: "IX_DaySchemas_PeriodId",
                schema: "Doctor",
                table: "DaySchemas",
                column: "PeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_DaySchemas_PlanPaddingTypeId",
                schema: "Doctor",
                table: "DaySchemas",
                column: "PlanPaddingTypeId",
                unique: true,
                filter: "[PlanPaddingTypeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DaySchemas_SpecialtyId",
                schema: "Doctor",
                table: "DaySchemas",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_DaySchemas_WeeklySchemaId",
                schema: "Doctor",
                table: "DaySchemas",
                column: "WeeklySchemaId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanPaddingTypes_Name",
                schema: "Classifier",
                table: "PlanPaddingTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeeklySchemas_DoctorId",
                schema: "Doctor",
                table: "WeeklySchemas",
                column: "DoctorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DayBreaks",
                schema: "Doctor");

            migrationBuilder.DropTable(
                name: "DaySchemas",
                schema: "Doctor");

            migrationBuilder.DropTable(
                name: "PlanPaddingTypes",
                schema: "Classifier");

            migrationBuilder.DropTable(
                name: "WeeklySchemas",
                schema: "Doctor");

            migrationBuilder.DropColumn(
                name: "BelongDate",
                schema: "Service",
                table: "DayPlans");
        }
    }
}
