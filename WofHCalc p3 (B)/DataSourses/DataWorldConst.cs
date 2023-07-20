using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WofHCalc.Models.jsonTemplates;
using WofHCalc.Supports;
using System.IO;
using System.Linq.Expressions;
using WofHCalc.DataSourses.DataLoader;
using System.Runtime.InteropServices.JavaScript;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Reflection.Metadata.Ecma335;

namespace WofHCalc.DataSourses
{
    public sealed class DataWorldConst
    {
        [JsonIgnore]
        private static DataWorldConst? instance; //синглтон

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
        public Unit[] Units { get; } //инфа по юнитам. Пока только для рабочих нужна
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
        private float rebuild_return;
        public float RebuildReturn
        {
            get => rebuild_return;
            private set=> rebuild_return = value;
        } //из файла        
        private int switch_cost;
        public int SwitchCost 
        {
            get => switch_cost;            
            private set => switch_cost = value;
        }   //из файла        
        private double[] colony_destroy;
        public double[] ColonyDestroy 
        { 
            get => colony_destroy;
            private set => colony_destroy = value;
        }    //из файла
        private double[] admin_culture;
        public double[] AdministrationCulture 
        {
            get => admin_culture;//new double[] { 500.0001f, 400 };         
            private set => admin_culture = value;
        } //из файла
        //по чудесам света в источнике очень неприятная каша, так что пока руками перепишу нужное.
        //Потом можно будет сделать частичную привязку к серверу.                
        private Dictionary<BuildName, double> wounders_effect;
        public Dictionary<BuildName, double> WounderEffects { get => wounders_effect; } //пока не трогать, но надо из файла

        //получалка синглтона
        [JsonIgnore]
        private static readonly object threadlock = new object();
        public static DataWorldConst GetInstance(int world)
        {
            lock (threadlock)
            {
                if (instance is null) instance = Init(world);// Init(world);                
                return instance!;
            }
        }
        //конструкторы
        //закрытый, возможно не стоит ему быть пустым
        private DataWorldConst() { }
        private static DataWorldConst Init(int world)
        {
            string path = $"DataSourses/JSON/{world}/";
            //попытка загрузки из файлов. Нет смысла каждый отдельно проверять, тут проще будет запросить новые данные с серва и перезаписать файлы
            try
            {
                //return FromLocalFiles(path);
                throw new NotImplementedException();
            }
            catch
            {
                //если файлов нет или загрузка провалена, запрос данных с сервера                
                return FromConstJSON(GetData.GetConstjson(world));
            }            
        }
        //конструктор для заполнения штук
        private DataWorldConst(
            Resource[] res_data,
            Deposit[] deposits_data,
            Build[] bulds_data,
            LuckBonus[] luck_bonuses_data,
            AreaImprovement[] area_improvements_data,
            Unit[] units_data,
            float rebuild_ret,
            int switch_c,
            double[] colony_destr,
            double[] admin_cult,
            Dictionary<BuildName, double> we)
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
            this.wounders_effect = we;
        }
        private static DataWorldConst FromConstJSON (string constjson) 
        {
            JObject jdata = JObject.Parse(constjson);
            Dictionary<BuildName, double> we = new()
            {
                { BuildName.Pagan_temple, jdata["wonder"]["83"]["effect"].Value<double>() }, //капище, даёт прирост                
                { BuildName.The_Hanging_Gardens, jdata["wonder"]["92"]["local"].Value<double>() }, //висячие сады, прирост локальный
                { BuildName.Earth_Mother,  jdata["wonder"]["43"]["effect"].Value<double>()}, //культура
                { BuildName.Coliseum, jdata["wonder"]["58"]["culture"].Value<double>() }, //культура, ВБ тут не учтён
                { BuildName.The_Pyramids, jdata["wonder"]["73"]["local"].Value<double>() },//культура
                { BuildName.Stonehenge, jdata["wonder"]["94"]["traders"].Value<double>() }, //торгаши
                { BuildName.Earthen_dam, jdata["wonder"]["76"]["effect"].Value<double>() }, //рыба
                { BuildName.The_Colossus, jdata["wonder"]["75"]["local"].Value<double>() }, //монеты, пристань не учтена никак
                { BuildName.Geoglyph, jdata["wonder"]["56"]["effect"].Value<double>() }, //колбы
                { BuildName.The_Great_Library, jdata["wonder"]["60"]["effect"].Value<double>() },//колбы
                { BuildName.Helioconcentrator, jdata["wonder"]["91"]["effect"].Value<double>() }//колбы
            };            

            return new DataWorldConst(
                jdata["resource"]!["data"]!.Children().Select(x => x.ToObject<Resource>()).ToArray()!, //res
                jdata["map"]!["deposit"]!.Children().Select(x => x.ToObject<Deposit>()).ToArray()!, //deposits
                jdata["builds"]!.Children().Select(x => x.ToObject<Build>()).ToArray()!, //builds
                jdata["luckbonus"]!["town"]!.Children().Select(x => x.ToObject<LuckBonus>()).ToArray()!,//luck
                jdata["map"]!["environment"]!.Children().Select(x => x.ToObject<AreaImprovement>()).ToArray()!,//aid
                jdata["units"]!["list"]!.Children().Select(x=>x.ToObject<Unit>()).ToArray()!,//units
                jdata["build"]!["rebuildreturn"]!.ToObject<float>(),//rbr
                jdata["build"]!["switchcost"]!.ToObject<int>(),//sc
                jdata["war"]!["colonydestroy"]!.Children().Select(x => x.ToObject<double>()).ToArray()!,//col_destr
                jdata["build"]!["administrationculture"]!.Children().Select(x => x.ToObject<double>()).ToArray()!,//admcult
                we
                );            
            //заполнить инстанс через конструктор со всем коплектом
        }
        //private static DataWorldConst FromLocalFiles(string path) //потом будет один файл
        //{
        //    //отдельная куча
        //    DataWorldConst d = new();
        //    string data = File.ReadAllText($"{path}data.json");
        //    d = JsonConvert.DeserializeObject<DataWorldConst>(data)!; //одиночные штуки из основного файла //может ломаться            
        //    //по отдельным файлам
        //    data = File.ReadAllText($"{path}resourses.json");
        //    var res_data = System.Text.Json.JsonSerializer.Deserialize<Resource[]>(data)!;            
        //    data = File.ReadAllText($"{path}deposits.json");
        //    var deposits_data = System.Text.Json.JsonSerializer.Deserialize<Deposit[]>(data)!;            
        //    data = File.ReadAllText($"{path}builds.json");
        //    var buildinds_data = Build.FromJsonToArray(data)!;
        //    data = File.ReadAllText($"{path}luckytown.json");
        //    var luck_bonuses_data = System.Text.Json.JsonSerializer.Deserialize<LuckBonus[]>(data)!;
        //    data = File.ReadAllText($"{path}AreaImprovements.json");                        
        //    var area_improvements_data = System.Text.Json.JsonSerializer.Deserialize<AreaImprovement[]>(data)!;
        //    ////Переделать!
            
        //    //WounderEffects = new()
        //    //{
        //    //    { BuildName.Pagan_temple, 10 }, //капище, даёт прирост
        //    //    { BuildName.The_Hanging_Gardens, 20 }, //прирост. Число не правильное
        //    //    { BuildName.Earth_Mother, 100 }, //культура
        //    //    { BuildName.Coliseum, 200 }, //культура, ВБ тут не учтён
        //    //    { BuildName.The_Pyramids, 1000 },//культура
        //    //    { BuildName.Stonehenge, 15 }, //торгаши
        //    //    { BuildName.Earthen_dam, 160 }, //рыба
        //    //    { BuildName.The_Colossus, 200 }, //монеты
        //    //    { BuildName.Geoglyph, 30 }, //колбы
        //    //    { BuildName.The_Great_Library, 100 },//колбы
        //    //    { BuildName.Helioconcentrator, 200 }, //колбы
        //    return new DataWorldConst(res_data,
        //                              deposits_data,
        //                              buildinds_data,
        //                              luck_bonuses_data,
        //                              area_improvements_data,
        //                              d.RebuildReturn,
        //                              d.switch_cost,
        //                              d.colony_destroy,
        //                              d.admin_culture);            
        //}
    }
}
