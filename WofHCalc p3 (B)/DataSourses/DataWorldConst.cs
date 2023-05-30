using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WofHCalc.Models.jsonTemplates;
using WofHCalc.Supports.jsonTemplates;
using WofHCalc.Supports;
using System.Text.Json.Serialization;

namespace WofHCalc.DataSourses
{
    internal class DataWorldConst
    {
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
        [JsonInclude]
        public float RebuildReturn
        {
            get => rebuild_return; 
            set => rebuild_return = value;
        } //из файла
        [JsonIgnore]
        private int switch_cost;
        [JsonInclude]
        public int SwitchCost 
        {
            get => switch_cost;
            set => switch_cost = value;
        }   //из файла
        [JsonIgnore]
        private double[] colony_destroy;
        [JsonInclude]
        public double[] ColonyDestroy 
        { 
            get => colony_destroy;
            set => colony_destroy = value;
        }    //из файла
        [JsonIgnore]
        private double[] admin_culture;
        [JsonInclude]
        public double[] AdministrationCulture 
        {
            get => admin_culture;//new double[] { 500.0001f, 400 }; 
            set => admin_culture = value;
        } //из файла
        //по чудесам света в источнике очень неприятная каша, так что пока руками перепишу нужное.
        //Потом можно будет сделать частичную привязку к серверу.        
        [JsonIgnore]
        public Dictionary<BuildName, double> WounderEffects { get; } //не трогать, но надо из файла

        //Не вызывать из кода! Конструктор без параметров используется только для создания объекта из JSON файлов. 
        DataWorldConst()
        {

        }
        DataWorldConst(int world)
        {
            //проверка наличия файлов
            //попытка загрузки из файлов
            //если файлов нет или загрузка провалена, запрос данных с сервера
            //сохранение в файл ассинхронное
        }
    }
}
