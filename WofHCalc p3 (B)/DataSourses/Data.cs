using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WofHCalc.Supports;
using System.IO;
using WofHCalc.Supports.jsonTemplates;
using WofHCalc.Models.jsonTemplates;

namespace WofHCalc.DataSourses
{
    //Через это класс можно удобно получить нужные константы из *.json файлов и некоторые другие.
    public static class Data
    {
        public static Resource[] ResData { get; } //из файла
        public static Deposit[] DepositsData { get; } //из файла
        public static Build[] BuildindsData { get; } //из файла
        public static LuckBonus[] LuckBonusesData { get; } //из файла
        public static AreaImprovement[] AreaImprovementsData {get; } //из файла
        public static float RaceEffect_Consumption(Race race) =>        //пока можно оставить //из файла
            race == Race.indians ? 0.85f : 1;        
        public static float RaceEffect_Culture(Race race)
        {
            if (race == Race.europeans) return 1.05f;
            else return 1;
        } //пока можно оставить
        public static float RaceEffect_PopulationGrowth(Race race)
        {
            if (race == Race.asians) return 1.1f;
            else return 1;
        } //пока можно оставить
        public static float RaceEffect_Upkeep(Race race)
        {
            if (race == Race.africans) return 0.9f;
            else return 1;
        } //пока можно оставить
        public static float RaceEffect_ProdMod(Race race, ResName res) //пока оставить
        {
            if ((int)res < 11 || (int)res > 14) return 1;
            if (race == Race.asians && res == ResName.rice) return 1.3f;
            if (race == Race.europeans && res == ResName.grain) return 1.3f;
            if (race == Race.indians && res == ResName.corn) return 1.3f;
            if (race == Race.africans && res == ResName.fruit) return 1.08f;
            return 0;
        } 
        public static float ClimateEffect(Climate climate, ResProdType rpt) //пока оставить
        {
            //                наука деньги  с/х     пром
            float[,] ans = {{ 0,    0,      0,      0 },
                            { 1,    1,      1.3f,   1 },
                            { 1,    1.15f,  1.1f,   1.1f},
                            { 1.1f, 1.05f,  0.85f,  1},
                            { 1,    0.9f,   0.85f,  1.3f} };
            return ans[(int)climate - 1, (int)rpt];
        }
        public static float BaseProduction(ResName res, bool on_hill, byte water_places) //точно оставить
        {
            switch (res)
            {
                case ResName.science:
                case ResName.money:
                case ResName.wood:
                case ResName.fruit:
                case ResName.meat:
                case ResName.films:
                case ResName.books:
                    return 1;
                case ResName.fish:
                    if (water_places > 1) return 1;
                    else return 0.5f;
                case ResName.iron: 
                    return 0.3f;
                case ResName.stone:
                    if (on_hill) return 0.2f; else return 0;
                default: return 0;
            }
        }
        public static float RebuildReturn { get => 0.35f; } //из файла
        public static int SwitchCost { get => 60; }   //из файла
        public static double[] ColonyDestroy { get => new double[] { 3000, 1.5d }; }    //из файла
        public static double[] AdministrationCulture { get => new double[] { 500.0001f, 400 }; } //из файла
        //по чудесам света в источнике очень неприятная каша, так что пока руками перепишу нужное.
        //Потом можно будет сделать частичную привязку к серверу.        
        public static Dictionary<BuildName,double> WounderEffects{ get; } //не трогать

        static Data()
        {
            string path = "DataSourses/JSON/";
            //+
            ResData = new Resource[23];
            string data = File.ReadAllText($"{path}resourses.json");
            ResData = System.Text.Json.JsonSerializer.Deserialize<Resource[]>(data)!;
            //+
            DepositsData = new Deposit[53];
            data = File.ReadAllText($"{path}deposits.json");
            DepositsData = System.Text.Json.JsonSerializer.Deserialize<Deposit[]>(data)!;
            //+
            BuildindsData = new Build[120];
            data = File.ReadAllText($"{path}builds.json");
            BuildindsData = Build.FromJson(data)!;
            //?
            LuckBonusesData = new LuckBonus[14];
            data = File.ReadAllText($"{path}luckytown.json");
            LuckBonusesData = System.Text.Json.JsonSerializer.Deserialize<LuckBonus[]>(data)!;
            //?
            AreaImprovementsData = new AreaImprovement[12];
            data = File.ReadAllText($"{path}AreaImprovements.json");
            AreaImprovementsData = System.Text.Json.JsonSerializer.Deserialize<AreaImprovement[]>(data)!;

            WounderEffects = new()
            {
                { BuildName.Pagan_temple, 10 }, //капище, даёт прирост
                { BuildName.The_Hanging_Gardens, 20 }, //прирост. Мб не правильно это            
                { BuildName.Earth_Mother, 100 }, //культура
                { BuildName.Coliseum, 200 }, //культура, ВБ тут не учтён
                { BuildName.The_Pyramids, 1000 },//культура
                { BuildName.Stonehenge, 15 }, //торгаши
                { BuildName.Earthen_dam, 160 }, //рыба
                { BuildName.The_Colossus, 200 }, //монеты
                { BuildName.Geoglyph, 30 }, //колбы
                { BuildName.The_Great_Library, 100 },//колбы
                { BuildName.Helioconcentrator, 200 }, //колбы
            };
        }

    }
}
