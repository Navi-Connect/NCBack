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
                name: "IntermediateUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    City = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<int>(type: "integer", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AvatarPath = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: false),
                    Success = table.Column<bool>(type: "boolean", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntermediateUser", x => x.Id);
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
                    Data = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    City = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<int>(type: "integer", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AvatarPath = table.Column<string>(type: "text", nullable: false),
                    CredoAboutMyself = table.Column<string>(type: "text", nullable: true),
                    LanguageOfCommunication = table.Column<string>(type: "text", nullable: true),
                    Nationality = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<string>(type: "text", nullable: true),
                    MaritalStatus = table.Column<string>(type: "text", nullable: true),
                    GetAcquaintedWith = table.Column<string>(type: "text", nullable: true),
                    MeetFor = table.Column<string>(type: "text", nullable: true),
                    From = table.Column<int>(type: "integer", nullable: true),
                    To = table.Column<int>(type: "integer", nullable: true),
                    IWantToLearn = table.Column<string>(type: "text", nullable: true),
                    PreferredPlaces = table.Column<List<string>>(type: "text[]", nullable: false),
                    Interests = table.Column<List<string>>(type: "text[]", nullable: false),
                    Profession = table.Column<string>(type: "text", nullable: true),
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
                    City = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<string>(type: "text", nullable: true),
                    AgeTo = table.Column<int>(type: "integer", nullable: true),
                    AgeFrom = table.Column<int>(type: "integer", nullable: true),
                    CaltulationType = table.Column<string>(type: "text", nullable: true),
                    CaltulationSum = table.Column<string>(type: "text", nullable: true),
                    LanguageCommunication = table.Column<string>(type: "text", nullable: true),
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
                    { 1, 1, "Отель, Гостиница" },
                    { 2, 1, "Кофейня, Чайхана" },
                    { 3, 1, "Ресторан, Кафе" },
                    { 4, 1, "Вилла, Коттедж, Дача" },
                    { 5, 1, "Трц, Трк" },
                    { 6, 1, "Лаундж бар, Паб, Пивной бар" },
                    { 7, 1, "Квартира, Апартаменты, Пентхауз" },
                    { 8, 1, "Ночной клуб, Диско бар" },
                    { 9, 1, "Пицерия, Суши бар" },
                    { 10, 1, "Фастфуд, Столовая" },
                    { 11, 1, "Стенд Ап/Комеди клуб" },
                    { 12, 1, "Вип ресторан" },
                    { 13, 2, "Горы, Достопримечательности" },
                    { 14, 2, "На свежем воздухе, Пешая прогулка" },
                    { 15, 2, "На лавочке, В парке" },
                    { 16, 2, "Речка, Озёро" },
                    { 17, 2, "Поход, Пикник, Кемпинг" },
                    { 18, 3, "Детский игровый зал, детская площадка" },
                    { 19, 3, "Компьютерный клуб, PS клуб" },
                    { 20, 3, "Боулинг центр, бильярдная" },
                    { 21, 3, "Каток, Скейт/Ролл дром" },
                    { 22, 3, "Кинотеатр, Театр" },
                    { 23, 3, "Выставка и Музей" },
                    { 24, 3, "Дискотека, Караоке, Танц клуб" },
                    { 25, 3, "Аквапарк, Лунапарк, Атракционы" },
                    { 26, 4, "Онлайн комп. и моб. игры" },
                    { 27, 4, "Шахматы, шашки, нарды" },
                    { 28, 4, "Монополия, мафия" },
                    { 29, 4, "Антикафе, Квеструм" },
                    { 30, 4, "Пеинтбол центр, Тир, Полигон" },
                    { 31, 4, "Шопинг и совместные покупки" },
                    { 32, 4, "Зоопарк, Ботанический сад" },
                    { 33, 4, "Баня, Хамам, Сауна" },
                    { 34, 4, "Бассеин, Пляж" },
                    { 35, 4, "Салон СПА, салон красоты, барбер шоп" },
                    { 36, 4, "Курорт, Санаторий, Зона отдыха" },
                    { 37, 4, "Центр массажа" },
                    { 38, 5, "Теннисный корт/зал" },
                    { 39, 5, "Футбольное поле, баскетбольное поле" },
                    { 40, 5, "Волейбольная площадка" },
                    { 41, 5, "Тренажёрный зал/Фитнес клуб" }
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
        }
    }
}
