﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NCBack.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NCBack.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("NCBack.Models.AccedEventUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("EventId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("AccedEventUser");
                });

            modelBuilder.Entity("NCBack.Models.AimOfTheMeeting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("NameAimOfTheMeeting")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AimOfTheMeeting");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            NameAimOfTheMeeting = "Знакомство и общение"
                        },
                        new
                        {
                            Id = 2,
                            NameAimOfTheMeeting = "Поиск друга/Собеседника"
                        },
                        new
                        {
                            Id = 3,
                            NameAimOfTheMeeting = "Свидания и поиск второй половинки"
                        },
                        new
                        {
                            Id = 4,
                            NameAimOfTheMeeting = "Для родителей (Прогулка с детьми)"
                        },
                        new
                        {
                            Id = 5,
                            NameAimOfTheMeeting = "Обсуждение идеи/бизнеса (брейншторм)"
                        },
                        new
                        {
                            Id = 6,
                            NameAimOfTheMeeting = "Обсуждение фильма/сериала/книги"
                        },
                        new
                        {
                            Id = 7,
                            NameAimOfTheMeeting = "Совместная выпивка"
                        },
                        new
                        {
                            Id = 8,
                            NameAimOfTheMeeting = "Совместные тренировки"
                        },
                        new
                        {
                            Id = 9,
                            NameAimOfTheMeeting = "Обсуждение разных проблем"
                        },
                        new
                        {
                            Id = 10,
                            NameAimOfTheMeeting = "Саморазвитие, обучение"
                        },
                        new
                        {
                            Id = 11,
                            NameAimOfTheMeeting = "Совместный отдых"
                        },
                        new
                        {
                            Id = 12,
                            NameAimOfTheMeeting = "Рыбалка и охота "
                        },
                        new
                        {
                            Id = 13,
                            NameAimOfTheMeeting = "Совместные онлайн игры"
                        },
                        new
                        {
                            Id = 14,
                            NameAimOfTheMeeting = "Совместные спортивные игры"
                        },
                        new
                        {
                            Id = 15,
                            NameAimOfTheMeeting = "Душевный разговор"
                        },
                        new
                        {
                            Id = 16,
                            NameAimOfTheMeeting = "Знакомства и встречи молодых людей"
                        },
                        new
                        {
                            Id = 17,
                            NameAimOfTheMeeting = "Практика языков"
                        },
                        new
                        {
                            Id = 18,
                            NameAimOfTheMeeting = "Обсуждение рабочих моментов"
                        },
                        new
                        {
                            Id = 19,
                            NameAimOfTheMeeting = "Встреча людей с ограниченными возможностями"
                        },
                        new
                        {
                            Id = 20,
                            NameAimOfTheMeeting = "Прогулка с животными"
                        },
                        new
                        {
                            Id = 21,
                            NameAimOfTheMeeting = "Совместное путешествие, туризм"
                        },
                        new
                        {
                            Id = 22,
                            NameAimOfTheMeeting = "Съёмки контента"
                        },
                        new
                        {
                            Id = 23,
                            NameAimOfTheMeeting = "Танцы"
                        },
                        new
                        {
                            Id = 24,
                            NameAimOfTheMeeting = "Послушать/Записать музыку"
                        },
                        new
                        {
                            Id = 25,
                            NameAimOfTheMeeting = "Творчество и изобразительное искусство"
                        },
                        new
                        {
                            Id = 26,
                            NameAimOfTheMeeting = "Встреча пожилых людей "
                        },
                        new
                        {
                            Id = 27,
                            NameAimOfTheMeeting = "Только девушки"
                        },
                        new
                        {
                            Id = 28,
                            NameAimOfTheMeeting = "Встреча туристов/Иностранцев"
                        },
                        new
                        {
                            Id = 29,
                            NameAimOfTheMeeting = "Встреча болельщиков/Фанатов"
                        },
                        new
                        {
                            Id = 30,
                            NameAimOfTheMeeting = "Совместный активные отдых"
                        },
                        new
                        {
                            Id = 31,
                            NameAimOfTheMeeting = "Встреча мусульман"
                        });
                });

            modelBuilder.Entity("NCBack.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("AgeFrom")
                        .HasColumnType("integer");

                    b.Property<int?>("AgeTo")
                        .HasColumnType("integer");

                    b.Property<int>("AimOfTheMeetingId")
                        .HasColumnType("integer");

                    b.Property<string>("CaltulationSum")
                        .HasColumnType("text");

                    b.Property<string>("CaltulationType")
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<string>("Gender")
                        .HasColumnType("text");

                    b.Property<string>("IWant")
                        .HasColumnType("text");

                    b.Property<List<string>>("Interests")
                        .HasColumnType("text[]");

                    b.Property<List<string>>("LanguageCommunication")
                        .HasColumnType("text[]");

                    b.Property<double?>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<double?>("Longitude")
                        .HasColumnType("double precision");

                    b.Property<int?>("MeatingPlaceId")
                        .HasColumnType("integer");

                    b.Property<int>("MeetingCategoryId")
                        .HasColumnType("integer");

                    b.Property<int?>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("TimeFinish")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("TimeStart")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AimOfTheMeetingId");

                    b.HasIndex("MeatingPlaceId");

                    b.HasIndex("MeetingCategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("NCBack.Models.IntermediateUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AvatarPath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("Code")
                        .HasColumnType("integer");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("Success")
                        .HasColumnType("boolean");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("IntermediateUser");
                });

            modelBuilder.Entity("NCBack.Models.MeatingPlace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("MeetingCategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("NameMeatingPlace")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("MeetingCategoryId");

                    b.ToTable("MeatingPlace");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            MeetingCategoryId = 1,
                            NameMeatingPlace = "Отель, Гостиница"
                        },
                        new
                        {
                            Id = 2,
                            MeetingCategoryId = 1,
                            NameMeatingPlace = "Кофейня, Чайхана"
                        },
                        new
                        {
                            Id = 3,
                            MeetingCategoryId = 1,
                            NameMeatingPlace = "Ресторан, Кафе"
                        },
                        new
                        {
                            Id = 4,
                            MeetingCategoryId = 1,
                            NameMeatingPlace = "Вилла, Коттедж, Дача"
                        },
                        new
                        {
                            Id = 5,
                            MeetingCategoryId = 1,
                            NameMeatingPlace = "Трц, Трк"
                        },
                        new
                        {
                            Id = 6,
                            MeetingCategoryId = 1,
                            NameMeatingPlace = "Лаундж бар, Паб, Пивной бар"
                        },
                        new
                        {
                            Id = 7,
                            MeetingCategoryId = 1,
                            NameMeatingPlace = "Квартира, Апартаменты, Пентхауз"
                        },
                        new
                        {
                            Id = 8,
                            MeetingCategoryId = 1,
                            NameMeatingPlace = "Ночной клуб, Диско бар"
                        },
                        new
                        {
                            Id = 9,
                            MeetingCategoryId = 1,
                            NameMeatingPlace = "Пицерия, Суши бар"
                        },
                        new
                        {
                            Id = 10,
                            MeetingCategoryId = 1,
                            NameMeatingPlace = "Фастфуд, Столовая"
                        },
                        new
                        {
                            Id = 11,
                            MeetingCategoryId = 1,
                            NameMeatingPlace = "Стенд Ап/Комеди клуб"
                        },
                        new
                        {
                            Id = 12,
                            MeetingCategoryId = 1,
                            NameMeatingPlace = "Вип ресторан"
                        },
                        new
                        {
                            Id = 13,
                            MeetingCategoryId = 2,
                            NameMeatingPlace = "Горы, Достопримечательности"
                        },
                        new
                        {
                            Id = 14,
                            MeetingCategoryId = 2,
                            NameMeatingPlace = "На свежем воздухе, Пешая прогулка"
                        },
                        new
                        {
                            Id = 15,
                            MeetingCategoryId = 2,
                            NameMeatingPlace = "На лавочке, В парке"
                        },
                        new
                        {
                            Id = 16,
                            MeetingCategoryId = 2,
                            NameMeatingPlace = "Речка, Озёро"
                        },
                        new
                        {
                            Id = 17,
                            MeetingCategoryId = 2,
                            NameMeatingPlace = "Поход, Пикник, Кемпинг"
                        },
                        new
                        {
                            Id = 18,
                            MeetingCategoryId = 3,
                            NameMeatingPlace = "Детский игровый зал, детская площадка"
                        },
                        new
                        {
                            Id = 19,
                            MeetingCategoryId = 3,
                            NameMeatingPlace = "Компьютерный клуб, PS клуб"
                        },
                        new
                        {
                            Id = 20,
                            MeetingCategoryId = 3,
                            NameMeatingPlace = "Боулинг центр, бильярдная"
                        },
                        new
                        {
                            Id = 21,
                            MeetingCategoryId = 3,
                            NameMeatingPlace = "Каток, Скейт/Ролл дром"
                        },
                        new
                        {
                            Id = 22,
                            MeetingCategoryId = 3,
                            NameMeatingPlace = "Кинотеатр, Театр"
                        },
                        new
                        {
                            Id = 23,
                            MeetingCategoryId = 3,
                            NameMeatingPlace = "Выставка и Музей"
                        },
                        new
                        {
                            Id = 24,
                            MeetingCategoryId = 3,
                            NameMeatingPlace = "Дискотека, Караоке, Танц клуб"
                        },
                        new
                        {
                            Id = 25,
                            MeetingCategoryId = 3,
                            NameMeatingPlace = "Аквапарк, Лунапарк, Атракционы"
                        },
                        new
                        {
                            Id = 26,
                            MeetingCategoryId = 4,
                            NameMeatingPlace = "Онлайн комп. и моб. игры"
                        },
                        new
                        {
                            Id = 27,
                            MeetingCategoryId = 4,
                            NameMeatingPlace = "Шахматы, шашки, нарды"
                        },
                        new
                        {
                            Id = 28,
                            MeetingCategoryId = 4,
                            NameMeatingPlace = "Монополия, мафия"
                        },
                        new
                        {
                            Id = 29,
                            MeetingCategoryId = 4,
                            NameMeatingPlace = "Антикафе, Квеструм"
                        },
                        new
                        {
                            Id = 30,
                            MeetingCategoryId = 4,
                            NameMeatingPlace = "Пеинтбол центр, Тир, Полигон"
                        },
                        new
                        {
                            Id = 31,
                            MeetingCategoryId = 4,
                            NameMeatingPlace = "Шопинг и совместные покупки"
                        },
                        new
                        {
                            Id = 32,
                            MeetingCategoryId = 4,
                            NameMeatingPlace = "Зоопарк, Ботанический сад"
                        },
                        new
                        {
                            Id = 33,
                            MeetingCategoryId = 4,
                            NameMeatingPlace = "Баня, Хамам, Сауна"
                        },
                        new
                        {
                            Id = 34,
                            MeetingCategoryId = 4,
                            NameMeatingPlace = "Бассеин, Пляж"
                        },
                        new
                        {
                            Id = 35,
                            MeetingCategoryId = 4,
                            NameMeatingPlace = "Салон СПА, салон красоты, барбер шоп"
                        },
                        new
                        {
                            Id = 36,
                            MeetingCategoryId = 4,
                            NameMeatingPlace = "Курорт, Санаторий, Зона отдыха"
                        },
                        new
                        {
                            Id = 37,
                            MeetingCategoryId = 4,
                            NameMeatingPlace = "Центр массажа"
                        },
                        new
                        {
                            Id = 38,
                            MeetingCategoryId = 5,
                            NameMeatingPlace = "Теннисный корт/зал"
                        },
                        new
                        {
                            Id = 39,
                            MeetingCategoryId = 5,
                            NameMeatingPlace = "Футбольное поле, баскетбольное поле"
                        },
                        new
                        {
                            Id = 40,
                            MeetingCategoryId = 5,
                            NameMeatingPlace = "Волейбольная площадка"
                        },
                        new
                        {
                            Id = 41,
                            MeetingCategoryId = 5,
                            NameMeatingPlace = "Тренажёрный зал/Фитнес клуб"
                        });
                });

            modelBuilder.Entity("NCBack.Models.MeetingCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("NameMeetingCategory")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("MeetingCategory");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            NameMeetingCategory = "В Заведении/Помещении"
                        },
                        new
                        {
                            Id = 2,
                            NameMeetingCategory = "На природе"
                        },
                        new
                        {
                            Id = 3,
                            NameMeetingCategory = "Развлечения и Игры"
                        },
                        new
                        {
                            Id = 4,
                            NameMeetingCategory = "Релакс, Красота и Здоровье  "
                        },
                        new
                        {
                            Id = 5,
                            NameMeetingCategory = "Спорт"
                        });
                });

            modelBuilder.Entity("NCBack.Models.News", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Data")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("LinkVideo")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Photo")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("News");
                });

            modelBuilder.Entity("NCBack.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AvatarPath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("Code")
                        .HasColumnType("integer");

                    b.Property<string>("CredoAboutMyself")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("From")
                        .HasColumnType("integer");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Gender")
                        .HasColumnType("text");

                    b.Property<string>("GetAcquaintedWith")
                        .HasColumnType("text");

                    b.Property<string>("IWantToLearn")
                        .HasColumnType("text");

                    b.Property<List<string>>("Interests")
                        .HasColumnType("text[]");

                    b.Property<List<string>>("LanguageOfCommunication")
                        .HasColumnType("text[]");

                    b.Property<string>("MaritalStatus")
                        .HasColumnType("text");

                    b.Property<string>("MeetFor")
                        .HasColumnType("text");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nationality")
                        .HasColumnType("text");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<List<string>>("PreferredPlaces")
                        .HasColumnType("text[]");

                    b.Property<string>("Profession")
                        .HasColumnType("text");

                    b.Property<bool>("Success")
                        .HasColumnType("boolean");

                    b.Property<int?>("To")
                        .HasColumnType("integer");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("NCBack.Models.UserEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("EventId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("UserEvent");
                });

            modelBuilder.Entity("NCBack.Models.AccedEventUser", b =>
                {
                    b.HasOne("NCBack.Models.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NCBack.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NCBack.Models.Event", b =>
                {
                    b.HasOne("NCBack.Models.AimOfTheMeeting", "AimOfTheMeeting")
                        .WithMany()
                        .HasForeignKey("AimOfTheMeetingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NCBack.Models.MeatingPlace", "MeatingPlace")
                        .WithMany()
                        .HasForeignKey("MeatingPlaceId");

                    b.HasOne("NCBack.Models.MeetingCategory", "MeetingCategory")
                        .WithMany()
                        .HasForeignKey("MeetingCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NCBack.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AimOfTheMeeting");

                    b.Navigation("MeatingPlace");

                    b.Navigation("MeetingCategory");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NCBack.Models.MeatingPlace", b =>
                {
                    b.HasOne("NCBack.Models.MeetingCategory", "MeetingCategory")
                        .WithMany()
                        .HasForeignKey("MeetingCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MeetingCategory");
                });

            modelBuilder.Entity("NCBack.Models.UserEvent", b =>
                {
                    b.HasOne("NCBack.Models.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NCBack.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
