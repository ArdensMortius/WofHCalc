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
    internal class ExtendedTown: Town, ICloneable
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
        double GrowthPriceTotal
        {
            get => acc.WofHFuncs.GrowthFullUpkeep(this, acc);
        }
        double GrowthPricePerOne //цена на +1 челика в день
        {
            get => GrowthPriceTotal / Growth;
        }
        public int Culture //культура
        {
            get => acc.WofHFuncs.TownCulture(acc, this);
        }
        double CulturePriceTotal
        {
            get => acc.WofHFuncs.CultureFullUpkeep(this, acc);
        }
        double CulturePricePerHundred //цена за сотку культуры
        {
            get => CulturePriceTotal / Culture * 100d;
        }
        public double[] Products //полный объем производства
        {
            get => acc.WofHFuncs.TownProduction(acc, this);
        }
        public double ProductsValuation //деньги, которые можно получить за произведённое 
        {
            get => acc.WofHFuncs.TownProductionValue(acc, this);
        }        
        public int Traiders
        {
            get => acc.WofHFuncs.Traiders(this, acc);
        }
        public double TraiderPrice
        {
            get => acc.WofHFuncs.TraiderPrice(this, acc);
        }
        public double ResPerTraider //текущее соотношение промки и торгов города. Помогает спрогнозировать, на сколько надо апать рынок при апе промки
        {
            get => acc.WofHFuncs.ResPerTraider(acc, this);
        } 
        public double TotalUpkeep //Собственные расходы города
        {
            get => acc.WofHFuncs.TownUpkeep(this, acc);
        }
        public double[] ResoursesConsumption //потребление ресов
        {
            get => acc.WofHFuncs.GetResConsumption(acc, this);
        }
        public long TownStrength//прочность города. Ну учтён баф европейцев да и фиг с ним
        {
            get => acc.WofHFuncs.TownStrength(this, acc);
        }
        public long TownPrice //цена постройки города (только затраты ресов)
        {
            get =>acc.WofHFuncs.TownPrice(this, acc);
        }
        public double TownProfitForOwner //доход от города владельцу города
        {
            get =>acc.WofHFuncs.TownProfitForOwner(this, acc);
        }
        public double TownProfitForCountry //на сколько богаче становится страна от города (не казна!)
        {
            get => acc.WofHFuncs.TownProfitForCountry(this, acc);
        }
        public override object Clone()        
            => new ExtendedTown(acc, (Town)(this as Town).Clone());
        
    }
}
