using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolWeb.DataAccess.Migrations
{
    public partial class changeBussAndSchoolFeesToDouble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "SchoolFeesAmount",
                table: "MonthlyPayments",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "BusFeesAmount",
                table: "MonthlyPayments",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SchoolFeesAmount",
                table: "MonthlyPayments",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "BusFeesAmount",
                table: "MonthlyPayments",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
