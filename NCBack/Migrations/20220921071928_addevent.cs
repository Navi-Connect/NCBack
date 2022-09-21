using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NCBack.Migrations
{
    public partial class addevent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AimOfTheMeeting = table.Column<string>(type: "text", nullable: false),
                    MeetingCategory = table.Column<string>(type: "text", nullable: false),
                    MeatingName = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TimeStart = table.Column<TimeSpan>(type: "interval", nullable: false),
                    TimeFinish = table.Column<TimeSpan>(type: "interval", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    AgeTo = table.Column<int>(type: "integer", nullable: false),
                    AgeFrom = table.Column<int>(type: "integer", nullable: false),
                    CaltulationType = table.Column<string>(type: "text", nullable: false),
                    CaltulationSum = table.Column<string>(type: "text", nullable: false),
                    LanguageCommunication = table.Column<string>(type: "text", nullable: false),
                    MeatingPlace = table.Column<string>(type: "text", nullable: false),
                    MeatingInterests = table.Column<string>(type: "text", nullable: false),
                    UsreId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
