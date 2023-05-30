using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WofHCalc.DataSourses;
using WofHCalc.Supports;

namespace WofHCalc.MathFuncs
{
    internal class BuildFuncs
    {
        private static int[] BuildUpResCost(BuildName name, int under_construction_lvl)
        {
            int[] ans = new int[23];
            double A;
            double B;
            double C;
            double D = (double)Data.BuildindsData[(int)name].Cost[3].Double!;
            for (int i = 0; i < 23; i++)
            {
                A = (double)Data.BuildindsData[(int)name].Cost[0].DoubleArray[i];
                B = (double)Data.BuildindsData[(int)name].Cost[1].DoubleArray[i];
                C = (double)Data.BuildindsData[(int)name].Cost[2].DoubleArray[i];
                int res = (int)FuncsBase.MainFunc(A, B, C, D, under_construction_lvl);
                if (res < 0) res = 0;
                ans[i] = res;
            }
            return ans;
        }
        public static int[] BuildTotalResCost(BuildName name, int current_lvl)
        {
            int[] ans = new int[23];
            if (current_lvl == 0 || name == BuildName.none) return ans;
            for (int i = 1; i <= current_lvl; i++)
            {
                int[] lvlcost = BuildUpResCost(name, i);
                for (int j = 0; j < 23; j++)
                    ans[j] += lvlcost[j];
            }
            return ans;
        }
        public static int[] BuildUpFromToResCost(BuildName name, int current_lvl, int target_lvl)
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
        public static int[] RebuildResCost(BuildName current_build, int cur_lvl, BuildName new_build)
        {
            if (Data.BuildindsData[(int)current_build].Next[0] == new_build)
            {
                int[] ans = new int[23];
                int[] ob = BuildTotalResCost(current_build, cur_lvl);
                int[] nb = BuildUpResCost(new_build, 1);
                for (int i = 0; i < 23; i++)
                {
                    ans[i] = nb[i] - (int)(Data.RebuildReturn * ob[i]);
                    if (ans[i] < 0) ans[i] = 0;
                }
                return ans;
            }
            else return BuildUpResCost(new_build, 1);
        }
        public static double BuildEffect(BuildName name, int lvl)
        {
            if (name == BuildName.none) return 0;
            return FuncsBase.MainFunc(Data.BuildindsData[(int)name].Effect, lvl);        
        }        
        private static bool AvailableRaceCheck(BuildName build, Race acc) =>
            ((Data.BuildindsData[(int)build].Race & acc) == acc);
        private static bool AvailableCountGroupCheck(BuildName newbuild, BuildName[] townbuilds)
        {            
            int c = 0;
            int group = Data.BuildindsData[(int)newbuild].Group;
            for (int i = 0; i < townbuilds.Length; i++)
            {
                if (townbuilds[i] == BuildName.none) continue;
                if (townbuilds[i] == newbuild) c++; //считаем количество таких же
                else if (group == 0) continue; //эта группа для построек, которые ни с чем не конкурируют
                else if (Data.BuildindsData[(int)townbuilds[i]].Group == group) return false; //проверяем конкуренцию
            }
            if (c >= Data.BuildindsData[(int)newbuild].Maxcount) return false;                        
            return true;
        }
        private static bool AvailableTerrainCheck(BuildName newbuild, bool on_hill, byte water_places)
        {
            Terrain t = Data.BuildindsData[(int)newbuild].Terrain;
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
        private static bool AvailableSlotCheck(BuildName newbuild, Slot slot) =>
            Data.BuildindsData[(int)newbuild].Slot == slot;        
        public static bool AvailableCheck(BuildName newbuild, Race acc_race, bool town_on_hill, byte town_water_places, BuildName[] town_builds, Slot slot)
        {
            if (newbuild == BuildName.none) return true;
            return (
                AvailableSlotCheck(newbuild, slot)
                && AvailableRaceCheck(newbuild, acc_race)
                && AvailableCountGroupCheck(newbuild, town_builds)
                && AvailableTerrainCheck(newbuild, town_on_hill, town_water_places)
                );
        }
        public static double Pay(BuildName name, int lvl)
        {
            if (name == BuildName.none) return 0;
            double ans = FuncsBase.MainFunc(Data.BuildindsData[(int)name].Pay, lvl);
            if (ans < 0) return 0;
            else return ans;
        }                                
        public static double Ungrown(BuildName name, int lvl)
        {
            if (BuildName.none == name) return 0;
            return FuncsBase.MainFunc(Data.BuildindsData[(int)name].Ungrown, lvl);
        }
        public static double AdminCulture(BuildName name, int lvl) => //культура от админки
            Data.AdministrationCulture[0] + Data.AdministrationCulture[1]*BuildEffect(name,lvl);
        public static double AdminEconimy(BuildName name, int lvl) => //экономия в долях
            1 - 1 / (1 + BuildEffect(name, lvl));
        public static double BuildStrength(BuildName name, int lvl)
        {
            int[] resourses = BuildTotalResCost(name, lvl);
            int sum = 0;
            foreach (int r in resourses) sum += r;
            return sum;
        }
        public static double BuildTimeFromTo(BuildName name, int current_lvl, int target_lvl)
        {

            return 0;
        }
    }
}
