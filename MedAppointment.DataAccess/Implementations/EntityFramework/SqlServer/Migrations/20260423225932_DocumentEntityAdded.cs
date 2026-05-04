using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class DocumentEntityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Documents",
                schema: "File",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    DoctorId = table.Column<long>(type: "bigint", nullable: false),
                    SpecialtyId = table.Column<long>(type: "bigint", nullable: true),
                    IsProfessionBackground = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    IsExperience = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    Filename = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(127)", maxLength: 127, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Issuer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PeriodOfYear = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalSchema: "Client",
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Documents_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalSchema: "Classifier",
                        principalTable: "Specialties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DoctorId",
                schema: "File",
                table: "Documents",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocumentId",
                schema: "File",
                table: "Documents",
                column: "DocumentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_SpecialtyId",
                schema: "File",
                table: "Documents",
                column: "SpecialtyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documents",
                schema: "File");
        }
    }
}
