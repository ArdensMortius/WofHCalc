﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using WofHCalc.DataSourses;
using WofHCalc.Models;
using WofHCalc.Models.jsonTemplates;
using WofHCalc.Supports;

namespace WofHCalc.MathFuncs
{
    public class WofHMath //вся математика этой игры
    {
        public DataWorldConst data;
        public WofHMath (DataWorldConst data) { this.data = data; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private WofHMath() { } //скрыт, потому что не должен использоваться никогда
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        //основная функция, по которой считается почти всё в двух вариантах её вызова
        internal static double MainFunc(double A, double B, double C, double D, int lvl)
        {
            double k = (lvl > 1) ? 0 : A;
            return (k + B + C * (Math.Pow(lvl, D)));
        }
        internal static double MainFunc(double[]? abcd, int n)
        {
            if (abcd is null) return 0;
            else return MainFunc(abcd[0], abcd[1], abcd[2], abcd[3], n);
        }
        #region разное
        //Эффективность труда (число работников) //+
        public double LaborEfficiency(int numworkers)
        {
            return Math.Pow(numworkers, -0.12d);
        }
        //Коррупция (число городов, уровень суда) //+
        public double Corruption(int numtowns, byte courthouse_level)
        {
            if (numtowns < 1 || courthouse_level < 0 || courthouse_level > 20) throw new Exception("Corruption error");
            double ans = ((numtowns - 1) * 20 / MainFunc(data.BuildindsData[(int)BuildName.courthouse].Effect, courthouse_level));
            return ans > 90 ? 90 : ans;
        }
        //Бонус ВГ
        public double GreatCitizenBonus(int n) =>//конкретные числа должны быть в data где-нибудь, а не тут
            MainFunc(0, 1, 0.04d, 0.75d, n);
        //Цена бонуса удачи (бонус, число городов, уровень бонуса)
        public double LuckBonusPrice(LuckBonusNames name, int numtowns, int lvl)
        {
            double baseprice = (double)data.LuckBonusesData[(int)name].cost[lvl];
            double costk = data.LuckBonusesData[(int)name].costk;
            return Math.Ceiling(baseprice / numtowns * (1 + (costk * (numtowns - 1))));
        }
        //базовая цена за единицу населения для рассчётов цены за УМ 
        public float BaseOneHumanPrice(FinancialPolicy f)
        {
            int x = (int)ResName.corn;
            int y = (int)ResName.rice;
            int growth_food_price = f.Prices.Where((item, index) => index >= x && index <= y).Max();//самая дорогая растишка
            return data.ResData[x].consumption * growth_food_price * 0.024f; //домножается, чтоб из расхода в час получить цену за 1 человечка
        }
        public float OneHumanPriceForRace(Account acc)
        {
            return BaseOneHumanPrice(acc.Financial)*data.RaceEffect_PopulationGrowth(acc.Race);
        }
        //Стоимость ресов
        private double ResToMoney<T>(T[] res, IList<int> prices) where T: INumber<T>
        {
            double ans = 0;
            for (int i = 0; i < 23; i++)
                if (res[i].GetType()==typeof(int))                
                    ans += (double)(int)(object)(res[i]) * (double)prices[i] * 0.001f;
                else ans += (double)(object)(res[i]) * (double)prices[i] * 0.001f;
            return ans;
        }
        //Прочность МР (число городов)
        public int ColonyDestroy(int numtowns) =>
            (int)(Math.Pow(numtowns, data.ColonyDestroy[1]) * data.ColonyDestroy[0]);
        #endregion
        #region юниты
        public float UnitResPrice(UnitsNames unit, FinancialPolicy f)
        {
            float ans = 0;
            int[] res = data.Units[(int)unit].ResCost;
            for (int i = 0; i < res.Length; i++)
                ans += res[i] * f.Prices[i] * 0.001f;            
            return ans;
        }
        public float UnitPopPriceForRace(UnitsNames unit, Account acc) =>
            data.Units[(int)unit].PopCost * OneHumanPriceForRace(acc);
        public float UnitPopPriceBase(UnitsNames unit, FinancialPolicy f) =>
            data.Units[(int)unit].PopCost * BaseOneHumanPrice(f);
        //цена юнита рассчётная (от фактической отличается, т.к. цена прироста зависит от города
        public float UnitPriceForAcc(int unit_id, Account acc)
        {
            float ans =0;
            int[] res = data.Units[unit_id].ResCost;
            for (int i = 0; i < res.Length; i++)             
                ans += res[i] * acc.Financial.Prices[i] * 0.001f;
            ans += data.Units[unit_id].PopCost * OneHumanPriceForRace(acc);
            return ans;
        }
        public float BaseUnitPrice(int unit_id, FinancialPolicy f)
        {
            float ans = 0;
            int[] res = data.Units[unit_id].ResCost;
            for (int i = 0; i < res.Length; i++)
                ans += res[i] * f.Prices[i] * 0.001f;
            ans += data.Units[unit_id].PopCost * BaseOneHumanPrice(f);
            return ans;
        }
        public float UnitUpkeep(UnitsNames name, FinancialPolicy f)
        {
            float ans = 0;
            foreach (var x in data.Units[(int)name].Pay)            
                ans += f.Prices[x[0]] * x[1] * 0.001f;
            return ans;
        }        
        #endregion
        #region функции УМ
        //Эффективность УМ (кол-во пользователи)
        private double AreaImprovementEfficiencyPerUser(byte users)
        {
            double u = (double)users;
            return (u + 1) / (2 * u); //результат надо округлить предположительно до сотых
        }
        //Величина бонуса от УМ (УМ, уровень, к-во пользователей)
        public double AreaImprovementBonus(AreaImprovementName name, int lvl, byte users = 1)
        {
            if (lvl == 0) return 0;
            return 0 + (data.AreaImprovementsData[(int)name].levels[lvl - 1].effect) * AreaImprovementEfficiencyPerUser(users);
        }
        //Затраты ресов на улучшение местности (УМ, уровень) (как учитывать рабочих-то?)
        //public int[] AreaImprovementResCost()
        //Цена улучшения местности
        public double AreaImprovementPriceForAcc(AreaImprovementName ArImp, byte lvl, Account acc)
        {
            try
            {
                if (ArImp == AreaImprovementName.none || lvl == 0) return 0;
                double ans = 0;
                int[] rc = data.AreaImprovementsData[(int)ArImp].levels[lvl-1].GetResCost();
                for (int i = 0; i < rc.Length; i++)
                    ans += rc[i] * acc.Financial.Prices[i] * 0.001d;
                ans += data.AreaImprovementsData[(int)ArImp].levels[lvl-1].workers * UnitPriceForAcc(56, acc);
                return ans;
            }
            catch
            {
                return 0;
            }
        }
        public double AreaImprovementPriceBase(AreaImprovementName ArImp, byte lvl, Account acc)
        {
            try
            {
                if (ArImp == AreaImprovementName.none || lvl == 0) return 0;
                double ans = 0;
                int[] rc = data.AreaImprovementsData[(int)ArImp].levels[lvl - 1].GetResCost();
                for (int i = 0; i < rc.Length; i++)
                    ans += rc[i] * acc.Financial.Prices[i] * 0.001d;
                ans += data.AreaImprovementsData[(int)ArImp].levels[lvl - 1].workers * BaseUnitPrice(56, acc.Financial);
                return ans;
            }
            catch
            {
                return 0;
            }
        }
        #endregion
        #region функции для построек
        //Затраты ресов на уровень (домик, строимый уровень)
        private int[] BuildUpResCost(BuildName name, int under_construction_lvl)
        {
            int[] ans = new int[23];
            if (under_construction_lvl == 0) return ans;
            double A;
            double B;
            double C;
            double D = (double)data.BuildindsData[(int)name].Cost[3].Double!;
            for (int i = 0; i < 23; i++)
            {
                A = (double)data.BuildindsData[(int)name].Cost[0].DoubleArray[i];
                B = (double)data.BuildindsData[(int)name].Cost[1].DoubleArray[i];
                C = (double)data.BuildindsData[(int)name].Cost[2].DoubleArray[i];
                int res = (int)MainFunc(A, B, C, D, under_construction_lvl); 
                //но тогда стоимость апа не верная получается.. Надо подумоть
                //потому что текущий уровень надо в mainfunc слать
                if (res < 0) res = 0;
                ans[i] = res;
            }
            return ans;
        }
        //Полные затраты ресов на домик (домик, уровень)
        public int[] BuildTotalResCost(BuildName name, int? current_lvl)
        {
            int[] ans = new int[23];
            if (current_lvl is null || current_lvl == 0 || name == BuildName.none) return ans;
            for (int i = 1; i <= current_lvl; i++)
            {
                int[] lvlcost = BuildUpResCost(name, i);
                for (int j = 0; j < 23; j++)
                    ans[j] += lvlcost[j];
            }
            return ans;
        }
        //Затраты ресов на ап домика(домик, имеющийся уровень, планируемый уровень)
        public int[] BuildUpFromToResCost(BuildName name, int current_lvl, int target_lvl)
        {
            var ans = new int[23];
            for (int l = current_lvl + 1; l <= target_lvl; l++)
            {
                var t = BuildUpResCost(name, l);
                for (int i = 0; i < 23; i++)
                    ans[i] += t[i];
            }
            return ans;
        }
        //Затраты на перестройку из одного домика в другой(домик в наличии, уровень домика, на что меняем)
        public int[] RebuildResCost(BuildName current_build, int? cur_lvl, BuildName new_build)
        {
            //Перестраиваем или строим или сносим?
            if (current_build == BuildName.none) return BuildUpResCost(new_build, 1);
            if (new_build == BuildName.none) return new int[23];
            //Проверка доступности перестройки            
            if (data.BuildindsData[(int)current_build].Next[0] == new_build)
            {
                int[] ans = new int[23];
                int[] oldb = BuildTotalResCost(current_build, cur_lvl);
                int[] newb = BuildUpResCost(new_build, 1);
                for (int i = 0; i < 23; i++)
                {
                    ans[i] = newb[i] - (int)(data.RebuildReturn * oldb[i]);
                    if (ans[i] < 0) ans[i] = 0;
                }
                return ans;
            }
            //если нельзя перестроить, то сносим и строим новый
            else return BuildUpResCost(new_build, 1);
        }
        //Значение "эффект" домика (домик, уровень). Смысл "эффекта" отличается для разных типов домиков.
        public double BuildEffect(BuildName name, int? lvl)
        {
            if (name == BuildName.none || lvl is null || lvl == 0) return 0;
            return MainFunc(data.BuildindsData[(int)name].Effect, (int)lvl!);
        }
        //Проверка доступности домика по расе(домик, раса)
        private bool AvailableRaceCheck(BuildName build, Race acc) =>
            (data.BuildindsData[(int)build].Race & acc) == acc;
        //можно ли поставить домик или он заблокирован конкурирующими (новый домик, имеющаяся застройка)
        private bool AvailableCountGroupCheck(BuildName newbuild, BuildName[] townbuilds)
        {
            int c = 0;
            int group = data.BuildindsData[(int)newbuild].Group;
            for (int i = 0; i < townbuilds.Length; i++)
            {
                if (townbuilds[i] == BuildName.none) continue;
                if (townbuilds[i] == newbuild) c++; //считаем количество таких же
                else if (group == 0) continue; //эта группа для построек, которые ни с чем не конкурируют
                else if (data.BuildindsData[(int)townbuilds[i]].Group == group) return false; //проверяем конкуренцию
            }
            if (c >= data.BuildindsData[(int)newbuild].Maxcount) return false;
            return true;
        }
        //можно ли поставить домик при текущем расположении города. Это важно для чудес.
        private bool AvailableTerrainCheck(BuildName newbuild, bool on_hill, byte water_places)
        {
            Terrain t = data.BuildindsData[(int)newbuild].Terrain;
            return t switch
            {
                Terrain.everywhere => true,
                Terrain.hill => on_hill,
                Terrain.plane => !on_hill,
                Terrain.plane_no_water => (!on_hill && water_places == 0),
                Terrain.plane_water => (!on_hill && water_places > 0),
                Terrain.nowhere => false,
                _ => false,
            };
        }
        //подходит ли домик в выбранный слот
        private bool AvailableSlotCheck(BuildName newbuild, Slot slot) =>
            data.BuildindsData[(int)newbuild].Slot == slot;
        //можно ли поставить домик в текущих условиях со всеми параметрами
        public bool AvailableCheck(BuildName newbuild, Race acc_race, bool town_on_hill, byte town_water_places, BuildName[] town_builds, Slot slot)
        {
            if (newbuild == BuildName.none) return true;
            return (
                AvailableSlotCheck(newbuild, slot)
                && AvailableRaceCheck(newbuild, acc_race)
                && AvailableCountGroupCheck(newbuild, town_builds)
                && AvailableTerrainCheck(newbuild, town_on_hill, town_water_places)
                );
        }
        //Сколько денег жрёт домик без учёта экономии (домик, уровень)
        public double Pay(BuildName name, int lvl)
        {
            if (name == BuildName.none || lvl == 0) return 0;
            double ans = MainFunc(data.BuildindsData[(int)name].Pay, lvl);
            if (ans < 0) return 0;
            else return ans;
        }
        //Спад населения от домика (домик, уровень) в день
        public double Ungrown(BuildName name, int? lvl)
        {
            if (BuildName.none == name || lvl is null || lvl == 0) return 0;
            return MainFunc(data.BuildindsData[(int)name].Ungrown, (int)lvl!)*24;
        }
        //культура от административного домика (замок, ратуша, мерия)
        public double AdminCulture(BuildName name, int? lvl) => 
            (lvl is null || lvl == 0) ? 0 : data.AdministrationCulture[0] + data.AdministrationCulture[1] * BuildEffect(name, lvl);
        //экономия в долях от административного домика
        public double AdminEconimy(BuildName name, int lvl) =>  
            lvl > 0 ? (1 - 1 / (1 + BuildEffect(name, lvl))) : 0;
        //прочность домика
        public int BuildStrength(BuildName name, int lvl)
        {
            int[] resourses = BuildTotalResCost(name, lvl);
            int sum = 0;
            foreach (int r in resourses) sum += r;
            return sum;
        }
        //время стройки уровня домика
        private double BuildUpTime(BuildName name, int under_construction_lvl)
            //в часах            
            => MainFunc(data.BuildindsData[(int)name].Buildtime, under_construction_lvl);
        //время апа домика с текущего уровня до нового
        public double BuildUpTimeFromTo(BuildName name, int current_lvl, int target_lvl)
        {
            //в часах
            double ans = 0;
            for (int i = current_lvl + 1; i <= target_lvl; i++)            
                ans += BuildUpTime(name, i);            
            return ans;
        }
        //время сноса
        public double BuildDestroyTime(BuildName name, int current_lvl)
        => BuildUpTime(name, current_lvl);
        //время перестройки
        public double RebuildTime(BuildName current_build, int cur_lvl, BuildName new_build)
        {
            double ans = BuildUpTime(new_build,1);
            if (data.BuildindsData[(int)current_build].Next[0] != new_build)
                ans += BuildDestroyTime(current_build, cur_lvl);
            return ans;
        }
        //время стройки или перестройки
        #endregion
        #region функции города
        //прирост в городе от всех домиков на прирост (застройка и уровни)
        private double TownGrowthFromBuilds(BuildName[] builds, int?[] lvls)
        {
            double ans = 0;
            for (int i = 3; i < builds.Length; i++)
            {
                if (builds[i] != BuildName.none && data.BuildindsData[(int)builds[i]].Type == BuildType.grown)
                    ans += BuildEffect(builds[i], (int)lvls[i]!);
            }
            ans *= 24;
            if (lvls[0] == 20)
            {
                switch (builds[0])
                {
                    case BuildName.Pagan_temple: ans += data.WounderEffects[BuildName.Pagan_temple]; break;
                    case BuildName.The_Hanging_Gardens: ans += data.WounderEffects[BuildName.The_Hanging_Gardens]; break;
                    default: break;                    
                }                
            }
            return ans;
        }
        //спад от домиков в день
        private double TownUngrownFromBuilds(BuildName[] builds, int?[] lvls) //+ 
        {
            double ans = 0;
            for (int i = 2; i < builds.Length; i++)
                ans += Ungrown(builds[i], lvls[i]);
            return ans;
        }
        //прирост в городе без учета спада и ресурсов, но с учётом остального. Нужен для рассчёта потребления растишек
        private double TownGrowthWOUngrownAndResourses(Account acc, Town town)
        {
            double ans = TownGrowthFromBuilds(town.TownBuilds.Select(x => x.Building).ToArray(), town.TownBuilds.Select(x => (int?)x.Level).ToArray()) + acc.PopulationGrowth;
            ans *= data.RaceEffect_PopulationGrowth(acc.Race);
            ans *= GreatCitizenBonus(town.GreatCitizens[(int)GreatCitizensNames.Doctor]);
            if (town.Deposit != DepositName.none) ans *= 0.85f;
            double aiinc = 1;
            for (int i = 0; i < town.AreaImprovements.Count; i++)
                if (town.AreaImprovements[i].AIName == AreaImprovementName.Suburb)
                    aiinc += AreaImprovementBonus(AreaImprovementName.Suburb, town.AreaImprovements[i].Level, town.AreaImprovements[i].Users);
            ans *= aiinc;
            ans *= 1 + data.LuckBonusesData[(int)LuckBonusNames.grown].effect[(int)town.LuckyTown[(int)LuckBonusNames.grown]!];
            return ans;
        }                
        //прирост в городе с учётом всего
        public double TownGrowth(Account acc, Town town)
        {
            double ans = TownGrowthFromBuilds(town.TownBuilds.Select(x => x.Building).ToArray(), town.TownBuilds.Select(x => (int?)x.Level).ToArray()) + acc.PopulationGrowth;
            ans *= data.RaceEffect_PopulationGrowth(acc.Race);
            ans *= GreatCitizenBonus(town.GreatCitizens[(int)GreatCitizensNames.Doctor]);
            if (town.Deposit != DepositName.none) ans *= 0.85f;
            double aiinc = 1;
            double aidec = 0;
            for (int i = 0; i < town.AreaImprovements.Count; i++)
            {
                if (town.AreaImprovements[i].AIName == AreaImprovementName.Suburb) aiinc += AreaImprovementBonus(town.AreaImprovements[i].AIName, town.AreaImprovements[i].Level, town.AreaImprovements[i].Users);
                else if (town.AreaImprovements[i].AIName == AreaImprovementName.SkiResort) aidec += AreaImprovementBonus(town.AreaImprovements[i].AIName, town.AreaImprovements[i].Level, town.AreaImprovements[i].Users) * 0.027d;
            }
            ans *= aiinc;
            ans *= 1 + data.LuckBonusesData[(int)LuckBonusNames.grown].effect[(int)town.LuckyTown[(int)LuckBonusNames.grown]!];
            double resbonus = 1;
            for (int i = (int)ResName.fruit; i <= (int)ResName.meat; i++)
                if (town.ResConsumption[i]) resbonus += data.ResData[i].effect;
            ans *= resbonus;
            ans -= aidec;
            ans -= TownUngrownFromBuilds(town.TownBuilds.Select(x => x.Building).ToArray(), town.TownBuilds.Select(x => (int?)x.Level).ToArray());
            return ans;
        }
        //культура от домиков
        private int TownCultureFromBuilds(BuildName[] builds, int?[] lvls)
        {
            int ans = 0;
            //обычные домики
            for (int i = 3; i < builds.Length; i++)
                if (builds[i] != BuildName.none && data.BuildindsData[(int)builds[i]].Type == BuildType.culture)
                    ans += (int)BuildEffect(builds[i], (int)lvls[i]!);
            //чудо
            if ((lvls[0] == 20)
                && ((builds[0] == BuildName.Earth_Mother
                    || builds[0] == BuildName.Coliseum
                    || builds[0] == BuildName.The_Pyramids)))
                ans += (int)data.WounderEffects[builds[0]];
            //центр
            if (builds[2] != BuildName.none)
            {
                if (data.BuildindsData[(int)builds[2]].Type == BuildType.culture)
                    ans += (int)BuildEffect(builds[2], (int)lvls[2]!);
                else if (data.BuildindsData[(int)builds[2]].Type == BuildType.administration)
                    ans += (int)AdminCulture(builds[2], (int)lvls[2]!);
            }
            return ans;
        }
        //культура города без учёта ресов
        public double TownCultureWOResourses(Account acc, Town town)
        {
            //строения и база
            double ans = acc.Culture + TownCultureFromBuilds(town.TownBuilds.Select(x => x.Building).ToArray(), town.TownBuilds.Select(x => (int?)x.Level).ToArray());
            ans *= data.RaceEffect_Culture(acc.Race);
            //ВГ
            ans *= GreatCitizenBonus(town.GreatCitizens[(int)GreatCitizensNames.Creator]);
            //УМ
            double aiinc = 0;
            var ai = town.AreaImprovements.ToArray();
            for (int i = 0; i < ai.Length; i++)
                if (ai[i].AIName == AreaImprovementName.Reservation) aiinc += AreaImprovementBonus(ai[i].AIName, ai[i].Level, ai[i].Users);
            ans *= 1 + aiinc;
            //Удача
            ans *= 1 + data.LuckBonusesData[(int)LuckBonusNames.culture].effect[(int)town.LuckyTown[(int)LuckBonusNames.culture]!];
            return ans;
        }
        //культура города
        public int TownCulture(Account acc, Town town)
        {
            double ans = TownCultureWOResourses(acc, town);
            double resbonus = 1;
            for (int i = (int)ResName.wine; i <= (int)ResName.films; i++)
                if (town.ResConsumption[i]) resbonus += data.ResData[i].effect;
            ans *= resbonus;
            return (int)ans;
        }
        //производство
        //Считается из предположения, что все рабочие места во всех строениях заполнены.
        //--------
        //Есть проблема с фермами и шахтами, т.к. они могут производить несколько разных ресурсов,
        //но такой вариант использования является заведомо плохим, поэтому рассматривать его нет смысла.  
        //ДС тоже проблема, но они уже должны отмереть к моменту, когда эту прогу стоит использовать.
        //модификатор количества рабочих мест в промдомиках
        private double Workplacesmod(BuildName[] builds, int?[] lvls) //кузня, манка, завод
        {
            double workplacesmod = 1;
            for (int i = 4; i < builds.Length; i++)
            {
                if (builds[i] == BuildName.none) continue;
                if (data.BuildindsData[(int)builds[i]].Type == BuildType.prodBoost)
                {
                    workplacesmod += BuildEffect(builds[i], (int)lvls[i]!);
                    break;
                }
            }
            return workplacesmod;
        }
        //модификатор количества рабочих мест в научдомиках от кампусов
        private double WorkplacesmodScience(AreaImprovementName[] areaimps, byte[] ailvls, byte[] aiusers)//
        {
            double ans = 1;
            for (int i = 0; i < areaimps.Length; i++)
                if (areaimps[i] == AreaImprovementName.Campus)
                    ans += AreaImprovementBonus(areaimps[i], ailvls[i], aiusers[i]) * 2;
            return ans;
        }
        //модификаторы производств от улучшений местности по ресурсам в долях
        private double[] AIBonuses(AreaImprovementName[] areaimps, byte[] ailvls, byte[] aiusers, DepositName deposit)//+
        {            
            double[] ans = new double[23];
            for (int i = 0; i < 23; i++)
                ans[i] = 1;
            for (int i = 0; i < areaimps.Length; i++)
            {
                if (ailvls[i] == 0) continue;
                double bonus = AreaImprovementBonus(areaimps[i], ailvls[i], aiusers[i]);
                switch (areaimps[i])
                {
                    case AreaImprovementName.Irrigation:
                        for (int j = 0; j < 23; j++)
                            if (data.ResData[j].prodtype == ResProdType.agriculture)
                                ans[j] += bonus;
                        break;
                    case AreaImprovementName.Mines:
                    case AreaImprovementName.HydrotechnicalFacility:
                        for (int j = 0; j < 23; j++)
                            if (data.ResData[j].prodtype == ResProdType.industry)
                                ans[j] += bonus;
                        break;
                    case AreaImprovementName.Campus:
                    case AreaImprovementName.ResearchWaterArea:
                        ans[(int)ResName.science] += bonus;
                        break;
                    case AreaImprovementName.Fair:
                    case AreaImprovementName.Resort:
                        ans[(int)ResName.money] += bonus;
                        break;
                    case AreaImprovementName.FishingArea:
                        ans[(int)ResName.fish] += bonus;
                        if (deposit == DepositName.pearls) ans[(int)ResName.jewelry] += bonus;
                        break;
                    case AreaImprovementName.SkiResort:
                        break; //он специфичный, его тут нельзя учитывать
                    default: break;
                }
            }
            ans = ans.Select(x => Math.Round(x, 4)).ToArray(); //не уверен, что округление тут правильное
            return ans;
        }
        //базовое производство ресов в городе с учётом мр и положения города
        private double[] BaseProd(DepositName deposit, bool on_hill, byte waterplaces)//+
        {
            double[] ans = new double[23];
            for (int i = 0; i < 23; i++)
                ans[i] = data.BaseProduction((ResName)i, on_hill, waterplaces);
            if (deposit != DepositName.none)
                foreach (var x in data.DepositsData[(int)deposit].res)
                    ans[(int)x[1]] += x[0];
            return ans.Select(x=>(double)(float)x).ToArray(); //в double какие-то лишние цифры появляются, это их отгрызает. Может быть стоит просто не использовать double
        }
        //модификатор производства от ВГ (количество ВГ)
        private double[] GreatSitizensProdBonus(byte[] gs)//+
        {
            double[] ans = new double[4];
            ans[(int)ResProdType.research] = GreatCitizenBonus(gs[(int)GreatCitizensNames.Scientist]);
            ans[(int)ResProdType.finance] = GreatCitizenBonus(gs[(int)GreatCitizensNames.Financier]);
            ans[(int)ResProdType.agriculture] = GreatCitizenBonus(gs[(int)GreatCitizensNames.Agronomist]);
            ans[(int)ResProdType.industry] = GreatCitizenBonus(gs[(int)GreatCitizensNames.Engineer]);
            return ans;
        }
        //Большая жуткая штука, которая считает промку города полную.
        public double[] TownProduction(Account acc, Town town)
        {
            double[] ans = new double[23];
            if (acc is null || town is null) return ans;
            var builds = town.TownBuilds.Select(x => x.Building).ToArray();
            var lvls = town.TownBuilds.Select(x => (int?)x.Level).ToArray();
            var areaimps = town.AreaImprovements.Select(x => x.AIName).ToArray();
            var ailvls = town.AreaImprovements.Select(x => x.Level).ToArray();
            var aiusers = town.AreaImprovements.Select(x => x.Users).ToArray();
            double workplacesmod = Workplacesmod(builds, lvls);
            double workplacesmodSciense = WorkplacesmodScience(areaimps, ailvls, aiusers);
            int total_wp = 0;
            //считаем все рабочие места для эффективности труда
            //и рабочие места по ресам с учётом эффективности строения
            int[] workplaces = new int[23]; //+
            double[] workplaces_efbuildmod = new double[23];
            for (int i = 3; i < builds.Length; i++)
            {
                if (builds[i] == BuildName.none) continue;
                if (data.BuildindsData[(int)builds[i]].Type == BuildType.production)
                {
                    double wp = BuildEffect(builds[i], (int)lvls[i]!);
                    int nwp = 0;
                    foreach (var r in data.BuildindsData[(int)builds[i]].Productres)
                    {
                        if (r.Res == ResName.science)
                            nwp = (int)(wp * workplacesmodSciense);
                        else
                            nwp = (int)(wp * workplacesmod);
                        workplaces[(int)r.Res] += nwp;
                        workplaces_efbuildmod[(int)r.Res] += nwp * data.BuildindsData[(int)builds[i]].Efficiency;
                    }
                    total_wp += nwp;
                }
            }
            //ээфективность труда города
            double townefficiency = LaborEfficiency(total_wp);
            //базовое производство с учётом МР
            double[] baseprod = BaseProd(town.Deposit, town.OnHill, town.WaterPlaces);
            //модификаторы от ум
            double[] aibonuses = AIBonuses(areaimps, ailvls, aiusers, town.Deposit);//+
            //великие граждание по типу производства
            var gsb = GreatSitizensProdBonus(town.GreatCitizens.ToArray());
            //коррупция города
            double corruption = TownCorruption(builds, lvls, acc.Towns.Count);
            //теперь всё посчитать!
            for (int i = 0; i < 23; i++)
            {
                if (town.Product[i]) ans[i] = workplaces_efbuildmod[i];
                else { ans[i] = 0; continue; }
                int type = (int)data.ResData[i].prodtype;
                ans[i] *= townefficiency;
                ans[i] *= 1 - corruption / 100;
                ans[i] *= data.ClimateEffect(town.Climate, (ResProdType)type);
                ans[i] *= (double)acc.Science_Bonuses[type] / 100d;
                ans[i] *= baseprod[i];
                ans[i] *= gsb[type];
                ans[i] *= aibonuses[i];
                ans[i] *= data.RaceEffect_ProdMod(acc.Race, (ResName)i);
            }
            //учёт МУ
            var LevelLuckBonusProd = town.LuckyTown[(int)LuckBonusNames.production];
            var LevelLuckBonusSciense = town.LuckyTown[(int)LuckBonusNames.science];
            for (int i = 1; i < 23; i++)
                ans[i] *= 1 + data.LuckBonusesData[(int)LuckBonusNames.production].effect[(int)LevelLuckBonusProd!]; //кроме науки
            ans[(int)ResName.science] *= 1 + data.LuckBonusesData[(int)LuckBonusNames.science].effect[(int)LevelLuckBonusSciense!];
            //+горнолыжки
            double srb = 0;
            for (int i = 1; i < areaimps.Length; i++)
                if (areaimps[i] == AreaImprovementName.SkiResort)
                    srb += AreaImprovementBonus(AreaImprovementName.SkiResort, ailvls[i], aiusers[i]);
            srb *= 1 + data.LuckBonusesData[(int)LuckBonusNames.production].effect[(int)LevelLuckBonusProd!];
            ans[(int)ResName.money] += srb;
            //книжки
            if (town.ResConsumption[(int)ResName.books]) ans[(int)ResName.science] *= 1 + data.ResData[(int)ResName.books].effect;
            //+чудеса с фиксированным бонусом
            if (lvls[0] == 20)
                switch (builds[0])
                {
                    case BuildName.Geoglyph:
                    case BuildName.The_Great_Library:
                    case BuildName.Helioconcentrator:
                        ans[(int)ResName.science] += data.WounderEffects[builds[0]];
                        break;
                    case BuildName.Earthen_dam:
                        ans[(int)ResName.fish] += data.WounderEffects[builds[0]];
                        break;
                    case BuildName.The_Colossus:
                        ans[(int)ResName.money] += data.WounderEffects[builds[0]];
                        break;
                    default: break;
                }
            //вроде ничего не забыл
            ans = ans.Select(x => x is double.NaN ? 0 : x).ToArray();
            return ans;
        }
        //производ без му, к сожалению нужно считать целиком отдельно из-за сложной формулы. Потом мб перепишу, поделив всё на множители и слагаемые отдельные
        private double[] TownProdWOLuck(Town town, Account acc)
        {
            double[] ans = new double[23];
            if (acc is null || town is null) return ans;
            var builds = town.TownBuilds.Select(x => x.Building).ToArray();
            var lvls = town.TownBuilds.Select(x => (int?)x.Level).ToArray();
            var areaimps = town.AreaImprovements.Select(x => x.AIName).ToArray();
            var ailvls = town.AreaImprovements.Select(x => x.Level).ToArray();
            var aiusers = town.AreaImprovements.Select(x => x.Users).ToArray();
            double workplacesmod = Workplacesmod(builds, lvls);
            double workplacesmodSciense = WorkplacesmodScience(areaimps, ailvls, aiusers);
            int total_wp = 0;
            //считаем все рабочие места для эффективности труда
            //и рабочие места по ресам с учётом эффективности строения
            int[] workplaces = new int[23]; //+
            double[] workplaces_efbuildmod = new double[23];
            for (int i = 3; i < builds.Length; i++)
            {
                if (builds[i] == BuildName.none) continue;
                if (data.BuildindsData[(int)builds[i]].Type == BuildType.production)
                {
                    double wp = BuildEffect(builds[i], (int)lvls[i]!);
                    int nwp = 0;
                    foreach (var r in data.BuildindsData[(int)builds[i]].Productres)
                    {
                        if (r.Res == ResName.science)
                            nwp = (int)(wp * workplacesmodSciense);
                        else
                            nwp = (int)(wp * workplacesmod);
                        workplaces[(int)r.Res] += nwp;
                        workplaces_efbuildmod[(int)r.Res] += nwp * data.BuildindsData[(int)builds[i]].Efficiency;
                    }
                    total_wp += nwp;
                }
            }
            //ээфективность труда города
            double townefficiency = LaborEfficiency(total_wp);
            //базовое производство с учётом МР
            double[] baseprod = BaseProd(town.Deposit, town.OnHill, town.WaterPlaces);
            //модификаторы от ум
            double[] aibonuses = AIBonuses(areaimps, ailvls, aiusers, town.Deposit);//+
            //великие граждание по типу производства
            var gsb = GreatSitizensProdBonus(town.GreatCitizens.ToArray());
            //коррупция города
            double corruption = TownCorruption(builds, lvls, acc.Towns.Count);
            //теперь всё посчитать!
            for (int i = 0; i < 23; i++)
            {
                if (town.Product[i]) ans[i] = workplaces_efbuildmod[i];
                else { ans[i] = 0; continue; }
                int type = (int)data.ResData[i].prodtype;
                ans[i] *= townefficiency;
                ans[i] *= 1 - corruption / 100;
                ans[i] *= data.ClimateEffect(town.Climate, (ResProdType)type);
                ans[i] *= (double)acc.Science_Bonuses[type] / 100d;
                ans[i] *= baseprod[i];
                ans[i] *= gsb[type];
                ans[i] *= aibonuses[i];
                ans[i] *= data.RaceEffect_ProdMod(acc.Race, (ResName)i);
            }
            //учёт МУ
            //var LevelLuckBonusProd = town.LuckyTown[(int)LuckBonusNames.production];
            //var LevelLuckBonusSciense = town.LuckyTown[(int)LuckBonusNames.science];
            //for (int i = 1; i < 23; i++)
            //    ans[i] *= 1 + data.LuckBonusesData[(int)LuckBonusNames.production].effect[(int)LevelLuckBonusProd!]; //кроме науки
            //ans[(int)ResName.science] *= 1 + data.LuckBonusesData[(int)LuckBonusNames.science].effect[(int)LevelLuckBonusSciense!];
            //+горнолыжки
            double srb = 0;
            for (int i = 1; i < areaimps.Length; i++)
                if (areaimps[i] == AreaImprovementName.SkiResort)
                    srb += AreaImprovementBonus(AreaImprovementName.SkiResort, ailvls[i], aiusers[i]);
            //srb *= 1 + data.LuckBonusesData[(int)LuckBonusNames.production].effect[(int)LevelLuckBonusProd!];
            ans[(int)ResName.money] += srb;
            //книжки
            if (town.ResConsumption[(int)ResName.books]) ans[(int)ResName.science] *= 1 + data.ResData[(int)ResName.books].effect;
            //+чудеса с фиксированным бонусом
            if (lvls[0] == 20)
                switch (builds[0])
                {
                    case BuildName.Geoglyph:
                    case BuildName.The_Great_Library:
                    case BuildName.Helioconcentrator:
                        ans[(int)ResName.science] += data.WounderEffects[builds[0]];
                        break;
                    case BuildName.Earthen_dam:
                        ans[(int)ResName.fish] += data.WounderEffects[builds[0]];
                        break;
                    case BuildName.The_Colossus:
                        ans[(int)ResName.money] += data.WounderEffects[builds[0]];
                        break;
                    default: break;
                }
            //вроде ничего не забыл
            ans = ans.Select(x => x is double.NaN ? 0 : x).ToArray();
            return ans;
        }
        //Количество реса на одного торга
        public double ResPerTraider(Account acc, Town town)
        {
            var r = TownProduction(acc, town);
            double export = 0;
            for (int i = 3; i < 23; i++)
            {
                export += r[i];
            }
            return export / (double)Traiders(town, acc);
        }
        //денежный эквивалент промки
        public double TownProductionValue(Account acc, Town town)
            => ResToMoney<double>(TownProduction(acc, town), acc.Financial.Prices);
        //коррупция города
        private double TownCorruption(BuildName[] builds, int?[] lvls, int numoftowns)//+
        {
            byte chl = 0;
            for (int i = 3; i < builds.Length; i++)
                if (builds[i] == BuildName.courthouse)
                {
                    chl = (byte)lvls[i]!;
                    break;
                }
            return Corruption(numoftowns, chl);
        }
        //потребление ресурсов
        //культишка
        //сколько жрут реса
        private double CultCons(double cultWOres, ResName res, Race r)
        {
            if (res < ResName.wine || res > ResName.films) return 0;
            else return data.ResData[(int)res].consumption * (int)cultWOres * data.ResData[(int)res].effect * data.RaceEffect_Consumption(r);
        }
        //цена поедаемой культуры
        private double CultConsPrice(Town town, Account acc)
        {
            double ans = 0;
            double cultWOres = TownCultureWOResourses(acc, town);
            for (int i = (int)ResName.wine; i <= (int)ResName.films; i++)
            {
                if (!town.ResConsumption[i]) continue;
                ans += CultCons(cultWOres, (ResName)i, acc.Race) * acc.Financial.Prices[i] * 0.001d;
            }
            return ans;
        }
        //растишка
        private double GrowthCons(double growthWOres, ResName res, Race r)
        {
            if (res < ResName.fruit || res > ResName.meat) return 0;
            else return data.ResData[(int)res].consumption * growthWOres * data.ResData[(int)res].effect * data.RaceEffect_Consumption(r);
        }
        //цена поедаемых ростов
        private double GrowthConsPrice(Town town, Account acc)
        {
            double ans = 0;
            double growthWOres = TownGrowthWOUngrownAndResourses(acc, town);
            for (int i = (int)ResName.fruit; i <= (int)ResName.meat; i++)
            {
                if (!town.ResConsumption[i]) continue;
                ans += GrowthCons(growthWOres, (ResName)i, acc.Race) * acc.Financial.Prices[i] * 0.001d;
            }
            return ans;
        }
        //экономика города
        //экономия от городского центра
        private double TownAdminEconomyMultiplier(Town town)
        {
            double ans = 1;
            var b = town.TownBuilds[2].Building;
            var l = town.TownBuilds[2].Level;
            if (b != BuildName.none
                && data.BuildindsData[(int)b].Type == BuildType.administration)
                ans -= AdminEconimy(b, (int)l!);            
            return ans;
        }
        //массив потребления ресов
        public double[] GetResConsumption(Account acc, Town town)
        {
            double[] ans = new double[23];
            double growthWOres = TownGrowthWOUngrownAndResourses(acc, town);
            double cultWOres = TownCultureWOResourses(acc, town);
            double scienseWObooks = TownProduction(acc, town)[0];
            for (int i = 0; i < 23; i++)
            {
                if (!town.ResConsumption[i]) continue;
                int t = data.ResData[i].type;
                if (t == 1) ans[i] = GrowthCons(growthWOres, (ResName)i, acc.Race); //food
                if (t == 2) ans[i] = CultCons(cultWOres, (ResName)i, acc.Race); //cult
                if (t == 3) ans[i] = data.ResData[i].effect * scienseWObooks * data.ResData[i].consumption;//books
            }
            return ans;
        }
        //расходы на содержание города (денежные + закуп растишек и культишек + закуп МУ с рынка на МУ бонусы
        public double TownUpkeep(Town town, Account acc)
        {
            //домики
            double ans = TownBuildsUpkeep(town, acc);
            //ресы
            var rc = GetResConsumption(acc, town);
            for (int i = 0; i < 23; i++)            
                ans += rc[i] * acc.Financial.Prices[i] * 0.001d;
            //МУ
            var lc = town.LuckyTown;
            for (int i = 0; i < lc.Count; i++)
            {
                if (lc[i] is null) continue;
                ans += LuckBonusPrice((LuckBonusNames)i, acc.Towns.Count, (int)lc[i]!) / 168d * acc.Financial.LuckCoinPrice;
            }
            return ans;
        }
        //потребление ДЕНЕГ всеми домиками города
        public double TownBuildsUpkeep(Town town, Account acc)
        {            
            double ans = 0;
            var builds = town.TownBuilds.Select(x => x.Building).ToList();
            var lvls = town.TownBuilds.Select(x => x.Level).ToList();
            for (int i = 0; i < 19; i++)
            {
                if (builds[i] != BuildName.none && data.BuildindsData[(int)builds[i]].Pay is not null)
                {
                    ans += Pay(builds[i], (int)lvls[i]!);
                }
            }
            ans *= data.RaceEffect_Upkeep(acc.Race);     
            ans *= TownAdminEconomyMultiplier(town);
            return ans;
        }
        //содержание демографических строений
        private double BuildsUpkeepGrowth(Town town, Account acc)
        {
            //содержание демографических строений
            {
                var builds = town.TownBuilds.Select(x => x.Building).ToList();
                var lvls = town.TownBuilds.Select(x => x.Level).ToList();
                double ans = 0;
                for (int i = 0; i < 19; i++)
                {
                    int id = (int)builds[i];
                    if (builds[i] != BuildName.none && data.BuildindsData[id].Type == BuildType.grown && data.BuildindsData[id].Pay is not null)
                    {
                        ans += Pay(builds[i], (int)lvls[i]!);
                    }
                }
                ans *= data.RaceEffect_Upkeep(acc.Race);
                ans *= TownAdminEconomyMultiplier(town);
                return ans;
            }
        }
        //полные затраты на прирост с учётом МУ
        public double GrowthFullUpkeep(Town town, Account acc)
        {
            double ans = 0;
            ans += BuildsUpkeepGrowth(town, acc);
            ans += GrowthConsPrice(town, acc);
            ans += LuckBonusPrice(LuckBonusNames.grown, acc.Towns.Count, (int)town.LuckyTown[(int)LuckBonusNames.grown]!) / 168d * acc.Financial.LuckCoinPrice;
            return ans;
        }     
        //содержание посольки и ПВО
        private double BuildsUpkeepStrategic(Town town, Account acc)
        {

            var builds = town.TownBuilds.Select(x => x.Building).ToList();
            var lvls = town.TownBuilds.Select(x => x.Level).ToList();
            double ans = 0;
            for (int i = 2; i < 19; i++)
            {
                int id = (int)builds[i];
                if (builds[i] != BuildName.none
                    && (data.BuildindsData[id].Type == BuildType.embassy ||
                        data.BuildindsData[id].Type == BuildType.airdef)
                    && data.BuildindsData[id].Pay is not null)
                {
                    ans += Pay(builds[i], (int)lvls[i]!);
                }
            }
            ans *= data.RaceEffect_Upkeep(acc.Race);
            ans *= TownAdminEconomyMultiplier(town);
            return ans;
        }
        //содержание военных строений
        private double BuildsUpkeepWarBuilds(Town town, Account acc)
        {

            var builds = town.TownBuilds.Select(x => x.Building).ToList();
            var lvls = town.TownBuilds.Select(x => x.Level).ToList();
            double ans = 0;
            for (int i = 2; i < 19; i++)
            {
                int id = (int)builds[i];
                if (builds[i] != BuildName.none
                    && (data.BuildindsData[id].Type == BuildType.train ||
                        data.BuildindsData[id].Type == BuildType.waterarmyspeed ||
                        data.BuildindsData[id].Type == BuildType.airarmyspeed)
                    && data.BuildindsData[id].Pay is not null)
                {
                    ans += Pay(builds[i], (int)lvls[i]!);
                }
            }            
            ans *= data.RaceEffect_Upkeep(acc.Race);
            ans *= TownAdminEconomyMultiplier(town);
            return ans;
        }
        //содержание научных строений
        private double BuildsUpkeepScience(Town town, Account acc)
        {
            var builds = town.TownBuilds.Select(x => x.Building).ToList();
            var lvls = town.TownBuilds.Select(x => x.Level).ToList();
            double ans = 0;
            for (int i = 2; i < 19; i++)
            {
                int id = (int)builds[i];
                if (builds[i] != BuildName.none
                    && data.BuildindsData[id].Type == BuildType.production
                    && data.BuildindsData[id].Productres[0].Res == ResName.science
                    && data.BuildindsData[id].Pay is not null)
                {
                    ans += Pay(builds[i], (int)lvls[i]!);
                }
            }            
            ans *= data.RaceEffect_Upkeep(acc.Race);
            ans *= TownAdminEconomyMultiplier(town);
            return ans;
        }
        //содержание фортификации
        private double BuildsUpkeepFort(Town town, Account acc)
        {
            var builds = town.TownBuilds.Select(x => x.Building).ToList();
            var lvls = town.TownBuilds.Select(x => x.Level).ToList();
            double ans = 0;
            int id = (int)builds[1];
            if (builds[1] != BuildName.none && data.BuildindsData[id].Pay is not null)                
                ans += Pay(builds[1], (int)lvls[1]!);                                        
            ans *= data.RaceEffect_Upkeep(acc.Race);
            ans *= TownAdminEconomyMultiplier(town);
            return ans;
        }
        //содержание культурных строений
        private double BuildsUpkeepCulture(Town town, Account acc)
        {
            var builds = town.TownBuilds.Select(x => x.Building).ToList();
            var lvls = town.TownBuilds.Select(x => x.Level).ToList();
            double ans = 0;
            for (int i = 2; i < 19; i++)
            {
                int id = (int)builds[i];
                if (builds[i] != BuildName.none && data.BuildindsData[id].Type == BuildType.culture && data.BuildindsData[id].Pay is not null)
                {
                    ans += Pay(builds[i], (int)lvls[i]!);
                }
            }
            if (builds[2] != BuildName.none && data.BuildindsData[(int)builds[2]].Pay is not null) ans += Pay(builds[2], (int)lvls[2]!);
            ans *= data.RaceEffect_Upkeep(acc.Race);
            ans *= TownAdminEconomyMultiplier(town);
            return ans;
        }
        //полные затраты на культуру с МУ
        public double CultureFullUpkeep(Town town, Account acc)
        {
            double ans = 0;
            ans += BuildsUpkeepCulture(town, acc) + CultConsPrice(town, acc);
            ans += LuckBonusPrice(LuckBonusNames.culture, acc.Towns.Count, (int)town.LuckyTown[(int)LuckBonusNames.culture]!) / 168d * acc.Financial.LuckCoinPrice;
            return ans;
        }
        //содержание торговых домиков
        private double BuildsUpkeepTraiding(Town town, Account acc) 
        {
            double ans = 0;
            var builds = town.TownBuilds.Select(x => x.Building).ToList();
            var lvls = town.TownBuilds.Select(x => x.Level).ToList();
            for (int i = 3; i < 19; i++)
            {
                int id = (int)builds[i];
                if (builds[i] != BuildName.none &&
                    (data.BuildindsData[id].Type == BuildType.trade
                    || data.BuildindsData[id].Type == BuildType.tradespeed
                    || data.BuildindsData[id].Type == BuildType.watertradespeed)
                    && data.BuildindsData[id].Pay is not null
                    && lvls[i] is not null
                    && lvls[i] > 0)
                { ans += Pay(builds[i], (int)lvls[i]!); }
            }
            ans *= data.RaceEffect_Upkeep(acc.Race);
            ans *= TownAdminEconomyMultiplier(town);
            return ans;
        }
        //количество торгашей в городе
        public int Traiders(Town town, Account acc)
        {
            //торгаши от наук
            int ans = acc.Traiders;
            //+торгаши от домиков с торгашами
            var builds = town.TownBuilds.Select(x=>x.Building).ToArray();
            var lvls = town.TownBuilds.Select(x=>x.Level).ToArray();            
            for (int i = 3; i < builds.Length; i++) 
            {
                if (builds[i] == BuildName.none || lvls[i] is null || lvls[i]==0) continue;
                if (data.BuildindsData[(int)builds[i]].Type != BuildType.trade) continue;
                ans += (int)Math.Truncate(BuildEffect(builds[i], (int)lvls[i]!));
            }
            //+стоунхендж
            if (builds[0] == BuildName.Stonehenge && lvls[0] == 20) ans += (int)data.WounderEffects[BuildName.Stonehenge];
            //+торгаши за МУ (мб не совем верно, надо проверять)
            ans += (int)Math.Round(data.LuckBonusesData[(int)LuckBonusNames.traders].effect[(int)town.LuckyTown[(int)LuckBonusNames.traders]!]); 
            return ans;
        }
        //цена за 1 торгаша с учётом ускорительных домиков всяких
        public double TraiderPrice(Town town, Account acc)
        {
            double ans = BuildsUpkeepTraiding(town, acc);
            ans += LuckBonusPrice(LuckBonusNames.traders, acc.Towns.Count, (int)town.LuckyTown[(int)LuckBonusNames.traders]!)*acc.Financial.LuckCoinPrice/7d/24d; //затраты му в неделю, а расходы в час
            ans /= Traiders(town, acc);
            return ans;
        }
        //Прочность города полная
        public long TownStrength(Town town, Account acc)
        {
            int ans = 0;
            if (town.Deposit != DepositName.none) ans += ColonyDestroy(acc.Towns.Count);//+прочность мр
            //+прочность домиков, кроме чуда и ямы
            BuildName b;
            byte? l;
            for (int i = 2; i < town.TownBuilds.Count; i++)
            {
                b = town.TownBuilds[i].Building;
                l = town.TownBuilds[i].Level;
                if (b != BuildName.none && l is not null && l > 0)
                    ans += BuildStrength(b, (int)l!);
            }
            //+фортификация            
            b = town.TownBuilds[1].Building;
            l = town.TownBuilds[1].Level;
            if (b != BuildName.none && b!= BuildName.moat && l is not null && l > 0)
                ans += BuildStrength(b, (int)l!);
            return ans;
        }
        //Полные затраты ресов на постройку города
        private int TownSlotsCost(Town town)
        {
            int ans = 0;
            //покупаемые слоты: 11,14,15,16
            List<byte> bs = new List<byte> { 11, 14, 15, 16 };
            byte c = 0;
            foreach (byte b in bs)
                if (town.TownBuilds[b].Available) 
                { 
                    ans += 5000 * (int)Math.Pow( 10, c);
                    c++;
                }            
            return ans;
        }
        public long[] TownTotalResCost(Town town, Account acc, bool aimultiuser = false) 
        {
            long[] ans = new long[23];
            //строения
            BuildName b;
            int l;
            int[][] btrc = new int[19][];
            for (int i = 0; i < town.TownBuilds.Count; i++)
            {
                b = town.TownBuilds[i].Building;
                l = town.TownBuilds[i].Level is null ? 0 : (int)town.TownBuilds[i].Level!;
                btrc[i] = BuildTotalResCost(b, l);                
            }
            for (int i = 0; i < 19; i++) //по домикам            
                for (int j = 0; j < 23; j++) //по ресам                
                    ans[j] += btrc[i][j];
            //улучшения местности учитываются в виде денег. По другому их учесть затруднительно.
            for (int i = 0; i < town.AreaImprovements.Count; i++)
            {
                double aic = AreaImprovementPriceForAcc(town.AreaImprovements[i].AIName, town.AreaImprovements[i].Level, acc);
                if (aimultiuser)
                {
                    ans[(int)ResName.money] += (int)(aic / town.AreaImprovements[i].Users);
                }
                else ans[(int)ResName.money] += (int)aic;
            }
            //слоты под постройки
            //Может некорректно работать, если пользователь отметит некупленными клетки, который не могут быть таковыми.
            //Это очень странный сценарий использования и маловероятный, так что пока можно не чинить.
            ans[(int)ResName.money] += TownSlotsCost(town);
            return ans;
        }
        //Цена города
        public long TownPrice(Town town, Account acc, bool aimultiuser = false)
        {
            double ans = 0;
            long[] ttrc = TownTotalResCost(town, acc, aimultiuser);
            for (int i = 0; i < ttrc.Length; i++)            
                ans += acc.Financial.Prices[i] * ttrc[i] * 0.001f;            
            return (long)ans;
        }
        //Дотация городу из казны в час
        public double TownDotation(Town town, Account acc)
        {
            double ans = 0;
            double[] tprod = TownProduction(acc, town);
            //за промку
            for (int i = 0; i < 23; i++)            
                ans += (acc.Financial.Taxes[i] < 0 ? -acc.Financial.Taxes[i]*0.001d : 0) * tprod[i];            
            //за влив колб
            if (tprod[(int)ResName.science] > 0 && town.ScienceEfficiency is not null && acc.Financial.ForKnowledgeInvestment > 0)
                ans += acc.Financial.ForKnowledgeInvestment * tprod[(int)ResName.science] * (double)town.ScienceEfficiency!*0.01d;
            //за посольки и ПВО
            if (acc.Financial.ForStrategicBuildings > 0)
                ans += BuildsUpkeepStrategic(town, acc) * acc.Financial.ForStrategicBuildings;
            //ForMillitaryBuildings
            if (acc.Financial.ForMillitaryBuildings > 0)
                ans += BuildsUpkeepWarBuilds(town, acc) * acc.Financial.ForMillitaryBuildings;
            //ForScientificBuildings
            if (acc.Financial.ForScientificBuildings > 0)
                ans += BuildsUpkeepScience(town, acc) * acc.Financial.ForScientificBuildings;
            //ForFortificationBuildings
            if (acc.Financial.ForFortificationBuildings > 0)
                ans += BuildsUpkeepFort(town, acc) * acc.Financial.ForFortificationBuildings;
            //Дота городу, пока не реализовано
            return ans;
        }
        //Налог с города
        public double TownTax(Town town, Account acc)
        {
            double ans = 0;
            double[] tprod = TownProdWOLuck(town,acc);
            for (int i = 0; i < 23; i++)
                ans += (acc.Financial.Taxes[i] > 0 ? acc.Financial.Taxes[i]*0.001d : 0) * tprod[i];
            if (town.Deposit != DepositName.none)
                ans += acc.Financial.DepositsTaxes[(int)town.Deposit];
            return ans;
        }
        //доход игрока (как вырасти быстрее всех?)
        public double TownProfitForOwner(Town town, Account acc)        
            => TownProductionValue(acc, town) - TownUpkeep(town, acc) - TownTax(town, acc) + TownDotation(town, acc);                    
        //доход страны (эта цифра нужна для выбора целей воеводами) 
        public double TownProfitForCountry(Town town, Account acc)
            => TownProductionValue(acc, town) - TownUpkeep(town, acc) + TownTax(town, acc) - TownDotation(town, acc);
        //затраты ресов на перестройку/ап города
        public int[] TownRebuildResCost(Town town_old, Town town_new, Account acc, bool aimultiuser = false)
        {
            //постройки
            var b1 = town_old.TownBuilds;
            var b2 = town_new.TownBuilds;
            int[] rc = new int[23];
            for (int i = 0; i < 19; i++)
            {
                int[] t = new int[23];                
                if (b2[i].Building == BuildName.none || b2[i].Level is null || b2[i].Level == 0) continue; //если в новом городе клетка пустая, то скипаем дальше
                //перестройка
                if (b1[i].Building != b2[i].Building)
                {
                    t = RebuildResCost(b1[i].Building, b1[i].Level, b2[i].Building);
                    for (int j = 0; j < 23; j++) rc[j] += t[j];
                    if (b2[i].Level > 1)
                    {
                        t = BuildUpFromToResCost(b2[i].Building, 1, (int)b2[i].Level!);
                        for (int j = 0; j < 23; j++) rc[j] += t[j];
                    }
                }
                //ап уровня
                else if (b1[i].Level < b2[i].Level)
                {
                    t = BuildUpFromToResCost(b1[i].Building, (int)b1[i].Level!, (int)b2[i].Level!);
                    for (int j = 0; j < 23; j++) rc[j] += t[j];                    
                }
            }
            //ум
            //хз как считать правильно, потому что могут быть замены и сносы ум
            //первый вариант будет считать из предположения что УМ только добавляются и апаются и идут в одинаковом порядке
            //потом надо бы переделать
            var ai1 = town_old.AreaImprovements;
            var ai2 = town_new.AreaImprovements;
            for (int i = 0; i < ai2.Count; i++)
            {
                rc[(int)ResName.money] += (int)AreaImprovementPriceForAcc(ai2[i].AIName, ai2[i].Level, acc);
                if (i < ai1.Count)
                    rc[(int)ResName.money] -= (int)AreaImprovementPriceForAcc(ai1[i].AIName, ai1[i].Level, acc);
            }
            //клетки
            rc[(int)ResName.money] += TownSlotsCost(town_new) - TownSlotsCost(town_old);
            return rc;
        }
        //стоимость перестройки/апа города
        public double TownRebuildCost(Town town_old, Town town_new, Account acc, bool aimultiuser = false)
        {            
            //постройки            
            int[] rc = TownRebuildResCost(town_old,town_new,acc,aimultiuser);            
            return ResToMoney<int>(rc, acc.Financial.Prices);
        }
        //вреня на перестройку/ап города
        public double TownRebuildTime(Town town_old, Town town_new, Account acc, bool aimultiuser = false)
        {
            double ans = 0;
            var ob = town_old.TownBuilds.Select(x => x.Building).ToArray();
            var ol = town_old.TownBuilds.Select(x => (int)(x.Level is null ? 0 : x.Level)).ToArray();
            var nb = town_new.TownBuilds.Select(x => x.Building).ToArray();
            var nl = town_new.TownBuilds.Select(x => (int)(x.Level is null ? 0 : x.Level)).ToArray();
            for (int i = 0; i < town_new.TownBuilds.Count; i++)
            {
                //пусто или без изменений
                if (town_new.TownBuilds[i] == town_old.TownBuilds[i]) continue;
                //ап уровня
                if (ob[i] == nb[i])
                {
                    if (nl[i] > ol[i]) { ans += BuildUpTimeFromTo(nb[i], ol[i], nl[i]); continue; } 
                    else continue;
                }
                //новое
                if (ob[i] == BuildName.none && nb[i] != BuildName.none && nl[i]>0) { ans += BuildUpTimeFromTo(nb[i], 0, nl[i]); continue; }
                //снос
                if (nb[i] == BuildName.none && ob[i] != BuildName.none) { ans += BuildDestroyTime(ob[i], ol[i]); continue; }
                //перестройка
                if (ob[i] != BuildName.none && nb[i] != BuildName.none) 
                {
                    ans += RebuildTime(ob[i], ol[i], nb[i]);
                    if (nl[i] > 1) ans += BuildUpTimeFromTo(nb[i], 1, nl[i]);
                    continue; 
                }                
            }
            return ans;
        }
        //Доступно всего рабочих мест в городе
        public int TownWorkplaces(Town town)
        {
            int ans = 0;
            var b = town.TownBuilds;
            double workplacesmod = Workplacesmod(b.Select(x => x.Building).ToArray(), b.Select(x => (int?)x.Level).ToArray()) ;
            var areaimps = town.AreaImprovements.Select(x => x.AIName).ToArray();
            var ailvls = town.AreaImprovements.Select(x => x.Level).ToArray();
            var aiusers = town.AreaImprovements.Select(x => x.Users).ToArray();
            double workplacesmodSciense = WorkplacesmodScience(areaimps, ailvls, aiusers);
            for (int i = 3; i < b.Count; i++)
            {
                if (b[i].Building == BuildName.none) continue;
                if (data.BuildindsData[(int)b[i].Building].Type == BuildType.production)
                {
                    double wp = BuildEffect(b[i].Building, (int)b[i].Level!);
                    int nwp = 0;
                    foreach (var r in data.BuildindsData[(int)b[i].Building].Productres)
                    {
                        if (r.Res == ResName.science)
                            nwp = (int)(wp * workplacesmodSciense);
                        else
                            nwp = (int)(wp * workplacesmod);                        
                    }
                    ans += nwp;
                }
            }
            return ans;
        }
        #endregion
    }
}
