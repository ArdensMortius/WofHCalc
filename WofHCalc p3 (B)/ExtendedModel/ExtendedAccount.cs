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
        public ObservableCollection<ExtendedTown> VariantsET1 { get; set; }
        [JsonIgnore]
        public ObservableCollection<ExtendedTown> VariantsET2 { get; set; }
        //public ObservableCollection<ExtendedTown>? TargetETowns { get; set; }
        [JsonIgnore]
        public DataWorldConst data;
        [JsonIgnore]
        public WofHMath WofHFuncs;
        public ExtendedAccount(Account acc) : base(acc) //или тут не работает или не тут
        {            
            data = DataWorldConst.GetInstance(acc.World);
            ExtendedTowns = new ObservableCollection<ExtendedTown>();
            VariantsET1 = new ObservableCollection<ExtendedTown>();
            VariantsET2 = new ObservableCollection<ExtendedTown>();
            foreach (Town town in acc.Towns) 
            {
                var et = new ExtendedTown (this, town);
                ExtendedTowns.Add(et);
                VariantsET1.Add((ExtendedTown)et.Clone());
                VariantsET2.Add((ExtendedTown)et.Clone());
            }
            WofHFuncs = new WofHMath(data);
        }
        public double RebuildCost(int n, byte variant)
        {
            switch (variant)
            {
                case 1: return WofHFuncs.TownRebuildCost((Town)ExtendedTowns[n], (Town)VariantsET1[n], (Account)this);
                case 2: return WofHFuncs.TownRebuildCost((Town)ExtendedTowns[n], (Town)VariantsET2[n], (Account)this);
                default: return 0;
            }            
        }
        //все изменения для сравнения с тем, что было
        public double DGrowth(int n, byte variant)
        {
            switch (variant)
            {
                case 1: return (VariantsET1[n].Growth - ExtendedTowns[n].Growth);
                case 2: return (VariantsET2[n].Growth - ExtendedTowns[n].Growth);
                default: return 0;
            }
        }
        public double DGrowthPriceTotal(int n, byte variant)
        {
            switch (variant)
            {
                case 1: return (VariantsET1[n].GrowthPriceTotal - ExtendedTowns[n].GrowthPriceTotal);
                case 2: return (VariantsET2[n].GrowthPriceTotal - ExtendedTowns[n].GrowthPriceTotal);
                default: return 0;
            }
        }
        public double DGrowthPricePerOne(int n, byte variant)
        {
            switch (variant)
            {
                case 1: return (VariantsET1[n].GrowthPricePerOne - ExtendedTowns[n].GrowthPricePerOne);
                case 2: return (VariantsET2[n].GrowthPricePerOne - ExtendedTowns[n].GrowthPricePerOne);
                default: return 0;
            }
        }
        public int DCulture(int n, byte variant)
        {
            switch (variant)
            {
                case 1: return (VariantsET1[n].Culture - ExtendedTowns[n].Culture);
                case 2: return (VariantsET2[n].Culture - ExtendedTowns[n].Culture);
                default: return 0;
            }
        }
        public double DCulturePriceTotal(int n, byte variant)
        {
            switch (variant)
            {
                case 1: return (VariantsET1[n].CulturePriceTotal - ExtendedTowns[n].CulturePriceTotal);
                case 2: return (VariantsET2[n].CulturePriceTotal - ExtendedTowns[n].CulturePriceTotal);
                default: return 0;
            }
        }
        public double DCulturePricePerHundred(int n, byte variant)
        {
            switch (variant)
            {
                case 1: return (VariantsET1[n].CulturePricePerHundred - ExtendedTowns[n].CulturePricePerHundred);
                case 2: return (VariantsET2[n].CulturePricePerHundred - ExtendedTowns[n].CulturePricePerHundred);
                default: return 0;
            }
        }
        public double[] DProducts(int n, byte variant)
        {
            switch (variant)
            {
                case 1: return (ArrMinusArr(VariantsET1[n].Products, ExtendedTowns[n].Products));
                case 2: return (ArrMinusArr(VariantsET2[n].Products, ExtendedTowns[n].Products));
                default: return new double[23];
            }
        }
        public double DProductsValuation(int n, byte variant)
        {
            switch (variant)
            {
                case 1: return (VariantsET1[n].ProductsValuation - ExtendedTowns[n].ProductsValuation);
                case 2: return (VariantsET2[n].ProductsValuation - ExtendedTowns[n].ProductsValuation);
                default: return 0;
            }
        }
        public int DTraiders(int n, byte variant)
        {
            switch (variant)
            {
                case 1: return (VariantsET1[n].Traiders - ExtendedTowns[n].Traiders);
                case 2: return (VariantsET2[n].Traiders - ExtendedTowns[n].Traiders);
                default: return 0;
            }
        }
        public double DTraiderPrice(int n, byte variant)
        {
            switch (variant)
            {
                case 1: return (VariantsET1[n].TraiderPrice - ExtendedTowns[n].TraiderPrice);
                case 2: return (VariantsET2[n].TraiderPrice - ExtendedTowns[n].TraiderPrice);
                default: return 0;
            }
        }
        public double DResPerTraider(int n, byte variant)
        {
            switch (variant)
            {
                case 1: return (VariantsET1[n].ResPerTraider - ExtendedTowns[n].ResPerTraider);
                case 2: return (VariantsET2[n].ResPerTraider - ExtendedTowns[n].ResPerTraider);
                default: return 0;
            }
        }
        public double DTotalUpkeep(int n, byte variant)
        {
            switch (variant)
            {
                case 1: return (VariantsET1[n].TotalUpkeep - ExtendedTowns[n].TotalUpkeep);
                case 2: return (VariantsET2[n].TotalUpkeep - ExtendedTowns[n].TotalUpkeep);
                default: return 0;
            }
        }
        public double[] DResoursesConsumption(int n, byte variant)
        {
            switch (variant)
            {
                case 1: return (ArrMinusArr(VariantsET1[n].ResoursesConsumption, ExtendedTowns[n].ResoursesConsumption));
                case 2: return (ArrMinusArr(VariantsET2[n].ResoursesConsumption, ExtendedTowns[n].ResoursesConsumption));
                default: return new double[23];
            }
        }
        public long DTownStrength(int n, byte variant)
        {
            switch (variant)
            {
                case 1: return (VariantsET1[n].TownStrength - ExtendedTowns[n].TownStrength);
                case 2: return (VariantsET2[n].TownStrength - ExtendedTowns[n].TownStrength);
                default: return 0;
            }
        }
        public long DTownPrice(int n, byte variant)
        {
            switch (variant)
            {
                case 1: return (VariantsET1[n].TownPrice - ExtendedTowns[n].TownPrice);
                case 2: return (VariantsET2[n].TownPrice - ExtendedTowns[n].TownPrice);
                default: return 0;
            }
        }
        public double DTownProfitForOwner(int n, byte variant)
        {
            switch (variant)
            {
                case 1: return (VariantsET1[n].TownProfitForOwner - ExtendedTowns[n].TownProfitForOwner);
                case 2: return (VariantsET2[n].TownProfitForOwner - ExtendedTowns[n].TownProfitForOwner);
                default: return 0;
            }
        }
        public double DTownProfitForCountry(int n, byte variant)
        {
            switch (variant)
            {
                case 1: return (VariantsET1[n].TownProfitForCountry - ExtendedTowns[n].TownProfitForCountry);
                case 2: return (VariantsET2[n].TownProfitForCountry - ExtendedTowns[n].TownProfitForCountry);
                default: return 0;
            }
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
