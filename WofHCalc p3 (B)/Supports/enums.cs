using System;
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
    public enum ResName //+
    {
        science = 0,
        money = 1,
        food = 2,
        wood = 3,
        iron = 4,
        fuel = 5,
        stone = 6,
        horses = 7,
        sulfur = 8,
        aluminium = 9,
        uranium = 10,
        fruit = 11,
        corn = 12,
        grain = 13,
        rice = 14,
        fish = 15,
        meat = 16,
        wine = 17,
        jewelry = 18,
        clothes = 19,
        music = 20,
        films = 21,
        books = 22
    }
    public enum ResProdType //+
    {
        research = 0,
        finance = 1,
        agriculture = 2,
        industry = 3,
    }
    public enum Climate //+
    {
        unknown = 0,
        water = 1,
        meadows = 2,
        steppe = 3,
        desert = 4,
        snow = 5
    }
    public enum Slot //+
    {
        plain = 0,
        center = 1,
        hill = 2,
        water = 3,
        fort = 4,
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

    public enum DepositName //+
    {
        none = 0,
        wood_meadows = 1,
        wood_steppe = 2,
        wood_snow = 3,
        oasis = 4,
        bananas = 5,
        apples = 6,
        apricots = 7,
        grape_meadows = 8,
        grape_steppe = 9,
        corn = 10,
        grain = 11,
        rice = 12,
        fish = 13,
        whales = 14,
        crabs = 15,
        oysters = 16,
        pigs = 17,
        cows = 18,
        deers = 19,
        ships_steppe = 20,
        ships_meadows = 21,
        cotton = 22,
        linen = 23,
        gold = 24,
        silver = 25,
        diamonds = 26,
        emeralds = 27,
        rubies = 28,
        pearls = 29,
        iron_snow = 30,
        iron_meadows = 31,
        iron_steppe = 32,
        stone_snow = 33,
        stone_meadows = 34,
        stone_steppe = 35,
        horses = 36,
        camels = 37,
        elephants = 38,
        sulfur_meadows = 39,
        sulfur_steppe = 40,
        sulfur_desert = 41,
        gas_snow = 42,
        gas_steppe = 43,
        oil_desert = 44,
        oil_meadows = 45,
        oil_water = 46,
        coal_meadow = 47,
        coal_steppe = 48,
        uran_snow = 49,
        uran_steppe = 50,
        uran_desert = 51,
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
        [Description("водазабор")]
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
        [Description("Мать-Земля")]
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
        [Description("Мачу-Пикчу")]
        Machu_Picchu = 100,
        [Description("Гелиоконцентратор")]
        Helioconcentrator = 91,
        [Description("Орден Тамплиеров")]
        The_Order_of_Knights_Templar = 98,
        [Description("Академия СунЦзы")]
        Sun_Tzus_War_Academy = 99,
        [Description("Царь-пушка")]
        The_Tsar_Cannon = 95,
        [Description("Бранденбургские ворота")]
        Brandenburg_Gate = 108,
        [Description("Лас-Вегас")]
        LasVegas = 107,
        [Description("Грузовой аэропорт")]
        Freight_airport = 30,
        [Description("Центр управления полетами")]
        Mission_control_center = 29,
        [Description("Космодром")]
        Spaceport = 77,
    }


}