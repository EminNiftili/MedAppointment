using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class FixedDoctorSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DaySchemas_PlanPaddingTypeId",
                schema: "Doctor",
                table: "DaySchemas");

            migrationBuilder.CreateIndex(
                name: "IX_DaySchemas_PlanPaddingTypeId",
                schema: "Doctor",
                table: "DaySchemas",
                column: "PlanPaddingTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DaySchemas_PlanPaddingTypeId",
                schema: "Doctor",
                table: "DaySchemas");

            migrationBuilder.CreateIndex(
                name: "IX_DaySchemas_PlanPaddingTypeId",
                schema: "Doctor",
                table: "DaySchemas",
                column: "PlanPaddingTypeId",
                unique: true,
                filter: "[PlanPaddingTypeId] IS NOT NULL");
        }
    }
}
