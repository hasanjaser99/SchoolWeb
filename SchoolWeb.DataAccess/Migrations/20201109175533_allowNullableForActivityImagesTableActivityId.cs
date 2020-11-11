using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolWeb.DataAccess.Migrations
{
    public partial class allowNullableForActivityImagesTableActivityId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ActivityId",
                table: "ActivityImages",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ActivityId",
                table: "ActivityImages",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
