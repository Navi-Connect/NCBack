using System;
using System.Collections.Generic;
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
                name: "AimOfTheMeeting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameAimOfTheMeeting = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AimOfTheMeeting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CityList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CityName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GenderList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GenderName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenderList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeetingCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameMeetingCategory = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    LinkVideo = table.Column<string>(type: "text", nullable: true),
                    Photo = table.Column<string>(type: "text", nullable: true),
                    LinkWebSites = table.Column<string>(type: "text", nullable: true),
                    Data = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    DeviceId = table.Column<string>(type: "text", nullable: true),
                    IsAndroiodDevice = table.Column<bool>(type: "boolean", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhoneEditing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<int>(type: "integer", nullable: true),
                    Success = table.Column<bool>(type: "boolean", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneEditing", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntermediateUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CityId = table.Column<int>(type: "integer", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<int>(type: "integer", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    GenderId = table.Column<int>(type: "integer", nullable: false),
                    AvatarPath = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: false),
                    Success = table.Column<bool>(type: "boolean", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntermediateUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntermediateUser_CityList_CityId",
                        column: x => x.CityId,
                        principalTable: "CityList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IntermediateUser_GenderList_GenderId",
                        column: x => x.GenderId,
                        principalTable: "GenderList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CityId = table.Column<int>(type: "integer", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<int>(type: "integer", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AvatarPath = table.Column<string>(type: "text", nullable: false),
                    CredoAboutMyself = table.Column<string>(type: "text", nullable: true),
                    LanguageOfCommunication = table.Column<List<string>>(type: "text[]", nullable: true),
                    Nationality = table.Column<string>(type: "text", nullable: true),
                    GenderId = table.Column<int>(type: "integer", nullable: true),
                    MaritalStatus = table.Column<string>(type: "text", nullable: true),
                    Сhildren = table.Column<string>(type: "text", nullable: true),
                    GetAcquaintedWith = table.Column<string>(type: "text", nullable: true),
                    MeetFor = table.Column<string>(type: "text", nullable: true),
                    From = table.Column<int>(type: "integer", nullable: true),
                    To = table.Column<int>(type: "integer", nullable: true),
                    IWantToLearn = table.Column<string>(type: "text", nullable: true),
                    PreferredPlaces = table.Column<List<string>>(type: "text[]", nullable: true),
                    Interests = table.Column<List<string>>(type: "text[]", nullable: true),
                    Profession = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: true),
                    DeviceId = table.Column<string>(type: "text", nullable: true),
                    Success = table.Column<bool>(type: "boolean", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_CityList_CityId",
                        column: x => x.CityId,
                        principalTable: "CityList",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_GenderList_GenderId",
                        column: x => x.GenderId,
                        principalTable: "GenderList",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MeatingPlace",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameMeatingPlace = table.Column<string>(type: "text", nullable: false),
                    MeetingCategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeatingPlace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeatingPlace_MeetingCategory_MeetingCategoryId",
                        column: x => x.MeetingCategoryId,
                        principalTable: "MeetingCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AimOfTheMeetingId = table.Column<int>(type: "integer", nullable: false),
                    MeetingCategoryId = table.Column<int>(type: "integer", nullable: false),
                    MeatingPlaceId = table.Column<int>(type: "integer", nullable: true),
                    IWant = table.Column<string>(type: "text", nullable: true),
                    TimeStart = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    TimeFinish = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreateAdd = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CityId = table.Column<int>(type: "integer", nullable: true),
                    GenderId = table.Column<int>(type: "integer", nullable: true),
                    AgeTo = table.Column<int>(type: "integer", nullable: true),
                    AgeFrom = table.Column<int>(type: "integer", nullable: true),
                    CaltulationType = table.Column<string>(type: "text", nullable: true),
                    CaltulationSum = table.Column<string>(type: "text", nullable: true),
                    LanguageCommunication = table.Column<List<string>>(type: "text[]", nullable: true),
                    Interests = table.Column<List<string>>(type: "text[]", nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_AimOfTheMeeting_AimOfTheMeetingId",
                        column: x => x.AimOfTheMeetingId,
                        principalTable: "AimOfTheMeeting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Events_CityList_CityId",
                        column: x => x.CityId,
                        principalTable: "CityList",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Events_GenderList_GenderId",
                        column: x => x.GenderId,
                        principalTable: "GenderList",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Events_MeatingPlace_MeatingPlaceId",
                        column: x => x.MeatingPlaceId,
                        principalTable: "MeatingPlace",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Events_MeetingCategory_MeetingCategoryId",
                        column: x => x.MeetingCategoryId,
                        principalTable: "MeetingCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Events_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccedEventUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    EventId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccedEventUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccedEventUser_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccedEventUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserEvent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    EventId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserEvent_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserEvent_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AimOfTheMeeting",
                columns: new[] { "Id", "NameAimOfTheMeeting" },
                values: new object[,]
                {
                    { 1, "Знакомство и общение" },
                    { 2, "Поиск друга/Собеседника" },
                    { 3, "Свидания и поиск второй половинки" },
                    { 4, "Для родителей (Прогулка с детьми)" },
                    { 5, "Обсуждение идеи/бизнеса (брейншторм)" },
                    { 6, "Обсуждение фильма/сериала/книги" },
                    { 7, "Совместная выпивка" },
                    { 8, "Совместные тренировки" },
                    { 9, "Обсуждение разных проблем" },
                    { 10, "Саморазвитие, обучение" },
                    { 11, "Совместный отдых" },
                    { 12, "Рыбалка и охота " },
                    { 13, "Совместные онлайн игры" },
                    { 14, "Совместные спортивные игры" },
                    { 15, "Душевный разговор" },
                    { 16, "Знакомства и встречи молодых людей" },
                    { 17, "Практика языков" },
                    { 18, "Обсуждение рабочих моментов" },
                    { 19, "Встреча людей с ограниченными возможностями" },
                    { 20, "Прогулка с животными" },
                    { 21, "Совместное путешествие, туризм" },
                    { 22, "Съёмки контента" },
                    { 23, "Танцы" },
                    { 24, "Послушать/Записать музыку" },
                    { 25, "Творчество и изобразительное искусство" },
                    { 26, "Встреча пожилых людей " },
                    { 27, "Только девушки" },
                    { 28, "Встреча туристов/Иностранцев" },
                    { 29, "Встреча болельщиков/Фанатов" },
                    { 30, "Совместный активные отдых" },
                    { 31, "Встреча мусульман" }
                });

            migrationBuilder.InsertData(
                table: "CityList",
                columns: new[] { "Id", "CityName" },
                values: new object[,]
                {
                    { 1, "Алматы" },
                    { 2, "Астана" },
                    { 3, "Конаев" },
                    { 4, "Шымкент" },
                    { 5, "Караганда" },
                    { 6, "Тараз" },
                    { 7, "Семей" },
                    { 8, "Актобе" },
                    { 9, "Актау" },
                    { 10, "Атырау" },
                    { 11, "Костанай" },
                    { 12, "Петропавловск" },
                    { 13, "Павлодар" },
                    { 14, "Уральск" },
                    { 15, "Ускаман" },
                    { 16, "Кызылорда" },
                    { 17, "Талдыкорган" },
                    { 18, "Кокшетау" }
                });

            migrationBuilder.InsertData(
                table: "GenderList",
                columns: new[] { "Id", "GenderName" },
                values: new object[,]
                {
                    { 1, "М" },
                    { 2, "Ж" },
                    { 3, "М/Ж" }
                });

            migrationBuilder.InsertData(
                table: "MeetingCategory",
                columns: new[] { "Id", "NameMeetingCategory" },
                values: new object[,]
                {
                    { 1, "В Заведении/Помещении" },
                    { 2, "На природе" },
                    { 3, "Развлечения и Игры" },
                    { 4, "Релакс, Красота и Здоровье  " },
                    { 5, "Спорт" }
                });

            migrationBuilder.InsertData(
                table: "MeatingPlace",
                columns: new[] { "Id", "MeetingCategoryId", "NameMeatingPlace" },
                values: new object[,]
                {
                    { 1, 1, "Кафе" },
                    { 2, 1, "Ресторан" },
                    { 3, 1, "Кофейня" },
                    { 4, 1, "ТРЦ" },
                    { 5, 1, "Пицерия" },
                    { 6, 1, "Суши бар" },
                    { 7, 1, "Фастфуд" },
                    { 8, 1, "Паб" },
                    { 9, 1, "Пивной бар" },
                    { 10, 1, "Стендап" },
                    { 11, 1, "Лаундж бар" },
                    { 12, 1, "Ночной клуб" },
                    { 13, 1, "Отель" },
                    { 14, 1, "Гостиница" },
                    { 15, 1, "Квартира" },
                    { 16, 1, "Коттедж" },
                    { 17, 1, "Дача" },
                    { 18, 2, "Горы" },
                    { 19, 2, "На свежем воздухе" },
                    { 20, 2, "Пешая прогулка" },
                    { 21, 2, "На лавочке" },
                    { 22, 2, "В парке" },
                    { 23, 2, "Речка" },
                    { 24, 2, "Озеро" },
                    { 25, 3, "Детский игровая зона" },
                    { 26, 3, "Компьютерный клуб " },
                    { 27, 3, "PS клуб" },
                    { 28, 3, "Боулинг центр" },
                    { 29, 3, "Бильярдная" },
                    { 30, 3, "Ледовый Каток" },
                    { 31, 3, "Скейт/Ролл дром" },
                    { 32, 3, "Кинотеатр" },
                    { 33, 3, "Театр" },
                    { 34, 3, "Выставка и Музей" },
                    { 35, 3, "Дискотека" },
                    { 36, 3, "Караоке" },
                    { 37, 3, "Аквапарк" },
                    { 38, 3, "Атракционы" },
                    { 39, 3, "Зоопарк" },
                    { 40, 3, "Ботанический сад" },
                    { 41, 3, "Шахматы" },
                    { 42, 3, "Нарды" },
                    { 43, 3, "Монополия" },
                    { 44, 3, "Мафия" },
                    { 45, 3, "Антикафе" },
                    { 46, 3, "Квеструм" },
                    { 47, 3, "Пеинтбол центр" },
                    { 48, 3, "Тир" },
                    { 49, 3, "Шопинг" },
                    { 50, 4, "Баня" },
                    { 51, 4, "Сауна" },
                    { 52, 4, "Бассеин" },
                    { 53, 4, "СПА салон" },
                    { 54, 4, "Салон красоты" },
                    { 55, 4, "Барбер шоп" },
                    { 56, 4, "Санаторий" },
                    { 57, 4, "Зона отдыха" },
                    { 58, 4, "Массажный центр" },
                    { 59, 5, "Теннисный корт" },
                    { 60, 5, "Настольный теннис" },
                    { 61, 5, "Футбольное поле" },
                    { 62, 5, "Баскетбольное поле" },
                    { 63, 5, "Волейбольная площадка" },
                    { 64, 5, "Тренажёрный зал" },
                    { 65, 5, "Фитнес клуб" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccedEventUser_EventId",
                table: "AccedEventUser",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_AccedEventUser_UserId",
                table: "AccedEventUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_AimOfTheMeetingId",
                table: "Events",
                column: "AimOfTheMeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_CityId",
                table: "Events",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_GenderId",
                table: "Events",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_MeatingPlaceId",
                table: "Events",
                column: "MeatingPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_MeetingCategoryId",
                table: "Events",
                column: "MeetingCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_UserId",
                table: "Events",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IntermediateUser_CityId",
                table: "IntermediateUser",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_IntermediateUser_GenderId",
                table: "IntermediateUser",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_MeatingPlace_MeetingCategoryId",
                table: "MeatingPlace",
                column: "MeetingCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEvent_EventId",
                table: "UserEvent",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEvent_UserId",
                table: "UserEvent",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CityId",
                table: "Users",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_GenderId",
                table: "Users",
                column: "GenderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccedEventUser");

            migrationBuilder.DropTable(
                name: "IntermediateUser");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "NotificationModel");

            migrationBuilder.DropTable(
                name: "PhoneEditing");

            migrationBuilder.DropTable(
                name: "UserEvent");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "AimOfTheMeeting");

            migrationBuilder.DropTable(
                name: "MeatingPlace");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "MeetingCategory");

            migrationBuilder.DropTable(
                name: "CityList");

            migrationBuilder.DropTable(
                name: "GenderList");
        }
    }
}
