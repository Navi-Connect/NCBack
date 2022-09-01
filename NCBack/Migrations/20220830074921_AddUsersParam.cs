using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NCBack.Migrations
{
    public partial class AddUsersParam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AboutMyself",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Credo",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FavoritePlace",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GetAcquaintedWith",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IWantToLearn",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LanguageOfCommunication",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaritalStatus",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MyInterests",
                table: "Users",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AboutMyself",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Credo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FavoritePlace",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GetAcquaintedWith",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IWantToLearn",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LanguageOfCommunication",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MaritalStatus",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MyInterests",
                table: "Users");
        }
    }
}
