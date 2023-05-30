using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WofHCalc.Models.jsonTemplates;
using WofHCalc.Supports.jsonTemplates;
using WofHCalc.Supports;
using System.IO;
using System.Linq.Expressions;
using System.Text.Json;
using WofHCalc.DataSourses.DataLoader;
using System.Runtime.InteropServices.JavaScript;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WofHCalc.DataSourses
{
    public sealed class DataWorldConst
    {
        [JsonIgnore]
        private static DataWorldConst instance; //синглтон, а ещё поможет избавиться от асинхронного кода

        //данные из отдельных файлов
        [JsonIgnore]
        public Resource[] ResData { get; } //из файла
        [JsonIgnore]
        public Deposit[] DepositsData { get; } //из файла
        [JsonIgnore]
        public Build[] BuildindsData { get; } //из файла
        [JsonIgnore]
        public LuckBonus[] LuckBonusesData { get; } //из файла
        [JsonIgnore]
        public AreaImprovement[] AreaImprovementsData { get; } //из файла        

        //методы
        public float RaceEffect_Consumption(Race race) //пока можно оставить //из файла
        {            
            return race == Race.indians ? 0.85f : 1;
        }
        public float RaceEffect_Culture(Race race)
        {
            if (race == Race.europeans) return 1.05f;
            else return 1;
        } //пока можно оставить
        public float RaceEffect_PopulationGrowth(Race race)
        {
            if (race == Race.asians) return 1.1f;
            else return 1;
        } //пока можно оставить
        public float RaceEffect_Upkeep(Race race)
        {
            if (race == Race.africans) return 0.9f;
            else return 1;
        } //пока можно оставить
        public float RaceEffect_ProdMod(Race race, ResName res) //пока оставить
        {
            if ((int)res < 11 || (int)res > 14) return 1;
            if (race == Race.asians && res == ResName.rice) return 1.3f;
            if (race == Race.europeans && res == ResName.grain) return 1.3f;
            if (race == Race.indians && res == ResName.corn) return 1.3f;
            if (race == Race.africans && res == ResName.fruit) return 1.08f;
            return 0;
        }
        public float ClimateEffect(Climate climate, ResProdType rpt) //пока оставить
        {
            //                наука деньги  с/х     пром
            float[,] ans = {{ 0,    0,      0,      0 },
                            { 1,    1,      1.3f,   1 },
                            { 1,    1.15f,  1.1f,   1.1f},
                            { 1.1f, 1.05f,  0.85f,  1},
                            { 1,    0.9f,   0.85f,  1.3f} };
            return ans[(int)climate - 1, (int)rpt];
        }
        public float BaseProduction(ResName res, bool on_hill, byte water_places) //точно оставить
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
        
        //отдельные числа просто кучей в свой файл
        [JsonIgnore]
        private float rebuild_return;
        public float RebuildReturn
        {
            get => rebuild_return;             
        } //из файла
        [JsonIgnore]
        private int switch_cost;
        public int SwitchCost 
        {
            get => switch_cost;            
        }   //из файла
        [JsonIgnore]
        private double[] colony_destroy;
        public double[] ColonyDestroy 
        { 
            get => colony_destroy;
        }    //из файла
        [JsonIgnore]
        private double[] admin_culture;
        public double[] AdministrationCulture 
        {
            get => admin_culture;//new double[] { 500.0001f, 400 };         
        } //из файла
        //по чудесам света в источнике очень неприятная каша, так что пока руками перепишу нужное.
        //Потом можно будет сделать частичную привязку к серверу.        
        [JsonIgnore]
        public Dictionary<BuildName, double>? WounderEffects { get; } //не трогать, но надо из файла

        //
        public static DataWorldConst getInstance(int world)
        {
            if (instance is null)
            {
                Init(world);                
            }
            return instance!;
        }
        private static void Init(int world)
        {
            string path = $"DataSourses/JSON/{world}/";
            //попытка загрузки из файлов. Нет смысла каждый отдельно проверять, тут проще будет запросить новые данные с серва и перезаписать файлы
            try
            {
                ////отдельная куча
                //string data = File.ReadAllText($"{path}data.json");                
                //var tmp = System.Text.Json.JsonSerializer.Deserialize<DataWorldConst>(data);
                //RebuildReturn = tmp!.RebuildReturn;
                //SwitchCost = tmp!.SwitchCost;
                //ColonyDestroy = tmp!.ColonyDestroy;
                //AdministrationCulture = tmp!.AdministrationCulture;
                ////по отдельным файлам
                //ResData = new Resource[23];
                //data = File.ReadAllText($"{path}resourses.json");
                //ResData = System.Text.Json.JsonSerializer.Deserialize<Resource[]>(data)!;
                //DepositsData = new Deposit[53];
                //data = File.ReadAllText($"{path}deposits.json");
                //DepositsData = System.Text.Json.JsonSerializer.Deserialize<Deposit[]>(data)!;
                //BuildindsData = new Build[120];
                //data = File.ReadAllText($"{path}builds.json");
                //BuildindsData = Build.FromJson(data)!;
                //LuckBonusesData = new LuckBonus[14];
                //data = File.ReadAllText($"{path}luckytown.json");
                //LuckBonusesData = System.Text.Json.JsonSerializer.Deserialize<LuckBonus[]>(data)!;
                //AreaImprovementsData = new AreaImprovement[12];
                //data = File.ReadAllText($"{path}AreaImprovements.json");
                //AreaImprovementsData = System.Text.Json.JsonSerializer.Deserialize<AreaImprovement[]>(data)!;
                
                

                ////Переделать!
                //WounderEffects = new()
                //{
                //    { BuildName.Pagan_temple, 10 }, //капище, даёт прирост
                //    { BuildName.The_Hanging_Gardens, 20 }, //прирост. Число не правильное
                //    { BuildName.Earth_Mother, 100 }, //культура
                //    { BuildName.Coliseum, 200 }, //культура, ВБ тут не учтён
                //    { BuildName.The_Pyramids, 1000 },//культура
                //    { BuildName.Stonehenge, 15 }, //торгаши
                //    { BuildName.Earthen_dam, 160 }, //рыба
                //    { BuildName.The_Colossus, 200 }, //монеты
                //    { BuildName.Geoglyph, 30 }, //колбы
                //    { BuildName.The_Great_Library, 100 },//колбы
                //    { BuildName.Helioconcentrator, 200 }, //колбы
                //};

            }
            catch
            {
                string constjson = GetData.GetConstjson(world);
                FromConstJSON(constjson);
            }
            //если файлов нет или загрузка провалена, запрос данных с сервера            
        }
        //конструктор для заполнения штук
        private DataWorldConst(
            Resource[] res_data,
            Deposit[] deposits_data,
            Build[] bulds_data,
            LuckBonus[] luck_bonuses_data,
            AreaImprovement[] area_improvements_data,
            float rebuild_ret,
            int switch_c,
            double[] colony_destr,
            double[] admin_cult)
        { 
            this.ResData = res_data;
            this.DepositsData = deposits_data;
            this.BuildindsData = bulds_data;
            this.LuckBonusesData= luck_bonuses_data;
            this.AreaImprovementsData= area_improvements_data;
            this.rebuild_return = rebuild_ret;
            this.switch_cost = switch_c;
            this.colony_destroy = colony_destr;
            this.admin_culture = admin_cult;
        }
        private static void FromConstJSON (string constjson) //void
        {
            //DataWorldConst res;
            JObject jdata = JObject.Parse(constjson);
            Resource[] res_data = jdata["resource"]["data"].Children().Select(x => x.ToObject<Resource>()).ToArray();//+
            Deposit[] deposits_data = jdata["map"]["deposit"].Children().Select(x => x.ToObject<Deposit>()).ToArray();
            Build[] builds_data = jdata["builds"].Children().Select(x => x.ToObject<Build>()).ToArray();


            //заполнить инстанс через конструктор со всем коплектом
        }
        private static void FromLocalFiles()
        {

        }
    }
}
