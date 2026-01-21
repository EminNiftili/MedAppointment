using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class ChangedDecimalTypeTo4Float : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Coefficent",
                schema: "Classifier",
                table: "Currencies",
                type: "decimal(8,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Coefficent",
                schema: "Classifier",
                table: "Currencies",
                type: "decimal(8,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,4)");
        }
    }
}
