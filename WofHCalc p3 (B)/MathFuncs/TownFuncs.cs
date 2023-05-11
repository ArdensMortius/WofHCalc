using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WofHCalc.Supports;
using WofHCalc.DataSourses;
using System.Runtime.InteropServices.Marshalling;
using WofHCalc.Supports.jsonTemplates;
using System.Windows.Documents;
using Microsoft.Windows.Themes;
using WofHCalc.Models;
using System.Diagnostics;
using WofHCalc.Models.jsonTemplates;

namespace WofHCalc.MathFuncs
{
    public static class TownFuncs
    {
        //прирост
        private static double TownGrowthFromBuilds(BuildName[] builds, int?[] lvls)
        {
            double ans = 0;
            for (int i = 3; i < builds.Length;i++)
            {
                if (builds[i] != BuildName.none && Data.BuildindsData[(int)builds[i]].Type == BuildType.grown)                
                    ans += BuildFuncs.BuildEffect(builds[i], (int)lvls[i]!);                                    
            }
            ans *= 24;
            if (builds[0] == BuildName.Pagan_temple && lvls[0] == 20)
            {
                ans += Data.WounderEffects[BuildName.Pagan_temple];
            }
            return ans;
        }
        private static double TownUngrownFromBuilds(BuildName[] builds, int?[] lvls) //+ но погрешность небольшая есть
        {
            double ans = 0;
            for (int i = 2; i<builds.Length;i++)            
                if (builds[i] != BuildName.none)
                    ans +=FuncsBase.MainFunc(Data.BuildindsData[(int)builds[i]].Ungrown, (int)lvls[i]!);            
            return ans*24;
        }

        public static double TownGrowthWOUngrownAndResourses(Account acc, Town town) //перегрузка с более удобным вызовом
        {
            double ans = TownGrowthFromBuilds(town.TownBuilds.Select(x => x.Building).ToArray(), town.TownBuilds.Select(x => (int?)x.Level).ToArray()) + acc.PopulationGrowth;
            ans *= Data.RaceEffect_PopulationGrowth(acc.Race);
            ans *= FuncsBase.GreatCitizenBonus(town.GreatCitizens[(int)GreatCitizensNames.Doctor]);
            if (town.Deposit != DepositName.none) ans *= 0.85f;
            double aiinc = 1;            
            for (int i = 0; i < town.AreaImprovements.Count(); i++)
                if (town.AreaImprovements[i].AIName == AreaImprovementName.Suburb) 
                    aiinc += FuncsBase.AreaImprovementBonus(AreaImprovementName.Suburb, town.AreaImprovements[i].Level, town.AreaImprovements[i].Users);
            ans *= aiinc;
            ans *= 1 + Data.LuckBonusesData[(int)LuckBonusNames.grown].effect[town.LuckyTown[(int)LuckBonusNames.grown]];
            return ans;
        }
        public static double TownGrowthWOUngrownAndResourses(double basegrown,BuildName[] builds, int?[] lvls, Race race, bool deposit, byte numofdoctors, AreaImprovementName[] areaimps, byte[] ailvls, byte[] aiusers, byte luck_bonus_lvl)
        {            
            double ans = TownGrowthFromBuilds(builds,lvls) + basegrown;
            ans *= Data.RaceEffect_PopulationGrowth(race);
            ans *= FuncsBase.GreatCitizenBonus(numofdoctors);
            if (deposit) ans *= 0.85f; //число убрать в datasourses
            double aiinc = 1;
            for (int i = 0; i < areaimps.Length; i++)
                if (areaimps[i] == AreaImprovementName.Suburb) aiinc += FuncsBase.AreaImprovementBonus(areaimps[i], ailvls[i], aiusers[i]);
            ans *= aiinc;
            ans *= 1+Data.LuckBonusesData[(int)LuckBonusNames.grown].effect[luck_bonus_lvl];            
            return ans;
        }
        public static double TownGrowth(Account acc, Town town) //удобный вызов
        {
            double ans = TownGrowthFromBuilds(town.TownBuilds.Select(x => x.Building).ToArray(), town.TownBuilds.Select(x => (int?)x.Level).ToArray()) + acc.PopulationGrowth;
            ans *= Data.RaceEffect_PopulationGrowth(acc.Race);
            ans *= FuncsBase.GreatCitizenBonus(town.GreatCitizens[(int)GreatCitizensNames.Doctor]);
            if (town.Deposit != DepositName.none) ans *= 0.85f;
            double aiinc = 1;
            double aidec = 0;
            for (int i = 0; i < town.AreaImprovements.Count(); i++)
            {
                if (town.AreaImprovements[i].AIName == AreaImprovementName.Suburb) aiinc += FuncsBase.AreaImprovementBonus(town.AreaImprovements[i].AIName, town.AreaImprovements[i].Level, town.AreaImprovements[i].Users);
                else if (town.AreaImprovements[i].AIName == AreaImprovementName.SkiResort) aidec += FuncsBase.AreaImprovementBonus(town.AreaImprovements[i].AIName, town.AreaImprovements[i].Level, town.AreaImprovements[i].Users) * 0.027d;
            }
            ans *= aiinc;
            ans *= 1 + Data.LuckBonusesData[(int)LuckBonusNames.grown].effect[town.LuckyTown[(int)LuckBonusNames.grown]];
            double resbonus = 1;
            for (int i = (int)ResName.fruit; i <= (int)ResName.meat; i++)
                if (town.ResConsumption[i]) resbonus += Data.ResData[i].effect;
            ans *= resbonus;
            ans -= aidec;
            ans -= TownUngrownFromBuilds(town.TownBuilds.Select(x => x.Building).ToArray(), town.TownBuilds.Select(x => (int?)x.Level).ToArray());
            return ans;
        }
        public static double TownGrowth(double basegrown, BuildName[] builds, int?[] lvls, Race race, bool havedeposit, bool[] resconsumption, byte numofdoctors, AreaImprovementName[] areaimps, byte[] ailvls, byte[] aiusers, byte luck_bonus_lvl)
        {
            double ans = TownGrowthFromBuilds(builds,lvls) + basegrown;
            ans *= Data.RaceEffect_PopulationGrowth(race);
            ans *= FuncsBase.GreatCitizenBonus(numofdoctors);
            if (havedeposit) ans *= 0.85f; //число убрать в datasourses
            double aiinc = 1;
            double aidec = 0;
            for (int i = 0; i < areaimps.Length; i++)
            {
                if (areaimps[i] == AreaImprovementName.Suburb) aiinc += FuncsBase.AreaImprovementBonus(areaimps[i], ailvls[i], aiusers[i]);
                else if (areaimps[i] == AreaImprovementName.SkiResort) aidec += FuncsBase.AreaImprovementBonus(areaimps[i], ailvls[i], aiusers[i])*0.027d;
            }
            ans *= aiinc;
            ans *= 1 + Data.LuckBonusesData[(int)LuckBonusNames.grown].effect[luck_bonus_lvl];
            double resbonus = 1;
            for (int i = (int)ResName.fruit; i <= (int)ResName.meat; i++)
                if (resconsumption[i]) resbonus += Data.ResData[i].effect;
            ans *= resbonus;
            ans -= aidec;
            ans -= TownUngrownFromBuilds(builds, lvls);
            return ans;
        }
        //культура
        private static int TownCultureFromBuilds(BuildName[] builds, int?[] lvls)
        {
            int ans = 0;
            //обычные домики
            for (int i = 3; i<builds.Length;i++)             
                if (builds[i]!=BuildName.none && Data.BuildindsData[(int)builds[i]].Type == BuildType.culture)
                    ans += (int)BuildFuncs.BuildEffect(builds[i], (int)lvls[i]!);            
            //чудо
            if ((lvls[0] == 20)
                &&((builds[0] == BuildName.Earth_Mother 
                    || builds[0] == BuildName.Coliseum 
                    || builds[0] == BuildName.The_Pyramids)))
                ans += (int)Data.WounderEffects[builds[0]];                
            //центр
            if (builds[2] != BuildName.none)
            {
                if (Data.BuildindsData[(int)builds[2]].Type == BuildType.culture)
                    ans += (int)BuildFuncs.BuildEffect(builds[2], (int)lvls[2]!);
                else if (Data.BuildindsData[(int)builds[2]].Type == BuildType.administration)
                    ans += (int)BuildFuncs.AdminCulture(builds[2], (int)lvls[2]!);
            }
            return ans;
        }
        public static double TownCultureWOResourses(Account acc, Town town)
        {
            //строения и база
            double ans = acc.Culture + TownCultureFromBuilds(town.TownBuilds.Select(x => x.Building).ToArray(), town.TownBuilds.Select(x => (int?)x.Level).ToArray());
            ans *= Data.RaceEffect_Culture(acc.Race);
            //ВГ
            ans *= FuncsBase.GreatCitizenBonus(town.GreatCitizens[(int)GreatCitizensNames.Creator]);
            //УМ
            double aiinc = 0;
            var ai = town.AreaImprovements.ToArray();
            for (int i = 0; i < ai.Length; i++)
                if (ai[i].AIName == AreaImprovementName.Reservation) aiinc += FuncsBase.AreaImprovementBonus(ai[i].AIName, ai[i].Level, ai[i].Users);
            ans *= 1 + aiinc;
            //Удача
            ans *= 1 + Data.LuckBonusesData[(int)LuckBonusNames.culture].effect[town.LuckyTown[(int)LuckBonusNames.culture]];
            return ans;
        }
        public static double TownCultureWOResourses(int baseculture, BuildName[] builds, int?[] lvls, Race race, byte numofcreators, AreaImprovementName[] areaimps, byte[] ailvls, byte[] aiusers, byte luck_bonus_lvl)
        {
            double ans = baseculture + TownCultureFromBuilds(builds, lvls);
            //строения
            ans *= Data.RaceEffect_Culture(race);
            ans *= FuncsBase.GreatCitizenBonus(numofcreators);
            double aiinc = 0;
            for (int i = 0; i < areaimps.Length; i++)            
                if (areaimps[i] == AreaImprovementName.Reservation) aiinc += FuncsBase.AreaImprovementBonus(areaimps[i], ailvls[i], aiusers[i]);        
            ans *= 1 + aiinc;
            ans *= 1 + Data.LuckBonusesData[(int)LuckBonusNames.culture].effect[luck_bonus_lvl];                                                                            
            return ans;
        }
        public static int TownCulture(Account acc, Town town)
        {
            double ans = TownCultureWOResourses(acc, town);
            double resbonus = 1;
            for (int i = (int)ResName.wine; i <= (int)ResName.films; i++)
                if (town.ResConsumption[i]) resbonus += Data.ResData[i].effect;
            ans *= resbonus;
            return (int)ans;
        }
        public static int TownCulture(int baseculture, BuildName[] builds, int?[] lvls, Race race, byte numofcreators, bool[] resconsumption, AreaImprovementName[] areaimps, byte[] ailvls, byte[] aiusers, byte luck_bonus_lvl)
        {
            double ans = TownCultureWOResourses(baseculture, builds, lvls, race, numofcreators, areaimps, ailvls, aiusers, luck_bonus_lvl);
            double resbonus = 1;
            for (int i = (int)ResName.wine; i <= (int)ResName.films; i++)
                if (resconsumption[i]) resbonus += Data.ResData[i].effect;
            ans *= resbonus;
            return (int)ans;
        }
        //производство
        //Считается из предположения, что все рабочие места во всех строениях заполнены.
        //--------
        //Есть проблема с фермами и шахтами, т.к. они могут производить несколько разных ресурсов,
        //но такой вариант использования является заведомо плохим, поэтому рассматривать его нет смысла.        
        private static double Workplacesmod(BuildName[] builds, int?[] lvls) //кузня, манка, завод
        {
            double workplacesmod = 1;
            for (int i = 4; i<builds.Length; i++) 
            {
                if (builds[i] == BuildName.none) continue;                
                if (Data.BuildindsData[(int)builds[i]].Type == BuildType.prodBoost)
                {
                    workplacesmod += BuildFuncs.BuildEffect(builds[i], (int)lvls[i]!);
                    break;
                }                          
            }            
            return workplacesmod;
        }
        private static double WorkplacesmodScience(AreaImprovementName[] areaimps, byte[] ailvls, byte[] aiusers)//
        {
            double ans = 1;
            for (int i = 0; i < areaimps.Length; i++)            
                if (areaimps[i] == AreaImprovementName.Campus)
                    ans+=FuncsBase.AreaImprovementBonus(areaimps[i], ailvls[i], aiusers[i])*2;
            return ans;
        }
        private static double[] AIBonuses(AreaImprovementName[] areaimps, byte[] ailvls, byte[] aiusers, DepositName deposit)//+
        {
            double[] ans = new double[23];
            for (int i = 0; i < 23; i++)
                ans[i] = 1;
            for (int i = 0; i < areaimps.Length; i++)
            {
                if (ailvls[i] == 0) continue;
                double bonus = FuncsBase.AreaImprovementBonus(areaimps[i], ailvls[i], aiusers[i]);
                switch (areaimps[i])
                {
                    case AreaImprovementName.Irrigation:
                        for (int j = 0; j < 23; j++)
                            if (Data.ResData[j].prodtype == ResProdType.agriculture)
                                ans[j] += bonus;
                        break;
                    case AreaImprovementName.Mines:
                    case AreaImprovementName.HydrotechnicalFacility:
                        for (int j = 0; i < 23; i++)
                            if (Data.ResData[j].prodtype == ResProdType.industry)
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
            return ans;
        }
        private static double[] BaseProd(DepositName deposit, bool on_hill, byte waterplaces)//+
        {
            double[] ans = new double[23];
            for (int i= 0; i < 23;i++)
                ans[i]=Data.BaseProduction((ResName)i, on_hill, waterplaces);
            if (deposit != DepositName.none)
                foreach (var x in Data.DepositsData[(int)deposit].res)                
                    ans[(int)x[1]] += x[0];
            return ans;
        }
        private static double[] GreatSitizensProdBonus(byte[] gs)//+
        {
            double[] ans = new double[4];
            ans[(int)ResProdType.research] = FuncsBase.GreatCitizenBonus(gs[(int)GreatCitizensNames.Scientist]);
            ans[(int)ResProdType.finance] = FuncsBase.GreatCitizenBonus(gs[(int)GreatCitizensNames.Financier]);
            ans[(int)ResProdType.agriculture] = FuncsBase.GreatCitizenBonus(gs[(int)GreatCitizensNames.Agronomist]);
            ans[(int)ResProdType.industry] = FuncsBase.GreatCitizenBonus(gs[(int)GreatCitizensNames.Engineer]);
            return ans;
        }
        public static double[] Production(Account acc, Town town)
        {
            double[] ans = new double[23];
            var builds = town.TownBuilds.Select(x=>x.Building).ToArray();
            var lvls = town.TownBuilds.Select(x=>(int?)x.Level).ToArray();
            var areaimps = town.AreaImprovements.Select(x=>x.AIName).ToArray();
            var ailvls = town.AreaImprovements.Select(x=>x.Level).ToArray();
            var aiusers = town.AreaImprovements.Select(x=>x.Users).ToArray();
            double workplacesmod = Workplacesmod(builds, lvls);
            double workplacesmodSciense = WorkplacesmodScience(areaimps, ailvls, aiusers);
            int total_wp = 0;
            //считаем все рабочие места
            //и рабочие места по ресам с учётом эффективности строения
            int[] workplaces = new int[23];
            double[] workplaces_efbuildmod = new double[23];
            for (int i = 3; i < builds.Length; i++)
            {
                if (builds[i] == BuildName.none) continue;
                if (Data.BuildindsData[(int)builds[i]].Type == BuildType.production)
                {
                    double wp = BuildFuncs.BuildEffect(builds[i], (int)lvls[i]!);
                    int nwp = 0;
                    foreach (var r in Data.BuildindsData[(int)builds[i]].Productres)
                    {
                        if (r.Res == ResName.science)
                            nwp = (int)(wp * workplacesmodSciense);
                        else
                            nwp = (int)(wp * workplacesmod);
                        workplaces[(int)r.Res] += nwp;
                        workplaces_efbuildmod[(int)r.Res] += nwp * Data.BuildindsData[(int)builds[i]].Efficiency;
                    }
                    total_wp += nwp;
                }
            }
            //ээфективность труда города
            double townefficiency = FuncsBase.LaborEfficiency(total_wp);
            //базовое производство с учётом МР
            double[] baseprod = BaseProd(town.Deposit, town.OnHill, town.WaterPlaces);
            /////////////
            //модификаторы от ум
            double[] aibonuses = AIBonuses(areaimps, ailvls, aiusers, town.Deposit);//-
            //отдельно бонус от горнолыжек
            double skyresortbonus = 0;
            for (int i = 0; i < areaimps.Length; i++)
                if (areaimps[i] == AreaImprovementName.SkiResort)
                    skyresortbonus += FuncsBase.AreaImprovementBonus(AreaImprovementName.SkiResort, ailvls[i], aiusers[i]);
            //великие граждание по типу производства
            var gsb = GreatSitizensProdBonus(town.GreatCitizens.ToArray());
            //теперь всё посчитать!
            double corruption = Corruption(builds, lvls, acc.Towns.Count);
            for (int i = 0; i < 23; i++)
            {
                if (town.Product[i]) ans[i] = workplaces_efbuildmod[i];
                else { ans[i] = 0; continue; }
                int type = (int)Data.ResData[i].prodtype;
                ans[i] *= townefficiency;
                ans[i] *= 1 - corruption / 100;
                ans[i] *= Data.ClimateEffect(town.Climate, (ResProdType)type);
                ans[i] *= (double)acc.Science_Bonuses[type] / 100d;
                ans[i] *= baseprod[i];
                ans[i] *= gsb[type];
                ans[i] *= aibonuses[i];
            }
            var LevelLuckBonusProd = town.LuckyTown[(int)LuckBonusNames.production];
            var LevelLuckBonusSciense = town.LuckyTown[(int)LuckBonusNames.science];
            for (int i = 1; i < 23; i++)
                ans[i] *= 1 + Data.LuckBonusesData[(int)LuckBonusNames.production].effect[LevelLuckBonusProd]; //кроме науки
            ans[(int)ResName.science] *= 1 + Data.LuckBonusesData[(int)LuckBonusNames.science].effect[LevelLuckBonusSciense];
            //+горнолыжки
            double srb = 0;
            for (int i = 1; i < areaimps.Length; i++)
                if (areaimps[i] == AreaImprovementName.SkiResort)
                    srb += FuncsBase.AreaImprovementBonus(AreaImprovementName.SkiResort, ailvls[i], aiusers[i]);
            srb *= 1 + Data.LuckBonusesData[(int)LuckBonusNames.production].effect[LevelLuckBonusProd];
            ans[(int)ResName.money] += srb;
            //+чудеса с фиксированным бонусом
            switch (builds[0])
            {
                case BuildName.Geoglyph:
                case BuildName.The_Great_Library:
                case BuildName.Helioconcentrator:
                    ans[(int)ResName.science] += Data.WounderEffects[builds[0]];
                    break;
                case BuildName.Earthen_dam:
                    ans[(int)ResName.fish] += Data.WounderEffects[builds[0]];
                    break;
                case BuildName.The_Colossus:
                    ans[(int)ResName.money] += Data.WounderEffects[builds[0]];
                    break;
                default: break;
            }
            if (town.ResConsumption[(int)ResName.books]) ans[(int)ResName.science] *= 1 + Data.ResData[(int)ResName.books].effect;
            //вроде ничего не забыл
            return ans;
        }
        public static double[] Production(BuildName[] builds, int?[] lvls, byte[] greatsitizens, double corruption, bool[] product, DepositName deposit, Climate climate, bool on_hill, byte waterplaces, int[] Science_Bonuses, AreaImprovementName[] areaimps, byte[] ailvls, byte[] aiusers, byte LevelLuckBonusSciense = 0, byte LevelLuckBonusProd = 0, bool eatbooks = false)//+
        {
            double[] ans = new double[23];            
            double workplacesmod = Workplacesmod(builds, lvls);
            double workplacesmodSciense = WorkplacesmodScience(areaimps, ailvls, aiusers);
            int total_wp = 0;
            //считаем все рабочие места
            //и рабочие места по ресам с учётом эффективности строения
            int[] workplaces = new int[23];
            double[] workplaces_efbuildmod = new double[23];
            for (int i = 3; i<builds.Length; i++) 
            {
                if (builds[i] == BuildName.none) continue;
                if (Data.BuildindsData[(int)builds[i]].Type == BuildType.production)
                {
                    double wp = BuildFuncs.BuildEffect(builds[i], (int)lvls[i]!);
                    int nwp = 0;
                    foreach (var r in Data.BuildindsData[(int)builds[i]].Productres)
                    {
                        if (r.Res == ResName.science)
                            nwp = (int)(wp * workplacesmodSciense);
                        else
                            nwp = (int)(wp * workplacesmod);                        
                        workplaces[(int)r.Res] += nwp;
                        workplaces_efbuildmod[(int)r.Res] += nwp * Data.BuildindsData[(int)builds[i]].Efficiency;
                    }
                    total_wp += nwp;
                }                
            }
            //ээфективность труда города
            double townefficiency = FuncsBase.LaborEfficiency(total_wp);
            //базовое производство с учётом МР
            double[] baseprod = BaseProd(deposit, on_hill, waterplaces);
            /////////////
            //модификаторы от ум
            double[] aibonuses = AIBonuses(areaimps, ailvls, aiusers, deposit);//-
            //отдельно бонус от горнолыжек
            double skyresortbonus = 0;
            for (int i = 0; i < areaimps.Length; i++)
                if (areaimps[i]==AreaImprovementName.SkiResort)
                    skyresortbonus += FuncsBase.AreaImprovementBonus(AreaImprovementName.SkiResort, ailvls[i], aiusers[i]);
            //великие граждание по типу производства
            var gsb = GreatSitizensProdBonus(greatsitizens);
            //теперь всё посчитать!
            for (int i = 0; i < 23; i++)
            {
                if (product[i]) ans[i] = workplaces_efbuildmod[i];
                else {ans[i] = 0; continue; }
                int type = (int)Data.ResData[i].prodtype;
                ans[i] *= townefficiency;
                ans[i] *= 1 - corruption/100;
                ans[i] *= Data.ClimateEffect(climate, (ResProdType)type);
                //double x = (double)Science_Bonuses[type]/100d;
                ans[i] *= (double)Science_Bonuses[type]/100d;
                ans[i] *= baseprod[i];
                ans[i] *= gsb[type];
                ans[i] *= aibonuses[i];
            }
            for (int i = 1; i<23; i++)
                ans[i] *= 1 + Data.LuckBonusesData[(int)LuckBonusNames.production].effect[LevelLuckBonusProd]; //кроме науки
            ans[(int)ResName.science] *= 1 + Data.LuckBonusesData[(int)LuckBonusNames.science].effect[LevelLuckBonusSciense];

            //+горнолыжки
            double srb = 0;
            for (int i = 1; i < areaimps.Length; i++)
                if (areaimps[i] == AreaImprovementName.SkiResort)
                    srb += FuncsBase.AreaImprovementBonus(AreaImprovementName.SkiResort, ailvls[i], aiusers[i]);
            srb *= 1 + Data.LuckBonusesData[(int)LuckBonusNames.production].effect[LevelLuckBonusProd];
            ans[(int)ResName.money] += srb;
            
            //+чудеса с фиксированным бонусом
            switch (builds[0])
            {
                case BuildName.Geoglyph:
                case BuildName.The_Great_Library:
                case BuildName.Helioconcentrator:
                    ans[(int)ResName.science] += Data.WounderEffects[builds[0]];
                    break;
                case BuildName.Earthen_dam:
                    ans[(int)ResName.fish] += Data.WounderEffects[builds[0]];
                    break;
                case BuildName.The_Colossus:
                    ans[(int)ResName.money] += Data.WounderEffects[builds[0]];
                    break;
                default: break;
            }
            if (eatbooks) ans[(int)ResName.science] *= 1+Data.ResData[(int)ResName.books].effect;
            //вроде ничего не забыл
            return ans;
        }        
        public static double Corruption(BuildName[] builds, int?[] lvls, int numoftowns)//+
        {
            byte chl = 0;
            for (int i = 3; i < builds.Length; i++)
                if (builds[i] == BuildName.courthouse)
                {
                    chl = (byte)lvls[i]!;
                    break;
                }
            return FuncsBase.Corruption(numoftowns, chl);
        }
        //потребление ресурсов
        //культишка
        private static double CultCons(double cultWOres, ResName res)
        {
            if (res < ResName.wine || res > ResName.films) return 0;
            else return Data.ResData[(int)res].consumption * (int)cultWOres * Data.ResData[(int)res].effect;
        }
        //растишка
        private static double GrowthCons(double growthWOres, ResName res)
        {
            if (res < ResName.fruit || res > ResName.meat) return 0;
            else return Data.ResData[(int)res].consumption * growthWOres * Data.ResData[(int)res].effect;
        }
        public static double[] GetResConsumption(bool[] eat, double growthWOres, double cultWOres, double scienseWObooks = 0)
        {
            double[] ans = new double[23];
            for (int i = 0; i < 23; i++)
            {
                if (!eat[i]) continue;
                int t = Data.ResData[i].type;
                if (t == 1) ans[i] = GrowthCons(growthWOres, (ResName)i); //food
                if (t == 2) ans[i] = CultCons(cultWOres, (ResName)i); //cult
                if (t == 3) ans[i] = Data.ResData[i].effect * scienseWObooks * Data.ResData[i].consumption;//books
            }
            return ans;
        }
        //потребление денег
        public static double BuildsUpkeep(BuildName[] builds, int?[] lvls, Race race)
        {
            double ans = 0;
            for (int i = 0; i < 19; i++)
            {
                if (builds[i] != BuildName.none && Data.BuildindsData[(int)builds[i]].Pay is not null)
                {
                    ans += BuildFuncs.Pay(builds[i], (int)lvls[i]!);
                }
            }
            ans *= Data.RaceEffect_Upkeep(race);
            double w_economy = 1;
            if (builds[2] != BuildName.none && Data.BuildindsData[(int)builds[2]].Type == BuildType.administration)
                w_economy -= BuildFuncs.AdminEconimy(builds[2], (int)lvls[2]!);
            ans *= w_economy;
            return ans;
        }
    }
}
