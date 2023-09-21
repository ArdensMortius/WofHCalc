using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WofHCalc.DataSourses;
using WofHCalc.MathFuncs;
using WofHCalc.Models;

namespace WofHCalc.ExtendedModel
{
    public class ExtendedAccount : Account, INotifyPropertyChanged
    {
        [JsonIgnore]
        public ObservableCollection<ExtendedTown> ExtendedTowns { get; set; }
        [JsonIgnore]
        public ObservableCollection<ExtendedTown> variantsET1 { get; set; }
        [JsonIgnore]
        public ObservableCollection<ExtendedTown> variantsET2 { get; set; }
        //public ObservableCollection<ExtendedTown>? TargetETowns { get; set; }
        [JsonIgnore]
        public DataWorldConst data;
        [JsonIgnore]
        public WofHMath WofHFuncs;
        public ExtendedAccount(Account acc) : base(acc)
        {          
            data = DataWorldConst.GetInstance(acc.World);
            if (acc.UnitsVisibilyty is null) //для обратной совместимости со старыми сохранениями
            {
                UnitsVisibilyty = new ObservableCollection<bool>();
                for (int i = 0; i < data.Units.Length; i++) UnitsVisibilyty.Add(true);
            }
            else this.UnitsVisibilyty = acc.UnitsVisibilyty;
            ExtendedTowns = new ObservableCollection<ExtendedTown>();
            variantsET1 = new ObservableCollection<ExtendedTown>();
            variantsET2 = new ObservableCollection<ExtendedTown>();
            foreach (Town town in acc.Towns) 
            {
                var et = new ExtendedTown (this, town);
                ExtendedTowns.Add(et);
                variantsET1.Add((ExtendedTown)et.Clone());
                variantsET2.Add((ExtendedTown)et.Clone());
            }
            WofHFuncs = new WofHMath(data);
        }
        //все изменения для сравнения с тем, что было
        private int nt;
        [JsonIgnore]
        public int ntown 
        { 
            set { nt = value; OnPropertyChanged("ntown"); }
            get => nt;
        }
        private int nv;
        [JsonIgnore]
        public int nvariant 
        {
            set { nv = value; OnPropertyChanged("nvariant"); } 
            get => nv;
        }
        public double RebuildCost
        {
            get 
            {
                switch (nvariant)
                {
                    case 1: return WofHFuncs.TownRebuildCost((Town)ExtendedTowns[ntown], (Town)variantsET1[ntown], (Account)this);
                    case 2: return WofHFuncs.TownRebuildCost((Town)ExtendedTowns[ntown], (Town)variantsET2[ntown], (Account)this);
                    default: return 0;
                }            
            }
        }
        public double DGrowth
        {
            get
            {
                switch (nvariant)
                {
                    case 1: return (variantsET1[ntown].Growth - ExtendedTowns[ntown].Growth);
                    case 2: return (variantsET2[ntown].Growth - ExtendedTowns[ntown].Growth);
                    default: return 0;
                }
            }
        }
        public double DGrowthPriceTotal
        {
            get
            { 
                switch (nvariant)
                {
                    case 1: return (variantsET1[ntown].GrowthPriceTotal - ExtendedTowns[ntown].GrowthPriceTotal);
                    case 2: return (variantsET2[ntown].GrowthPriceTotal - ExtendedTowns[ntown].GrowthPriceTotal);
                    default: return 0;
                }
            }
        }
        public double DGrowthPricePerOne
        {
            get
            {
                switch (nvariant)
                {
                    case 1: return (variantsET1[ntown].GrowthPricePerOne - ExtendedTowns[ntown].GrowthPricePerOne);
                    case 2: return (variantsET2[ntown].GrowthPricePerOne - ExtendedTowns[ntown].GrowthPricePerOne);
                    default: return 0;
                }
            }
        }
        public double DGrowthAsMoney
        {
            get
            {
                switch (nvariant)
                {
                    case 1: return (variantsET1[ntown].GrowthAsMoney - ExtendedTowns[ntown].GrowthAsMoney);
                    case 2: return (variantsET2[ntown].GrowthAsMoney - ExtendedTowns[ntown].GrowthAsMoney);
                    default: return 0;
                }
            }
        }
        public int DCulture
        {
            get 
            {
                switch (nvariant)
                {
                    case 1: return (variantsET1[ntown].Culture - ExtendedTowns[ntown].Culture);
                    case 2: return (variantsET2[ntown].Culture - ExtendedTowns[ntown].Culture);
                    default: return 0;
                }
            }
        }
        public double DCulturePriceTotal
        {
            get
            {
                switch (nvariant)
                {
                    case 1: return (variantsET1[ntown].CulturePriceTotal - ExtendedTowns[ntown].CulturePriceTotal);
                    case 2: return (variantsET2[ntown].CulturePriceTotal - ExtendedTowns[ntown].CulturePriceTotal);
                    default: return 0;
                }
            }
        }
        public double DCulturePricePerHundred
        {
            get
            {
                switch (nvariant)
                {
                    case 1: return (variantsET1[ntown].CulturePricePerHundred - ExtendedTowns[ntown].CulturePricePerHundred);
                    case 2: return (variantsET2[ntown].CulturePricePerHundred - ExtendedTowns[ntown].CulturePricePerHundred);
                    default: return 0;
                }
            }
        }
        public double[] DProducts
        {
            get
            {
                switch (nvariant)
                {
                    case 1: return (ArrMinusArr(variantsET1[ntown].Products, ExtendedTowns[ntown].Products));
                    case 2: return (ArrMinusArr(variantsET2[ntown].Products, ExtendedTowns[ntown].Products));
                    default: return new double[23];
                }
            }
        }
        public double DProductsValuation
        {
            get
            {
                switch (nvariant)
                {
                    case 1: return (variantsET1[ntown].ProductsValuation - ExtendedTowns[ntown].ProductsValuation);
                    case 2: return (variantsET2[ntown].ProductsValuation - ExtendedTowns[ntown].ProductsValuation);
                    default: return 0;
                }
            }
        }
        public int DTraiders
        {
            get
            {
                switch (nvariant)
                {
                    case 1: return (variantsET1[ntown].Traiders - ExtendedTowns[ntown].Traiders);
                    case 2: return (variantsET2[ntown].Traiders - ExtendedTowns[ntown].Traiders);
                    default: return 0;
                }
            }
        }
        public double DTraiderPrice
        {
            get
            {
                switch (nvariant)
                {
                    case 1: return (variantsET1[ntown].TraiderPrice - ExtendedTowns[ntown].TraiderPrice);
                    case 2: return (variantsET2[ntown].TraiderPrice - ExtendedTowns[ntown].TraiderPrice);
                    default: return 0;
                }
            }
        }
        public double DResPerTraider
        {
            get
            {
                switch (nvariant)
                {
                    case 1: return (variantsET1[ntown].ResPerTraider - ExtendedTowns[ntown].ResPerTraider);
                    case 2: return (variantsET2[ntown].ResPerTraider - ExtendedTowns[ntown].ResPerTraider);
                    default: return 0;
                }
            }
        }
        public double DTotalUpkeep
        {
            get
            {
                switch (nvariant)
                {
                    case 1: return (variantsET1[ntown].TotalUpkeep - ExtendedTowns[ntown].TotalUpkeep);
                    case 2: return (variantsET2[ntown].TotalUpkeep - ExtendedTowns[ntown].TotalUpkeep);
                    default: return 0;
                }
            }
        }
        public double[] DResoursesConsumption
        {
            get
            {
                switch (nvariant)
                {
                    case 1: return (ArrMinusArr(variantsET1[ntown].ResoursesConsumption, ExtendedTowns[ntown].ResoursesConsumption));
                    case 2: return (ArrMinusArr(variantsET2[ntown].ResoursesConsumption, ExtendedTowns[ntown].ResoursesConsumption));
                    default: return new double[23];
                }
            }
        }
        public long DTownStrength
        {
            get
            {
                switch (nvariant)
                {
                    case 1: return (variantsET1[ntown].TownStrength - ExtendedTowns[ntown].TownStrength);
                    case 2: return (variantsET2[ntown].TownStrength - ExtendedTowns[ntown].TownStrength);
                    default: return 0;
                }
            }
        }
        public long DTownPrice
        {
            get
            {
                switch (nvariant)
                {
                    case 1: return (variantsET1[ntown].TownPrice - ExtendedTowns[ntown].TownPrice);
                    case 2: return (variantsET2[ntown].TownPrice - ExtendedTowns[ntown].TownPrice);
                    default: return 0;
                }
            }
        }
        public double DTownProfitForOwner
        {
            get
            {
                switch (nvariant)
                {
                    case 1: return (variantsET1[ntown].TownProfitForOwner - ExtendedTowns[ntown].TownProfitForOwner);
                    case 2: return (variantsET2[ntown].TownProfitForOwner - ExtendedTowns[ntown].TownProfitForOwner);
                    default: return 0;
                }
            }
        }
        public double DTownProfitForCountry
        {
            get
            {
                switch (nvariant)
                {
                    case 1: return (variantsET1[ntown].TownProfitForCountry - ExtendedTowns[ntown].TownProfitForCountry);
                    case 2: return (variantsET2[ntown].TownProfitForCountry - ExtendedTowns[ntown].TownProfitForCountry);
                    default: return 0;
                }
            }
        }
        public double RebuildTime
        {
            get
            {
                switch (nvariant)
                {
                    case 1: return WofHFuncs.TownRebuildTime(ExtendedTowns[ntown], variantsET1[ntown], this);
                    case 2: return WofHFuncs.TownRebuildTime(ExtendedTowns[ntown], variantsET2[ntown], this);
                    default: return 0;
                }
            }
        }
        public int[] RebuildResCost
        {
            get
            {
                switch (nvariant)
                {
                    case 1: return WofHFuncs.TownRebuildResCost(ExtendedTowns[ntown], variantsET1[ntown], this);
                    case 2: return WofHFuncs.TownRebuildResCost(ExtendedTowns[ntown], variantsET2[ntown], this);
                    default: return new int[23];
                }
            }
        }
        public double PaybackPeriod
        {
            get => DTownProfitForOwner > 0 ? RebuildCost / DTownProfitForOwner : 0;
        }
        public double PaybackWithGrowthPeriod
        {
            get => (DTownProfitForOwner + DGrowthAsMoney) > 0 ? RebuildCost / (DTownProfitForOwner + DGrowthAsMoney) : 0;
        }
        public override string ToJSON() //Костыль для сохранения данных
        {
            Towns = new ObservableCollection<Town>();
            for (int i = 0; i < ExtendedTowns.Count; i++)            
                Towns.Add(new Town(ExtendedTowns[i]));
            return base.ToJSON();
        }

        //вспомогательная штука, не стал её куда-то отдельно убирать
        static double[] ArrMinusArr(double[] arr1, double[] arr2)
        {
            if (arr1.Length != arr2.Length) return null;
            var ans = new double[arr1.Length];
            for (int i = 0; i< arr1.Length; i++) ans[i] = arr1[i] - arr2[i];
            return ans;
        }
    }
}
