using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolWeb.DataAccess.Migrations
{
    public partial class changeCascadeOptionsforStudentFee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyPayments_StudentFees_StudentFeeId",
                table: "MonthlyPayments");

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyPayments_StudentFees_StudentFeeId",
                table: "MonthlyPayments",
                column: "StudentFeeId",
                principalTable: "StudentFees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyPayments_StudentFees_StudentFeeId",
                table: "MonthlyPayments");

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyPayments_StudentFees_StudentFeeId",
                table: "MonthlyPayments",
                column: "StudentFeeId",
                principalTable: "StudentFees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
