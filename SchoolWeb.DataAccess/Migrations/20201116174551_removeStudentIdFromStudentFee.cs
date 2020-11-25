using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolWeb.DataAccess.Migrations
{
    public partial class removeStudentIdFromStudentFee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyPayments_StudentFees_StudentFeeId",
                table: "MonthlyPayments");

            migrationBuilder.DropIndex(
                name: "IX_Students_StudentFeeId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "StudentFees");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentFeeId",
                table: "Students",
                column: "StudentFeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyPayments_StudentFees_StudentFeeId",
                table: "MonthlyPayments",
                column: "StudentFeeId",
                principalTable: "StudentFees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyPayments_StudentFees_StudentFeeId",
                table: "MonthlyPayments");

            migrationBuilder.DropIndex(
                name: "IX_Students_StudentFeeId",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "StudentFees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentFeeId",
                table: "Students",
                column: "StudentFeeId",
                unique: true,
                filter: "[StudentFeeId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyPayments_StudentFees_StudentFeeId",
                table: "MonthlyPayments",
                column: "StudentFeeId",
                principalTable: "StudentFees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
