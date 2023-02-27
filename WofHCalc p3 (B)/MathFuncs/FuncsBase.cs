using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using WofHCalc.DataSourses;
using WofHCalc.Models;
using WofHCalc.Supports;

namespace WofHCalc.MathFuncs
{
    public class FuncsBase
    {
        internal static double MainFunc(double A, double B, double C, double D, int n)
        {
            double k = (n == 0) ? A : 0;
            return (k + B + C * (Math.Pow(n, D)));
        }
        internal static double MainFunc(double[]? abcd, int n)
        {            
            if (abcd is null) return 0;
            else return MainFunc(abcd[0], abcd[1], abcd[2], abcd[3], n);
        }
        public static double LaborEfficiency(int numworkers)
        {
            return Math.Pow(numworkers, -0.12d);
        }
        public static double Corruption(int numtowns, byte courthouse_level)
        {
            if (numtowns < 1 || courthouse_level < 0 || courthouse_level > 20) throw new Exception("Corruption error");            
            return ((numtowns - 1) * 20/MainFunc(DataSourses.Data.BuildindsData[(int)BuildName.courthouse].Effect, courthouse_level));
        }
        public static double GreatCitizenBonus(int n) =>//конкретные числа должны быть в datasourses где-нибудь, а не тут
            MainFunc(0, 1, 0.04d, 0.75d, n);                
        
        public static double LuckBonusPrice(LuckBonusNames name, int numtowns, int lvl)
        {
            double baseprice = (double)Data.LuckBonusesData[(int)name].cost[lvl];
            double costk = Data.LuckBonusesData[(int)name].costk;
            return Math.Ceiling(baseprice / numtowns * (1 + (costk * (numtowns - 1))));            
        }
        private static double AreaImprovementEfficiencyPerUser(int users)
        {
            double u = (double)users;
            return (u + 1) / (2 * u);
        }
        public static double AreaImprovementBonus(AreaImprovementName name, int lvl, int users = 1)
        {
            if (lvl == 0) return 0;
            return 0 + (Data.AreaImprovementsData[(int)name].levels[lvl - 1].effect)*AreaImprovementEfficiencyPerUser(users); 
        }

    }
}
