using Microsoft.EntityFrameworkCore.Migrations;

namespace NexusBankApi.Migrations
{
    public partial class updatedProfilePictureColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "Customers",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
