﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WofHCalc.Supports
{
    //в этом файле сопоставляются ID из источника с названиями
    [Flags]
    public enum Race //++
    {
        [Description("Не указана")]
        unknown = 0,
        [Description("Индейцы")]
        indians = 1,
        [Description("Европейцы")]
        europeans = 2,
        [Description("Азиаты")]
        asians = 4,
        [Description("Африканцы")]
        africans = 8
    }
    public enum ResName //++
    {
        [Description("Наука")]
        science = 0,
        [Description("Деньги")]
        money = 1,
        [Description("Еда")]
        food = 2,
        [Description("Дерево")]
        wood = 3,
        [Description("Железо")]
        iron = 4,
        [Description("Топливо")]
        fuel = 5,
        [Description("Гранит")]
        stone = 6,
        [Description("Вьюки")]
        horses = 7,
        [Description("Сера")]
        sulfur = 8,
        [Description("Алюминий")]
        aluminium = 9,
        [Description("Уран")]
        uranium = 10,
        [Description("Фрукты")]
        fruit = 11,
        [Description("Кукуруза")]
        corn = 12,
        [Description("Пшеница")]
        grain = 13,
        [Description("Рис")]
        rice = 14,
        [Description("Рыба")]
        fish = 15,
        [Description("Мясо")]
        meat = 16,
        [Description("Вино")]
        wine = 17,
        [Description("Драги")]
        jewelry = 18,
        [Description("Одежда")]
        clothes = 19,
        [Description("Музыка")]
        music = 20,
        [Description("Фильмы")]
        films = 21,
        [Description("Книги")]
        books = 22
    }
    public enum ResProdType //+
    {
        research = 0,
        finance = 1,
        agriculture = 2,
        industry = 3,
    }
    public enum Climate //++
    {
        [Description("Не указан")]
        unknown = 0,
        [Description("Вода")]
        water = 1,
        [Description("Луга")]
        meadows = 2,
        [Description("Степь")]
        steppe = 3,
        [Description("Пустыня")]
        desert = 4,
        [Description("Снега")]
        snow = 5
    }
    public enum Slot //++
    {
        [Description("Равнина")]
        plain = 0,
        [Description("Центр")]
        center = 1,
        [Description("Холм")]
        hill = 2,
        [Description("Вода")]
        water = 3,
        [Description("Защита")]
        fort = 4,
        [Description("Чудо")]
        wounder = 5,
    }
    public enum Terrain //+
    {
        everywhere = 0,
        hill = 1,
        plane_water = 2,
        plane_no_water = 3,
        plane = 4,
        nowhere = 5
    }
    public enum BuildType
    {
        production = 0,
        prodBoost = 1,
        store = 4,
        defence = 5,
        train = 6,
        grown = 7,
        culture = 8,
        administration = 9,
        hide = 10,
        trade = 11,
        embassy = 12,
        corruption = 13,
        watertradespeed = 14,
        waterarmyspeed = 15,
        wonder = 16,
        airdef = 17,
        airarmyspeed = 18,
        fake = 19,
        ecology = 20,
        tradespeed = 21
    }
    public enum AreaImprovementName //++
    {
        [Description("Нет")]
        none = 400,
        [Description("Мост")]
        Bridge = 0,
        [Description("Ирригация")]
        Irrigation = 1,
        [Description("Шахты")]
        Mines = 2,
        [Description("Кампус")]
        Campus = 3,
        [Description("Ярмарка")]
        Fair = 4, //ярмарка
        [Description("ГТС")]
        HydrotechnicalFacility = 5, //гтс
        [Description("Пригород")]
        Suburb = 6, //пригород
        [Description("Заповедник")]
        Reservation = 7, //заповедник
        [Description("Курорт")]
        Resort = 10, //курорт
        [Description("Горнолыжка")]
        SkiResort = 11, //горнолыжка
        [Description("Промысловая зона")]
        FishingArea = 8, //промысловая зоны
        [Description("Исслед. аква")]
        ResearchWaterArea = 9 //аква
    }
    public enum UnitsNames
    {
        [Description("Воин")]
        Warrior = 0,
        [Description("Пращник")]
        Slinger = 1,
        [Description("Копейщик")]
        Lance_knight = 2,
        [Description("Топорщик")]
        Axeman = 3,
        [Description("Пикинер")]
        Pikeman = 4,
        [Description("Бомбарда")]
        Bombard = 5,
        [Description("Бомбардирский_корабль")]
        Bombardier_ship = 6,
        [Description("Пароход")]
        Steamboat = 7,
        [Description("Метатель_дротиков")]
        Javelin_thrower = 8,
        [Description("Трирема")]
        Trireme = 9,
        [Description("Ладья")]
        Bark = 10,
        [Description("Боевой_слон")]
        Battle_elephant = 11,
        [Description("Дубинщик")]
        Maceman = 12,
        [Description("Всадник")]
        Horseman = 13,
        [Description("Бронеавтомобиль")]
        Armored_car = 14,
        [Description("Драгун")]
        Dragoon = 15,
        [Description("Колесница")]
        Chariot = 16,
        [Description("Автоматчик")]
        Submachine_gunner = 17,
        [Description("Пулеметчик")]
        Machine_gunner = 18,
        [Description("Лёгкий_танк")]
        Light_tank = 19,
        [Description("Метатель_бола")]
        Bola_thrower = 20,
        [Description("Дивизионная_пушка")]
        Squadron_cannon = 21,
        [Description("Солдат")]
        Soldier = 22,
        [Description("Длинный_лук")]
        Longbow = 23,
        [Description("Кирасир")]
        Cuirassier = 24,
        [Description("Мушкетер")]
        Musketeer = 25,
        [Description("Егерь")]
        Ranger = 26,
        [Description("Кавалерист")]
        Cavalryman = 27,
        [Description("Алебардщик")]
        Halberdier = 28,
        [Description("Арбалетчик")]
        Crossbowman = 29,
        [Description("Гренадер")]
        Grenadier = 30,
        [Description("Современный_танк")]
        Modern_tank = 31,
        [Description("Рыцарь")]
        Knight = 32,
        [Description("Самурай")]
        Samurai = 33,
        [Description("Галера")]
        Galley = 34,
        [Description("Разведчик")]
        Scout = 35,
        [Description("Лучник")]
        Archer = 36,
        [Description("Мечник")]
        Swordsman = 37,
        [Description("Пушка")]
        Cannon = 38,
        [Description("Мортира")]
        Mortar = 39,
        [Description("Гвардеец")]
        Guardsman = 40,
        [Description("Катапульта")]
        Catapult = 41,
        [Description("Рутта")]
        Springald = 42,
        [Description("Поселенец")]
        Settler = 43,
        [Description("Моргенштерн")]
        Morgenstern = 44,
        [Description("Онагр")]
        Onager = 45,
        [Description("Требушет")]
        Trebuchet = 46,
        [Description("Гранатометчик")]
        Grenade_launcher = 47,
        [Description("Лодка")]
        Boat = 48,
        [Description("Каноэ")]
        Canoe = 49,
        [Description("Пехотинец")]
        Infantry = 50,
        [Description("Конный_лучник")]
        Horse_archer = 51,
        [Description("Конный_арбалетчик")]
        Horse_crossbowman = 52,
        [Description("Каравелла")]
        Caravel = 53,
        [Description("Фрегат")]
        Frigate = 54,
        [Description("Галеон")]
        Galleon = 55,
        [Description("Рабочий")]
        Worker = 56,
        [Description("Ополченец")]
        Militiaman = 57,
        [Description("Реактивная_артиллерия")]
        Rocket_artillery = 58,
        [Description("Боевая_машина пехоты")]
        Infantry_fighting_vehicle = 59,
        [Description("КК_Двигатель")]
        SS_Engine = 60,
        [Description("КК_Рубка")]
        SS_Cockpit = 61,
        [Description("КК_Обшивка")]
        SS_Hull = 62,
        [Description("КК_Топливный бак")]
        SS_Fuel_tank = 63,
        [Description("КК_Солнечная батарея")]
        SS_Solarcell_array = 64,
        [Description("КК_Модуль обеспечения")]
        SS_Support_module = 65,
        [Description("Броненосец")]
        Ironclad = 66,
        [Description("Эсминец")]
        Destroyer = 67,
        [Description("Транспортный_корабль")]
        Transport_ship = 68,
        [Description("Дредноут")]
        Dreadnought = 69,
        [Description("Подводная_лодка")]
        Submarine = 70,
        [Description("Линкор")]
        Battleship = 71,
        [Description("Атомная_подводная лодка")]
        Nuclear_submarine = 72,
        [Description("Крейсер")]
        Cruiser = 73,
        [Description("Дирижабль")]
        Dirigible = 74,
        [Description("Робопёс")]
        Robodog = 75,
        [Description("Снайпер")]
        Sniper = 76,
        [Description("Джонка")]
        Junk = 77,
        [Description("Легионер")]
        Legionary = 78,
        [Description("Воинягуар")]
        Jaguarwarrior = 79,
        [Description("Чокону")]
        Chukonu = 80,
        [Description("Мотоциклист")]
        Motorcyclist = 81,
        [Description("Легкий_истребитель")]
        Lightweight_fighter = 82,
        [Description("Легкий_бомбардировщик")]
        Light_bomber = 83,
        [Description("Пикировщик")]
        Dive_bomber = 84,
        [Description("Аэростат")]
        Aerostat = 85,
        [Description("Зенитное_орудие")]
        Air_defense_gun = 86,
        [Description("Тачанка_ПВО")]
        Mobile_SAM = 87,
        [Description("Танкягуар")]
        Jaguar_tank = 88,
        [Description("Истребитель")]
        Fighter_aircraft = 89,
        [Description("Бомбардировщик")]
        Bomber_aircraft = 90,
        [Description("Штурмовик")]
        Attack_aircraft = 91,
        [Description("Двухместная_колесница")]
        TwoSeat_Chariot = 92,
        //[Description("Скользкий_Крыс")]
        //Slippery_Rat = 93,
        [Description("Реактивный_истребитель")]
        Jet_fighter = 94,
        [Description("Реактивный_бомбардировщик")]
        Jet_bomber = 95,
        [Description("Импи")]
        Impi = 96,
        [Description("Мамлюк")]
        Mamluk = 97,
        [Description("Папирусная_лодка")]
        Papyrus_Boat = 98,
        [Description("Латник")]
        Landsknecht = 99,
        [Description("Минометчик")]
        Trenchmortar = 100,
        [Description("Тяжелый_танк")]
        Heavy_tank = 101,
        [Description("Бронетранспортер")]
        APC = 102,
        [Description("Реактивный_миномет")]
        Rocket_launcher = 103,
        [Description("Огнеметчик")]
        Flamethrower = 104,
        [Description("ЗРПК")]
        SPAAG = 105,        
        //[Description("Дед")]
        //Santa = 111,
        //[Description("Внучка")]
        //Snowflake = 112,
        //[Description("Лось")]
        //Deer = 113
    }

    public enum DepositName //++
    {
        [Description("Нет")]
        none = 0,
        [Description("Лес луг")]
        wood_meadows = 1,
        [Description("Лес степь")]
        wood_steppe = 2,
        [Description("Лес снег")]
        wood_snow = 3,
        [Description("Оазис")]
        oasis = 4,
        [Description("Бананы")]
        bananas = 5,
        [Description("Яблоки")]
        apples = 6,
        [Description("Абрикосы")]
        apricots = 7,
        [Description("Виноград луг")]
        grape_meadows = 8,
        [Description("Виногдад степь")]
        grape_steppe = 9,
        [Description("Кукуруза")]
        corn = 10,
        [Description("Пшеница")]
        grain = 11,
        [Description("Рис")]
        rice = 12,
        [Description("Рыба")]
        fish = 13,
        [Description("Киты")]
        whales = 14,
        [Description("Крабы")]
        crabs = 15,
        [Description("Устрицы")]
        oysters = 16,
        [Description("Свиньи")]
        pigs = 17,
        [Description("Коровы")]
        cows = 18,
        [Description("Олени")]
        deers = 19,
        [Description("Овцы степь")]
        ships_steppe = 20,
        [Description("Овцы луг")]
        ships_meadows = 21,
        [Description("Хлопок")]
        cotton = 22,
        [Description("Лён")]
        linen = 23,
        [Description("Золото")]
        gold = 24,
        [Description("Серебро")]
        silver = 25,
        [Description("Алмазы")]
        diamonds = 26,
        [Description("Изумруды")]
        emeralds = 27,
        [Description("Рубины")]
        rubies = 28,
        [Description("Жемчуг")]
        pearls = 29,
        [Description("Железо снег")]
        iron_snow = 30,
        [Description("Железо луг")]
        iron_meadows = 31,
        [Description("Железо степь")]
        iron_steppe = 32,
        [Description("Гранит снег")]
        stone_snow = 33,
        [Description("Гранит луг")]
        stone_meadows = 34,
        [Description("Гранит степь")]
        stone_steppe = 35,
        [Description("Лошади")]
        horses = 36,
        [Description("Верблюды")]
        camels = 37,
        [Description("Слоны")]
        elephants = 38,
        [Description("Сера луг")]
        sulfur_meadows = 39,
        [Description("Сера степь")]
        sulfur_steppe = 40,
        [Description("Сера пустыня")]
        sulfur_desert = 41,
        [Description("Газ снег")]
        gas_snow = 42,
        [Description("Газ степь")]
        gas_steppe = 43,
        [Description("Нефть пустыня")]
        oil_desert = 44,
        [Description("Нефть луг")]
        oil_meadows = 45,
        [Description("Нефть вода")]
        oil_water = 46,
        [Description("Уголь луг")]
        coal_meadow = 47,
        [Description("Уголь степь")]
        coal_steppe = 48,
        [Description("Уран снег")]
        uran_snow = 49,
        [Description("Уран степь")]
        uran_steppe = 50,
        [Description("Уран пустыня")]
        uran_desert = 51,
        [Description("Источник мудрости")]
        source_of_wisdom = 52
    }
    public enum BuildName //++
    {
        [Description("")]
        none = 400,
        //fortification
        [Description("ров")]
        moat = 3,
        [Description("частокол")]
        palisade = 19,
        [Description("стена")]
        walls = 38,
        [Description("укреп")]
        fortified_zone = 57,

        [Description("зенитка")]
        sam_batery = 101, //
        [Description("пво")]
        air_defense_system = 109, //

        //Scientific
        [Description("петроглиф")]
        petroglyph = 88,
        [Description("ФШ")]
        school_of_philosophy = 33, //
        [Description("обсерва")]
        observatory = 80, //

        [Description("библа")]
        library = 35, //
        [Description("академка")]
        academy = 70, //
        [Description("уник")]
        university = 61, //
        [Description("лаба")]
        laboratory = 72, //

        //Production and finances
        [Description("пилка")]
        lumber_mill = 5,//
        [Description("ферма")]
        farm = 6, //
        [Description("гранитный карьер")]
        granite_quarry = 12, // 
        [Description("шахта")]
        mine = 21, //
        [Description("пастбище")]
        animal_farm = 23,//
        [Description("ловушка для рыбы")]
        fishing_snare = 84,//
        [Description("промысловый порт")]
        fishing_harbor = 24,//
        [Description("дом охотника")]
        hunters_house = 27,//
        [Description("дом собирателя")]
        collectors_house = 96,//
        [Description("монетный двор")]
        mint = 51,//
        [Description("банк")]
        bank = 40, //
        [Description("винодельня")]
        winery = 22,//
        [Description("типография")]
        printing_office = 42,//
        [Description("топливный завод")]
        fuel_plant = 46, //
        [Description("дом ткача")]
        weavers_house = 36,//
        [Description("ткацкая фабрика")]
        weaving_mill = 52,//
        [Description("киностудия")]
        motionpicture_studio = 47, //
        [Description("сероплавильный завод")]
        sulphursmelting_plant = 89,//
        [Description("обогатительный завод")]
        concentrating_plant = 66,//

        [Description("кузница")]
        forge = 90,//
        [Description("мануфактура")]
        factory = 49,//
        [Description("завод")]
        power_plant = 54,//

        //town center
        [Description("алтарь")]
        shrine = 0,//
        [Description("резиденция")]
        residence = 25, //
        [Description("замок")]
        castle = 39, //
        [Description("ратуша")]
        city_hall = 53, //
        [Description("мерия")]
        mayors_office = 55, //

        //Storage and trade
        [Description("землянка")]
        hut = 2, // 
        [Description("склад")]
        depot = 17, //
        [Description("такаюка")]
        takayuka_depot = 45, //

        [Description("площадь")]
        town_square = 87, //
        [Description("рынок")]
        market = 26, //
        [Description("ТБ")]
        trading_base = 104, //

        [Description("пристань")]
        pier = 20, //
        [Description("порт")]
        harbor = 44, //

        //Culture
        [Description("обелиск")]
        obelisk = 8, //
        [Description("храм")]
        temple = 79, //
        [Description("монастырь")]
        monastery = 82, //
        [Description("собор")]
        cathedral = 63, //
        [Description("театр")]
        theatre = 67, //
        [Description("музей")]
        museum = 62, //
        [Description("статуя")]
        monument = 68,//

        //Military
        [Description("бойцовская яма")]
        fight_pit = 1,
        [Description("казарма")]
        barracks = 7,
        [Description("конюшня")]
        stable = 10,
        [Description("стрельбище")]
        shooting_range = 15,
        [Description("мастерская")]
        workshop = 32,
        [Description("ВЗ")]
        military_factory = 41,
        [Description("КЗ")]
        spaceship_factory = 59,
        [Description("ВЧ")]
        military_base = 74,

        [Description("маяк")]
        lighthouse = 69,
        [Description("верфь")]
        shipyard = 16,
        [Description("сухой док")]
        dry_dock = 78,

        [Description("ангар")]
        hangar = 93,
        [Description("РЛС")]
        radar_station = 103,

        //Demographic
        [Description("хижина")]
        shack = 85, //
        [Description("типи")]
        tipi = 18, //
        [Description("дом")]
        house = 48, //
        [Description("колодец")]
        well = 4, //
        [Description("фонтан")]
        fountain = 34, //
        [Description("водопровод")]
        aqueduct = 71, //
        [Description("водозабор")]
        water_intake = 28, //
        [Description("больница")]
        hospital = 37, //
        [Description("госпиталь")]
        infirmary = 81, //

        //Special
        [Description("посоль")]
        embassy = 13,//
        [Description("мохро вождей")]
        chieftains_mokhoro = 111, //
        [Description("суд")]
        courthouse = 14, //
        [Description("тайник")]
        cache = 86, //
        [Description("муляж")]
        decoy = 105,//

        //wounders
        [Description("Капище")]
        Pagan_temple = 83,
        [Description("Оракул")]
        Oracle = 97,
        [Description("Жертвенник")]
        Sacrificial_altar = 11,
        [Description("Стоунхендж")]
        Stonehenge = 94,
        [Description("Земляная плотина")]
        Earthen_dam = 76,
        [Description("Геоглиф")]
        Geoglyph = 56,
        [Description("МатьЗемля")]
        Earth_Mother = 43,
        [Description("Висячие сады")]
        The_Hanging_Gardens = 92,
        [Description("Колизей")]
        Coliseum = 58,
        [Description("Великая библиотека")]
        The_Great_Library = 60,
        [Description("Пирамида")]
        The_Pyramids = 73,
        [Description("Сфинкс")]
        Sphinx = 112,
        [Description("Терракотовая армия")]
        The_Terracotta_Army = 9,
        [Description("Колосс")]
        The_Colossus = 75,
        [Description("МачуПикчу")]
        Machu_Picchu = 100,
        [Description("Гелиоконцентратор")]
        Helioconcentrator = 91,
        [Description("Орден Тамплиеров")]
        The_Order_of_Knights_Templar = 98,
        [Description("Академия СунЦзы")]
        Sun_Tzus_War_Academy = 99,
        [Description("Царьпушка")]
        The_Tsar_Cannon = 95,
        [Description("Бранденбургские ворота")]
        Brandenburg_Gate = 108,
        [Description("ЛасВегас")]
        LasVegas = 107,
        [Description("Грузовой аэропорт")]
        Freight_airport = 30,
        [Description("ЦУП")]
        Mission_control_center = 29,
        [Description("Космодром")]
        Spaceport = 77,
    }
    public enum LuckBonusNames //++значения в этом перечислении отличались от оригинальных! А почему я так сделал я забыл(((
    {
        [Description("Прирост")]
        grown = 0,
        [Description("Наука")]
        science = 1,
        [Description("Производ")]
        production = 3,//3,
        [Description("ВБ")]
        war = 4,//4,
        [Description("Культура")]
        culture = 5,//5,
        //sciencePack= 6,//наука СРАЗУ
        //productionPack= 7,//производство СРАЗУ
        [Description("Торговцы")]
        traders =9, //9,              
    }
    public enum GreatCitizensNames//++
    {
        [Description ("Учёный")]
        Scientist = 0,
        [Description("Инжинер")]
        Engineer = 1,
        [Description("Агроном")]
        Agronomist = 2,
        [Description("Финансист")]
        Financier = 3,
        [Description("Полководец")]
        General = 4,
        [Description("Творец")]
        Creator = 5,
        [Description("Врач")]
        Doctor = 6,
    }

}