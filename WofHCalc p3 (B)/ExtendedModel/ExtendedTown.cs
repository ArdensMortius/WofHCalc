using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WofHCalc.Models;
using WofHCalc.MathFuncs;
using System.Threading;
using WofHCalc.DataSourses;
using System.Xml.Linq;
using WofHCalc.Models.jsonTemplates;

namespace WofHCalc.ExtendedModel
{
    public class ExtendedTown: Town, ICloneable
    {
        private readonly ExtendedAccount acc;

        public ExtendedTown(ExtendedAccount acc, Town town)
        {
            this.acc = acc;
            this.ScienceEfficiency = town.ScienceEfficiency;
            this.Name = town.Name;
            this.Climate = town.Climate;
            this.Deposit = town.Deposit;
            this.WaterPlaces = town.WaterPlaces;
            this.OnHill = town.OnHill;
            this.TownBuilds = town.TownBuilds;
            this.GreatCitizens = town.GreatCitizens;
            this.LuckyTown = town.LuckyTown;
            this.ResConsumption = town.ResConsumption;
            this.Product = town.Product;
            this.AreaImprovements = town.AreaImprovements;
        }
        public double Growth //прирост
        {
            get => acc.WofHFuncs.TownGrowth(acc, this);
        }
        public double GrowthPriceTotal
        {
            get => acc.WofHFuncs.GrowthFullUpkeep(this, acc);
        }
        public double GrowthPricePerOne //цена на +1 челика в день
        {
            get => GrowthPriceTotal / Growth * 24;
        }
        public int Culture //культура
        {
            get => acc.WofHFuncs.TownCulture(acc, this);
        }
        public double CulturePriceTotal
        {
            get => acc.WofHFuncs.CultureFullUpkeep(this, acc);
        }
        public double CulturePricePerHundred //цена за сотку культуры
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
        public int Workplaces //количество рабочих мест
        {
            get => acc.WofHFuncs.TownWorkplaces(this);
        }
        public double GrowthAsMoney
        {
            get => Growth * acc.WofHFuncs.BaseOneHumanPrice(acc.Financial) / 24d;
        }
        public override object Clone()
        {
            Town t = (Town)base.Clone();
            return new ExtendedTown(acc, t);
        }
        
    }
}
