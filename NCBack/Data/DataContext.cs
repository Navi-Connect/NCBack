﻿using Microsoft.EntityFrameworkCore;
using NCBack.Models;
using NCBack.NotificationModels;

namespace NCBack.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        /*AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);*/
    }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<RefreshToken> RefreshToken { get; set; }
    public virtual DbSet<IntermediateUser> IntermediateUser { get; set; }
    public virtual DbSet<News> News { get; set; }
    public virtual DbSet<Event> Events { get; set; }
    public virtual DbSet<UserEvent> UserEvent { get; set; }
    public virtual DbSet<AccedEventUser> AccedEventUser { get; set; } 
    public virtual DbSet<AimOfTheMeeting> AimOfTheMeeting { get; set; } 
    public virtual DbSet<MeetingCategory> MeetingCategory { get; set; } 
    public virtual DbSet<MeatingPlace> MeatingPlace { get; set; }
    public virtual DbSet<CityList> CityList { get; set; }
    public virtual DbSet<GenderList> GenderList { get; set; }
    public virtual DbSet<PhoneEditing> PhoneEditing { get; set; }

    public virtual DbSet<NotificationModel> NotificationModel { get; set; }
    public virtual DbSet<AccedReporting> AccedReporting { get; set; }
    /*public virtual DbSet<MyInterests> MyInterests { get; set; }
    public virtual DbSet<MainСategories> MainСategories { get; set; }*/
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<AimOfTheMeeting>().HasData(
            new { Id = 1, NameAimOfTheMeeting = "Знакомство и общение" }, 
            new { Id = 2, NameAimOfTheMeeting = "Поиск друга/Собеседника" },
            new { Id = 3, NameAimOfTheMeeting = "Свидания и поиск второй половинки" }, 
            new { Id = 4, NameAimOfTheMeeting = "Для родителей (Прогулка с детьми)" },
            new { Id = 5, NameAimOfTheMeeting = "Встреча мусульман" }, 
            new { Id = 6, NameAimOfTheMeeting = "Обсуждение фильма/сериала/книги" },
            new { Id = 7, NameAimOfTheMeeting = "Совместный активные отдых" }, 
            new { Id = 8, NameAimOfTheMeeting = "Совместные тренировки" },
            new { Id = 9, NameAimOfTheMeeting = "Обсуждение разных проблем" }, 
            new { Id = 10, NameAimOfTheMeeting = "Саморазвитие, обучение" },
            new { Id = 11, NameAimOfTheMeeting = "Совместный отдых" }, 
            new { Id = 12, NameAimOfTheMeeting = "Рыбалка и охота " },
            new { Id = 13, NameAimOfTheMeeting = "Совместные онлайн игры" }, 
            new { Id = 14, NameAimOfTheMeeting = "Совместные спортивные игры" },
            new { Id = 15, NameAimOfTheMeeting = "Душевный разговор" }, 
            new { Id = 16, NameAimOfTheMeeting = "Знакомства и встречи молодых людей" },
            new { Id = 17, NameAimOfTheMeeting = "Практика языков" }, 
            new { Id = 18, NameAimOfTheMeeting = "Обсуждение рабочих моментов" },
            new { Id = 19, NameAimOfTheMeeting = "Встреча людей с ограниченными возможностями" }, 
            new { Id = 20, NameAimOfTheMeeting = "Прогулка с животными" },
            new { Id = 21, NameAimOfTheMeeting = "Совместное путешествие, туризм" }, 
            new { Id = 22, NameAimOfTheMeeting = "Съёмки контента" },
            new { Id = 23, NameAimOfTheMeeting = "Танцы" }, 
            new { Id = 24, NameAimOfTheMeeting = "Послушать/Записать музыку" },
            new { Id = 25, NameAimOfTheMeeting = "Творчество и изобразительное искусство" }, 
            new { Id = 26, NameAimOfTheMeeting = "Встреча пожилых людей " },
            new { Id = 27, NameAimOfTheMeeting = "Только девушки" }, 
            new { Id = 28, NameAimOfTheMeeting = "Встреча туристов/Иностранцев" },
            new { Id = 29, NameAimOfTheMeeting = "Встреча болельщиков/Фанатов" }, 
            new { Id = 30, NameAimOfTheMeeting = "Совместная выпивка" },
            new { Id = 31, NameAimOfTheMeeting = "Обсуждение идеи/бизнеса (брейншторм)" }
        );
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<MeetingCategory>().HasData(
            new { Id = 1, NameMeetingCategory = "В Заведении/Помещении" },
            new { Id = 2, NameMeetingCategory = "На природе" },
            new { Id = 3, NameMeetingCategory = "Развлечения и Игры" },
            new { Id = 4, NameMeetingCategory = "Релакс, Красота и Здоровье  " },
            new { Id = 5, NameMeetingCategory = "Спорт" }
        );
        
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<MeatingPlace>().HasData(
            new { Id = 1, NameMeatingPlace = "Кафе", MeetingCategoryId =  1},
            new { Id = 2, NameMeatingPlace = "Ресторан", MeetingCategoryId =  1},
            new { Id = 3, NameMeatingPlace = "Кофейня", MeetingCategoryId =  1},
            new { Id = 4, NameMeatingPlace = "ТРЦ", MeetingCategoryId =  1},
            new { Id = 5, NameMeatingPlace = "Пицерия", MeetingCategoryId =  1},
            new { Id = 6, NameMeatingPlace = "Суши бар", MeetingCategoryId =  1},
            new { Id = 7, NameMeatingPlace = "Фастфуд", MeetingCategoryId =  1},
            new { Id = 8, NameMeatingPlace = "Паб", MeetingCategoryId =  1},
            new { Id = 9, NameMeatingPlace = "Пивной бар", MeetingCategoryId =  1},
            new { Id = 10, NameMeatingPlace = "Стендап", MeetingCategoryId =  1},
            new { Id = 11, NameMeatingPlace = "Лаундж бар", MeetingCategoryId =  1},
            new { Id = 12, NameMeatingPlace = "Ночной клуб", MeetingCategoryId =  1},
            new { Id = 13, NameMeatingPlace = "Отель", MeetingCategoryId =  1},
            new { Id = 14, NameMeatingPlace = "Гостиница", MeetingCategoryId =  1},
            new { Id = 15, NameMeatingPlace = "Квартира", MeetingCategoryId = 1},
            new { Id = 16, NameMeatingPlace = "Коттедж", MeetingCategoryId =  1},
            new { Id = 17, NameMeatingPlace = "Дача", MeetingCategoryId =  1},
            new { Id = 18, NameMeatingPlace = "Горы", MeetingCategoryId =  2},
            new { Id = 19, NameMeatingPlace = "На свежем воздухе", MeetingCategoryId =  2},
            new { Id = 20, NameMeatingPlace = "Пешая прогулка", MeetingCategoryId =  2},
            new { Id = 21, NameMeatingPlace = "На лавочке", MeetingCategoryId =  2},
            new { Id = 22, NameMeatingPlace = "В парке", MeetingCategoryId =  2},
            new { Id = 23, NameMeatingPlace = "Речка", MeetingCategoryId =  2},
            new { Id = 24, NameMeatingPlace = "Озеро", MeetingCategoryId =  2},
            new { Id = 25, NameMeatingPlace = "Детский игровая зона", MeetingCategoryId =  3},
            new { Id = 26, NameMeatingPlace = "Компьютерный клуб ", MeetingCategoryId = 3},
            new { Id = 27, NameMeatingPlace = "PS клуб", MeetingCategoryId =  3},
            new { Id = 28, NameMeatingPlace = "Боулинг центр", MeetingCategoryId =  3},
            new { Id = 29, NameMeatingPlace = "Бильярдная", MeetingCategoryId =  3},
            new { Id = 30, NameMeatingPlace = "Ледовый Каток", MeetingCategoryId =  3},
            new { Id = 31, NameMeatingPlace = "Скейт/Ролл дром", MeetingCategoryId =  3},
            new { Id = 32, NameMeatingPlace = "Кинотеатр", MeetingCategoryId =  3},
            new { Id = 33, NameMeatingPlace = "Театр", MeetingCategoryId =  3},
            new { Id = 34, NameMeatingPlace = "Выставка и Музей", MeetingCategoryId =  3},
            new { Id = 35, NameMeatingPlace = "Дискотека", MeetingCategoryId =  3},
            new { Id = 36, NameMeatingPlace = "Караоке", MeetingCategoryId =  3},
            new { Id = 37, NameMeatingPlace = "Аквапарк", MeetingCategoryId =  3},
            new { Id = 38, NameMeatingPlace = "Атракционы", MeetingCategoryId =  3},
            new { Id = 39, NameMeatingPlace = "Зоопарк", MeetingCategoryId =  3},
            new { Id = 40, NameMeatingPlace = "Ботанический сад", MeetingCategoryId =  3},
            new { Id = 41, NameMeatingPlace = "Шахматы", MeetingCategoryId =  3},
            new { Id = 42, NameMeatingPlace = "Нарды", MeetingCategoryId =  3},
            new { Id = 43, NameMeatingPlace = "Монополия", MeetingCategoryId =  3},
            new { Id = 44, NameMeatingPlace = "Мафия", MeetingCategoryId =  3},
            new { Id = 45, NameMeatingPlace = "Антикафе", MeetingCategoryId =  3},
            new { Id = 46, NameMeatingPlace = "Квеструм", MeetingCategoryId =  3},
            new { Id = 47, NameMeatingPlace = "Пеинтбол центр", MeetingCategoryId =  3},
            new { Id = 48, NameMeatingPlace = "Тир", MeetingCategoryId =  3},
            new { Id = 49, NameMeatingPlace = "Шопинг", MeetingCategoryId =  3},
            new { Id = 50, NameMeatingPlace = "Баня", MeetingCategoryId = 4},
            new { Id = 51, NameMeatingPlace = "Сауна", MeetingCategoryId =  4},
            new { Id = 52, NameMeatingPlace = "Бассеин", MeetingCategoryId =  4},
            new { Id = 53, NameMeatingPlace = "СПА салон", MeetingCategoryId =  4},
            new { Id = 54, NameMeatingPlace = "Салон красоты", MeetingCategoryId =  4},
            new { Id = 55, NameMeatingPlace = "Барбер шоп", MeetingCategoryId =  4},
            new { Id = 56, NameMeatingPlace = "Санаторий", MeetingCategoryId =  4},
            new { Id = 57, NameMeatingPlace = "Зона отдыха", MeetingCategoryId =  4},
            new { Id = 58, NameMeatingPlace = "Массажный центр", MeetingCategoryId =  4},
            new { Id = 59, NameMeatingPlace = "Теннисный корт", MeetingCategoryId =  5},
            new { Id = 60, NameMeatingPlace = "Настольный теннис", MeetingCategoryId =  5},
            new { Id = 61, NameMeatingPlace = "Футбольное поле", MeetingCategoryId =  5},
            new { Id = 62, NameMeatingPlace = "Баскетбольное поле", MeetingCategoryId =  5},
            new { Id = 63, NameMeatingPlace = "Волейбольная площадка", MeetingCategoryId =  5},
            new { Id = 64, NameMeatingPlace = "Тренажёрный зал", MeetingCategoryId =  5},
            new { Id = 65, NameMeatingPlace = "Фитнес клуб", MeetingCategoryId =  5}
        );
        
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<CityList>().HasData(
            new { Id = 1, CityName = "Алматы" },
            new { Id = 2, CityName = "Астана" },
            new { Id = 3, CityName = "Конаев" },
            new { Id = 4, CityName = "Шымкент" },
            new { Id = 5, CityName = "Караганда" },
            new { Id = 6, CityName = "Тараз" },
            new { Id = 7, CityName = "Семей" },
            new { Id = 8, CityName = "Актобе" },
            new { Id = 9, CityName = "Актау" },
            new { Id = 10, CityName = "Атырау" },
            new { Id = 11, CityName = "Костанай" },
            new { Id = 12, CityName = "Петропавловск" },
            new { Id = 13, CityName = "Павлодар" },
            new { Id = 14, CityName = "Уральск" },
            new { Id = 15, CityName = "Ускаман" },
            new { Id = 16, CityName = "Кызылорда" },
            new { Id = 17, CityName = "Талдыкорган" },
            new { Id = 18, CityName = "Кокшетау" }
      
        );
        
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<GenderList>().HasData(
            new { Id = 1, GenderName = "М" },
            new { Id = 2, GenderName = "Ж" }
        );
        
        /*
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<MyInterests>().HasData(
            new { Id = 1, NameMyInterests = "Боевые единоборства" },
            new { Id = 2, NameMyInterests = "Виды музыки" },
            new { Id = 3, NameMyInterests = "Другие виды спорта" },
            new { Id = 4, NameMyInterests = "Животные и растения" },
            new { Id = 5, NameMyInterests = "Изучение языков" },
            new { Id = 6, NameMyInterests = "Коллекционирование" },
            new { Id = 7, NameMyInterests = "Кинематография" },
            new { Id = 8, NameMyInterests = "Кулинария и предпочтения" },
            new { Id = 9, NameMyInterests = "Музыкальные интрументы" },
            new { Id = 10, NameMyInterests = "Профессии " },
            new { Id = 11, NameMyInterests = "Психология и саморазвитие" },
            new { Id = 12, NameMyInterests = "Религия" },
            new { Id = 13, NameMyInterests = "Социальные сети и работа в них" },
            new { Id = 14, NameMyInterests = "Спортивные игры" },
            new { Id = 15, NameMyInterests = "Танцы" },
            new { Id = 16, NameMyInterests = "Творческие интересы" },
            new { Id = 17, NameMyInterests = "Техника и инновации" },
            new { Id = 18, NameMyInterests = "Транспортные средства" },
            new { Id = 19, NameMyInterests = "Экстрим" }
        );
        
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<MainСategories>().HasData(
            new { Id = 1, NameMainСategories = "#Айкидо", MyInterestsId = 1 },
            new { Id = 2, NameMainСategories = "#БДД (Бразильское Джиу Джитсу)", MyInterestsId = 1 },
            new { Id = 3, NameMainСategories = "#БоевоеСамбо", MyInterestsId = 1 },
            new { Id = 4, NameMainСategories = "#Бокс", MyInterestsId = 1 },
            new { Id = 5, NameMainСategories = "#БразильскоеДД (Джиу-Джитсу)", MyInterestsId = 1 },
            new { Id = 6, NameMainСategories = "#ВольнаяБорьба", MyInterestsId = 1 },
            new { Id = 7, NameMainСategories = "#Греко-римскаяБорьба", MyInterestsId = 1 },
            new { Id = 8, NameMainСategories = "#Джиу-Джитсу ", MyInterestsId = 1 },
            new { Id = 9, NameMainСategories = "#Дзюдо", MyInterestsId = 1 },
            new { Id = 10, NameMainСategories = "#ДругиеБоевыеЕдиноборства", MyInterestsId = 1 },
            new { Id = 11, NameMainСategories = "#Карате", MyInterestsId = 1 },
            new { Id = 12, NameMainСategories = "#Кикбоксинг", MyInterestsId = 1 },
            new { Id = 13, NameMainСategories = "#МиксФайт", MyInterestsId = 1 },
            new { Id = 14, NameMainСategories = "#Муай-Тай", MyInterestsId = 1 },
            new { Id = 15, NameMainСategories = "#РукопашныйБой", MyInterestsId = 1 },
            new { Id = 16, NameMainСategories = "#Таеквондо", MyInterestsId = 1 },
            new { Id = 17, NameMainСategories = "#Ушу", MyInterestsId = 1 },
            new { Id = 18, NameMainСategories = "#Акапелла", MyInterestsId = 2 },
            new { Id = 19, NameMainСategories = "#Блюз", MyInterestsId = 2 },
            new { Id = 20, NameMainСategories = "#Джазз", MyInterestsId = 2 },
            new { Id = 21, NameMainСategories = "#ДуховнаяМузыка", MyInterestsId = 2 },
            new { Id = 22, NameMainСategories = "#Кантри", MyInterestsId = 2 },
            new { Id = 23, NameMainСategories = "#Кл.Музыка (Классическая музыка)", MyInterestsId = 2 },
            new { Id = 24, NameMainСategories = "#КлубнаяМузыка", MyInterestsId = 2 },
            new { Id = 25, NameMainСategories = "#Поп", MyInterestsId = 2 },
            new { Id = 26, NameMainСategories = "#Регги", MyInterestsId = 2 },
            new { Id = 27, NameMainСategories = "#РелигиознаяМузыка", MyInterestsId = 2 },
            new { Id = 28, NameMainСategories = "#Рок", MyInterestsId = 2 },
            new { Id = 29, NameMainСategories = "#РокнРолл", MyInterestsId = 2 },
            new { Id = 30, NameMainСategories = "#Рэп", MyInterestsId = 2 },
            new { Id = 31, NameMainСategories = "#Техно", MyInterestsId = 2 },
            new { Id = 32, NameMainСategories = "#ХевиМетал", MyInterestsId = 2 },
            new { Id = 33, NameMainСategories = "#Шансон", MyInterestsId = 2 },
            new { Id = 34, NameMainСategories = "#Акробатика", MyInterestsId = 3 },
            new { Id = 35, NameMainСategories = "#Армреслинг", MyInterestsId = 3 },
            new { Id = 36, NameMainСategories = "#Аэробика", MyInterestsId = 3 },
            new { Id = 37, NameMainСategories = "#Бодибилдинг", MyInterestsId = 3 },
            new { Id = 38, NameMainСategories = "#ГирьевойСпорт", MyInterestsId = 3 },
            new { Id = 39, NameMainСategories = "#Гребля", MyInterestsId = 3 },
            new { Id = 40, NameMainСategories = "#Картинг", MyInterestsId = 3 },
            new { Id = 41, NameMainСategories = "#КонныйСпорт", MyInterestsId = 3 },
            new { Id = 42, NameMainСategories = "#ЛёгкаяАтлетика", MyInterestsId = 3 },
            new { Id = 43, NameMainСategories = "#Нарды", MyInterestsId = 3 },
            new { Id = 44, NameMainСategories = "#Панкратион", MyInterestsId = 3 },
            new { Id = 45, NameMainСategories = "#Пауэлифтинг", MyInterestsId = 3 },
            new { Id = 46, NameMainСategories = "#Пеинтбол", MyInterestsId = 3 },
            new { Id = 47, NameMainСategories = "#Пилатес", MyInterestsId = 3 },
            new { Id = 48, NameMainСategories = "#Пятиборье", MyInterestsId = 3 },
            new { Id = 49, NameMainСategories = "#РыболовныйСпорт", MyInterestsId = 3 },
            new { Id = 50, NameMainСategories = "#СпортдляИнвалидов", MyInterestsId = 3 },
            new { Id = 51, NameMainСategories = "#СтрелковыйСпорт", MyInterestsId = 3 },
            new { Id = 52, NameMainСategories = "#Стритбол", MyInterestsId = 3 },
            new { Id = 53, NameMainСategories = "#Триатлон", MyInterestsId = 3 },
            new { Id = 54, NameMainСategories = "#ТяжёлаяАтлетика", MyInterestsId = 3 },
            new { Id = 55, NameMainСategories = "#Фехтование", MyInterestsId = 3 },
            new { Id = 56, NameMainСategories = "#ФигурноеКатание", MyInterestsId = 3 },
            new { Id = 57, NameMainСategories = "#Фитнес", MyInterestsId = 3 },
            new { Id = 58, NameMainСategories = "#Ходьба", MyInterestsId = 3 },
            new { Id = 59, NameMainСategories = "#Шахматы", MyInterestsId = 3 },
            new { Id = 60, NameMainСategories = "#Шашки", MyInterestsId = 3 },
            new { Id = 61, NameMainСategories = "#ВыращиваниеЦветов", MyInterestsId = 4 },
            new { Id = 62, NameMainСategories = "#ДикиеЖивотные", MyInterestsId = 4 },
            new { Id = 63, NameMainСategories = "#Животноводство", MyInterestsId = 4 },
            new { Id = 64, NameMainСategories = "#Кошки", MyInterestsId = 4 },
            new { Id = 65, NameMainСategories = "#Кролики", MyInterestsId = 4 },
            new { Id = 66, NameMainСategories = "#Лошади", MyInterestsId = 4 },
            new { Id = 67, NameMainСategories = "#Попугаи", MyInterestsId = 4 },
            new { Id = 68, NameMainСategories = "#Растениеводство", MyInterestsId = 4 },
            new { Id = 69, NameMainСategories = "#Рыбы", MyInterestsId = 4 },
            new { Id = 70, NameMainСategories = "#Собаки", MyInterestsId = 4 },
            new { Id = 71, NameMainСategories = "#Теплицы", MyInterestsId = 4 },
            new { Id = 72, NameMainСategories = "#Хомяки", MyInterestsId = 4 },
            new { Id = 73, NameMainСategories = "#АнглийскийЯзык", MyInterestsId = 5 },
            new { Id = 74, NameMainСategories = "#АрабскийЯзык", MyInterestsId = 5 },
            new { Id = 75, NameMainСategories = "#АрмянскийЯзык", MyInterestsId = 5 },
            new { Id = 76, NameMainСategories = "#АфроЯзыки", MyInterestsId = 5 },
            new { Id = 77, NameMainСategories = "#ГреческийЯзык", MyInterestsId = 5 },
            new { Id = 78, NameMainСategories = "#ГрузинскийЯзык", MyInterestsId = 5 },
            new { Id = 79, NameMainСategories = "#ЕвроЯзыки", MyInterestsId = 5 },
            new { Id = 80, NameMainСategories = "#Иврит", MyInterestsId = 5 },
            new { Id = 81, NameMainСategories = "#ИспанскийЯзык", MyInterestsId = 5 },
            new { Id = 82, NameMainСategories = "#КазахскийЯзык", MyInterestsId = 5 },
            new { Id = 83, NameMainСategories = "#КитайскийЯзык", MyInterestsId = 5 },
            new { Id = 84, NameMainСategories = "#КорейскийЯзык", MyInterestsId = 5 },
            new { Id = 85, NameMainСategories = "#ЛатинскийЯзык", MyInterestsId = 5 },
            new { Id = 86, NameMainСategories = "#РусскийЯзык", MyInterestsId = 5 },
            new { Id = 87, NameMainСategories = "#ТатарскийЯзык", MyInterestsId = 5 },
            new { Id = 88, NameMainСategories = "#ТурецкийЯзык", MyInterestsId = 5 },
            new { Id = 89, NameMainСategories = "#ФранцузскийЯзык", MyInterestsId = 5 },
            new { Id = 90, NameMainСategories = "#Хинди", MyInterestsId = 5 },
            new { Id = 91, NameMainСategories = "#ЯпонскийЯзык", MyInterestsId = 5 },
            new { Id = 92, NameMainСategories = "#Антиквариат", MyInterestsId = 6 },
            new { Id = 93, NameMainСategories = "#ДрКоллекц-ние", MyInterestsId = 6 },
            new { Id = 94, NameMainСategories = "#Комиксы", MyInterestsId = 6 },
            new { Id = 95, NameMainСategories = "#Купюры", MyInterestsId = 6 },
            new { Id = 96, NameMainСategories = "#Марки", MyInterestsId = 6 },
            new { Id = 97, NameMainСategories = "#Монеты", MyInterestsId = 6 },
            new { Id = 98, NameMainСategories = "#Аниме", MyInterestsId = 7 },
            new { Id = 99, NameMainСategories = "#Боевики", MyInterestsId = 7 },
            new { Id = 100, NameMainСategories = "#Вестерны", MyInterestsId = 7 },
            new { Id = 101, NameMainСategories = "#ВоенноеКино", MyInterestsId = 7 },
            new { Id = 102, NameMainСategories = "#Детективы", MyInterestsId = 7 },
            new { Id = 103, NameMainСategories = "#Драмы", MyInterestsId = 7 },
            new { Id = 104, NameMainСategories = "#ЗарубежноеКино", MyInterestsId = 7 },
            new { Id = 105, NameMainСategories = "#КазахскоеКино", MyInterestsId = 7 },
            new { Id = 106, NameMainСategories = "#КлассикаЖанра", MyInterestsId = 7 },
            new { Id = 107, NameMainСategories = "#Комедии", MyInterestsId = 7 },
            new { Id = 108, NameMainСategories = "#КорейскиеДорамы", MyInterestsId = 7 },
            new { Id = 109, NameMainСategories = "#КорейскиеСериалы", MyInterestsId = 7 },
            new { Id = 110, NameMainСategories = "#Мелодрамы", MyInterestsId = 7 },
            new { Id = 111, NameMainСategories = "#Мультфильмы", MyInterestsId = 7 },
            new { Id = 112, NameMainСategories = "#Приключения", MyInterestsId = 7 },
            new { Id = 113, NameMainСategories = "#Сериалы", MyInterestsId = 7 },
            new { Id = 114, NameMainСategories = "#Триллеры", MyInterestsId = 7 },
            new { Id = 115, NameMainСategories = "#Ужасы", MyInterestsId = 7 },
            new { Id = 116, NameMainСategories = "#Фантастика", MyInterestsId = 7 },
            new { Id = 117, NameMainСategories = "#Фильмы", MyInterestsId = 7 },
            new { Id = 118, NameMainСategories = "#Фэнтэзи", MyInterestsId = 7 },
            new { Id = 119, NameMainСategories = "#Вегетарианство", MyInterestsId = 8 },
            new { Id = 120, NameMainСategories = "#ВосточнаяКухня", MyInterestsId = 8 },
            new { Id = 121, NameMainСategories = "#Выпечка", MyInterestsId = 8 },
            new { Id = 122, NameMainСategories = "#ЕвропейскаяКухня", MyInterestsId = 8 },
            new { Id = 123, NameMainСategories = "#КазахскаяКухня", MyInterestsId = 8 },
            new { Id = 124, NameMainСategories = "#КитайскаяКухня", MyInterestsId = 8 },
            new { Id = 125, NameMainСategories = "#Кошерное", MyInterestsId = 8 },
            new { Id = 126, NameMainСategories = "#Пастерианство", MyInterestsId = 8 },
            new { Id = 127, NameMainСategories = "#ПриготовлениеПитания", MyInterestsId = 8 },
            new { Id = 128, NameMainСategories = "#Салаты", MyInterestsId = 8 },
            new { Id = 129, NameMainСategories = "#Суши", MyInterestsId = 8 },
            new { Id = 130, NameMainСategories = "#ТайскаяКухня", MyInterestsId = 8 },
            new { Id = 131, NameMainСategories = "#Торты", MyInterestsId = 8 },
            new { Id = 133, NameMainСategories = "#DJпульты", MyInterestsId = 9 },
            new { Id = 134, NameMainСategories = "#Барабан", MyInterestsId = 9 },
            new { Id = 135, NameMainСategories = "#Гитара", MyInterestsId = 9 },
            new { Id = 136, NameMainСategories = "#Домбра", MyInterestsId = 9 },
            new { Id = 137, NameMainСategories = "#Саксофон", MyInterestsId = 9 },
            new { Id = 138, NameMainСategories = "#Синтезатор", MyInterestsId = 9 },
            new { Id = 139, NameMainСategories = "#Скрипка", MyInterestsId = 9 },
            new { Id = 140, NameMainСategories = "#Фортепиано", MyInterestsId = 9 },
            new { Id = 141, NameMainСategories = "#АктерскоеИскусство", MyInterestsId = 10 },
            new { Id = 142, NameMainСategories = "#Бухгалтерия", MyInterestsId = 10 },
            new { Id = 143, NameMainСategories = "#Видеооператор", MyInterestsId = 10 },
            new { Id = 144, NameMainСategories = "#Животноводство", MyInterestsId = 10 },
            new { Id = 145, NameMainСategories = "#Журналистика", MyInterestsId = 10 },
            new { Id = 146, NameMainСategories = "#Инвестиции", MyInterestsId = 10 },
            new { Id = 147, NameMainСategories = "#История", MyInterestsId = 10 },
            new { Id = 148, NameMainСategories = "#Косметология", MyInterestsId = 10 },
            new { Id = 149, NameMainСategories = "#Культура", MyInterestsId = 10 },
            new { Id = 150, NameMainСategories = "#Маркетинг", MyInterestsId = 10 },
            new { Id = 151, NameMainСategories = "#Медицина", MyInterestsId = 10 },
            new { Id = 152, NameMainСategories = "#Наука", MyInterestsId = 10 },
            new { Id = 153, NameMainСategories = "#Образование", MyInterestsId = 10 },
            new { Id = 154, NameMainСategories = "#Политика", MyInterestsId = 10 },
            new { Id = 155, NameMainСategories = "#Программирование", MyInterestsId = 10 },
            new { Id = 156, NameMainСategories = "#Производство", MyInterestsId = 10 },
            new { Id = 157, NameMainСategories = "#Психология", MyInterestsId = 10 },
            new { Id = 158, NameMainСategories = "#СельскоеХозяйство", MyInterestsId = 10 },
            new { Id = 159, NameMainСategories = "#СетевойМаркетинг", MyInterestsId = 10 },
            new { Id = 160, NameMainСategories = "#СМИ", MyInterestsId = 10 },
            new { Id = 161, NameMainСategories = "#СММ", MyInterestsId = 10 },
            new { Id = 162, NameMainСategories = "#Спорт", MyInterestsId = 10 },
            new { Id = 163, NameMainСategories = "#Стоматология", MyInterestsId = 10 },
            new { Id = 164, NameMainСategories = "#Строительство", MyInterestsId = 10 },
            new { Id = 165, NameMainСategories = "#Торговля", MyInterestsId = 10 },
            new { Id = 166, NameMainСategories = "#Фармацевтика", MyInterestsId = 10 },
            new { Id = 167, NameMainСategories = "#Финансы", MyInterestsId = 10 },
            new { Id = 168, NameMainСategories = "#Хирургия", MyInterestsId = 10 },
            new { Id = 169, NameMainСategories = "#ШвейноеДело", MyInterestsId = 10 },
            new { Id = 170, NameMainСategories = "#Экономика", MyInterestsId = 10 },
            new { Id = 171, NameMainСategories = "#Юриспруденция", MyInterestsId = 10 },
            new { Id = 172, NameMainСategories = "#БизнесМолодость", MyInterestsId = 11 },
            new { Id = 173, NameMainСategories = "#Биоэнерготерапия", MyInterestsId = 11 },
            new { Id = 174, NameMainСategories = "#ГештальтПсихология", MyInterestsId = 11 },
            new { Id = 175, NameMainСategories = "#Гипноз", MyInterestsId = 11 },
            new { Id = 176, NameMainСategories = "#Диетология", MyInterestsId = 11 },
            new { Id = 177, NameMainСategories = "#Йога", MyInterestsId = 11 },
            new { Id = 178, NameMainСategories = "#Коучинг", MyInterestsId = 11 },
            new { Id = 179, NameMainСategories = "#ЛогическиеИгры", MyInterestsId = 11 },
            new { Id = 180, NameMainСategories = "#Медитация", MyInterestsId = 11 },
            new { Id = 181, NameMainСategories = "#Мотивация", MyInterestsId = 11 },
            new { Id = 182, NameMainСategories = "#НЛП", MyInterestsId = 11 },
            new { Id = 183, NameMainСategories = "#Отношения", MyInterestsId = 11 },
            new { Id = 184, NameMainСategories = "#Подсознание", MyInterestsId = 11 },
            new { Id = 185, NameMainСategories = "#Психоанализ", MyInterestsId = 11 },
            new { Id = 186, NameMainСategories = "#Психосоматика", MyInterestsId = 11 },
            new { Id = 187, NameMainСategories = "#Самопознание", MyInterestsId = 11 },
            new { Id = 188, NameMainСategories = "#Трансерфинг", MyInterestsId = 11 },
            new { Id = 189, NameMainСategories = "#Физиогномика", MyInterestsId = 11 },
            new { Id = 190, NameMainСategories = "#ЧтениеКниг", MyInterestsId = 11 },
            new { Id = 191, NameMainСategories = "#Эпигенетика", MyInterestsId = 11 },
            new { Id = 192, NameMainСategories = "#Ислам", MyInterestsId = 12 },
            new { Id = 193, NameMainСategories = "#Православие", MyInterestsId = 12 },
            new { Id = 194, NameMainСategories = "#Тенгрианство", MyInterestsId = 12 },
            new { Id = 195, NameMainСategories = "#Facebook", MyInterestsId = 13 },
            new { Id = 196, NameMainСategories = "#Instagram", MyInterestsId = 13 },
            new { Id = 197, NameMainСategories = "#TikTok", MyInterestsId = 13 },
            new { Id = 198, NameMainСategories = "#Блогерство", MyInterestsId = 13 },
            new { Id = 199, NameMainСategories = "+NaviConnect", MyInterestsId = 13 },
            new { Id = 200, NameMainСategories = "#Баскетбол", MyInterestsId = 14 },
            new { Id = 201, NameMainСategories = "#Бильярд", MyInterestsId = 14 },
            new { Id = 202, NameMainСategories = "#Боулинг", MyInterestsId = 14 },
            new { Id = 203, NameMainСategories = "#ВелоСпорт", MyInterestsId = 14 },
            new { Id = 204, NameMainСategories = "#Волейбол", MyInterestsId = 14 },
            new { Id = 205, NameMainСategories = "#Гольф", MyInterestsId = 14 },
            new { Id = 206, NameMainСategories = "#Дартс", MyInterestsId = 14 },
            new { Id = 207, NameMainСategories = "#КиберСпорт", MyInterestsId = 14 },
            new { Id = 208, NameMainСategories = "#Коньки", MyInterestsId = 14 },
            new { Id = 209, NameMainСategories = "#Лыжи", MyInterestsId = 14 },
            new { Id = 210, NameMainСategories = "#НастольныйТеннис", MyInterestsId = 14 },
            new { Id = 211, NameMainСategories = "#Плавание", MyInterestsId = 14 },
            new { Id = 212, NameMainСategories = "#Самокат", MyInterestsId = 14 },
            new { Id = 213, NameMainСategories = "#Скейтборд", MyInterestsId = 14 },
            new { Id = 214, NameMainСategories = "#Сноуборд", MyInterestsId = 14 },
            new { Id = 215, NameMainСategories = "#Теннис", MyInterestsId = 14 },
            new { Id = 216, NameMainСategories = "#Футбол", MyInterestsId = 14 },
            new { Id = 217, NameMainСategories = "#Футзал", MyInterestsId = 14 },
            new { Id = 218, NameMainСategories = "#Хоккей", MyInterestsId = 14 },
            new { Id = 219, NameMainСategories = "#Балет", MyInterestsId = 15 },
            new { Id = 220, NameMainСategories = "#Брейк", MyInterestsId = 15 },
            new { Id = 221, NameMainСategories = "#Вальс", MyInterestsId = 15 },
            new { Id = 222, NameMainСategories = "#ВосточныеТанцы", MyInterestsId = 15 },
            new { Id = 223, NameMainСategories = "#ДискотечныеТанцы", MyInterestsId = 15 },
            new { Id = 224, NameMainСategories = "#ДругиеТанцы", MyInterestsId = 15 },
            new { Id = 225, NameMainСategories = "#Контемпорари", MyInterestsId = 15 },
            new { Id = 226, NameMainСategories = "#Крамп", MyInterestsId = 15 },
            new { Id = 227, NameMainСategories = "#Лезгинка", MyInterestsId = 15 },
            new { Id = 228, NameMainСategories = "#Модерн", MyInterestsId = 15 },
            new { Id = 229, NameMainСategories = "#Румба", MyInterestsId = 15 },
            new { Id = 230, NameMainСategories = "#Сальса", MyInterestsId = 15 },
            new { Id = 231, NameMainСategories = "#Самба", MyInterestsId = 15 },
            new { Id = 232, NameMainСategories = "#Танго", MyInterestsId = 15 },
            new { Id = 233, NameMainСategories = "#ТикТокТанцы", MyInterestsId = 15 },
            new { Id = 234, NameMainСategories = "#ХипХоп", MyInterestsId = 15 },
            new { Id = 235, NameMainСategories = "#Бачата", MyInterestsId = 15 },
            new { Id = 236, NameMainСategories = "#Бисероплетение", MyInterestsId = 16 },
            new { Id = 237, NameMainСategories = "#ВебРисование", MyInterestsId = 16 },
            new { Id = 238, NameMainСategories = "#Видеография", MyInterestsId = 16 },
            new { Id = 239, NameMainСategories = "#Визаж", MyInterestsId = 16 },
            new { Id = 240, NameMainСategories = "#ГончарноеДело", MyInterestsId = 16 },
            new { Id = 241, NameMainСategories = "#Диджеинг", MyInterestsId = 16 },
            new { Id = 242, NameMainСategories = "#Дизайн", MyInterestsId = 16 },
            new { Id = 243, NameMainСategories = "#Макияж", MyInterestsId = 16 },
            new { Id = 244, NameMainСategories = "#Мастеринг", MyInterestsId = 16 },
            new { Id = 245, NameMainСategories = "#Оформление", MyInterestsId = 16 },
            new { Id = 246, NameMainСategories = "#Писательство", MyInterestsId = 16 },
            new { Id = 247, NameMainСategories = "#Рисование", MyInterestsId = 16 },
            new { Id = 248, NameMainСategories = "#Скульптура", MyInterestsId = 16 },
            new { Id = 249, NameMainСategories = "#Фотография", MyInterestsId = 16 },
            new { Id = 250, NameMainСategories = "#ITинновации", MyInterestsId = 17 },
            new { Id = 251, NameMainСategories = "#MOBинновации", MyInterestsId = 17 },
            new { Id = 252, NameMainСategories = "#ВрАрИгры", MyInterestsId = 17 },
            new { Id = 253, NameMainСategories = "#Дроны", MyInterestsId = 17 },
            new { Id = 254, NameMainСategories = "#ЗелёнаяЭкономика", MyInterestsId = 17 },
            new { Id = 255, NameMainСategories = "#Инновации", MyInterestsId = 17 },
            new { Id = 256, NameMainСategories = "#Компьютеры", MyInterestsId = 17 },
            new { Id = 257, NameMainСategories = "#Майнинг", MyInterestsId = 17 },
            new { Id = 258, NameMainСategories = "#Смартфоны", MyInterestsId = 17 },
            new { Id = 259, NameMainСategories = "#Электромобили", MyInterestsId = 17 },
            new { Id = 260, NameMainСategories = "#Квадракоптеры", MyInterestsId = 17 },
            new { Id = 261, NameMainСategories = "#Автобусы", MyInterestsId = 18 },
            new { Id = 262, NameMainСategories = "#Автогонки", MyInterestsId = 18 },
            new { Id = 263, NameMainСategories = "#Автомобили", MyInterestsId = 18 },
            new { Id = 264, NameMainСategories = "#Велосипеды", MyInterestsId = 18 },
            new { Id = 265, NameMainСategories = "#Вертолеты", MyInterestsId = 18 },
            new { Id = 266, NameMainСategories = "#ГрузовыеАвто", MyInterestsId = 18 },
            new { Id = 267, NameMainСategories = "#Мотоциклы", MyInterestsId = 18 },
            new { Id = 268, NameMainСategories = "#Поезда", MyInterestsId = 18 },
            new { Id = 269, NameMainСategories = "#РемонтАвто", MyInterestsId = 18 },
            new { Id = 270, NameMainСategories = "#Самолеты", MyInterestsId = 18 },
            new { Id = 271, NameMainСategories = "#ТюнингАвто", MyInterestsId = 18 },
            new { Id = 272, NameMainСategories = "#Элетросамокаты", MyInterestsId = 18 },
            new { Id = 273, NameMainСategories = "#Альпинизм", MyInterestsId = 19 },
            new { Id = 274, NameMainСategories = "#Параплан", MyInterestsId = 19 },
            new { Id = 275, NameMainСategories = "#Парашют", MyInterestsId = 19 },
            new { Id = 276, NameMainСategories = "#Паркур", MyInterestsId = 19 }
        );
        */

    }
}

