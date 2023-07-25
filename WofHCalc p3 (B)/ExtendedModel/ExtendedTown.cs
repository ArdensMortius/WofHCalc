using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WofHCalc.Models;
using WofHCalc.MathFuncs;
using System.Threading;
using WofHCalc.DataSourses;

namespace WofHCalc.ExtendedModel
{
    internal class ExtendedTown: Town
    {
        private ExtendedAccount acc;
        //double Growth //прирост
        //{
        //    //get => 
        //}
        double GrowthPriceTotal;
        //double GrowthPricePerOne //цена на +1 челика в день
        //{
        //    get => GrowthPriceTotal / Growth;
        //}

        //int Culture //культура
        //{
        //    //get => TownCulture(acc, (Town)this);
        //}
        double CulturePriceTotal; 
        //double CulturePricePerHundred //цена за сотку культуры
        //{
        //    get => CulturePriceTotal / Culture * 100d;
        //}

        //double[] Products //полный объем производства
        //{
        //    //get => TownFuncs.Production(acc, this);
        //}
        //double[] ProductsValuation //деньги, которые можно получить за произведённое //колба не правильно учтена
        //{
        //    get
        //    {
        //        double[] ans = new double[23];
        //        double[] prod = Products;
        //        for (int i = 0; i < 23; i++)
        //            ans[i] = prod[i] * (acc.Financial.Prices[i]);
        //        return ans;
        //    }
        //}
        //double TotalProdPrice //суммарный производ города в деньгах
        //{
        //    get
        //    {
        //        double ans = 0;
        //        double[] pv = ProductsValuation;
        //        for (int i=0;i<23;i++)
        //            ans += pv[i];
        //        return ans;
        //    }
        //}

        //double TransportVolume //объем ресов, которые потенциально надо вывозить
        int Traiders;
        double TraiderPrice;
        double ResPerTraider;
        double TotalUpkeep
        {
            get => 0;
        }
        //double[] ResoursesConsumption 
        //{
        //    get => 
        //}
    }
}
