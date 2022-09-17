using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NCBack.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    City = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<int>(type: "integer", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    Lastname = table.Column<string>(type: "text", nullable: false),
                    SurName = table.Column<string>(type: "text", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AvatarPath = table.Column<string>(type: "text", nullable: false),
                    Credo = table.Column<string>(type: "text", nullable: true),
                    LanguageOfCommunication = table.Column<string>(type: "text", nullable: true),
                    AboutMyself = table.Column<string>(type: "text", nullable: true),
                    MaritalStatus = table.Column<string>(type: "text", nullable: true),
                    GetAcquaintedWith = table.Column<string>(type: "text", nullable: true),
                    IWantToLearn = table.Column<string>(type: "text", nullable: true),
                    FavoritePlace = table.Column<string>(type: "text", nullable: true),
                    MyInterests = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: true),
                    Success = table.Column<bool>(type: "boolean", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
