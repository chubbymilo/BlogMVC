using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogMVC.Data.Migrations
{
    public partial class Added_description : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Post",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Post");
        }
    }
}
