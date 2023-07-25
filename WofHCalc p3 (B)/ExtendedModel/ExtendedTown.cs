﻿using System;
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
        private readonly ExtendedAccount acc;

        public ExtendedTown(ExtendedAccount acc, Town town) : base() //вообще не уверен, что это будет работать
        {
            this.acc = acc;            
        }
        public double Growth //прирост
        {
            get => acc.WofHFuncs.TownGrowth(acc, this);
        }
        //double GrowthPriceTotal
        //{
        //    get => 0
        //}
        //double GrowthPricePerOne //цена на +1 челика в день
        //{
        //    get => GrowthPriceTotal / Growth;
        //}

        int Culture //культура
        {
            get => acc.WofHFuncs.TownCulture(acc, this);
        }
        //double CulturePriceTotal; 
        //double CulturePricePerHundred //цена за сотку культуры
        //{
        //    get => CulturePriceTotal / Culture * 100d;
        //}

        double[] Products //полный объем производства
        {
            get => acc.WofHFuncs.TownProduction(acc, this);
        }
        double ProductsValuation //деньги, которые можно получить за произведённое 
        {
            get => acc.WofHFuncs.TownProductionValue(acc, this);
        }
        //double TransportVolume //объем ресов, которые потенциально надо вывозить
        int Traiders
        {
            get => acc.WofHFuncs.Traiders(this, acc);
        }
        double TraiderPrice
        {
            get => acc.WofHFuncs.TraiderPrice(this, acc);
        }
        double ResPerTraider //текущее соотношение промки и торгов города. Помогает спрогнозировать, на сколько надо апать рынок при апе промки
        {
            get => acc.WofHFuncs.ResPerTraider(acc, this);
        } 
        double TotalUpkeep //Собственные расходы города
        {
            get => acc.WofHFuncs.TownUpkeep(this, acc);
        }
        double[] ResoursesConsumption
        {
            get => acc.WofHFuncs.GetResConsumption(acc, this);
        }
    }
}
