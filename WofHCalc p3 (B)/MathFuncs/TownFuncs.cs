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

namespace WofHCalc.MathFuncs
{
    public static class TownFuncs
    {
        //прирост
        private static double TownGrowsFromBuilds(BuildName[] builds, int?[] lvls)
        {
            double ans = 0;
            for (int i = 3; i < builds.Length;i++)
            {
                if (builds[i] != BuildName.none && Data.BuildindsData[(int)builds[i]].Type == BuildType.grown)                
                    ans += BuildFuncs.BuildEffect(builds[i], (int)lvls[i]!);                                    
            }
            if (builds[0] == BuildName.Pagan_temple && lvls[0] == 20)
            {
                ans += Data.WounderEffects[BuildName.Pagan_temple];
            }
            return ans;
        }
        private static double TownUngrownFromBuilds(BuildName[] builds, int?[] lvls)
        {
            double ans = 0;
            for (int i = 2; i<builds.Length;i++)            
                if (builds[i] != BuildName.none)
                    ans +=FuncsBase.MainFunc(Data.BuildindsData[(int)builds[i]].Ungrown, (int)lvls[i]!);            
            return ans;
        }
        public static double TownGrowsWOUngrownAndResourses(double basegrown,BuildName[] builds, int?[] lvls, Race race, bool deposit, byte numofdoctors, IList<AreaImprovementName> areaimps, int[] ailvls, byte[] aiusers, byte luck_bonus_lvl)
        {            
            double ans = TownGrowsFromBuilds(builds,lvls) + basegrown;
            ans *= Data.RaceEffect_PopulationGrowth(race);
            ans *= FuncsBase.GreatCitizenBonus(numofdoctors);
            if (deposit) ans *= 0.85f; //число убрать в datasourses
            double aiinc = 1;
            for (int i = 0; i < areaimps.Count; i++)
                if (areaimps[i] == AreaImprovementName.Suburb) aiinc += FuncsBase.AreaImprovementBonus(areaimps[i], ailvls[i], aiusers[i]);
            ans *= aiinc;
            ans *= Data.LuckBonusesData[(int)LuckBonusNames.grown].effect[luck_bonus_lvl];            
            return ans;
        }
        public static double TownGrows(double basegrown, BuildName[] builds, int?[] lvls, Race race, bool havedeposit, bool[] resconsumption, byte numofdoctors, IList<AreaImprovementName> areaimps, int[] ailvls, byte[] aiusers, byte luck_bonus_lvl)
        {
            double ans = TownGrowsFromBuilds(builds,lvls) + basegrown;
            ans *= Data.RaceEffect_PopulationGrowth(race);
            ans *= FuncsBase.GreatCitizenBonus(numofdoctors);
            if (havedeposit) ans *= 0.85f; //число убрать в datasourses
            double aiinc = 1;
            double aidec = 0;
            for (int i = 0; i < areaimps.Count; i++)
            {
                if (areaimps[i] == AreaImprovementName.Suburb) aiinc += FuncsBase.AreaImprovementBonus(areaimps[i], ailvls[i], aiusers[i]);
                else if (areaimps[i] == AreaImprovementName.SkiResort) aidec += FuncsBase.AreaImprovementBonus(areaimps[i], ailvls[i], aiusers[i])*0.027d;
            }
            ans *= aiinc;
            ans *= Data.LuckBonusesData[(int)LuckBonusNames.grown].effect[luck_bonus_lvl];
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
        public static double TownCultureWOResourses(int baseculture, BuildName[] builds, int?[] lvls, Race race, byte numofcreators, IList<AreaImprovementName> areaimps, int[] ailvls, byte[] aiusers, byte luck_bonus_lvl)
        {
            double ans = baseculture + TownCultureFromBuilds(builds, lvls);
            //строения
            ans *= Data.RaceEffect_Culture(race);
            ans *= FuncsBase.GreatCitizenBonus(numofcreators);
            double aiinc = 1;
            for (int i = 0; i < areaimps.Count; i++)            
                if (areaimps[i] == AreaImprovementName.Reservation) aiinc += FuncsBase.AreaImprovementBonus(areaimps[i], ailvls[i], aiusers[i]);        
            ans *= aiinc;
            ans *= Data.LuckBonusesData[(int)LuckBonusNames.culture].effect[luck_bonus_lvl];                                                                            
            return ans;
        }
        public static int TownCulture(int baseculture, BuildName[] builds, int?[] lvls, Race race, byte numofcreators, bool[] resconsumption, IList<AreaImprovementName> areaimps, int[] ailvls, byte[] aiusers, byte luck_bonus_lvl)
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
        private static double WorkplacesmodScience(IList<AreaImprovementName> areaimps, int[] ailvls, byte[] aiusers)
        {
            double ans = 1;
            for (int i = 0; i < areaimps.Count; i++)            
                if (areaimps[i] == AreaImprovementName.Campus)
                    ans+=FuncsBase.AreaImprovementBonus(areaimps[i], ailvls[i], aiusers[i])*2;
            return ans;
        }
        private static double[] AIBonuses(IList<AreaImprovementName> areaimps, int[] ailvls, byte[] aiusers, DepositName deposit)
        {
            double[] ans = new double[23];
            for (int i = 0; i < 23; i++)
                ans[i] = 1;
            for (int i = 0; i < areaimps.Count; i++)
            {
                if (ailvls[i] == 0) continue;
                double bonus = FuncsBase.AreaImprovementBonus(areaimps[i], ailvls[i], aiusers[i]);
                switch (areaimps[i])
                {
                    case AreaImprovementName.Irrigation:
                        for (int j = 0; i < 23; i++)
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
        private static double[] BaseProd(DepositName deposit, bool on_hill, byte waterplaces)
        {
            double[] ans = new double[23];
            for (int i= 0; i < 23;i++)
                ans[i]=Data.BaseProduction((ResName)i, on_hill, waterplaces);
            if (deposit != DepositName.none)
                foreach (var x in Data.DepositsData[(int)deposit].res)                
                    ans[(int)x[1]] += x[0];
            return ans;
        }
        private static double[] GreatSitizensProdBonus(byte[] gs)
        {
            double[] ans = new double[4];
            ans[(int)ResProdType.research] = FuncsBase.GreatCitizenBonus(gs[(int)GreatCitizensNames.Scientist]);
            ans[(int)ResProdType.finance] = FuncsBase.GreatCitizenBonus(gs[(int)GreatCitizensNames.Financier]);
            ans[(int)ResProdType.agriculture] = FuncsBase.GreatCitizenBonus(gs[(int)GreatCitizensNames.Agronomist]);
            ans[(int)ResProdType.industry] = FuncsBase.GreatCitizenBonus(gs[(int)GreatCitizensNames.Engineer]);
            return ans;
        }
        public static double[] Production(BuildName[] builds, int?[] lvls, byte[] greatsitizens, double corruption, bool[] product, DepositName deposit, Climate climate, bool on_hill, byte waterplaces, int[] Science_Bonuses, IList<AreaImprovementName> areaimps, int[] ailvls, byte[] aiusers, byte LevelLuckBonusSciense = 0, byte LevelLuckBonusProd = 0, bool eatbooks = false)
        {
            double[] ans = new double[23];            
            double workplacesmod = Workplacesmod(builds, lvls);
            double workplacesmodSciense = WorkplacesmodScience(areaimps, ailvls, aiusers);
            int total_wp = 0;
            //считаем все рабочие места
            //и рабочие места по ресам с учётом эффективности строения
            int[] workplaces = new int[23];
            double[] workplaces_efbuildmod = new double[23];
            for (int i = 4; i<builds.Length; i++) 
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
            double[] aibonuses = AIBonuses(areaimps, ailvls, aiusers, deposit);
            //отдельно бонус от горнолыжек
            double skyresortbonus = 0;
            for (int i = 0; i < areaimps.Count; i++)
                if (areaimps[i]==AreaImprovementName.SkiResort)
                    skyresortbonus += FuncsBase.AreaImprovementBonus(AreaImprovementName.SkiResort, ailvls[i], aiusers[i]);
            //великие граждание по типу производства
            var gsb = GreatSitizensProdBonus(greatsitizens);
            //теперь всё посчитать!
            for (int i = 0; i < 23; i++)
            {
                if (product[i]) ans[i] = workplaces_efbuildmod[i];
                else {ans[i] = 0; continue; }
                var type = (int)Data.ResData[i].prodtype;
                ans[i] *= townefficiency;
                ans[i] *= 1 - corruption;
                ans[i] *= Data.ClimateEffect(climate, (ResProdType)type);
                ans[i] *= Science_Bonuses[type];
                ans[i] *= baseprod[i];
                ans[i] *= gsb[type];
                ans[i] *= aibonuses[i];
            }
            for (int i = 1; i<23; i++)
                ans[i] *= 1 + Data.LuckBonusesData[(int)LuckBonusNames.production].effect[LevelLuckBonusProd]; //кроме науки
            ans[(int)ResName.science] *= 1 + Data.LuckBonusesData[(int)LuckBonusNames.science].effect[LevelLuckBonusSciense];

            //+горнолыжки
            double srb = 0;
            for (int i = 1; i < areaimps.Count; i++)
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
            if (eatbooks) ans[(int)ResName.science] *= Data.ResData[(int)ResName.books].effect;
            //вроде ничего не забыл
            return ans;
        }
        //потребление ресурсов и денег        
    }
}
