using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolWeb.DataAccess.Migrations
{
    public partial class addRelationBetweenCoursesTableAndSections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SectionId",
                table: "Courses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SectionId",
                table: "Courses",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Sections_SectionId",
                table: "Courses",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Sections_SectionId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_SectionId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "Courses");
        }
    }
}
