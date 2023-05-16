using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WofHCalc.Models;
using WofHCalc.MathFuncs;

namespace WofHCalc.ExtendedModel
{
    internal class ExtendedTown: Town
    {
        private Account acc;
        double Growth 
        {
            get => TownFuncs.TownGrowth(acc, this);
        }
        double GrowthPriceTotal;
        double GrowthPricePerOne
        {
            get => GrowthPriceTotal / Growth;
        }


        double Culture
        {
            get => TownFuncs.TownCulture(acc, this);
        }
        double CulturePriceTotal;
        double CulturePricePerHundred
        {
            get => CulturePriceTotal / Culture * 100d;
        }

        double[] Products
        {
            get => TownFuncs.Production(acc, this);
        }
        double TotalProd;

        int Traiders;
        double TraiderPrice;
        double ResPerTraider;

        double TotalUpkeep
        {
            get => 0;
        }
        double[] ResConsumption 
        {
            get => TownFuncs.GetResConsumption(acc, this);
        }
    }
}
